﻿using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.RelationalProviderStarter.Infrastructure
{
    public class MyRelationalProviderOptionsExtension : IDbContextOptionsExtension
    {
        public string ConnectionString { get; set; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSqlAnywhereProvider();
        }

        //public void ApplyServices(EntityFrameworkServicesBuilder builder)
        //{
        //    builder.AddMyRelationalProvider();
        //}
    }
}