
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Common.Behaviors;

namespace Notes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly()); //это по видео уроку, в 8.0 выскакивает ошибка Could not load type 'MediatR.ServiceFactory' from assembly 'MediatR, Version=12.0.0.0  ..В stackoverflow говорится на заменить 
            //https://stackoverflow.com/questions/75527541/could-not-load-type-mediatr-servicefactory

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValudationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            return services;
        }
    }
}
