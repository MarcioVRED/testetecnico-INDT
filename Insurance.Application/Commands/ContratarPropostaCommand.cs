namespace Insurance.Application.Commands;

public record ContratarPropostaCommand(Guid PropostaId, DateTime RequestedAt)
{
    public ContratarPropostaCommand(Guid propostaId) : this(propostaId, DateTime.UtcNow) { }
}
