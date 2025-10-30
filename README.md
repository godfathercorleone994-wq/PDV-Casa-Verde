# PDV Casa Verde - Sistema de Ponto de Venda

Sistema PDV (Ponto de Venda) para boates e bares desenvolvido em C# com .NET 9.0 e SQLite.

## Funcionalidades

### 🎫 Sistema de Comandas
- Abertura de comandas com nome do cliente
- Lançamento de produtos por código numérico
- Visualização de comandas abertas e fechadas
- Fechamento de comandas com impressão de recibo

### 🍺 Cadastro de Produtos
- Cadastro de produtos com código numérico
- Preços e categorias
- Produtos pré-cadastrados:
  - **Código 2**: Cerveja - R$ 5,00
  - **Código 52**: Balde de Skol - R$ 25,00
  - **Código 50**: Comissão - R$ 50,00

### 💰 Sistema de Comissões
- Registro de comissões por garota/staff
- Vinculação de comissões às comandas
- Relatórios de comissões por staff
- Relatórios de comissões por comanda
- Total geral de comissões

### 🖨️ Sistema de Impressão POS
- Impressão de recibos formatados
- Visualização de detalhes da comanda
- Listagem de itens com preços

## Requisitos

- .NET 9.0 SDK ou superior
- SQLite (incluído automaticamente via Entity Framework Core)

## Instalação

1. Clone o repositório:
```bash
git clone https://github.com/godfathercorleone994-wq/PDV-Casa-Verde.git
cd PDV-Casa-Verde
```

2. Navegue até o diretório do projeto:
```bash
cd PDVCasaVerde
```

3. Restaure as dependências:
```bash
dotnet restore
```

4. Execute o projeto:
```bash
dotnet run
```

O banco de dados SQLite (`pdv.db`) será criado automaticamente na primeira execução.

## Como Usar

### Menu Principal

O sistema apresenta um menu interativo com as seguintes opções:

#### Comandas
1. **Abrir Nova Comanda** - Cria uma nova comanda com número sequencial
2. **Lançar Produto na Comanda** - Adiciona produtos usando código numérico
3. **Visualizar Comanda** - Exibe detalhes e recibo da comanda
4. **Fechar Comanda** - Encerra a comanda e imprime recibo final
5. **Listar Comandas Abertas** - Mostra todas as comandas em aberto

#### Produtos
6. **Cadastrar Produto** - Registra novos produtos no sistema
7. **Listar Produtos** - Exibe todos os produtos cadastrados

#### Comissões
8. **Lançar Comissão** - Registra comissão de staff em uma comanda
9. **Visualizar Comissões** - Relatórios de comissões (por staff, comanda ou total)

### Fluxo de Trabalho Típico

1. **Abrir uma comanda** (opção 1)
   - Informar nome do cliente
   - Anotar o número da comanda gerado

2. **Lançar produtos** (opção 2)
   - Informar número da comanda
   - Informar código do produto (ex: 2 para cerveja)
   - Informar quantidade

3. **Adicionar comissões** (opção 8) se necessário
   - Informar número da comanda
   - Informar nome da garota
   - Informar valor da comissão

4. **Fechar comanda** (opção 4)
   - Informar número da comanda
   - O sistema exibe o recibo final

## Estrutura do Projeto

```
PDVCasaVerde/
├── Data/
│   └── PDVContext.cs          # Contexto do banco de dados
├── Models/
│   ├── Product.cs             # Modelo de Produto
│   ├── Command.cs             # Modelo de Comanda
│   ├── CommandItem.cs         # Modelo de Item da Comanda
│   └── Commission.cs          # Modelo de Comissão
├── Services/
│   ├── ProductService.cs      # Serviço de produtos
│   ├── CommandService.cs      # Serviço de comandas
│   ├── CommissionService.cs   # Serviço de comissões
│   └── PrintService.cs        # Serviço de impressão
└── Program.cs                 # Ponto de entrada e menu principal
```

## Tecnologias Utilizadas

- **C# 11** com .NET 9.0
- **Entity Framework Core 9.0** - ORM para acesso ao banco
- **SQLite** - Banco de dados embutido
- **Microsoft.EntityFrameworkCore.Sqlite** - Provider SQLite para EF Core
- **Microsoft.EntityFrameworkCore.Design** - Ferramentas de design do EF Core

## Banco de Dados

O sistema utiliza SQLite com as seguintes tabelas:

- **Products** - Produtos disponíveis
- **Commands** - Comandas (abertas e fechadas)
- **CommandItems** - Itens das comandas
- **Commissions** - Comissões registradas

O banco é criado automaticamente com dados iniciais (cerveja, balde de Skol e comissão).

## Licença

Este projeto é de código aberto e está disponível para uso livre.