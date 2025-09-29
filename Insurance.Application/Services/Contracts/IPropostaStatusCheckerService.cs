namespace Insurance.Contratacao.Application.Services.Contracts;

public interface IPropostaStatusCheckerService
{
    Task<bool> EhAprovadaAsync(Guid propostaId);
}
