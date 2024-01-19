using Application.Options;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;
public class SecretsService : ISecretsService
{
    private readonly SecretClient _secretClient;
    public SecretsService(IOptions<AzureAdOptions> azureAdOptions)
    {
        ArgumentNullException.ThrowIfNull(azureAdOptions);

        var clientId = azureAdOptions.Value.ClientId;
        var clientSecret = azureAdOptions.Value.ClientSecret;
        var tenantId = azureAdOptions.Value.TenantId;
        var keyVaultBaseUrl = new Uri(azureAdOptions.Value.KeyVault.BaseUrl);

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _secretClient = new SecretClient(keyVaultBaseUrl, credential);
    }

    public string GetSecretValue(string secretName)
    {
        var secret = _secretClient.GetSecret(secretName);

        return secret.Value.Value ?? throw new NotFoundException($"Secret {secretName} not found in KeyVault");
    }

    public async Task<string> GetSecretValueAsync(string secretName)
    {
        var secret = await _secretClient.GetSecretAsync(secretName);

        return secret.Value.Value ?? throw new NotFoundException($"Secret {secretName} not found in KeyVault");
    }
}
