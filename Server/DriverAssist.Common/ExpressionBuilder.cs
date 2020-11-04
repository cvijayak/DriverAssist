using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace DriverAssist.Common
{
    public class ExpressionBuilder<T>
    {
        private enum Method
        {
            Contains,
            StartsWith,
            EndsWith
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }

        private string GetDefaultValue(Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                return null;
            }

            return type.IsValueType ? Activator.CreateInstance(type).ToString() : null;
        }

        private ConstantExpression BuildConstantExpression(string propertyValue, Type propertyType)
        {
            var typeConverter = TypeDescriptor.GetConverter(propertyType);
            var propValue = typeConverter.ConvertFromString(propertyValue ?? GetDefaultValue(propertyType));
            return Expression.Constant(propValue);
        }

        private Tuple<Expression<Func<T, bool>>, string> BuildExpression(string propertyName, string propertyValue, Func<Expression, Expression, BinaryExpression> predicate)
        {
            var objectType = typeof(T);
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return new Tuple<Expression<Func<T, bool>>, string>(null, $"Unable to find Field, FieldName : {propertyName}");
            }
            var parameterExpression = Expression.Parameter(objectType);
            var propertyExpression = Expression.Property(parameterExpression, propertyName);
            var constantExpression = BuildConstantExpression(propertyValue, propertyInfo.PropertyType);
            var convertedConstantExpression = Expression.Convert(constantExpression, propertyInfo.PropertyType);
            var buildExpression = Expression.Lambda<Func<T, bool>>(predicate(propertyExpression, convertedConstantExpression), parameterExpression);
            return new Tuple<Expression<Func<T, bool>>, string>(buildExpression, string.Empty);
        }

        private Tuple<Expression<Func<T, bool>>, string> BuildMethodExpression(string propertyName, string propertyValue, Method methodName)
        {
            var objectType = typeof(T);
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return new Tuple<Expression<Func<T, bool>>, string>(null, $"Unable to find Field, FieldName : {propertyName}");
            }
            var parameterExpression = Expression.Parameter(objectType);
            var propertyType = propertyInfo.PropertyType;
            var method = propertyType.GetMethod(methodName.ToString(), new[] { propertyType });
            if (method == null)
            {
                return new Tuple<Expression<Func<T, bool>>, string>(null, $"Field not contains the method, FieldName : {propertyName}, Method : {methodName}");
            }
            var memberExpression = Expression.Property(parameterExpression, propertyName);
            var constantExpression = Expression.Constant(propertyValue);
            var convertedConstantExpression = Expression.Convert(constantExpression, propertyType);
            var bodyExpression = Expression.Call(memberExpression, method, convertedConstantExpression);
            var buildContainsExpression = Expression.Lambda<Func<T, bool>>(bodyExpression, parameterExpression);
            return new Tuple<Expression<Func<T, bool>>, string>(buildContainsExpression, string.Empty);
        }

        private Tuple<ExpressionBuilder<T>, string> MergeExpressionWithBinaryOperator(BinaryOperator binaryOperator, Tuple<Expression<Func<T, bool>>, string> expr)
        {
            if (!string.IsNullOrEmpty(expr.Item2))
            {
                return new Tuple<ExpressionBuilder<T>, string>(this, expr.Item2);
            }

            switch (binaryOperator)
            {
                case BinaryOperator.AND: return new Tuple<ExpressionBuilder<T>, string>(And(expr.Item1), string.Empty);
                case BinaryOperator.OR: return new Tuple<ExpressionBuilder<T>, string>(Or(expr.Item1), string.Empty);
                case BinaryOperator.NONE: return new Tuple<ExpressionBuilder<T>, string>(Initialize(expr.Item1), string.Empty);
                default: return new Tuple<ExpressionBuilder<T>, string>(this, "Unknown Logical Operator");
            }
        }

        private Tuple<ExpressionBuilder<T>, string> BuildExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue, Func<Expression, Expression, BinaryExpression> predicate)
        {
            var expr = BuildExpression(propertyName, propertyValue, predicate);
            return MergeExpressionWithBinaryOperator(binaryOperator, expr);
        }

        private Tuple<ExpressionBuilder<T>, string> BuildMethodExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue, Method methodName)
        {
            var expr = BuildMethodExpression(propertyName, propertyValue, methodName);
            return MergeExpressionWithBinaryOperator(binaryOperator, expr);
        }

        private ExpressionBuilder<T> Initialize(Expression<Func<T, bool>> expr)
        {
            Expr = expr;
            return this;
        }

        public ExpressionBuilder<T> True()
        {
            Expr = f => true;
            return this;
        }

        public ExpressionBuilder<T> False()
        {
            Expr = f => false;
            return this;
        }

        public ExpressionBuilder<T> Or(Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));
            var left = new ReplaceExpressionVisitor(Expr.Parameters[0], parameter).Visit(Expr.Body);
            var right = new ReplaceExpressionVisitor(expr.Parameters[0], parameter).Visit(expr.Body);
            Expr = Expression.Lambda<Func<T, bool>>(Expression.OrElse(left, right), parameter);
            return this;
        }

        public ExpressionBuilder<T> And(Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));
            var left = new ReplaceExpressionVisitor(Expr.Parameters[0], parameter).Visit(Expr.Body);
            var right = new ReplaceExpressionVisitor(expr.Parameters[0], parameter).Visit(expr.Body);
            Expr = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
            return this;
        }

        public Tuple<ExpressionBuilder<T>, string> BuildContainsExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildMethodExpression(binaryOperator, propertyName, propertyValue, Method.Contains);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildStartsWithExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildMethodExpression(binaryOperator, propertyName, propertyValue, Method.StartsWith);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildEndsWithExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildMethodExpression(binaryOperator, propertyName, propertyValue, Method.EndsWith);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildEqualToExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildExpression(binaryOperator, propertyName, propertyValue, Expression.Equal);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildGreaterThanOrEqualToExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildExpression(binaryOperator, propertyName, propertyValue, Expression.GreaterThanOrEqual);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildLessThanOrEqualToExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildExpression(binaryOperator, propertyName, propertyValue, Expression.LessThanOrEqual);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildGreaterThanExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildExpression(binaryOperator, propertyName, propertyValue, Expression.GreaterThan);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildLesserThanExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildExpression(binaryOperator, propertyName, propertyValue, Expression.LessThan);
        }

        public Tuple<ExpressionBuilder<T>, string> BuildNotEqualToExpression(BinaryOperator binaryOperator, string propertyName, string propertyValue)
        {
            return BuildExpression(binaryOperator, propertyName, propertyValue, Expression.NotEqual);
        }

        public Expression<Func<T, bool>> Expr { get; private set; }
    }
}
