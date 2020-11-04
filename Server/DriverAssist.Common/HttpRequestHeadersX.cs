//using System.Linq;
//using System.Net.Http.Headers;

//namespace DriverAssist.Common
//{
//    public static class HttpRequestHeadersX
//    {
//        public static RangeItemHeaderValue ReadRangeHeader(this IHeaderDictionary requestHeaders, string unit)
//        {
//            if (requestHeaders.Range?.Ranges != null)
//            {
//                if (requestHeaders.Range.Unit == unit && requestHeaders.Range.Ranges.Count == 1)
//                {
//                    return requestHeaders.Range.Ranges.First();
//                }
//            }

//            return null;
//        }
//    }
//}
