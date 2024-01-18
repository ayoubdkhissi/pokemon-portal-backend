namespace Domain.Interfaces;
public interface ISecretsService
{
    string GetSecretValue(string secretName);
    Task<string> GetSecretValueAsync(string secretName);
}