﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.RelationalProviderStarter.Infrastructure
{
    public class SqlAnywhereProviderOptionsExtension : IDbContextOptionsExtension
    {
        public string ConnectionString { get; set; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSqlAnywhereProvider();
        }        
    }
}