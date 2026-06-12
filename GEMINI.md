# LudoVault - Project Context

Este arquivo serve como guia de referência central para o desenvolvimento do projeto LudoVault.
A inteligência artificial deve consultar este arquivo para entender a arquitetura, regras de negócio e convenções do projeto.

---

## 🚀 Tecnologias e Frameworks

- **Plataforma:** .NET 10.0 (Web API ASP.NET Core)
- **Banco de Dados:** MySQL (Pomelo.EntityFrameworkCore.MySql)
- **ORM:** Entity Framework Core 9.0
- **Validação:** FluentValidation
- **Logging:** Serilog (AspNetCore, Sinks.Console)
- **Processamento de Imagem:** SixLabors.ImageSharp
- **Criptografia:** BCrypt.Net-Core

---

## 🏗️ Padrões de Projeto e Arquitetura

O projeto segue a **Clean Architecture (Arquitetura Limpa)** orientada a domínios, com a seguinte regra de dependência: *as dependências apontam apenas para dentro. O núcleo de negócio não depende de detalhes técnicos externos.*

```
[ LudoVault.Api ] ───> [ LudoVault.Application ] ───> [ LudoVault.Domain ]
       │                                                     ▲
       └───────────────────> [ LudoVault.Infra ] ────────────┘
```

### Divisão de Camadas e Responsabilidades:

1. **`LudoVault.Domain`** (O Núcleo/Core):
   - **Responsabilidade:** Contém entidades puras de negócio e os contratos de repositório.
   - **Regra:** **Zero dependências** de outros projetos da solução e sem dependência de bibliotecas web/banco de dados.

2. **`LudoVault.Application`** (Lógica de Negócio e Casos de Uso):
   - **Responsabilidade:** Contém DTOs (Request/Response), orquestradores de casos de uso (Serviços e suas interfaces), mapeadores de entidades e regras de validação.
   - **Regra:** Depende exclusivamente de `LudoVault.Domain`.

3. **`LudoVault.Infra`** (Infraestrutura e Acesso a Dados):
   - **Responsabilidade:** Implementações de acesso a banco de dados (DbContext, EF Mappings, Repositórios concretos), Migrations e serviços integrados com bibliotecas de terceiros (ex: salvamento físico de imagens com ImageSharp e criptografia com BCrypt).
   - **Regra:** Depende de `LudoVault.Domain` e `LudoVault.Application`.

4. **`LudoVault.Api`** (Ponto de Entrada / Apresentação Web):
   - **Responsabilidade:** Endpoints HTTP (Controllers), middlewares, políticas de CORS, arquivos de configuração (`appsettings.json`) e a composição da injeção de dependência (`Program.cs`).
   - **Regra:** Depende de `LudoVault.Application` e `LudoVault.Infra`.

---

## 🗄️ Modelagem de Dados e Entity Framework (Regras de Ouro)

O projeto possui regras estritas para a modelagem de dados que **devem ser sempre seguidas**:

- **Modelos POCO (Plain Old CLR Objects):** As classes na pasta `Model/` do Domain são 100% limpas. **Não utilizamos Data Annotations** (`[Table]`, `[Key]`, etc.). O Entity Framework identifica chaves primárias pela convenção da propriedade `Id`.
- **Fluent API:** Toda a configuração de tabelas, colunas, chaves e restrições de banco de dados é feita exclusivamente nos arquivos da pasta `Mappings/` do projeto `LudoVault.Infra` (ex: `GameMap.cs`).
- **Mapeamento de Relacionamentos (One-Sided):** Relacionamentos devem ser mapeados em **apenas um lado** (geralmente na entidade dependente ou tabela de junção) para evitar conflitos de compilação ou redundâncias.
- **Tabelas de Associação (N:N):** Relacionamentos de muitos-para-muitos (ex: `Game` x `Platform`, `User` x `Game`) usam tabelas de junção explícitas (ex: `GamePlatformModel`, `UserLibraryModel`, `UserListItemModel`). O mapeamento é feito usando `HasOne(...).WithMany(...)` partindo da tabela de junção, utilizando instâncias únicas obrigatórias ou anuláveis (ex: `PlatformModel? Platform`) em vez de propriedades de Lista (`List<>`).
- **DeleteBehavior:**
  - **Cascade:** Usado em relações de dependência forte ou associativas (ex: Deletar uma `UserList` apaga os `UserListItem` dela. Deletar um `Game` apaga seus `GamePlatform`).
  - **Restrict:** **Obrigatório** para proteger entidades do Catálogo Master. Não se deve permitir a exclusão de um `Game`, `Publisher` ou `Platform` se eles estiverem vinculados à biblioteca de um usuário ou a outras listas.
