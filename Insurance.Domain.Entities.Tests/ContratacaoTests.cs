using Insurance.Domain.Entities;

namespace Insurance.Domain.Tests.Entities;

public class ContratacaoTests
{
    [Fact]
    public void Construtor_DeveCriarContratacaoComValoresCorretos()
    {
        var propostaId = Guid.NewGuid();
        var contratacao = new Contratacao(propostaId);

        Assert.Equal(propostaId, contratacao.PropostaId);
        Assert.NotEqual(default, contratacao.DataContratacao);
        Assert.NotEqual(Guid.Empty, contratacao.Id);
        Assert.True(contratacao.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void ConstrutorPadrao_DeveCriarInstancia()
    {
        var contratacao = new Contratacao();

        Assert.NotNull(contratacao);
        Assert.Equal(Guid.Empty, contratacao.PropostaId); 
        Assert.Equal(default, contratacao.DataContratacao); 
        Assert.NotEqual(Guid.Empty, contratacao.Id); 
        Assert.True(contratacao.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Propriedades_SaoSomenteLeitura()
    {
        var propostaId = Guid.NewGuid();
        var contratacao = new Contratacao(propostaId);

        Assert.Equal(propostaId, contratacao.PropostaId);
        Assert.IsType<DateTime>(contratacao.DataContratacao);
    }

    [Fact]
    public void BaseEntity_PropriedadesDefinidasCorretamente()
    {
        var contratacao = new Contratacao(Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, contratacao.Id);
        Assert.True(contratacao.CreatedAt <= DateTime.UtcNow);
    }
}

