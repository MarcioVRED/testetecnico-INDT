using System.ComponentModel.DataAnnotations;
using Insurance.Domain.Entities.Enums;

public record UpdateStatusPropostaDto
{
    [Required(ErrorMessage = "Status é obrigatório.")]
    [EnumDataType(typeof(StatusProposta), ErrorMessage = "Status inválido.")]
    public StatusProposta Status { get; init; }
}