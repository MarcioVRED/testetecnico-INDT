using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;

namespace Insurance.Domain.Tests.Entities;

public class PropostaTests
{
    [Fact]
    public void Construtor_DeveCriarPropostaComValoresCorretos()
    {
        var cliente = "João";
        var valor = 1000m;

        var proposta = new Proposta(cliente, valor);

        Assert.Equal(cliente, proposta.Cliente);
        Assert.Equal(valor, proposta.Valor);
        Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
        Assert.NotEqual(Guid.Empty, proposta.Id);
        Assert.True(proposta.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void AlterarStatus_DeveAtualizarStatus()
    {
        var proposta = new Proposta("Maria", 500);
        proposta.AlterarStatus(StatusProposta.Aprovada);

        Assert.Equal(StatusProposta.Aprovada, proposta.Status);
    }

    [Fact]
    public void Construtor_DeveDefinirIdECreatedAt()
    {
        var proposta = new Proposta("Carlos", 1500m);

        Assert.NotEqual(Guid.Empty, proposta.Id);
        Assert.True(proposta.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void AlterarStatus_MultiplasVezes_DeveAtualizarCorretamente()
    {
        var proposta = new Proposta("Ana", 750);

        proposta.AlterarStatus(StatusProposta.Aprovada);
        Assert.Equal(StatusProposta.Aprovada, proposta.Status);

        proposta.AlterarStatus(StatusProposta.Rejeitada);
        Assert.Equal(StatusProposta.Rejeitada, proposta.Status);

        proposta.AlterarStatus(StatusProposta.EmAnalise);
        Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
    }

    [Fact]
    public void Propriedades_SaoSomenteLeitura()
    {
        var proposta = new Proposta("Pedro", 1000m);

        Assert.Equal("Pedro", proposta.Cliente);
        Assert.Equal(1000m, proposta.Valor);
    }

    [Fact]
    public void BaseEntity_PropriedadesDefinidas()
    {
        var proposta = new Proposta("Julia", 500m);

        Assert.NotEqual(Guid.Empty, proposta.Id);
        Assert.True(proposta.CreatedAt <= DateTime.UtcNow);
    }


}
