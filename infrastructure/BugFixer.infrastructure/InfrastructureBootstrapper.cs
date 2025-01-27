using BugFixer.Application.Contract.DTOS.Common;
using BugFixer.Application.Interfaces;
using BugFixer.infrastructure.EmailSender;
using BugFixer.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugFixer.infrastructure
{
    public static class InfrastructureBootstrapper
    {
        public static void Config(this IServiceCollection services, string connectionString )
        {

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISiteSettingRepository, SiteSettingRepository>();
            services.AddTransient<IEmailSend, EmailService>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddDbContext<BugFixerDbContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
         

        }
    }
}
