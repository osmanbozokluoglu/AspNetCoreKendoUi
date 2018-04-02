using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KendoUI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;

namespace AspNetCoreKendoUi.Utils
{

    public class KendoRequestModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;

        public KendoRequestModelBinder(IModelBinder fallbackBinder)
        {
            if (fallbackBinder == null)
                throw new ArgumentNullException(nameof(fallbackBinder));

            _fallbackBinder = fallbackBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return _fallbackBinder.BindModelAsync(bindingContext);
            }

            var valueAsString = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(valueAsString))
            {
                return _fallbackBinder.BindModelAsync(bindingContext);
            }

            var dataString = bindingContext.HttpContext.GetJsonDataFromQueryString();
            var dataSourceRequest = JsonConvert.DeserializeObject<DataSourceRequest>(dataString);
            bindingContext.Result = ModelBindingResult.Success(dataSourceRequest);

            return Task.CompletedTask;
        }
    }

    public class KendoRequestModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsComplexType) return null;

            var propName = context.Metadata.PropertyName;
            if (propName == null) return null;

            var propInfo = context.Metadata.ContainerType.GetProperty(propName);
            if (propInfo == null) return null;

            var attribute = propInfo.GetCustomAttributes(
                typeof(KendoRequestAttribute), false).FirstOrDefault();
            if (attribute == null) return null;

            return new BinderTypeModelBinder(typeof(KendoRequestModelBinder));
        }
    }
}
