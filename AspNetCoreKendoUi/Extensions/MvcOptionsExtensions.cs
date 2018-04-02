using AspNetCoreKendoUi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Linq;

namespace AspNetCoreKendoUi.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void UseKendoRequestModelBinding(this MvcOptions opts)
        {
            var binderToFind = opts.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));

            if (binderToFind == null) return;

            var index = opts.ModelBinderProviders.IndexOf(binderToFind);
            opts.ModelBinderProviders.Insert(index, new KendoRequestModelBinderProvider());
        }
    }
}
