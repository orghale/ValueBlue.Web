
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ValueBlue.Web.BusinessLogic;
using ValueBlue.Web.BusinessLogic.Interface;
using ValueBlue.Web.Service;
using ValueBlue.Web.Service.Interface;

namespace ValueBlue.Web
{
    public static class DiRegistry
    {
        public static IServiceCollection ServiceDi(this IServiceCollection services)
        {
            services.AddScoped<IMongoRepo, MongoRepo>();
            services.AddScoped<IApiCoreService, ApiCoreService>();
            services.AddScoped<IApiCallserv, ApiCallserv>();

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }
    }
}
