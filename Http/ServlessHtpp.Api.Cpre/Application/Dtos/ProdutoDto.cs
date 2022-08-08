using ServerlessHtpp.Api.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace ServerlessHtpp.Api.Core.Application.Dtos;

public class ProdutoDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("nome")]
    public string? Nome { get; set; }
    [JsonPropertyName("quantidade")]
    public int Quantidade { get; set; }
    [JsonPropertyName("preco")]
    public decimal Preco { get; set; }

    public ProdutoDto() { }

    public static implicit operator Produto(ProdutoDto produtoDto) =>
        new() { Nome = produtoDto.Nome, Quantidade = produtoDto.Quantidade, Preco = produtoDto.Preco };
}
