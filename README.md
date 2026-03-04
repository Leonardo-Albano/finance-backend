# Finance Back-End API

### Leonardo Albano
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=flat&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/leonardoalbano132224/)
[![GitHub](https://img.shields.io/badge/GitHub-100000?style=flat&logo=github&logoColor=white)](https://github.com/Leonardo-Albano)

---

##  Sobre o Projeto
Esta é uma API robusta para controle financeiro pessoal, desenvolvida para demonstrar boas práticas de arquitetura e desenvolvimento em **.NET 8**. O sistema permite a gestão de pessoas, categorias e transações financeiras, aplicando regras de negócio rigorosas e garantindo a integridade dos dados.

## 🛠 Tecnologias Utilizadas
* **Runtime:** .NET 8
* **ORM:** Entity Framework Core
* **Banco de Dados:** SQLite (pela portabilidade e simplicidade em ambiente de desenvolvimento)
* **Testes:** xUnit
* **Documentação:** Swagger / OpenAPI

## 🏗 Decisões de Arquitetura e Engenharia
Foquei em padrões que garantem a escalabilidade e a fácil manutenção do código:

* **Separação de Camadas:** Implementação de lógica de negócio isolada em **Services**, mantendo as **Controllers** apenas para orquestração.
* **Imutabilidade com Records:** Uso de `public record` para DTOs, garantindo segurança na transferência de dados entre camadas.
* **Encapsulamento de Domínio:** As entidades possuem construtores protegidos e métodos de atualização controlados (`UpdateDetails`), impedindo que o objeto assuma estados inválidos.
* **Prevenção de Ciclos de Objeto:** Uso estratégico de `[JsonIgnore]` e **ResponseDTOs** para evitar referências circulares durante a serialização JSON, mantendo a resposta da API limpa e eficiente.
* **Global Exception Handling:** Implementação de um Middleware customizado para capturar e padronizar erros em toda a aplicação.

## Regras de Negócio Implementadas
1. **Validação de Menores:** Pessoas com idade inferior a 18 anos são impedidas pelo sistema de registrar transações do tipo **Receita (Income)**, sendo permitido apenas **Despesa (Expense)**.
2. **Limites de Integridade:** Validação De comprimento de strings (400 caracteres para Categorias e 200 para Pessoas).
3. **Relatório Consolidado:** Endpoint específico que realiza a agregação de dados no backend para retornar totais de entrada, saída e saldo por pessoa.

## 🚀 Como Executar

### Pré-requisitos
* .NET 8 SDK instalado.

### Passo a Passo
1. **Clonar o repositório:**
```
git clone [https://github.com/](https://github.com/)Leonardo-Albano/Finance_BackEnd.git
```

2. **Restaurar dependências:**

```
dotnet restore
```

3. **Executar Migrations:**
O banco SQLite será criado automaticamente na primeira execução, ou utilize:

```
dotnet ef database update
```

4. **Rodar a aplicação:**

```
dotnet run --project Finance_BackEnd
```

A documentação Swagger estará disponível em: http://localhost:5000/swagger

### Testes Unitários
A aplicação possui uma suíte de testes unitários cobrindo:

**Domínio**: Validação de regras nas entidades Person, Category e Transaction.

**Negócio**: Teste do TransactionService garantindo o bloqueio de receitas para menores.

**Relatórios**: Validação dos cálculos matemáticos de saldos e totais.

Para rodar os testes unitários:

```
dotnet test
```

Desenvolvido por Leonardo Albano