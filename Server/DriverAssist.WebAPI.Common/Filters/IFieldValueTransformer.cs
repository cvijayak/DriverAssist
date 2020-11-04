namespace DriverAssist.WebAPI.Common.Filters
{
    public interface IFieldValueTransformer
    {
        string Transform(string fieldName, string fieldValue);
    }
}