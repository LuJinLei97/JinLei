#if NETFRAMEWORK
#else
using System.Diagnostics;
using System.IO;

using JinLei.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace JinLei.Utilities;

public partial class TemplateUtility
{
    private static IServiceCollection Services { get; } = new ServiceCollection().Do(t => t.AddLogging());

    private static IServiceProvider ServiceProvider { get; } = Services.BuildServiceProvider();

    private static ILoggerFactory LoggerFactory { get; } = ServiceProvider.GetRequiredService<ILoggerFactory>();

    /// <remarks><see href="https://learn.microsoft.com/aspnet/core/blazor/components/render-components-outside-of-aspnetcore">Source</see></remarks> 
    public static async Task<string?> RenderComponentAsync<TComponent>(ParameterView parameters = default) where TComponent : IComponent
    {
        await using var htmlRenderer = new HtmlRenderer(ServiceProvider, LoggerFactory);

        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await htmlRenderer.RenderComponentAsync<TComponent>(parameters);

            return output.ToHtmlString();
        });

        return html;
    }
}

public partial class TemplateUtility
{
    private static IServiceCollection Services2
    {
        get
        {
            if(services2.IsNull())
            {
                var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
                services2 = new ServiceCollection();
                services2.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
                services2.AddSingleton<DiagnosticListener>(diagnosticSource);
                services2.AddSingleton<DiagnosticSource>(diagnosticSource);
                services2.AddLogging();
                services2.AddMvc();
                services2.AddTransient<RazorViewToStringRenderer>();
            }

            return services2;
        }
    }
    private static IServiceCollection services2;

    private static IServiceProvider ServiceProvider2 { get; } = Services2.BuildServiceProvider();

    /// <remarks><see href="https://github.com/aspnet/samples/blob/main/samples/aspnetcore/mvc/renderviewtostring/RazorViewToStringRenderer.cs">Source</see></remarks> 
    internal static Task<string> RenderViewAsync<TModel>(string viewName, TModel model) => ServiceProvider2.GetRequiredService<RazorViewToStringRenderer>().RenderViewToStringAsync(viewName, model);

    private class RazorViewToStringRenderer
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            //var actionContext = GetActionContext();
            var actionContext = new ActionContext();
            var view = FindView(actionContext, viewName);

            using var output = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<TModel>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(
                    actionContext.HttpContext,
                    _tempDataProvider),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if(getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if(findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));
            ;

            throw new InvalidOperationException(errorMessage);
        }

        //private ActionContext GetActionContext()
        //{
        //    var httpContext = new DefaultHttpContext
        //    {
        //        RequestServices = _serviceProvider
        //    };
        //    return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        //}
    }
}
#endif