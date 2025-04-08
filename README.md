# TesteMouts

## Visão Geral
Este repositório contém a implementação do backend do projeto DeveloperStore, utilizando .NET 8.0 e PostgreSQL. O projeto segue a arquitetura Clean Architecture e CQRS.

## Tecnologias Utilizadas

### Backend
- .NET 8.0
- C#
- MediatR (CQRS)
- Entity Framework Core
- AutoMapper
- FluentValidation
- Moq (Testes)
- xUnit (Testes Unitários)
- Rebus (Mensageria com RabbitMQ)
- Docker (Banco de dados PostgreSQL)
- Swagger (Documentação da API)

### Banco de Dados
- PostgreSQL via Docker
- MongoDB (futuro suporte para logs e eventos)

## Estrutura do Projeto
```
TesteMouts/
│── src/
│   ├── Ambev.DeveloperEvaluation.Application/  # Camada de aplicação (CQRS, Handlers)
│   ├── Ambev.DeveloperEvaluation.Common/       # Utilitários e serviços compartilhados
│   ├── Ambev.DeveloperEvaluation.Domain/       # Entidades e regras de negócio
│   ├── Ambev.DeveloperEvaluation.IoC/          # Configuração de Inversão de Controle
│   ├── Ambev.DeveloperEvaluation.ORM/          # Camada de persistência
│   ├── Ambev.DeveloperEvaluation.WebApi/       # API e Controllers
│── tests/
│   ├── Ambev.DeveloperEvaluation.Unit/         # Testes unitários
│   ├── Ambev.DeveloperEvaluation.Integration/  # Testes de integração
│   ├── Ambev.DeveloperEvaluation.Functional/   # Testes funcionais
│── docker-compose.yml  # Configuração do Docker
│── README.md
```

## Configuração do Ambiente

### Clonando o Repositório
```bash
git clone https://github.com/elanofb/TesteMouts.git
cd TesteMouts
```

### Configurando o Banco de Dados (PostgreSQL via Docker)
```bash
docker-compose up -d
```
Isso iniciará um container PostgreSQL configurado no `docker-compose.yml`.

### Restaurando Dependências
```bash
dotnet restore
```

### Criando o Banco de Dados e Aplicando Migrations
```bash
dotnet ef database update
```

## Regras de Negócio Implementadas

### Usuários (Users)
- Criar, obter e deletar usuários
- Validações de email, senha e telefone

### Vendas (Sales)
- Criar, obter e deletar vendas
- Validação de quantidade e descontos

### Itens da Venda (SaleItems)
- Descontos automáticos baseados na quantidade:
  - 4+ unidades: 10% de desconto
  - 10 a 20 unidades: 20% de desconto
  - Máximo de 20 unidades por produto

## Execução da Aplicação

### Rodar a Aplicação
```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

### Acessar a API via Swagger
```
http://localhost:8171/swagger
```

## Testes Unitários e de Integração

### Executar Testes Unitários
```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit
```

### Executar Testes de Integração
```bash
dotnet test tests/Ambev.DeveloperEvaluation.Integration
```

## Mensageria com Rebus (RabbitMQ)

### Configurar RabbitMQ no Docker
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### Registrar Rebus no Program.cs
```csharp
builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost", "sales_queue_mouts"))
    .Logging(l => l.Console()));
