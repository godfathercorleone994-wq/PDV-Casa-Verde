# PDV Casa Verde - API Documentation

## Base URL
```
http://localhost:5000
```

## API Endpoints

### Products (Produtos)

#### GET /api/products
Lista todos os produtos ativos.

**Response:**
```json
[
  {
    "id": 1,
    "code": 2,
    "name": "Cerveja",
    "price": 5.00,
    "category": "Bebida",
    "isActive": true,
    "groupId": 1,
    "subgroupId": 1,
    "groupName": "Bebidas",
    "subgroupName": "Cervejas"
  }
]
```

#### GET /api/products/{id}
Obtém um produto por ID.

#### GET /api/products/code/{code}
Obtém um produto por código numérico.

#### POST /api/products
Cria um novo produto.

**Request Body:**
```json
{
  "code": 100,
  "name": "Novo Produto",
  "price": 10.00,
  "category": "Categoria",
  "groupId": 1,
  "subgroupId": 1
}
```

#### PUT /api/products/{id}
Atualiza um produto existente, incluindo vinculação com Grupo/Subgrupo.

**Request Body:**
```json
{
  "name": "Produto Atualizado",
  "price": 15.00,
  "category": "Nova Categoria",
  "isActive": true,
  "groupId": 2,
  "subgroupId": 3
}
```

#### DELETE /api/products/{id}
Desativa um produto (soft delete).

---

### Groups (Grupos)

#### GET /api/groups
Lista todos os grupos ativos com seus subgrupos.

**Response:**
```json
[
  {
    "id": 1,
    "name": "Bebidas",
    "description": "Grupo de bebidas",
    "isActive": true,
    "subgroups": [
      {
        "id": 1,
        "name": "Cervejas",
        "description": "Subgrupo de cervejas",
        "isActive": true,
        "groupId": 1
      }
    ]
  }
]
```

#### GET /api/groups/{id}
Obtém um grupo por ID com seus subgrupos e produtos.

#### POST /api/groups
Cria um novo grupo.

**Request Body:**
```json
{
  "name": "Bebidas",
  "description": "Grupo de bebidas"
}
```

#### PUT /api/groups/{id}
Atualiza um grupo existente.

**Request Body:**
```json
{
  "name": "Bebidas Atualizadas",
  "description": "Nova descrição",
  "isActive": true
}
```

#### DELETE /api/groups/{id}
Desativa um grupo (soft delete).

---

### Subgroups (Subgrupos)

#### GET /api/subgroups
Lista todos os subgrupos ativos.

#### GET /api/subgroups/group/{groupId}
Lista subgrupos de um grupo específico.

#### GET /api/subgroups/{id}
Obtém um subgrupo por ID.

#### POST /api/subgroups
Cria um novo subgrupo.

**Request Body:**
```json
{
  "name": "Cervejas",
  "description": "Subgrupo de cervejas",
  "groupId": 1
}
```

#### PUT /api/subgroups/{id}
Atualiza um subgrupo existente.

**Request Body:**
```json
{
  "name": "Cervejas Premium",
  "description": "Cervejas importadas",
  "groupId": 1,
  "isActive": true
}
```

#### DELETE /api/subgroups/{id}
Desativa um subgrupo (soft delete).

---

### Customers (Clientes)

#### GET /api/customers
Lista todos os clientes ativos.

**Response:**
```json
[
  {
    "id": 1,
    "name": "João Silva",
    "phone": "11999999999",
    "email": "joao@test.com",
    "address": "Rua Teste, 123",
    "balance": -50.00,
    "isActive": true
  }
]
```

#### GET /api/customers/{id}
Obtém um cliente por ID.

#### POST /api/customers
Cria um novo cliente.

**Request Body:**
```json
{
  "name": "João Silva",
  "phone": "11999999999",
  "email": "joao@test.com",
  "address": "Rua Teste, 123"
}
```

#### PUT /api/customers/{id}
Atualiza um cliente existente.

**Request Body:**
```json
{
  "name": "João Silva Jr.",
  "phone": "11988888888",
  "email": "joao.jr@test.com",
  "address": "Rua Nova, 456",
  "isActive": true
}
```

#### DELETE /api/customers/{id}
Desativa um cliente (soft delete).

#### POST /api/customers/{id}/credit
**Adiciona crédito à conta do cliente (pagar a conta).**

**Request Body:**
```json
{
  "amount": 50.00,
  "description": "Pagamento em dinheiro"
}
```

**Response:**
```json
{
  "id": 1,
  "name": "João Silva",
  "balance": 0.00,
  "isActive": true
}
```

#### GET /api/customers/{id}/ledger
Obtém o histórico da caderneta do cliente.

