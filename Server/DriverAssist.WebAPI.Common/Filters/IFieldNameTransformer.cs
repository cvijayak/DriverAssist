namespace DriverAssist.WebAPI.Common.Filters
{
    public interface IFieldNameTransformer
    {
        string Transform(string fieldName);
    }
}