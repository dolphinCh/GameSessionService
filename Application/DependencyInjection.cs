using Application.Common.Helpers.Service.Diagnostic;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            var sp = services.BuildServiceProvider();
            var mediatR = sp.GetService<IMediator>();
            services.AddSingleton<IPerformanceService>(new PerformanceService(mediatR));
            return services;
        }
    }
}
