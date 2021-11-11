
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ValueBlue.Web.BusinessLogic;
using ValueBlue.Web.BusinessLogic.Interface;

namespace ValueBlue.Web
{
    public static class DiRegistry
    {
        public static IServiceCollection ServiceDi(this IServiceCollection services)
        {
            services.AddScoped<IMongoRepo, MongoRepo>();

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }
    }
}
