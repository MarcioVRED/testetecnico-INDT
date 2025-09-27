using Insurance.Domain.Entities;

namespace Insurance.Domain.Tests.Entities;

public class BaseEntityTests
{
    private class DummyEntity : BaseEntity { }

    [Fact]
    public void Construtor_Default_DeveInicializarPropriedades()
    {
        var entity = new DummyEntity();

        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.True(entity.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Construtor_ComParametros_DeveAtribuirValores()
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTime(2025, 1, 1);

        var entity = new DummyEntityWithConstructor(id, createdAt);

        Assert.Equal(id, entity.Id);
        Assert.Equal(createdAt, entity.CreatedAt);
    }

    [Fact]
    public void Propriedades_SaoProtegidas()
    {
        var entity = new DummyEntity();

        Assert.True(entity.Id != Guid.Empty);
        Assert.True(entity.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void CreatedAt_NaoPodeSerFuturo()
    {
        var entity = new DummyEntity();
        Assert.True(entity.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void EntidadeFilha_HerdaPropriedadesBase()
    {
        var derived = new DerivedEntity();
        Assert.NotEqual(Guid.Empty, derived.Id);
        Assert.True(derived.CreatedAt <= DateTime.UtcNow);
        Assert.Equal("Teste", derived.Name);
    }

    private class DerivedEntity : BaseEntity
    {
        public string Name { get; set; } = "Teste";
    }


    private class DummyEntityWithConstructor : BaseEntity
    {
        public DummyEntityWithConstructor(Guid id, DateTime createdAt) : base(id, createdAt) { }
    }

}
