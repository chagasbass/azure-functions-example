namespace Serverless.Extensions.Shared.Configurations;

public class BaseConfigurationOptions
{
    public const string BaseConfig = "BaseConfiguration";
    public string? NomeAplicacao { get; set; }
    public string? Descricao { get; set; }
    public string? NomeDesenvolvedor { get; set; }
    public string? NomeEmpresa { get; set; }
    public string? UrlEmpresa { get; set; }
    public string? StringConexaoBancoDeDados { get; set; }
    public string? NomeBanco { get; set; }
    public bool TemAutenticacao { get; set; }

    public BaseConfigurationOptions() { }
}
