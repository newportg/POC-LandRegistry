using AutoMapper;
using FluentValidation;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Service.Requests;
using KnightFrank.Hub.LandRegistry.Service.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace KnightFrank.Hub.LandRegistry.Service
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddLandRegistryServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new Exception("Services are required");
            }

            services.AddValidatorsFromAssembly(typeof(Bootstrapper).Assembly);
            services.AddSingleton<ILandRegistrySvc, LandRegistrySvc>();

            // Register provider
            services.AddSingleton<ICertificateProvider>(sp =>
                new CertificateProvider(Environment.GetEnvironmentVariable("KeyVaultUri") ?? throw new InvalidOperationException("KeyVaultUri not set"),
                    sp.GetRequiredService<ILogger<CertificateProvider>>()));

            // register factories so DI constructs them (example patterns)
            services.AddTransient<ApplicationEnquiryFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new ApplicationEnquiryFactory(mapper, logger, cert);
            });

            // register other factories (transient) — simple constructors (no cert) can be registered directly: NEED TO CHANGE AS WELL
            services.AddTransient<LLCBankruptySearchFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new LLCBankruptySearchFactory(mapper, logger, cert);
            });
            services.AddTransient<DischargeActivityFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new DischargeActivityFactory(mapper, logger, cert);
            });
            services.AddTransient<EnquiryByPropertyDescriptionFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new EnquiryByPropertyDescriptionFactory(mapper, logger, cert);
            });
            services.AddTransient<LCFullSearchFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new LCFullSearchFactory(mapper, logger, cert);
            });
            services.AddTransient<OfficialCopyTitleKnownFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new OfficialCopyTitleKnownFactory(mapper, logger, cert);
            });
            services.AddTransient<OfficialSearchWholeFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new OfficialSearchWholeFactory(mapper, logger, cert);
            });
            services.AddTransient<OfficialSearchPartFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new OfficialSearchPartFactory(mapper, logger, cert);
            });

            // Polling factories:
            services.AddTransient<PollApplicationEnquiryFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollApplicationEnquiryFactory(mapper, logger, cert);
            });
            services.AddTransient<PollLLCBankruptySearchFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollLLCBankruptySearchFactory(mapper, logger, cert);
            });
            services.AddTransient<PollDischargeActivityFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollDischargeActivityFactory(mapper, logger, cert);
            });
            services.AddTransient<PollPropertyDescriptionEnquiryFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollPropertyDescriptionEnquiryFactory(mapper, logger, cert);
            });
            services.AddTransient<PollLCFullSearchFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollLCFullSearchFactory(mapper, logger, cert);
            });
            services.AddTransient<PollOfficialSearchWholeFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollOfficialSearchWholeFactory(mapper, logger, cert);
            });
            services.AddTransient<PollOfficialSearchPartFactory>(sp =>
            {
                var mapper = sp.GetRequiredService<IMapper>();
                var logger = sp.GetRequiredService<ILogger<LandRegistrySvc>>();
                var certProvider = sp.GetRequiredService<ICertificateProvider>();
                X509Certificate2? cert = null;
                var certName = Environment.GetEnvironmentVariable("CertName");
                if (!string.IsNullOrEmpty(certName))
                    cert = certProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                return new PollOfficialSearchPartFactory(mapper, logger, cert);
            });

            return services;
        }
    }
}