# PDV Casa Verde - Sistema de Ponto de Venda

Sistema PDV (Ponto de Venda) para boates e bares desenvolvido em C# com .NET 9.0, SQLite e API REST.

## 🚀 Funcionalidades

### 📊 API REST Completa
- **Produtos**: CRUD completo com vinculação a Grupos e Subgrupos
- **Grupos e Subgrupos**: Organização hierárquica de produtos
- **Clientes**: Gestão de clientes com sistema de caderneta (débito/crédito)
- **Vendas/Mesas**: Sistema de vendas vinculadas a mesas ou clientes
- **Caderneta**: Sistema de fiado com controle de débitos e pagamentos

### 🍺 Cadastro de Produtos
- Produtos com código numérico, nome, preço e categoria
- Vinculação a Grupos e Subgrupos
- Controle de estoque
- Produtos pré-cadastrados:
  - **Código 2**: Cerveja - R$ 5,00
  - **Código 52**: Balde de Skol - R$ 25,00
  - **Código 50**: Comissão - R$ 50,00

### 🏪 Sistema de Mesas (F4)
- Abertura de vendas vinculadas a mesas
- Lançamento de produtos por código
- Visualização de vendas abertas por mesa
- Fechamento com diferentes formas de pagamento

### 💳 Sistema de Caderneta (F5)
- Vendas vinculadas a clientes
- Controle de débitos automático
- Adicionar crédito (pagamento de conta)
- Histórico completo de transações

### 💰 Sistema de Comissões
- Registro de comissões por garota/staff
- Vinculação de comissões às comandas
- Relatórios de comissões

## 📋 Requisitos

- .NET 9.0 SDK ou superior
- SQLite (incluído automaticamente via Entity Framework Core)

## 🔧 Instalação

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

A API estará disponível em `http://localhost:5000`

## 📖 Documentação da API

Consulte [API_DOCUMENTATION.md](API_DOCUMENTATION.md) para documentação completa dos endpoints.

### Endpoints Principais

#### Produtos
- `GET /api/products` - Lista produtos
- `POST /api/products` - Cria produto
- `PUT /api/products/{id}` - Atualiza produto (incluindo grupo/subgrupo)
- `DELETE /api/products/{id}` - Desativa produto

#### Grupos e Subgrupos
- `GET /api/groups` - Lista grupos
- `POST /api/groups` - Cria grupo
- `GET /api/subgroups` - Lista subgrupos
- `POST /api/subgroups` - Cria subgrupo

#### Clientes
- `GET /api/customers` - Lista clientes
- `POST /api/customers` - Cria cliente
- `POST /api/customers/{id}/credit` - **Adiciona crédito (pagar conta)**
- `GET /api/customers/{id}/ledger` - Histórico da caderneta

#### Vendas/Mesas
- `POST /api/sales` - Cria venda (mesa ou cliente)
- `GET /api/sales/table/{tableNumber}` - **Vendas abertas de uma mesa**
- `POST /api/sales/{id}/items` - Adiciona produtos
- `POST /api/sales/{id}/close` - **Fecha venda com tipo de pagamento**

## 🎯 Casos de Uso

### Mesa (F4) - Venda em Aberto
```bash
# 1. Abrir mesa 1
curl -X POST http://localhost:5000/api/sales \
  -H "Content-Type: application/json" \
  -d '{"tableNumber":1}'

# 2. Adicionar 3 cervejas
curl -X POST http://localhost:5000/api/sales/1/items \
  -H "Content-Type: application/json" \
  -d '{"productCode":2,"quantity":3}'

# 3. Consultar mesa
curl http://localhost:5000/api/sales/table/1

# 4. Fechar mesa com dinheiro
curl -X POST http://localhost:5000/api/sales/1/close \
  -H "Content-Type: application/json" \
  -d '{"paymentType":"CASH"}'
```

