# LudoVault - Project Context

Este arquivo serve como guia de referência para o desenvolvimento do projeto LudoVault.

## 🚀 Tecnologias e Frameworks

- **Plataforma:** .NET 10.0 (Web API ASP.NET Core)
- **Banco de Dados:** MySQL (Pomelo.EntityFrameworkCore.MySql)
- **ORM:** Entity Framework Core 9.0
- **Validação:** FluentValidation
- **Logging:** Serilog (AspNetCore, Sinks.Console)
- **Processamento de Imagem:** SixLabors.ImageSharp
- **Criptografia:** BCrypt.Net-Core

## 🏗️ Padrões de Projeto e Arquitetura

O projeto segue uma estrutura baseada em camadas e padrões de design comuns em ecossistemas .NET modernos:

### 1. Repository & Service Pattern
- **Repositories:** Camada responsável pela persistência e consulta de dados. Utiliza injeção de dependência e lida com transações explícitas no banco de dados.
- **Services:** Camada de lógica de negócio. Orquestra chamadas aos repositórios, validações e transformações de dados.
- **Interfaces:** Todos os serviços e repositórios possuem interfaces para facilitar o desacoplamento e testes.

### 2. DTO (Data Transfer Objects)
- Separados em `Requests` (entrada) e `Responses` (saída) para proteger as entidades de domínio (`Model`).

### 3. Manual Mapping
- Em vez de AutoMapper, o projeto utiliza **Mapeadores Manuais** (classes estáticas na pasta `Services/Mapper`) para converter entre Models e DTOs. Isso garante maior controle e performance.

### 4. FluentValidation
- Validações de entrada centralizadas na pasta `Validations`, garantindo que os dados estejam corretos antes de chegarem à camada de serviço.

### 5. Configuração Modular
- Extensões de `IServiceCollection` localizadas na pasta `Configurations` para manter o `Program.cs` limpo e organizado.

### 6. Tratamento de Erros e Logs
- Uso de `ILogger` com Serilog para registrar eventos críticos e erros, especialmente na camada de repositório.

## 📂 Estrutura de Pastas Principal

- `Controllers/`: Endpoints da API.
- `Services/`: Lógica de negócio e interfaces.
- `Repositories/`: Acesso a dados e interfaces.
- `Model/`: Entidades do banco de dados.
- `DTO/`: Objetos de transferência de dados (Requests/Responses).
- `Data/`: Contexto do Entity Framework (`MysqlContext`).
- `Validations/`: Regras de validação do FluentValidation.
- `Configurations/`: Classes de configuração do sistema.
- `wwwroot/`: Arquivos estáticos e uploads.

## 📝 Convenções de Código

- **Nomenclatura:** PascalCase para classes e métodos, camelCase para variáveis locais e campos privados (com `_`).
- **Async/Await:** Uso extensivo de programação assíncrona para operações de I/O.
- **Injeção de Dependência:** Utilizada via construtor (inclusive Primary Constructors do C# 12+).