**Response:**
```json
[
  {
    "id": 1,
    "type": "SALE",
    "amount": -50.00,
    "description": "Venda #123",
    "createdAt": "2025-10-30T10:00:00",
    "saleId": 123
  },
  {
    "id": 2,
    "type": "PAYMENT",
    "amount": 50.00,
    "description": "Pagamento em dinheiro",
    "createdAt": "2025-10-30T11:00:00",
    "saleId": null
  }
]
```

---

### Sales (Vendas / Mesas)

#### GET /api/sales
Lista todas as vendas abertas.

**Response:**
```json
[
  {
    "id": 1,
    "saleNumber": 1,
    "tableNumber": 1,
    "customerId": null,
    "customerName": null,
    "openedAt": "2025-10-30T10:00:00",
    "closedAt": null,
    "isOpen": true,
    "totalAmount": 15.00,
    "paymentType": "",
    "status": "OPEN",
    "items": [
      {
        "id": 1,
        "productId": 1,
        "productName": "Cerveja",
        "quantity": 3,
        "unitPrice": 5.00,
        "totalPrice": 15.00,
        "addedAt": "2025-10-30T10:05:00"
      }
    ]
  }
]
```

#### GET /api/sales/table/{tableNumber}
**Lista vendas abertas de uma mesa específica (F4 - Mesas).**

#### GET /api/sales/{id}
Obtém uma venda por ID.

#### GET /api/sales/number/{saleNumber}
Obtém uma venda por número.

#### POST /api/sales
**Cria uma nova venda (abrir mesa ou iniciar venda para cliente).**

**Request Body (Mesa):**
```json
{
  "tableNumber": 1
}
```

**Request Body (Cliente - Caderneta):**
```json
{
  "customerId": 1
}
```

**Response:**
```json
{
  "id": 1,
  "saleNumber": 1,
  "tableNumber": 1,
  "customerId": null,
  "openedAt": "2025-10-30T10:00:00",
  "isOpen": true,
  "totalAmount": 0,
  "status": "OPEN",
  "items": []
}
```

#### POST /api/sales/{id}/items
**Adiciona itens à venda (lançar produtos na mesa).**

**Request Body:**
```json
{
  "productCode": 2,
  "quantity": 3
}
```

**Response:**
Retorna a venda atualizada com os itens.

#### POST /api/sales/{id}/close
**Fecha a venda (finalizar mesa ou caderneta - F5).**

**Request Body:**
```json
{
  "paymentType": "CASH"
}
```

**Tipos de Pagamento:**
- `CASH` - Dinheiro
- `CARD` - Cartão
- `LEDGER` - Caderneta (vincula débito ao cliente)

**Response:**
```json
{
  "id": 1,
  "saleNumber": 1,
  "tableNumber": 1,
  "closedAt": "2025-10-30T11:00:00",
  "isOpen": false,
  "totalAmount": 15.00,
  "paymentType": "CASH",
  "status": "CLOSED"
}
```

#### POST /api/sales/{id}/cancel
Cancela uma venda.

---

## Fluxos de Uso

### Fluxo 1: Venda em Mesa (F4)
1. Abrir mesa: `POST /api/sales` com `tableNumber`
2. Adicionar produtos: `POST /api/sales/{id}/items`
3. Consultar mesa: `GET /api/sales/table/{tableNumber}`
4. Fechar mesa: `POST /api/sales/{id}/close` com `paymentType: "CASH"` ou `"CARD"`

### Fluxo 2: Venda em Caderneta (F5)
1. Criar/buscar cliente: `POST /api/customers` ou `GET /api/customers`
2. Abrir venda: `POST /api/sales` com `customerId`
3. Adicionar produtos: `POST /api/sales/{id}/items`
4. Fechar venda na caderneta: `POST /api/sales/{id}/close` com `paymentType: "LEDGER"`
5. Cliente paga conta: `POST /api/customers/{id}/credit` com o valor
6. Consultar extrato: `GET /api/customers/{id}/ledger`

### Fluxo 3: Gerenciar Produtos com Grupos
1. Criar grupo: `POST /api/groups`
2. Criar subgrupo: `POST /api/subgroups` vinculado ao grupo
3. Criar produto: `POST /api/products` com `groupId` e `subgroupId`
4. Atualizar produto: `PUT /api/products/{id}` para alterar grupo/subgrupo

---

## Códigos de Status HTTP

- `200 OK` - Requisição bem-sucedida
- `201 Created` - Recurso criado com sucesso
- `204 No Content` - Exclusão bem-sucedida
- `400 Bad Request` - Dados inválidos
- `404 Not Found` - Recurso não encontrado
- `500 Internal Server Error` - Erro no servidor

---

## Notas Técnicas

- Todos os endpoints retornam JSON
- Datas estão em formato ISO 8601 (UTC)
- Valores monetários em decimal com 2 casas decimais
- Soft delete: recursos não são removidos, apenas marcados como inativos
- Balance negativo indica débito, positivo indica crédito
