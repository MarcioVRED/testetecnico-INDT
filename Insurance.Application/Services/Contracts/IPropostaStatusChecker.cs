namespace Insurance.Contratacao.Application.Services.Contracts;

public interface IPropostaStatusChecker
{
    Task<bool> EhAprovadaAsync(Guid propostaId);
}
