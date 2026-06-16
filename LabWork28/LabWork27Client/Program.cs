using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using LabWork28Client;

class Program
{
    static Orderer.OrdererClient _client;

    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5139");
        _client = new Orderer.OrdererClient(channel);

        while (true)
        {
            Console.WriteLine("1. Создать заказ");
            Console.WriteLine("2. Получить заказ по ID");
            Console.WriteLine("3. Обновить заказ");
            Console.WriteLine("4. Удалить заказ");
            Console.WriteLine("5. Показать все заказы");
            Console.WriteLine("6. Фильтрация (Дата + Стоимость)"); 
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateOrderAsync();
                        break;
                    case "2":
                        await GetOrderAsync();
                        break;
                    case "3":
                        await UpdateOrderAsync();
                        break;
                    case "4":
                        await DeleteOrderAsync();
                        break;
                    case "5":
                        await ListOrdersAsync();
                        break;
                    case "6":
                        await FilterOrdersAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    static async Task CreateOrderAsync()
    {
        var request = new CreateOrderRequest();
        InputProducts(request.Products);

        var response = await _client.CreateOrderAsync(request);
        Console.WriteLine($"Заказ создан. Id: {response.Id}.");
    }

    static async Task GetOrderAsync()
    {
        Console.Write("Введите id заказа: ");
        string id = Console.ReadLine();

        var response = await _client.GetOrderAsync(new GetOrderRequest { Id = id });
        PrintOrder(response);
    }

    static async Task UpdateOrderAsync()
    {
        Console.Write("Введите id заказа для обновления: ");
        string id = Console.ReadLine();

        var request = new UpdateOrderRequest { Id = id };
        InputProducts(request.Products);

        var response = await _client.UpdateOrderAsync(request);
        Console.WriteLine($"Заказ {response.Id} обновлен.");
    }

    static async Task DeleteOrderAsync()
    {
        Console.Write("Введите id заказа для удаления: ");
        string id = Console.ReadLine();

        await _client.DeleteOrderAsync(new DeleteOrderRequest { Id = id });
        Console.WriteLine("Заказ удален.");
    }

    static async Task ListOrdersAsync()
    {
        var response = await _client.ListOrdersAsync(new Empty());
        Console.WriteLine($" Всего заказов: {response.Orders.Count}");
        foreach (var order in response.Orders)
        {
            PrintOrder(order);
        }
    }

    static async Task FilterOrdersAsync()
    {
        Console.Write("Введите начальную дату (ГГГГ-ММ-ДД): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            startDate = DateTime.MinValue;

        Console.Write("Введите минимальную суммарную стоимость заказа (0,0): ");
        if (!double.TryParse(Console.ReadLine(), out double minPrice))
            minPrice = 0;

        var request = new FilterOrdersRequest
        {
            StartDate = Timestamp.FromDateTime(startDate.ToUniversalTime()),
            MinPrice = minPrice
        };

        var response = await _client.FilterOrdersAsync(request);
        Console.WriteLine($" Найдено заказов: {response.Orders.Count}");
        foreach (var order in response.Orders)
        {
            PrintOrder(order);
        }
    }

    static void InputProducts(RepeatedField<Product> productsList)
    {
        while (true)
        {
            Console.Write("Введите название товара (оставьте пустым для завершения): ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name)) 
                break;

            Console.Write("Введите цену товара (0,0): ");
            if (double.TryParse(Console.ReadLine(), out double price))
                productsList.Add(new Product { Name = name, Price = price });
            else
                Console.WriteLine("Некорректная цена.");
        }
    }

    static void PrintOrder(OrderResponse order)
    {
        Console.WriteLine($"--------------------------------------------------");
        Console.WriteLine($"ID: {order.Id}");
        Console.WriteLine($"Дата: {order.OrderDate.ToDateTime().ToLocalTime()}");
        Console.WriteLine("Товары:");
        double total = 0;
        foreach (var p in order.Products)
        {
            Console.WriteLine($" - {p.Name}: {p.Price} руб.");
            total += p.Price;
        }
        Console.WriteLine($"Итоговая стоимость: {total} руб.");
    }
}