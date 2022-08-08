using Flunt.Notifications;
using Flunt.Validations;
using Serverless.Extensions.Entities.Bases;

namespace ServlessKafka.Core.Domain.Entities;

public class Produto : BaseEntity
{
    public string? Nome { get; set; }
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }

    public Produto() { }

    public override void Validate()
    {
        AddNotifications(new Contract<Notification>()
                        .IsNotNullOrEmpty(nameof(Nome), "O nome é obrigatório.")
                        .IsGreaterThan(Quantidade, 0, nameof(Quantidade), "A quantidade é inválida.")
                        .IsGreaterThan(Preco, 0, "O preço é inválido."));
    }
}
