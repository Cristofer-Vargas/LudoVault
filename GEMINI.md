# LudoVault - Project Context

Este arquivo serve como guia de referência central para o desenvolvimento do projeto LudoVault.
A inteligência artificial deve consultar este arquivo para entender a arquitetura, regras de negócio e convenções do projeto.

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
- Validações de entrada centralizadas na pasta `Validations`, garantindo que os dados estajam corretos antes de chegarem à camada de serviço.

### 5. Configuração Modular
- Extensões de `IServiceCollection` localizadas na pasta `Configurations` para manter o `Program.cs` limpo e organizado.

### 6. Tratamento de Erros e Logs
- Uso de `ILogger` com Serilog para registrar eventos críticos e erros, especialmente na camada de repositório.

## 🗄️ Modelagem de Dados e Entity Framework (Regras de Ouro)

O projeto possui regras estritas para a modelagem de dados que **devem ser sempre seguidas**:

- **Modelos POCO (Plain Old CLR Objects):** As classes na pasta `Model/` são 100% limpas. **Não utilizamos Data Annotations** (`[Table]`, `[Key]`, etc.). O Entity Framework identifica chaves primárias pela convenção da propriedade `Id`.
- **Fluent API:** Toda a configuração de tabelas, colunas, chaves e restrições de banco de dados é feita exclusivamente nos arquivos da pasta `Mappings/` (ex: `GameMap.cs`).
- **Mapeamento de Relacionamentos (One-Sided):** Relacionamentos devem ser mapeados em **apenas um lado** (geralmente na entidade dependente ou tabela de junção) para evitar conflitos de compilação ou redundâncias.
- **Tabelas de Associação (N:N):** Relacionamentos de muitos-para-muitos (ex: `Game` x `Platform`, `User` x `Game`) usam tabelas de junção explícitas (ex: `GamePlatformModel`, `UserLibraryModel`, `UserListItemModel`). O mapeamento é feito usando `HasOne(...).WithMany(...)` partindo da tabela de junção, utilizando instâncias únicas obrigatórias ou anuláveis (ex: `PlatformModel? Platform`) em vez de propriedades de Lista (`List<>`).
- **DeleteBehavior:**
  - **Cascade:** Usado em relações de dependência forte ou associativas (ex: Deletar uma `UserList` apaga os `UserListItem` dela. Deletar um `Game` apaga seus `GamePlatform`).
  - **Restrict:** **Obrigatório** para proteger entidades do Catálogo Master. Não se deve permitir a exclusão de um `Game`, `Publisher` ou `Platform` se eles estiverem vinculados à biblioteca de um usuário ou a outras listas.
- **Propriedades de Navegação:** **NUNCA utilize o modificador `required`** nas propriedades de navegação dos Models (ex: `public required GameModel Game`). Isso gera erros de compilação (CS9035) ao tentar instanciar a entidade durante os mapeamentos ou seed. Utilize sempre o tipo anulável (ex: `public GameModel? Game { get; set; }`).
- **Data Inicial (Seed) Constante:** **NUNCA utilize dados dinâmicos como `DateTime.Now` ou `DateTime.UtcNow`** dentro de métodos `.HasData()` nas classes de `Mapping`. O Entity Framework identifica essa mudança a cada run e cria Migrations de snapshot desnecessárias e pendentes. Utilize sempre datas fixas (ex: `new DateTime(2024, 1, 1)`).
- **Injeção Automática de Maps:** No arquivo de contexto (`MysqlContext.cs`), sempre utilize o método `modelBuilder.ApplyConfigurationsFromAssembly(typeof(MysqlContext).Assembly);` dentro do `OnModelCreating`. Isso garante que todos os arquivos `.Map.cs` do projeto sejam registrados automaticamente.

## 📂 Estrutura de Pastas Principal

- `Controllers/`: Endpoints da API.
- `Services/`: Lógica de negócio e interfaces.
- `Repositories/`: Acesso a dados e interfaces.
- `Model/`: Entidades do banco de dados (Livres de anotações).
- `Mappings/`: Configurações do Entity Framework Core via Fluent API.
- `DTO/`: Objetos de transferência de dados (Requests/Responses).
- `dbContext/`: Contexto do Entity Framework (`MysqlContext`).
- `Validations/`: Regras de validação do FluentValidation.
- `Configurations/`: Classes de configuração do sistema.
- `wwwroot/`: Arquivos estáticos e uploads.

## 📝 Convenções de Código

- **Nomenclatura:** PascalCase para classes e métodos, camelCase para variáveis locais e campos privados (com `_`).
- **Async/Await:** Uso extensivo de programação assíncrona para operações de I/O.
- **Injeção de Dependência:** Utilizada via construtor (inclusive Primary Constructors do C# 12+).
