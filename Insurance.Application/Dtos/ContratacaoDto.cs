﻿using ContratacaoEntity = Insurance.Domain.Entities.Contratacao;

namespace Insurance.Application.Dtos;

public record ContratacaoDto(Guid Id, Guid PropostaId, DateTime ContratadoEm, DateTime CreatedAt)
{
    public static ContratacaoDto FromDomain(ContratacaoEntity c) =>
        new ContratacaoDto(c.Id, c.PropostaId, c.DataContratacao, c.CreatedAt);
}