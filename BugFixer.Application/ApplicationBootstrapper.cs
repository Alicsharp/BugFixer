using Microsoft.Extensions.Configuration;

using BugFixer.Application.Contract.DTOS.Common;
using BugFixer.Application.Mapper;
using BugFixer.Application.Services.Account.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BugFixer.Application
{
    public static class ApplicationBootstrapper
    {
        public static void  ApplicationConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(Mapping));
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(RegisterUserCommand));
            });
            services.Configure<ScoreManagementDto>(options =>
            {
                configuration.GetSection("ScoreManagement").Bind(options);
            });
            

        }
    }
}
