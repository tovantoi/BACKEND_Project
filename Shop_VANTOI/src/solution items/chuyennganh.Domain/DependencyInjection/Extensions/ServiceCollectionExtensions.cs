using chuyennganh.Domain.Constants;
using chuyennganh.Domain.DTOs;
using chuyennganh.Domain.Helpers;
using chuyennganh.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chuyennganh.Domain.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="EnvironmentHelper"/>, have easily way to get <see cref="IWebHostEnvironment"/>
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IWebHostEnvironment AddEnvironmentHelper(this IWebHostEnvironment env)
        {
            EnvironmentHelper.Environment = env;
            return env;
        }
        public static IServiceCollection AddConfigSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UploadSettings>(configuration.GetSection(Const.UPLOADED_SETTINGS));
            services.Configure<HostSettings>(configuration.GetSection(Const.DOMAIN_HOSTS));
            services.Configure<VnPayConfig>(configuration.GetSection(Const.VN_CONFIG));
            services.Configure<SocialLoginService>(configuration.GetSection(Const.SOCIAL));

            return services;
        }
    }
}