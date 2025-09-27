namespace Insurance.Application.Events;

public record PropostaAprovadaEvent
{
    public Guid PropostaId { get; init; }
}
