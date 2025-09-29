using Insurance.Domain.Entities.Enums;

namespace Insurance.Domain.Entities.Proposta;

public class Proposta : BaseEntity
{
    public string Cliente { get; private set; }
    public decimal Valor { get; private set; }
    public StatusProposta Status { get; private set; } = StatusProposta.EmAnalise;

    public Proposta(string cliente, decimal valor)
    {
        Cliente = cliente;
        Valor = valor;
    }

    public void AlterarStatus(StatusProposta status)
    {
        Status = status;
    }
}
