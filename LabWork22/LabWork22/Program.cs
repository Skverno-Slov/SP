//Task1();

using System.Threading.Channels;

internal class Program
{
    static string _commonVar;
    static int _counter;
    static readonly object _lock = new object();

    private static async Task Main(string[] args)
    {
        //Task1();
        //Task2();
        //await Task3Async();
        //Task4();
        //await Task5Async();

        static async Task Task5Async()
        {
            var cts = new CancellationTokenSource();

            var task = DownloadFilesAsync(cts.Token);

            while (true) 
            {
                if (Console.ReadLine() == "c")
                {
                    cts.Cancel();
                    break;
                }
            }

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Загрузка прервана");
            }

            static async Task DownloadFilesAsync(CancellationToken token)
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine("Загрузка...");
                    await Task.Delay(1000, token);
                }
            }
        }

        static void Task4()
        {
            int expected = 5000;

            var threads = new List<Thread>();
            for (int i = 0; i < 5; i++)
            {
                var thread = new Thread(() => { for (int i = 0; i < 1000; i++)
                    {
                        _counter++;
                        //lock (_lock)
                        //{
                        //    _counter++;
                        //}
                        Thread.Sleep(1);
                    } 
                });
                threads.Add(thread);
            }

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();

            if (_counter != expected)
                Console.WriteLine($"Не совпадают({expected} != {_counter})");
            else
                Console.WriteLine($"Совпадают({expected} = {_counter})");
        }

        static async Task Task3Async()
        {
            var channel = Channel.CreateUnbounded<int>();

            var produser = ProduceAsync(channel);
            var consumers = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                consumers[i] = ConsumeAsync(channel.Reader, i+1);
            }

            await produser;
            await Task.WhenAll(consumers);

            Console.WriteLine("жевачечки порубили");

            static async Task ProduceAsync(ChannelWriter<int> writer)
            {
                for (int i = 0; i < 20; i++)
                {
                    await writer.WriteAsync(i+1);
                }

                writer.Complete();
            }

            static async Task ConsumeAsync(ChannelReader<int> reader, int consumerId)
            {
                await foreach(int item in reader.ReadAllAsync())
                {
                    Console.WriteLine($"{consumerId} приобрёл {item}");
                    await Task.Delay(300);
                }
            }
        }

        static void Task2()
        {
            var readThread = new Thread(() => { _commonVar = Console.ReadLine(); });
            var writeThread = new Thread(() => { while (_commonVar != "x") 
                {
                    Console.WriteLine("Печенька");
                    Thread.Sleep(1000);
                }
            });

            readThread.Start();
            writeThread.Start();
        }

        static void Task1()
        {
            var thread = new Thread(Print);
            thread.Start();

            while (true)
            {
                Console.WriteLine("0");
                Thread.Sleep(500);
            }

            void Print()
            {
                while (true)
                {
                    Console.WriteLine("1");
                    Thread.Sleep(1500);
                }
            }
        }
    }
}