﻿namespace Application.Options;
public class AzureAdOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public KeyVaultOptions KeyVault { get; set; } = new();
}
