﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Olive.Mvc
{
    public class ListPaginationBinderProvider : IModelBinderProvider
    {
        IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType.IsA<ListPagination>())
                return new ListPaginationBinder();
            else
                return null;
        }
    }

    public class ListPaginationBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var prefix = bindingContext.ModelName.OrEmpty().Unless("p");
            if (prefix.EndsWith(".p")) prefix = prefix.Split('.').ExceptLast().ToString(".");

            var old = bindingContext.Model as ListPagination;

            bindingContext.Result = ModelBindingResult.Success(new ListPagination(old?.Container, value.FirstValue)
            {
                Prefix = prefix,
                UseAjaxPost = old != null && old.UseAjaxPost,
                UseAjaxGet = old != null && old.UseAjaxGet
            });

            return Task.CompletedTask;
        }
    }

}