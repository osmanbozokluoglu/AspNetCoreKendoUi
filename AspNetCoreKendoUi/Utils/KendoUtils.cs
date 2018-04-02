using KendoUI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AspNetCoreKendoUi.Utils
{
    public static class KendoUtils
    {
        public static string GetJsonDataFromQueryString(this HttpContext httpContext)
        {
            var rawQueryString = httpContext.Request.QueryString.ToString();
            var rawQueryStringKeyValue = QueryHelpers.ParseQuery(rawQueryString).FirstOrDefault();
            var dataString = Uri.UnescapeDataString(rawQueryStringKeyValue.Key);
            return dataString;
        }

        public static DataSourceRequest GetKendoDataSourceResult(this HttpContext httpContext)
        {
            var dataString = httpContext.GetJsonDataFromQueryString();
            return JsonConvert.DeserializeObject<DataSourceRequest>(dataString);
        }
    }
}
