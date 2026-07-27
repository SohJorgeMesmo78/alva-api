# Alva API

Backend em **.NET 10 Web API** estruturado segundo os princípios da **Clean Architecture** e **DDD (Domain-Driven Design)**, responsável pelo processamento de regras de negócio, autenticação JWT e persistência de dados do sistema **Alva**.

---

## 🏛️ Arquitetura do Projeto

A solução está dividida em 4 camadas principais localizadas no diretório `src/`:

```text
alva-api/
├── alva-api.slnx            # Arquivo da Solução (.NET Solution)
└── src/
    ├── Domain/              # Entidades, Enums e Regras de Negócio fundamentais
    ├── Application/         # Casos de uso, DTOs, interfaces e serviços de aplicação
    ├── Infrastructure/      # Entity Framework Core, PostgreSQL (Npgsql) e Migrações
    └── WebApi/              # Controllers REST, middlewares, autenticação JWT e OpenAPI
```

---

## 🚀 Tecnologias Utilizadas

- **.NET 10** (C#)
- **ASP.NET Core Web API**
- **Entity Framework Core** com provedor **Npgsql** (PostgreSQL)
- **JWT (JSON Web Token)** para Autenticação e Autorização
- **OpenAPI / Swagger** para documentação interativa de endpoints

---

## ⚙️ Como Executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download) instalado.
- Instância do PostgreSQL rodando (ou string de conexão válida configurada).

### Passos para execução

1. Navegue até o diretório do projeto `alva-api`:
   ```bash
   cd alva-api
   ```

2. Restaure as dependências do projeto:
   ```bash
   dotnet restore
   ```

3. Execute o projeto informando o caminho do WebApi:
   ```bash
   dotnet run --project src/WebApi/WebApi.csproj
   ```

   *Ou entre no diretório da WebApi e execute diretamente:*
   ```bash
   cd src/WebApi
   dotnet run
   ```

4. A API estará acessível por padrão em `https://localhost:7198` ou `http://localhost:5249`.

---

## 🔐 Configuração (`appsettings.json`)

Certifique-se de ajustar a string de conexão do PostgreSQL e a chave secreta do JWT no arquivo `src/WebApi/appsettings.json` (ou `appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=alva_db;Username=postgres;Password=suasenha"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_SUPER_SEGURA_AQUI",
    "Issuer": "AlvaApi",
    "Audience": "AlvaPortal"
  }
}
```

---

## 📌 Módulos e Endpoints Principais

- **`/api/auth`**: Cadastro e login de usuários com geração de token JWT.
- **`/api/tasks`**: Gestão completa de tarefas (criação, listagem, atualização, exclusão e filtros por tipo).
- **`/api/settings`**: Gerenciamento de preferências do usuário.
