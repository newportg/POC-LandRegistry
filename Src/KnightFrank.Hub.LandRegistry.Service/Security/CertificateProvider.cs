using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;

namespace KnightFrank.Hub.LandRegistry.Service.Security
{
    public interface ICertificateProvider
    {
        /// <summary>
        /// Get a client certificate from Key Vault (cached).
        /// Returns a certificate with private key if available and exportable, otherwise public-only cert.
        /// </summary>
        Task<X509Certificate2?> GetCertificateAsync(string certificateName, CancellationToken ct = default);
    }

    public sealed class CertificateProvider : ICertificateProvider, IDisposable
    {
        private readonly CertificateClient _certClient;
        private readonly SecretClient _secretClient;
        private readonly ILogger<CertificateProvider> _logger;

        private X509Certificate2? _cachedCert;
        private DateTimeOffset _cacheExpiry = DateTimeOffset.MinValue;
        private readonly SemaphoreSlim _sync = new(1, 1);
        private bool _disposed;

        public CertificateProvider(string keyVaultUri, ILogger<CertificateProvider> logger)
        {
            if (string.IsNullOrEmpty(keyVaultUri)) throw new ArgumentNullException(nameof(keyVaultUri));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var uri = new Uri(keyVaultUri);
            var cred = new DefaultAzureCredential();

            _certClient = new CertificateClient(uri, cred);
            _secretClient = new SecretClient(uri, cred);
        }

        public async Task<X509Certificate2?> GetCertificateAsync(string certificateName, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(certificateName)) throw new ArgumentNullException(nameof(certificateName));

            // return cached cert if still valid
            if (_cachedCert != null && DateTimeOffset.UtcNow < _cacheExpiry)
                return _cachedCert;

            await _sync.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (_cachedCert != null && DateTimeOffset.UtcNow < _cacheExpiry)
                    return _cachedCert;

                _logger.LogInformation("Fetching certificate '{name}' from Key Vault", certificateName);

                var certResponse = await _certClient.GetCertificateAsync(certificateName, ct).ConfigureAwait(false);
                var certBundle = certResponse.Value;
                if (certBundle == null)
                {
                    _logger.LogError("Certificate '{name}' not found in Key Vault", certificateName);
                    return null;
                }

                if (certBundle.Policy?.Exportable == true)
                {
                    var secretResponse = await _secretClient.GetSecretAsync(certificateName, cancellationToken: ct).ConfigureAwait(false);
                    var secretValue = secretResponse.Value?.Value;
                    if (string.IsNullOrEmpty(secretValue))
                    {
                        _logger.LogError("Secret for certificate '{name}' is empty", certificateName);
                        return null;
                    }

                    var pfxBytes = Convert.FromBase64String(secretValue);

                    // Use EphemeralKeySet so private key is not written to disk or store
                    var cert = new X509Certificate2(pfxBytes, (string?)null, X509KeyStorageFlags.EphemeralKeySet);

                    ValidateCertificateChain(cert, out var statusSummary);

                    // Cache until shortly before expiry
                    _cachedCert?.Dispose();
                    _cachedCert = cert;
                    _cacheExpiry = cert.NotAfter.ToUniversalTime().AddMinutes(-5);

                    _logger.LogInformation("Certificate '{name}' loaded; expires {expiryUtc}", certificateName, cert.NotAfter.ToUniversalTime());
                    return _cachedCert;
                }
                else
                {
                    // Private key not exportable: return public certificate only (cannot be used for client TLS auth)
                    _logger.LogWarning("Certificate '{name}' private key is not exportable. Returning public-only certificate.", certificateName);
                    return new X509Certificate2(certBundle.Cer);
                }
            }
            finally
            {
                _sync.Release();
            }
        }

        private void ValidateCertificateChain(X509Certificate2 cert, out string statusSummary)
        {
            statusSummary = string.Empty;
            try
            {
                using var chain = new X509Chain();
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // adjust in prod
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

                var built = chain.Build(cert);
                var statuses = chain.ChainStatus?.Select(s => s.StatusInformation.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? Array.Empty<string>();
                statusSummary = string.Join("; ", statuses);

                _logger.LogInformation("Certificate chain build result: {built}. Elements: {count}", built, chain.ChainElements.Count);
                foreach (var e in chain.ChainElements)
                {
                    _logger.LogDebug("Chain element: {subject} expires {expiry}", e.Certificate.Subject, e.Certificate.NotAfter.ToUniversalTime());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ValidateCertificateChain failed");
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _cachedCert?.Dispose();
            _sync.Dispose();
            _disposed = true;
        }
    }
}