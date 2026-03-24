
int x = 0;

object locker = new object();

for(int i = 0; i < 5; i++)
{
    Thread thread = new(Print);
    thread.Name = $"#{i}";
    thread.Start();
}


void Print()
{
    lock (locker)
    {
        x = 1;
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.Name}: {x}");
            x++;
            Thread.Sleep(100);
        }
    }
}

class Reader
{
    static Semaphore sem = new(3, 3);

    Thread thread;
    int count = 3;

    public Reader(int i)
    {
        thread = new Thread(Read);
        thread.Name = $"Читатель {i}";
        thread.Start();
    }

    private void Read()
    {
        while (count > 0)
        {
            sem.WaitOne();
            Console.WriteLine($"{Thread.CurrentThread.Name} Входит");
            Console.WriteLine($"{Thread.CurrentThread.Name} Читает");
            Thread.Sleep(1000);
            Console.WriteLine($"{Thread.CurrentThread.Name} Выходит");
            sem.Release();
            count--;
            Thread.Sleep(1000);
        }
    }
}

