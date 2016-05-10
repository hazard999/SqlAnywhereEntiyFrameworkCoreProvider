using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ProviderStarter.Infrastructure
{
    public class SqlAnyhwewProviderOptionsExtension : IDbContextOptionsExtension
    {
        public string ConnectionString { get; set; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddMyProvider();
        }

        public void ApplyServices(EntityFrameworkServicesBuilder builder)
        {
            builder.AddMyProvider();
        }
    }
}