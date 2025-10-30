using PDVCasaVerde.Data;
using PDVCasaVerde.Services;
using PDVCasaVerde.Models;

namespace PDVCasaVerde;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize database
        using var context = new PDVContext();
        await context.Database.EnsureCreatedAsync();

        var productService = new ProductService(context);
        var commandService = new CommandService(context);
        var commissionService = new CommissionService(context);
        var printService = new PrintService();

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            ShowMainMenu();
            var option = Console.ReadLine();

            try
            {
                switch (option)
                {
                    case "1":
                        await OpenNewCommand(commandService);
                        break;
                    case "2":
                        await AddItemToCommand(commandService, productService);
                        break;
                    case "3":
                        await ViewCommand(commandService, printService);
                        break;
                    case "4":
                        await CloseCommand(commandService, printService);
                        break;
                    case "5":
                        await ListOpenCommands(commandService);
                        break;
                    case "6":
                        await RegisterProduct(productService);
                        break;
                    case "7":
                        await ListProducts(productService);
                        break;
                    case "8":
                        await AddCommission(commissionService);
                        break;
                    case "9":
                        await ViewCommissions(commissionService);
                        break;
                    case "0":
                        Console.WriteLine("Encerrando sistema...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro: {ex.Message}");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║      CASA VERDE - SISTEMA PDV         ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("  COMANDAS:");
        Console.WriteLine("  1 - Abrir Nova Comanda");
        Console.WriteLine("  2 - Lançar Produto na Comanda");
        Console.WriteLine("  3 - Visualizar Comanda");
        Console.WriteLine("  4 - Fechar Comanda");
        Console.WriteLine("  5 - Listar Comandas Abertas");
        Console.WriteLine();
        Console.WriteLine("  PRODUTOS:");
        Console.WriteLine("  6 - Cadastrar Produto");
        Console.WriteLine("  7 - Listar Produtos");
        Console.WriteLine();
        Console.WriteLine("  COMISSÕES:");
        Console.WriteLine("  8 - Lançar Comissão");
        Console.WriteLine("  9 - Visualizar Comissões");
        Console.WriteLine();
        Console.WriteLine("  0 - Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    static async Task OpenNewCommand(CommandService commandService)
    {
        Console.Clear();
        Console.WriteLine("═══ ABRIR NOVA COMANDA ═══\n");
        Console.Write("Nome do Cliente: ");
        var clientName = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(clientName))
        {
            Console.WriteLine("Nome inválido!");
            Console.ReadKey();
            return;
        }

        var command = await commandService.CreateCommandAsync(clientName);
        Console.WriteLine($"\nComanda #{command.CommandNumber} aberta com sucesso!");
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static async Task AddItemToCommand(CommandService commandService, ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("═══ LANÇAR PRODUTO NA COMANDA ═══\n");
        Console.Write("Número da Comanda: ");
        if (!int.TryParse(Console.ReadLine(), out int commandNumber))
        {
            Console.WriteLine("Número inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Código do Produto: ");
        if (!int.TryParse(Console.ReadLine(), out int productCode))
        {
            Console.WriteLine("Código inválido!");
            Console.ReadKey();
            return;
        }

        var product = await productService.GetByCodeAsync(productCode);
        if (product == null)
        {
            Console.WriteLine("Produto não encontrado!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nProduto: {product.Name} - R$ {product.Price:F2}");
        Console.Write("Quantidade: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Quantidade inválida!");
            Console.ReadKey();
            return;
        }

        var success = await commandService.AddItemToCommandAsync(commandNumber, productCode, quantity);
        if (success)
        {
            Console.WriteLine($"\n{quantity}x {product.Name} adicionado(s) à comanda #{commandNumber}!");
        }
        else
        {
            Console.WriteLine("\nErro ao adicionar item. Verifique se a comanda está aberta.");
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static async Task ViewCommand(CommandService commandService, PrintService printService)
    {
        Console.Clear();
        Console.WriteLine("═══ VISUALIZAR COMANDA ═══\n");
        Console.Write("Número da Comanda: ");
        if (!int.TryParse(Console.ReadLine(), out int commandNumber))
        {
            Console.WriteLine("Número inválido!");
            Console.ReadKey();
            return;
        }

        var command = await commandService.GetCommandByNumberAsync(commandNumber);
        if (command == null)
        {
            Console.WriteLine("Comanda não encontrada!");
            Console.ReadKey();
            return;
        }

        printService.PrintToConsole(command);
    }

    static async Task CloseCommand(CommandService commandService, PrintService printService)
    {
        Console.Clear();
        Console.WriteLine("═══ FECHAR COMANDA ═══\n");
        Console.Write("Número da Comanda: ");
        if (!int.TryParse(Console.ReadLine(), out int commandNumber))
        {
            Console.WriteLine("Número inválido!");
            Console.ReadKey();
            return;
        }

        var command = await commandService.CloseCommandAsync(commandNumber);
        if (command == null)
        {
            Console.WriteLine("Comanda não encontrada ou já está fechada!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n═══ RECIBO FINAL ═══\n");
        printService.PrintToConsole(command);
    }

    static async Task ListOpenCommands(CommandService commandService)
    {
        Console.Clear();
        Console.WriteLine("═══ COMANDAS ABERTAS ═══\n");
        var commands = await commandService.GetOpenCommandsAsync();

        if (commands.Count == 0)
        {
            Console.WriteLine("Nenhuma comanda aberta.");
        }
        else
        {
            Console.WriteLine($"{"Nº",-6} {"Cliente",-20} {"Aberta em",-20} {"Total",10}");
            Console.WriteLine(new string('─', 60));
            foreach (var cmd in commands)
            {
                Console.WriteLine($"{cmd.CommandNumber,-6} {cmd.ClientName,-20} {cmd.OpenedAt:dd/MM/yyyy HH:mm,-20} R$ {cmd.TotalAmount,7:F2}");
            }
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static async Task RegisterProduct(ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("═══ CADASTRAR PRODUTO ═══\n");

        Console.Write("Código do Produto: ");
        if (!int.TryParse(Console.ReadLine(), out int code))
        {
            Console.WriteLine("Código inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Nome do Produto: ");
        var name = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nome inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Preço (R$): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
        {
            Console.WriteLine("Preço inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Categoria: ");
        var category = Console.ReadLine() ?? "Geral";

        var product = new Product
        {
            Code = code,
            Name = name,
            Price = price,
            Category = category
        };

        try
        {
            await productService.CreateAsync(product);
            Console.WriteLine($"\nProduto '{name}' (Código: {code}) cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro ao cadastrar produto: {ex.Message}");
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static async Task ListProducts(ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("═══ LISTA DE PRODUTOS ═══\n");
        var products = await productService.GetAllAsync();

        if (products.Count == 0)
        {
            Console.WriteLine("Nenhum produto cadastrado.");
        }
        else
        {
            Console.WriteLine($"{"Código",-8} {"Nome",-25} {"Categoria",-15} {"Preço",10}");
            Console.WriteLine(new string('─', 60));
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Code,-8} {product.Name,-25} {product.Category,-15} R$ {product.Price,7:F2}");
            }
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static async Task AddCommission(CommissionService commissionService)
    {
        Console.Clear();
        Console.WriteLine("═══ LANÇAR COMISSÃO ═══\n");

        Console.Write("Número da Comanda: ");
        if (!int.TryParse(Console.ReadLine(), out int commandNumber))
        {
            Console.WriteLine("Número inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Nome da Garota: ");
        var staffName = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(staffName))
        {
            Console.WriteLine("Nome inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Valor da Comissão (R$): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount < 0)
        {
            Console.WriteLine("Valor inválido!");
            Console.ReadKey();
            return;
        }

        Console.Write("Observações (opcional): ");
        var notes = Console.ReadLine();

        var commission = await commissionService.AddCommissionAsync(commandNumber, staffName, amount, notes);
        if (commission != null)
        {
            Console.WriteLine($"\nComissão de R$ {amount:F2} para {staffName} registrada na comanda #{commandNumber}!");
        }
        else
        {
            Console.WriteLine("\nErro: Comanda não encontrada!");
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static async Task ViewCommissions(CommissionService commissionService)
    {
        Console.Clear();
        Console.WriteLine("═══ VISUALIZAR COMISSÕES ═══\n");
        Console.WriteLine("1 - Por Garota");
        Console.WriteLine("2 - Por Comanda");
        Console.WriteLine("3 - Total Geral");
        Console.Write("\nEscolha: ");

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                Console.Write("Nome da Garota: ");
                var staffName = Console.ReadLine() ?? "";
                var staffCommissions = await commissionService.GetCommissionsByStaffAsync(staffName);
                
                Console.WriteLine($"\n═══ COMISSÕES - {staffName.ToUpper()} ═══\n");
                if (staffCommissions.Count == 0)
                {
                    Console.WriteLine("Nenhuma comissão encontrada.");
                }
                else
                {
                    decimal total = 0;
                    Console.WriteLine($"{"Data",-20} {"Comanda",-10} {"Valor",10}");
                    Console.WriteLine(new string('─', 45));
                    foreach (var c in staffCommissions)
                    {
                        Console.WriteLine($"{c.CreatedAt:dd/MM/yyyy HH:mm,-20} #{c.Command.CommandNumber,-9} R$ {c.Amount,7:F2}");
                        total += c.Amount;
                    }
                    Console.WriteLine(new string('─', 45));
                    Console.WriteLine($"{"TOTAL:",-30} R$ {total,7:F2}");
                }
                break;

            case "2":
                Console.Write("Número da Comanda: ");
                if (int.TryParse(Console.ReadLine(), out int commandNumber))
                {
                    var commandCommissions = await commissionService.GetCommissionsByCommandAsync(commandNumber);
                    
                    Console.WriteLine($"\n═══ COMISSÕES - COMANDA #{commandNumber} ═══\n");
                    if (commandCommissions.Count == 0)
                    {
                        Console.WriteLine("Nenhuma comissão encontrada.");
                    }
                    else
                    {
                        decimal total = 0;
                        Console.WriteLine($"{"Data",-20} {"Garota",-20} {"Valor",10}");
                        Console.WriteLine(new string('─', 55));
                        foreach (var c in commandCommissions)
                        {
                            Console.WriteLine($"{c.CreatedAt:dd/MM/yyyy HH:mm,-20} {c.StaffName,-20} R$ {c.Amount,7:F2}");
                            total += c.Amount;
                        }
                        Console.WriteLine(new string('─', 55));
                        Console.WriteLine($"{"TOTAL:",-40} R$ {total,7:F2}");
                    }
                }
                break;

            case "3":
                var totalCommissions = await commissionService.GetTotalCommissionsAsync();
                Console.WriteLine($"\nTOTAL GERAL DE COMISSÕES: R$ {totalCommissions:F2}");
                break;
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
