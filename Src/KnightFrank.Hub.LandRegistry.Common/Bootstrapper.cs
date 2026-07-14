using FluentValidation;
//using KnightFrank.Hub.LandRegistry.Common.Behaviours;
using KnightFrank.Hub.LandRegistry.Common.Mappings;
//using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace KnightFrank.Hub.LandRegistry.Common
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new Exception("Services are required");
            }

            services.AddMapProfiles("KnightFrank.Hub.LandRegistry");
            //services.AddMediatR(Assembly.GetExecutingAssembly());

            // Behaviours
            services.AddValidatorsFromAssembly(typeof(Bootstrapper).Assembly);
            ////services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}
