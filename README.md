# 🎮 LudoVault API

**LudoVault** é um projeto pessoal, independente e sem fins lucrativos, desenvolvido com foco acadêmico, estudo contínuo e aprimoramento de práticas de mercado no ecossistema .NET. 

A aplicação tem como propósito ser o "cofre" (vault) de jogos do usuário. Uma plataforma onde os amantes de videogames podem registrar seus interesses, gerenciar bibliotecas de jogos salvos de qualquer plataforma, ano e gênero, criar listas nostálgicas da infância ou acompanhar os lançamentos mais modernos.

---

## 🎯 Objetivo do Projeto

- **Estudo Contínuo:** Aplicação prática de tecnologias modernas e padrões de arquitetura corporativos.
- **Simulação de Mercado:** Experiência com fluxo de desenvolvimento ágil, levantamento de requisitos (via Slack) e implementação de *features* reais.
- **Portfólio e Evolução:** Construir uma base sólida que evoluirá de uma simples API Restful para um ecossistema escalável.

---

## 🚀 Tecnologias e Arquitetura

O projeto utiliza o que há de mais moderno no ecossistema .NET, prezando por performance, manutenibilidade e baixo acoplamento:

- **Plataforma:** .NET 10.0 (ASP.NET Core Web API)
- **Banco de Dados:** MySQL (via Pomelo.EntityFrameworkCore.MySql)
- **ORM:** Entity Framework Core 9.0
- **Segurança e Criptografia:** BCrypt.Net-Core e gestão de variáveis de ambiente com DotNetEnv
- **Validação:** FluentValidation (regras de validação de DTOs isoladas e centralizadas)
- **Logs:** Serilog (AspNetCore e Console Sinks)
- **Processamento de Imagens:** SixLabors.ImageSharp
- **Arquitetura & Padrões:** 
  - MVC (Model-View-Controller) para o roteamento de Endpoints
  - Camadas de *Services* (regras de negócio) e *Repositories* (persistência e transações DB)
  - *DTOs* (Data Transfer Objects) separados entre Requisição e Resposta
  - Mapeamento Manual de entidades (sem o uso de AutoMapper) para prover um controle rigoroso de performance e conversão

---

## 🛠️ Ferramentas Utilizadas
- **Modelagem de Dados:** [dbdiagram.io](https://dbdiagram.io/d/LudoVault-69d022270f7c9ef2c077381c)
- **Gestão de Projeto:** Slack (organização de *features* e alinhamento de objetivos reais de projeto)
- **Infraestrutura:** Docker e Docker Compose
- **Testes de API e DB:** Postman e MySQL Workbench

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) (para levantar o container do banco de dados localmente)

### Passos
1. Clone o repositório para o seu ambiente local.
2. Na raiz do projeto, garanta que o arquivo `.env` possua as chaves corretas sem espaços extras, baseado nas variáveis do `docker-compose.yml` (ex: `MySqlConnection`, `MYSQL_ROOT_PASSWORD`, `DATABASE_NAME`).
3. Suba o banco de dados MySQL via Docker Compose:
   ```bash
   docker-compose up -d
   ```
4. Restaure as dependências e rode a API via CLI ou usando a sua IDE preferida (ex: Visual Studio):
   ```bash
   dotnet restore
   dotnet run
   ```

---

## 🗺️ Roadmap e Futuro do Projeto

O LudoVault está em constante evolução. As seções abaixo ditam as intenções futuras e os conhecimentos arquiteturais que serão injetados gradativamente no sistema.

### 📌 Versão 2.0 (Em andamento)
- Padronização de respostas utilizando o **Result Pattern**.
- Automação e execução de **Migrations** do banco de dados (EF Core).
- Cobertura de código com **Testes Automatizados** (Unitários e de Integração).
- Expansão dos registros de **Logs** estruturados.

### 🔮 Versões Futuras
- **API Avançada:** Implementar Custom Serialization, Content Negotiation, Paginação e o Padrão HATEOAS.
- **Documentação Automática:** Integração com Swagger, OpenAPI e Scalar.
- **Recursos Nativos:** Sistema robusto para envio de arquivos (Upload de Imagens/Arquivos) e disparos de Emails (via MailKit/Gmail).
- **Segurança:** Autenticação e Autorização utilizando tokens JWT.
- **DevOps:** Dockerizar completamente a aplicação .NET, orquestrar fluxos de CI/CD com GitHub Actions e possivelmente explorar a implantação em nuvem (Cloud / Kubernetes).

### 💻 Front-End (SPA)
Em paralelo ao desenvolvimento desta API, está sendo projetada uma aplicação web utilizando **React.js**.  
O objetivo é que esse Front-End atue em conjunto com o Back-End para promover uma experiência rica de usuário e facilitar testes e interações em massa com os *endpoints*. Embora sem uma data de conclusão estipulada, o desejo imediato é que o sistema receba uma interface totalmente dedicada, escalando a API de um ambiente de desenvolvimento puro (Postman) para um sistema completo e acessível na web.

---

## 💡 Inspiração do Projeto

> *"Como amante de jogos e entusiasta em tecnologia, me inspirei na plataforma já existente usada pela [Twitch](https://www.twitch.tv/), a [IGDB](https://www.igdb.com/). A IGDB é uma API gratuita que fornece uma biblioteca vasta de jogos disponíveis no mercado, com informações de desenvolvedoras, datas de lançamentos e atualizações, imagens, e muitas outras informações relacionadas. É a principal API do mercado com uma variedade tão grande e precisa de catálogo, indo desde os jogos mais antigos até os lançamentos recentes."*

> *"Inspirado nesse sistema, venho desenvolvendo o LudoVault com o intuito de permitir que os usuários possam registrar seus interesses e ter um local na internet onde possam salvar seus jogos de forma organizada. Uma plataforma reunindo jogos a partir de qualquer sistema, ano e gênero — permitindo que o usuário crie desde listas nostálgicas que memorizam sua infância, até playlists de jogos modernos nos quais admiramos cada detalhe tecnológico implementado."* <br>
> **— Cristofer Vargas**