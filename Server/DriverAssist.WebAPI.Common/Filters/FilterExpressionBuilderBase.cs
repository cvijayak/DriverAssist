using DriverAssist.Common;
using DriverAssist.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DriverAssist.WebAPI.Common.Filters
{
    public abstract class FieldValueTransformerBase<T> where T : struct
    {
        private static readonly IReadOnlyCollection<T> IdFields = EnumX.FilterByAttribute<T, IdAttribute>().ToArray();

        public string Transform(string fieldName, string fieldValue)
        {
            try
            {
                var fieldNameEnum = fieldName.ToEnum<T>();
                //if (IdFields.Any(s => fieldNameEnum.Equals(s)))
                //{
                //    var internalId = fieldValue.SafeConvertInternalId();
                //    if (internalId == -1 || internalId.ToVisibleId() != fieldValue)
                //    {
                //        internalId = -1;
                //    }

                //    return internalId.ToString();
                //}

                return fieldValue;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class DriverFilterExpressionBuilder : FilterExpressionBuilderBase<DriverFilter, Driver>
    {
        private class FieldValueTransformer : FieldValueTransformerBase<DriverFilter.Fields>, IFieldValueTransformer
        {
        }

        private static readonly IFieldValueTransformer ValueTransformer = new FieldValueTransformer();

        public DriverFilterExpressionBuilder()
            : base(null, ValueTransformer)
        {
        }
    }

    public class VehicleFilterExpressionBuilder : FilterExpressionBuilderBase<VehicleFilter, Vehicle>
    {
        private class FieldValueTransformer : FieldValueTransformerBase<VehicleFilter.Fields>, IFieldValueTransformer
        {
        }

        private static readonly IFieldValueTransformer ValueTransformer = new FieldValueTransformer();

        public VehicleFilterExpressionBuilder()
            : base(null, ValueTransformer)
        {
        }
    }

    public abstract class FilterExpressionBuilderBase<T, K> where T : FilterBase
    {
        private readonly IFieldNameTransformer _nameTransformer;
        private readonly IFieldValueTransformer _valueTransformer;
        private IReadOnlyCollection<string> _supportedFields;

        protected FilterExpressionBuilderBase(IFieldNameTransformer nameTransformer, IFieldValueTransformer valueTransformer)
        {
            _nameTransformer = nameTransformer;
            _valueTransformer = valueTransformer;
        }

        private ExpressionBuilder<K> BuildLogicalExpression(ExpressionBuilder<K> builder, BinaryOperator binaryOperator, ConditionExpressionToken token)
        {
            if (_supportedFields != null && _supportedFields.All(s => s != token.FieldName))
            {
                throw new ArgumentException($"Field not supported, FieldName : {token.FieldName}, SupportedFields : {string.Join(",", _supportedFields)}");
            }

            var transformedFieldName = TransformFieldName(token.FieldName);
            var transformedFieldValue = token.Operand;
            if (transformedFieldValue != null)
            {
                transformedFieldValue = TransformFieldValue(token.FieldName, transformedFieldValue);
            }

            Func<string, string, Tuple<ExpressionBuilder<K>, string>> buildExpression = (fieldName, fieldValue) =>
            {
                try
                {
                    switch (token.Operator)
                    {
                        case LogicalOperatorType.EQ: return builder.BuildEqualToExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.GE: return builder.BuildGreaterThanOrEqualToExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.LE: return builder.BuildLessThanOrEqualToExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.GT: return builder.BuildGreaterThanExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.LT: return builder.BuildLesserThanExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.NE: return builder.BuildNotEqualToExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.CN: return builder.BuildContainsExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.SW: return builder.BuildStartsWithExpression(binaryOperator, fieldName, fieldValue);
                        case LogicalOperatorType.EW: return builder.BuildEndsWithExpression(binaryOperator, fieldName, fieldValue);
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException($"Invalid field value, Operator : {token.Operator}, " +
                                                $"FieldName : {fieldName}, " +
                                                $"FieldValue : {fieldValue ?? string.Empty}");
                }

                throw new ArgumentException($"Operator not supported, Operator : {token.Operator}");
            };

            var expression = buildExpression(transformedFieldName, transformedFieldValue);
            if (!string.IsNullOrEmpty(expression.Item2))
            {
                throw new ArgumentException(expression.Item2);
            }

            return expression.Item1;
        }

        private ExpressionBuilder<K> BuildExpression(ConditionExpressionToken token, ExpressionBuilder<K> builder)
        {
            if (token.PreviousToken == null)
            {
                return BuildLogicalExpression(builder, BinaryOperator.NONE, token);
            }

            var prevBinaryToken = token.PreviousToken as BinaryExpressionToken;
            if (prevBinaryToken == null)
            {
                throw new Exception();
            }

            switch (prevBinaryToken.Operator)
            {
                case BinaryOperatorType.AND: return BuildLogicalExpression(builder, BinaryOperator.AND, token);
                case BinaryOperatorType.OR: return BuildLogicalExpression(builder, BinaryOperator.OR, token);
                default: throw new ArgumentException($"Binary operator '{prevBinaryToken.Operator}' not supported");
            }
        }

        private ExpressionBuilder<K> BuildExpression(DefaultExpressionTokens tokens, ExpressionBuilder<K> builder)
        {
            var intermBuilder = new ExpressionBuilder<K>();
            intermBuilder = tokens.Tokens.Aggregate(intermBuilder, (b, t) =>
            {
                var lExpressionToken = t as ConditionExpressionToken;
                if (lExpressionToken != null)
                {
                    return BuildExpression(lExpressionToken, b);
                }

                var lTokens = t as DefaultExpressionTokens;
                if (lTokens != null)
                {
                    return BuildExpression(lTokens, b);
                }

                return b;
            });

            var fOperator = tokens.PreviousToken as BinaryExpressionToken;
            if (fOperator == null)
            {
                return intermBuilder;
            }

            switch (fOperator.Operator)
            {
                case BinaryOperatorType.AND: return builder.And(intermBuilder.Expr);
                case BinaryOperatorType.OR: return builder.Or(intermBuilder.Expr);
                default: throw new ArgumentException($"Binary operator '{fOperator.Operator}' not supported");
            }
        }

        private string TransformFieldName(string fieldName)
        {
            return _nameTransformer != null ? _nameTransformer.Transform(fieldName) : fieldName;
        }

        private string TransformFieldValue(string fieldName, string fieldValue)
        {
            return _valueTransformer != null ? _valueTransformer.Transform(fieldName, fieldValue) : fieldValue;
        }

        public Expression<Func<K, bool>> Build(T filter)
        {
            try
            {
                _supportedFields = filter.SupportedFields;
                return BuildExpression(filter.Tokens, new ExpressionBuilder<K>()).Expr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}