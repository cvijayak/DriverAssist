namespace DriverAssist.WebAPI.Common.Filters
{
    public abstract class FieldValueTransformerBase<T> where T : struct
    {
        public string Transform(string fieldName, string fieldValue)
        {
           return fieldValue;
        }
    }
}