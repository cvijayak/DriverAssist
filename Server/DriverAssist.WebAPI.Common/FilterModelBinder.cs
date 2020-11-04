using DriverAssist.WebAPI.Common.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Common
{
    public class FilterModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;
            var val = bindingContext.ValueProvider.GetValue(key);
            var s = val.FirstValue;
            bindingContext.Model = null;

            if (!string.IsNullOrEmpty(s))
            {
                using (var parser = new FilterParser())
                {
                    string errorMessage;
                    var filterExpression = parser.Process(s, () => Activator.CreateInstance(bindingContext.ModelType) as FilterBase, out errorMessage);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        bindingContext.ModelState.AddModelError("filter", errorMessage);
                    }
                    else
                    {
                        bindingContext.Model = filterExpression;
                    }
                }
            }

            return Task.FromResult(0);
        }
    }
}