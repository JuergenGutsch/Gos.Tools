using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Gos.Tools.Cqs.Event
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCqsEventStore<TAggregateRoot>(this IServiceCollection services) where TAggregateRoot : IAggregateRoot
        {
            services.AddScoped<IEventStore<TAggregateRoot>, EventStore<TAggregateRoot>>();

            return services;
        }
    }
}
