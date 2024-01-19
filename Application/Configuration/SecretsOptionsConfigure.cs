using Application.Options;
using Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Configuration;
public class SecretsOptionsConfigure : IConfigureOptions<SecretsOptions>
{
    private readonly ISecretsService _secretsService;
    private readonly string _envName;

    public SecretsOptionsConfigure(ISecretsService secretsService, string envName)
    {
        _secretsService = secretsService;
        _envName = envName;
    }

    public void Configure(SecretsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.DbUsername = GetSecret("DbUsername");
        options.DbPassword = GetSecret("DbPassword", true);
    }

    private string GetSecret(string keyName, bool includeEnv = false)
    {
        string keySuffix = includeEnv ? $"-{_envName}" : string.Empty;
        string secretName = $"{keyName}{keySuffix}";
        return _secretsService.GetSecretValue(secretName);
    }
}
