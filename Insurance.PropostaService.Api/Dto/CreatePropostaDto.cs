using System.ComponentModel.DataAnnotations;

namespace Insurance.PropostaService.Api.Dto
{
    public record CreatePropostaDto
    {
        [Required(ErrorMessage = "Cliente é obrigatório")]
        public string Cliente { get; init; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; init; }
    }
}