- **Propriedades de Navegação:** **NUNCA utilize o modificador `required`** nas propriedades de navegação dos Models (ex: `public required GameModel Game`). Isso gera erros de compilação (CS9035) ao tentar instanciar a entidade durante os mapeamentos ou seed. Utilize sempre o tipo anulável (ex: `public GameModel? Game { get; set; }`).
- **Data Inicial (Seed) Constante:** **NUNCA utilize dados dinâmicos como `DateTime.Now` ou `DateTime.UtcNow`** dentro de métodos `.HasData()` nas classes de `Mapping`. O Entity Framework identifica essa mudança a cada run e cria Migrations de snapshot desnecessárias e pendentes. Utilize sempre datas fixas (ex: `new DateTime(2024, 1, 1)`).
- **Injeção Automática de Maps:** No arquivo de contexto (`MysqlContext.cs`), sempre utilize o método `modelBuilder.ApplyConfigurationsFromAssembly(typeof(MysqlContext).Assembly);` dentro do `OnModelCreating`. Isso garante que todos os arquivos `.Map.cs` do projeto sejam registrados automaticamente.
- **Gerenciamento de Estado (Identity Map):** Ao atualizar entidades em Repositórios, atente-se aos conflitos de rastreamento. Desanexe entidades previamente rastreadas (via `AsNoTracking()` na leitura ou alterando `State = EntityState.Detached`) para evitar que o Entity Framework lance exceções de colisão de chaves idênticas em memória ao atualizar ou salvar uma nova instância.

---

## 📂 Estrutura de Pastas e Projetos

### 1. `LudoVault.Domain`
- `Interfaces/Repositories/`: Contratos dos repositórios (ex: `IUserRepository`).
- `Model/`: Entidades de domínio livres de anotações (ex: `UserModel`).

### 2. `LudoVault.Application`
- `Configurations/`: Configurações de DTOs e opções de imagem (ex: `DefaultImagesOptions`).
- `DTO/`:
  - `Requests/`: Classes de entrada de dados da API.
  - `Responses/`: Classes de saída de dados da API.
- `Interfaces/Services/`: Contratos dos serviços de aplicação (ex: `IUserServices`).
- `Services/`: Implementação da lógica de negócio (ex: `UserServices`).
  - `Mapper/`: Classes estáticas de mapeamento manual DTO <-> Model.
- `Validations/`: Validadores das requests usando FluentValidation (ex: `UserValidation`).

### 3. `LudoVault.Infra`
- `dbContext/`: Contexto do Entity Framework (`MysqlContext`).
- `Mappings/`: Mapeamentos via Fluent API.
- `Migrations/`: Arquivos de migrations de banco gerados.
- `Repositories/`: Implementação concreta de acesso a dados (ex: `UserRepository`).
- `Services/`: Serviços técnicos baseados em libs externas (ex: `ImageServices` com ImageSharp e `SecurityServices` com BCrypt).

### 4. `LudoVault.Api`
- `Controllers/`: Endpoints REST que chamam os serviços (ex: `UserController`).
- `Configurations/`: Registros de DI unificados da solução (ex: `ServicesAndRepositoriesConfiguration.cs`) e CORS.
- `Properties/`: Perfis de depuração e variáveis de ambiente local (ex: `launchSettings.json`).
- `wwwroot/`: Diretório de arquivos estáticos servidos pela API e uploads de mídia.
- `appsettings.json` e `appsettings.Development.json`: Arquivos de configuração de runtime.
- `Program.cs`: Arquivo de inicialização e composição dos serviços.

---

## 📝 Convenções de Código

- **Nomenclatura:** PascalCase para classes e métodos, camelCase para variáveis locais e campos privados (com `_`).
- **Async/Await:** Uso extensivo de programação assíncrona para operações de I/O.
- **Injeção de Dependência:** Utilizada via construtor (inclusive Primary Constructors do C# 12+).
- **Status HTTP e Retornos da API:** Siga rigorosamente os padrões HTTP para a estrutura de respostas gerenciada pela classe base `GetHttpResponseFromReports.cs`. Quando necessitar retornar informações como dados ou mensagens, aplique sempre o status HTTP `200 OK` nas modificações ou exclusões. O status HTTP `204 No Content` deve ser usado **apenas e estritamente** quando nenhum corpo (body) for devolvido ao frontend. Para recursos criados, mantenha o `201 Created`.