```

### Iniciar Rebus na Aplicação
```csharp
using (var scope = app.Services.CreateScope())
{
    var bus = scope.ServiceProvider.UseRebus();
}
```

## Deploy e CI/CD

### Criar e Publicar a Imagem Docker
```bash
docker build -t moutsambevelano-api .
docker run -p 5000:80 moutsambevelano-api
```

### CI/CD com GitHub Actions (Exemplo `ci.yml`)
```yaml
name: .NET Build & Test
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```

## Configuração Inicial

### Rodar o Projeto

Execute o seguinte comando para rodar o projeto:
```bash
dotnet run --project MoutsElanoApi
```

---

## Estrutura do Projeto

### Criando a Estrutura
1. Criar bibliotecas e domínios do projeto:
    ```bash
    dotnet new classlib -n Ambev.DeveloperEvaluation.Domain
    ```
2. Adicionar uma nova classe de migração EF:
    ```bash
    dotnet ef migrations add InitialCreate --project Ambev.DeveloperEvaluation.Infrastructure --startup-project Ambev.DeveloperEvaluation.API
    ```
3. Listar as migrações:
    ```bash
    dotnet ef migrations list --project Ambev.DeveloperEvaluation.Infrastructure --startup-project Ambev.DeveloperEvaluation.API
    ```
4. Atualizar o banco de dados:
    ```bash
    dotnet ef database update --project Ambev.DeveloperEvaluation.Infrastructure --startup-project Ambev.DeveloperEvaluation.API
    ```

---

## Testes

### Projetos de Teste
1. Criar projetos de testes:
    ```bash
    dotnet new xunit --name Ambev.DeveloperEvaluation.UnitTests
    dotnet new xunit --name Ambev.DeveloperEvaluation.IntegrationTests
    ```
2. Adicionar os projetos de teste à solução:
    ```bash
    dotnet sln add Ambev.DeveloperEvaluation.UnitTests/Ambev.DeveloperEvaluation.UnitTests.csproj
    dotnet sln add Ambev.DeveloperEvaluation.IntegrationTests/Ambev.DeveloperEvaluation.IntegrationTests.csproj
    ```
3. Referenciar bibliotecas nos testes:
    - UnitTests:
        ```bash
        dotnet add Ambev.DeveloperEvaluation.UnitTests reference Ambev.DeveloperEvaluation.Application
        dotnet add Ambev.DeveloperEvaluation.UnitTests reference Ambev.DeveloperEvaluation.Domain
        ```
    - IntegrationTests:
        ```bash
        dotnet add Ambev.DeveloperEvaluation.IntegrationTests reference Ambev.DeveloperEvaluation.API
        dotnet add Ambev.DeveloperEvaluation.IntegrationTests reference Ambev.DeveloperEvaluation.Infrastructure
        ```

---

## RabbitMQ

Adicionar dependências para suporte ao RabbitMQ:
```bash
dotnet add Ambev.DeveloperEvaluation.Infrastructure package RabbitMQ.Client
```
Para uma versão específica:
```bash
dotnet add Ambev.DeveloperEvaluation.Infrastructure package RabbitMQ.Client --version 6.5.0
```
Outros pacotes necessários:
```bash
dotnet add Ambev.DeveloperEvaluation.Infrastructure package Microsoft.Extensions.Hosting
```
Segue uma evidência da criação das Queues no Rabbit


---

## Configuração de Componentes

### API

Adicionar dependências e referências para o projeto API:
```bash
dotnet add Ambev.DeveloperEvaluation.API reference Ambev.DeveloperEvaluation.Domain
```
```bash
dotnet add Ambev.DeveloperEvaluation.API reference Ambev.DeveloperEvaluation.Application
```
```bash
dotnet add Ambev.DeveloperEvaluation.API reference Ambev.DeveloperEvaluation.CrossCutting
```
```bash
dotnet add Ambev.DeveloperEvaluation.API reference Ambev.DeveloperEvaluation.Infra
```
Dependências:
```bash
dotnet add Ambev.DeveloperEvaluation.API package Microsoft.AspNetCore.Mvc
```
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Infraestrutura

Adicionar pacotes e referências para infraestrutura:
```bash
dotnet add Ambev.DeveloperEvaluation.Infrastructure reference Ambev.DeveloperEvaluation.Domain
```
```bash
dotnet add Ambev.DeveloperEvaluation.Infrastructure package Microsoft.EntityFrameworkCore
```
```bash
dotnet add Ambev.DeveloperEvaluation.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
```
Atualizar banco de dados:
```bash
dotnet ef database update --project Ambev.DeveloperEvaluation.Infrastructure --startup-project MoutsElanoApi
```

### Application

Adicionar referência ao domínio:
```bash
dotnet add Ambev.DeveloperEvaluation.Application reference Ambev.DeveloperEvaluation.Domain
```

### CrossCutting

Adicionar referências e pacotes:
```bash
dotnet add Ambev.DeveloperEvaluation.CrossCutting reference Ambev.DeveloperEvaluation.Application
```
Pacotes Serilog:
```bash
dotnet add Ambev.DeveloperEvaluation.CrossCutting package Serilog
```
```bash
dotnet add Ambev.DeveloperEvaluation.CrossCutting package Serilog.Extensions.Logging
```
```bash
dotnet add Ambev.DeveloperEvaluation.CrossCutting package Serilog.Sinks.Console
```
```bash
dotnet add package Serilog.Sinks.File
```

---

### Testes Unitários

Adicionar pacotes úteis para testes unitários:
```bash
dotnet add Ambev.DeveloperEvaluation.UnitTests package FluentAssertions
```
```bash
dotnet add Ambev.DeveloperEvaluation.UnitTests package Bogus
```
```bash
dotnet add Ambev.DeveloperEvaluation.UnitTests package NSubstitute
```
Outros pacotes comuns:
```bash
dotnet add package Moq
```
```bash
dotnet add package xunit
```
```bash
dotnet add package xunit.runner.visualstudio
```

### Testes de Integração

Adicionar pacotes úteis para testes de integração:
```bash
dotnet add Ambev.DeveloperEvaluation.IntegrationTests package Testcontainers
```
```bash
dotnet add Ambev.DeveloperEvaluation.IntegrationTests package FluentAssertions
```
```bash
dotnet add Ambev.DeveloperEvaluation.IntegrationTests package Microsoft.AspNetCore.Mvc.Testing
```
```bash
dotnet add Ambev.DeveloperEvaluation.IntegrationTests package Microsoft.EntityFrameworkCore
```
```bash
dotnet add Ambev.DeveloperEvaluation.IntegrationTests package Microsoft.EntityFrameworkCore.InMemory
```

Testcontainers específicos:
```bash
dotnet add package Testcontainers.RabbitMq
```
```bash
dotnet add package Testcontainers.MsSql
```
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

---

## EF Core e Migrações

### Instalação do EF Core
```bash
dotnet tool install --global dotnet-ef
```
Verificar versão instalada:
```bash
dotnet ef --version
```
Adicionar pacote de design:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Criar e Atualizar Migrações
Criar uma nova migração:
```bash
dotnet ef migrations add InitialCreate
```
Atualizar o banco de dados:
```bash
dotnet ef database update
```

---

## Docker

Para verificar filas no RabbitMQ:
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
```
```bash
docker exec -it rabbitmq rabbitmqctl list_queues
```

## Conclusão
Este documento cobre todo o processo desde a instalação, configuração, execução, testes e deploy do projeto TesteMouts. Se houver dúvidas, consulte os arquivos-fonte ou documentações adicionais.

Contato: elanofb@gmail.com 
+55 (85) 98195.1011
