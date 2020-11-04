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
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var key = bindingContext.ModelName;
            var val = bindingContext.ValueProvider.GetValue(key);
            if (val == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(key, val);

            var s = val.FirstValue;

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
                        bindingContext.Result = ModelBindingResult.Success(filterExpression);
                    }
                }
            } 
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}