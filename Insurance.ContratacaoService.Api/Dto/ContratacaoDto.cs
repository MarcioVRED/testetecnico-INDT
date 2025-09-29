using ContratacaoEntity = Insurance.Domain.Entities.Contratacao;

namespace Insurance.ContratacaoService.Api.Dto;

public record ContratacaoDto(Guid Id, Guid PropostaId, DateTime DataContratacao)
{
    public static ContratacaoDto FromDomain(ContratacaoEntity c) =>
        new ContratacaoDto(c.Id, c.PropostaId, c.DataContratacao);
}