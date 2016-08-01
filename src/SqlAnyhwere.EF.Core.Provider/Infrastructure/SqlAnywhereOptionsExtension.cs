using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.RelationalProviderStarter.Infrastructure
{
    public class SqlAnywhereOptionsExtension : RelationalOptionsExtension
    {
        //public string ConnectionString { get; set; }

        public override void ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlAnywhere();
        }      
             
    }
}