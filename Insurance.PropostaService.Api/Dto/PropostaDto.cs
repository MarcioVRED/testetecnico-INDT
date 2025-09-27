using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;

namespace Insurance.PropostaService.Api.Dto
{
    public record PropostaDto(Guid Id, string Cliente, decimal Valor, StatusProposta Status, DateTime CreatedAt)
    {
        public static PropostaDto FromDomain(Proposta p) =>
            new PropostaDto(p.Id, p.Cliente, p.Valor, p.Status, p.CreatedAt);
    }
}