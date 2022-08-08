namespace Serverless.Extensions.Shared.Configurations;

public class MessagingConfigurationOptions
{
    public const string MessagingConfig = "MessagingConfiguration";

    public string? BootstrapServer { get; set; }
    public int SessionTimeoutMs { get; set; }
    public int ConsumeTimeout { get; set; }
    public string? GroupId { get; set; }
    public string? Topic { get; set; }

    public MessagingConfigurationOptions() { }

}
