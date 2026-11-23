# Sistema de Venda de Apps

Projeto desenvolvido para a disciplina de **Banco de Dados** do curso de Engenharia de Computação.
O sistema simula uma loja de aplicativos, com uma API em .NET conectada a um banco de dados **Oracle** e um frontend web.

## Tecnologias

- **Backend:** C# / ASP.NET Core (.NET 10) – Web API
- **Banco de dados:** Oracle (Oracle.ManagedDataAccess)
- **Documentação da API:** Swagger
- **Frontend:** HTML, CSS e JavaScript

## Arquitetura

O backend foi organizado em camadas, separando as responsabilidades:

```
backend/src/
├── SistemaVendas.Api             # Endpoints da API e configuração
├── SistemaVendas.Application     # Regras de negócio (serviços)
├── SistemaVendas.Domain          # Entidades e interfaces
└── SistemaVendas.Infrastructure  # Acesso ao banco Oracle (repositórios)
