# Insurance System

Solução modular em .NET 8 com arquitetura hexagonal, composta por:

- **Insurance.PropostaService.Api** – API de gerenciamento de propostas  
- **Insurance.ContratacaoService.Api** – API de contratação de propostas aprovadas  
- **Domain** – Entidades e regras de negócio  
- **Application** – Serviços de aplicação, DTOs, eventos e mensageria  
- **Infra** – Repositórios e persistência  

Inclui **RabbitMQ** e **MassTransit** para eventos assíncronos e **SQL Server** para persistência.

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Docker e Docker Compose](https://www.docker.com/)  
- [Postman](https://www.postman.com/) (opcional, para testar endpoints)  

---

## Clone do repositório

```bash
git clone <URL_DO_REPOSITORIO>
cd testetecnico-INDT
```

## Execução com Docker
Build e execução de todos os containers

```bash
docker-compose up --build
```

Remover os Containers

```bash
docker-compose down -v
```


## Acesso às APIs e serviços

PropostaService API: http://localhost:5000/swagger
ContratacaoService API: http://localhost:5001/swagger

RabbitMQ Management: http://localhost:15672
- Usuário: guest
- Senha: guest

SQL Server:

Servidor: localhost:1433
- Usuário: sa
- Senha: SenhaForte12345"

As migrations podem ser aplicadas automaticamente na inicialização da API.


## Banco de Dados
Migrations com EF Core

1. Criar migrations (executar na pasta do projeto de API):

```bash
cd Insurance.Infra.Persistence
dotnet ef migrations add InitialCreate
```

2. Aplicar migrations:

```bash
dotnet ef database update
```

## Scripts SQL

Opcionalmente, todos os scripts SQL estão em:

- docs/database/scripts/scriptsInsurance.sql

Seguindo a ordem de versionamento.


## Testes

Executar todos os testes unitários:

```bash
dotnet test
```

- Cobertura acima de 80%

- Testes de Domain, Application, API e Consumer/Mensageria incluídos


## Postman Collection

Coleção Postman pronta em:

- docs/Insurance.postman_collection.json

- Para importar:

- - Abra o Postman → Import → selecione o arquivo .json

- - Ajuste a URL base (localhost:5000 ou 5001)

- - Execute os requests de teste


## Arquitetura e Fluxo

Domain – Regras de negócio, entidades, enums, validações

Application – Serviços, DTOs, eventos e mensageria (RabbitMQ)

Infra – Repositórios e persistência (SQL Server)

API – Controllers (Ports)

Eventos – PropostaAprovadaEvent propagado via RabbitMQ (Adapters)


## Comunicação entre APIs via Mensageria

A solução implementa comunicação assíncrona entre serviços utilizando RabbitMQ como message broker e MassTransit como abstração de mensageria.

Ao aprovar uma proposta na PropostaService API, um evento PropostaAprovadaEvent é publicado.

A ContratacaoService API escuta esse evento e, ao recebê-lo, inicia o processo de contratação da proposta automaticamente.

Essa arquitetura desacopla os serviços e garante escalabilidade e resiliência.



Diagrama completo disponível em:

- docs/diagrama-solucao.png

