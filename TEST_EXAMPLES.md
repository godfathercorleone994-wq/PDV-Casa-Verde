# PDV Casa Verde - Test Examples

## Environment Setup

```bash
# Start the API
cd PDVCasaVerde
dotnet run

# API will be available at http://localhost:5000
```

## Test Scenarios

### Scenario 1: Complete Product with Groups Workflow

```bash
# 1. Create a Group
curl -X POST http://localhost:5000/api/groups \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Bebidas",
    "description": "Todas as bebidas do estabelecimento"
  }'

# Response: { "id": 1, "name": "Bebidas", ... }

# 2. Create a Subgroup
curl -X POST http://localhost:5000/api/subgroups \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Cervejas",
    "description": "Cervejas nacionais e importadas",
    "groupId": 1
  }'

# Response: { "id": 1, "name": "Cervejas", "groupId": 1, ... }

# 3. Create another Subgroup
curl -X POST http://localhost:5000/api/subgroups \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Refrigerantes",
    "description": "Refrigerantes diversos",
    "groupId": 1
  }'

# Response: { "id": 2, "name": "Refrigerantes", "groupId": 1, ... }

# 4. Create a Product with Group/Subgroup
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "code": 100,
    "name": "Heineken Lata 350ml",
    "price": 8.50,
    "category": "Bebida",
    "groupId": 1,
    "subgroupId": 1
  }'

# Response: { "id": 4, "code": 100, "name": "Heineken Lata 350ml", ... }

# 5. List all products with their groups
curl http://localhost:5000/api/products | jq .

# 6. Get products by group
curl http://localhost:5000/api/subgroups/1 | jq .

# 7. Update product to change group/subgroup
curl -X PUT http://localhost:5000/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Cerveja",
    "price": 5.50,
    "category": "Bebida",
    "isActive": true,
    "groupId": 1,
    "subgroupId": 1
  }'
```

### Scenario 2: Table Sales (F4) - Mesa

```bash
# 1. Open table 5
curl -X POST http://localhost:5000/api/sales \
  -H "Content-Type: application/json" \
  -d '{"tableNumber": 5}'

# Response: { "id": 1, "saleNumber": 1, "tableNumber": 5, "isOpen": true, ... }

# 2. Add 4 beers (code 2)
curl -X POST http://localhost:5000/api/sales/1/items \
  -H "Content-Type: application/json" \
  -d '{
    "productCode": 2,
    "quantity": 4
  }'

# Response shows updated sale with totalAmount: 20.00

# 3. Add 2 Skol buckets (code 52)
curl -X POST http://localhost:5000/api/sales/1/items \
  -H "Content-Type: application/json" \
  -d '{
    "productCode": 52,
    "quantity": 2
  }'

# Response shows updated sale with totalAmount: 70.00

# 4. Check all open sales for table 5
curl http://localhost:5000/api/sales/table/5 | jq .

# 5. Check specific sale
curl http://localhost:5000/api/sales/1 | jq .

# 6. Close table with cash payment
curl -X POST http://localhost:5000/api/sales/1/close \
  -H "Content-Type: application/json" \
  -d '{"paymentType": "CASH"}'

# Response: { "isOpen": false, "status": "CLOSED", "paymentType": "CASH", ... }

# 7. Verify table is closed
curl http://localhost:5000/api/sales/table/5 | jq .
# Should return empty array []
```

### Scenario 3: Customer Ledger (F5) - Caderneta

```bash
# 1. Create a customer
curl -X POST http://localhost:5000/api/customers \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Maria Santos",
    "phone": "11988887777",
    "email": "maria@example.com",
    "address": "Rua das Flores, 123"
  }'

# Response: { "id": 1, "name": "Maria Santos", "balance": 0, ... }

# 2. Open a sale for the customer (caderneta)
curl -X POST http://localhost:5000/api/sales \
  -H "Content-Type: application/json" \
  -d '{"customerId": 1}'

# Response: { "id": 2, "saleNumber": 2, "customerId": 1, "isOpen": true, ... }

# 3. Add items to the sale
curl -X POST http://localhost:5000/api/sales/2/items \
  -H "Content-Type: application/json" \
  -d '{"productCode": 2, "quantity": 6}'

curl -X POST http://localhost:5000/api/sales/2/items \
  -H "Content-Type: application/json" \
  -d '{"productCode": 52, "quantity": 1}'

# Total should be: (6 * 5.00) + (1 * 25.00) = 55.00

# 4. Close sale with LEDGER payment type
curl -X POST http://localhost:5000/api/sales/2/close \
  -H "Content-Type: application/json" \
  -d '{"paymentType": "LEDGER"}'

# Response: { "isOpen": false, "paymentType": "LEDGER", ... }

# 5. Check customer balance
curl http://localhost:5000/api/customers/1 | jq .
# Response: { "balance": -55.00, ... } (negative = debt)

# 6. Check customer ledger history
curl http://localhost:5000/api/customers/1/ledger | jq .
# Shows: SALE entry with amount: -55.00

# 7. Customer makes partial payment
curl -X POST http://localhost:5000/api/customers/1/credit \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 30.00,
    "description": "Pagamento parcial em dinheiro"
  }'

# Response: { "balance": -25.00, ... }

# 8. Customer pays remaining balance
curl -X POST http://localhost:5000/api/customers/1/credit \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 25.00,
    "description": "Pagamento final"
  }'

# Response: { "balance": 0.00, ... }

# 9. Check complete ledger history
curl http://localhost:5000/api/customers/1/ledger | jq .
# Shows all transactions: SALE, PAYMENT, PAYMENT
```