### Caderneta (F5) - Débito do Cliente
```bash
# 1. Criar cliente
curl -X POST http://localhost:5000/api/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"João Silva","phone":"11999999999"}'

# 2. Abrir venda para cliente
curl -X POST http://localhost:5000/api/sales \
  -H "Content-Type: application/json" \
  -d '{"customerId":1}'

# 3. Adicionar produtos
curl -X POST http://localhost:5000/api/sales/2/items \
  -H "Content-Type: application/json" \
  -d '{"productCode":52,"quantity":2}'

# 4. Fechar venda na caderneta
curl -X POST http://localhost:5000/api/sales/2/close \
  -H "Content-Type: application/json" \
  -d '{"paymentType":"LEDGER"}'

# 5. Cliente paga R$ 50,00
curl -X POST http://localhost:5000/api/customers/1/credit \
  -H "Content-Type: application/json" \
  -d '{"amount":50,"description":"Pagamento"}'

# 6. Ver extrato
curl http://localhost:5000/api/customers/1/ledger
```

### Produto com Grupo/Subgrupo
```bash
# 1. Criar grupo
curl -X POST http://localhost:5000/api/groups \
  -H "Content-Type: application/json" \
  -d '{"name":"Bebidas","description":"Grupo de bebidas"}'

# 2. Criar subgrupo
curl -X POST http://localhost:5000/api/subgroups \
  -H "Content-Type: application/json" \
  -d '{"name":"Cervejas","groupId":1}'

# 3. Atualizar produto
curl -X PUT http://localhost:5000/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Cerveja","price":5.00,"category":"Bebida","isActive":true,"groupId":1,"subgroupId":1}'
```

## 🏗️ Estrutura do Projeto

```
PDVCasaVerde/
├── Controllers/          # Controladores da API
│   ├── ProductsController.cs
│   ├── GroupsController.cs
│   ├── SubgroupsController.cs
│   ├── CustomersController.cs
│   └── SalesController.cs
├── Data/
│   └── PDVContext.cs    # Contexto do banco de dados
├── DTOs/                # Data Transfer Objects
│   ├── ProductDtos.cs
│   ├── GroupDtos.cs
│   ├── CustomerDtos.cs
│   └── SaleDtos.cs
├── Models/              # Modelos do domínio
│   ├── Product.cs
│   ├── Group.cs
│   ├── Subgroup.cs
│   ├── Customer.cs
│   ├── CustomerLedgerEntry.cs
│   ├── Sale.cs
│   ├── SaleItem.cs
│   ├── Command.cs       # Sistema legado
│   └── Commission.cs
├── Services/            # Lógica de negócio
│   ├── ProductService.cs
│   ├── GroupService.cs
│   ├── SubgroupService.cs
│   ├── CustomerService.cs
│   └── SaleService.cs
└── Program.cs          # Configuração da API
```

## 💾 Banco de Dados

O sistema utiliza SQLite com as seguintes tabelas:

- **Products** - Produtos com grupos e subgrupos
- **Groups** - Grupos de produtos
- **Subgroups** - Subgrupos de produtos
- **Customers** - Clientes com saldo
- **CustomerLedgerEntries** - Histórico de débitos/créditos
- **Sales** - Vendas (mesas e clientes)
- **SaleItems** - Itens das vendas
- **Commands** - Comandas (sistema legado)
- **Commissions** - Comissões

O banco é criado automaticamente na primeira execução.

## 🔒 Tecnologias Utilizadas

- **C# 11** com .NET 9.0
- **ASP.NET Core** - Framework Web API
- **Entity Framework Core 9.0** - ORM
- **SQLite** - Banco de dados embutido
- **Swagger/OpenAPI** - Documentação da API
- **Microsoft.AspNetCore.OpenApi** - Suporte OpenAPI
- **Swashbuckle.AspNetCore** - UI do Swagger

## 📝 Licença

Este projeto é de código aberto e está disponível para uso livre.

## 👨‍💻 Desenvolvimento

Para contribuir com o projeto:

1. Faça um fork do repositório
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 🐛 Problemas e Sugestões

Para reportar bugs ou sugerir melhorias, abra uma [issue](https://github.com/godfathercorleone994-wq/PDV-Casa-Verde/issues) no GitHub.
