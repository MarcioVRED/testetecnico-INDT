namespace Insurance.Application.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class PropostaNaoEncontradaException : Exception
{
    public PropostaNaoEncontradaException(Guid propostaId)
        : base($"Proposta com id {propostaId} não encontrada.") { }
}

public class PropostaNaoAprovadaException : Exception
{
    public PropostaNaoAprovadaException(Guid propostaId)
        : base($"Proposta com id {propostaId} não está aprovada.") { }
}

public class ContratacaoNaoEncontradaException : Exception
{
    public ContratacaoNaoEncontradaException(Guid id)
        : base($"Contratação com id {id} não encontrada.") { }
}