### Scenario 4: Multiple Tables Management

```bash
# Open multiple tables
curl -X POST http://localhost:5000/api/sales -H "Content-Type: application/json" -d '{"tableNumber": 1}'
curl -X POST http://localhost:5000/api/sales -H "Content-Type: application/json" -d '{"tableNumber": 2}'
curl -X POST http://localhost:5000/api/sales -H "Content-Type: application/json" -d '{"tableNumber": 3}'

# Add items to each table
curl -X POST http://localhost:5000/api/sales/3/items -H "Content-Type: application/json" -d '{"productCode": 2, "quantity": 2}'
curl -X POST http://localhost:5000/api/sales/4/items -H "Content-Type: application/json" -d '{"productCode": 52, "quantity": 1}'
curl -X POST http://localhost:5000/api/sales/5/items -H "Content-Type: application/json" -d '{"productCode": 2, "quantity": 5}'

# List all open sales
curl http://localhost:5000/api/sales | jq .

# Check specific table
curl http://localhost:5000/api/sales/table/2 | jq .

# Close tables one by one
curl -X POST http://localhost:5000/api/sales/3/close -H "Content-Type: application/json" -d '{"paymentType": "CASH"}'
curl -X POST http://localhost:5000/api/sales/4/close -H "Content-Type: application/json" -d '{"paymentType": "CARD"}'
curl -X POST http://localhost:5000/api/sales/5/close -H "Content-Type: application/json" -d '{"paymentType": "CASH"}'
```

### Scenario 5: Mixed Operations

```bash
# Create multiple customers
curl -X POST http://localhost:5000/api/customers -H "Content-Type: application/json" -d '{"name": "Pedro Alves", "phone": "11977776666"}'
curl -X POST http://localhost:5000/api/customers -H "Content-Type: application/json" -d '{"name": "Ana Costa", "phone": "11966665555"}'

# List all customers
curl http://localhost:5000/api/customers | jq .

# Create groups and products
curl -X POST http://localhost:5000/api/groups -H "Content-Type: application/json" -d '{"name": "Alimentos", "description": "Porções e lanches"}'
curl -X POST http://localhost:5000/api/subgroups -H "Content-Type: application/json" -d '{"name": "Porções", "groupId": 2}'
curl -X POST http://localhost:5000/api/products -H "Content-Type: application/json" -d '{"code": 200, "name": "Porção de Batata Frita", "price": 15.00, "category": "Alimento", "groupId": 2, "subgroupId": 3}'

# Open sale for table with food and drinks
curl -X POST http://localhost:5000/api/sales -H "Content-Type: application/json" -d '{"tableNumber": 10}'
curl -X POST http://localhost:5000/api/sales/6/items -H "Content-Type: application/json" -d '{"productCode": 200, "quantity": 1}'
curl -X POST http://localhost:5000/api/sales/6/items -H "Content-Type: application/json" -d '{"productCode": 2, "quantity": 4}'

# Check sale before closing
curl http://localhost:5000/api/sales/6 | jq .

# Close sale
curl -X POST http://localhost:5000/api/sales/6/close -H "Content-Type: application/json" -d '{"paymentType": "CARD"}'
```

## Verification Commands

```bash
# List all products
curl http://localhost:5000/api/products | jq .

# List all groups with subgroups
curl http://localhost:5000/api/groups | jq .

# List all customers
curl http://localhost:5000/api/customers | jq .

# List all open sales
curl http://localhost:5000/api/sales | jq .

# Get specific customer balance and history
curl http://localhost:5000/api/customers/1 | jq .
curl http://localhost:5000/api/customers/1/ledger | jq .
```

## Error Handling Tests

```bash
# Try to add item with invalid product code
curl -X POST http://localhost:5000/api/sales/1/items \
  -H "Content-Type: application/json" \
  -d '{"productCode": 9999, "quantity": 1}'
# Should return 400 Bad Request

# Try to close already closed sale
curl -X POST http://localhost:5000/api/sales/1/close \
  -H "Content-Type: application/json" \
  -d '{"paymentType": "CASH"}'
# Should return 404 Not Found if already closed

# Try to get non-existent customer
curl http://localhost:5000/api/customers/9999 | jq .
# Should return 404 Not Found

# Try to create product with duplicate code
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{"code": 2, "name": "Test", "price": 1.00, "category": "Test"}'
# Should return 400 Bad Request
```

## Performance Tests

```bash
# Create 10 tables simultaneously
for i in {11..20}; do
  curl -X POST http://localhost:5000/api/sales -H "Content-Type: application/json" -d "{\"tableNumber\": $i}" &
done
wait

# List all open tables
curl http://localhost:5000/api/sales | jq 'length'
# Should return count of open sales
```

## Summary

All test scenarios validate:
- ✅ Product CRUD operations
- ✅ Group and Subgroup management
- ✅ Product linking to Groups/Subgroups
- ✅ Table sales (F4) workflow
- ✅ Customer ledger (F5) workflow
- ✅ Credit/payment operations
- ✅ Transaction history
- ✅ Multiple concurrent operations
- ✅ Error handling
