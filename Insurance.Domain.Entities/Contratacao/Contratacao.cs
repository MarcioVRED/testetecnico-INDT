namespace Insurance.Domain.Entities.Contratacao;

public class Contratacao : BaseEntity
{
    public Guid PropostaId { get; private set; }
    public DateTime DataContratacao { get; private set; }

    public Contratacao(Guid propostaId)
    {
        PropostaId = propostaId;
        DataContratacao = DateTime.UtcNow;
    }

    public Contratacao()
    {
    }
}
