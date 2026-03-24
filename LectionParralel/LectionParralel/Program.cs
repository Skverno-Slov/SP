
Parallel.Invoke(
    () => { Console.WriteLine("123"); },
    () => { Console.WriteLine("321"); });

Parallel.For(1, 11, (i) => { Console.WriteLine(i * i - 1); });

List<Point> points = new List<Point>() { new Point(1, 2), new Point(4, 5), new Point(4, 5)};
Parallel.ForEach(points, (point) => { point.X = 5; });

foreach (var point in points)
{
    Console.WriteLine($"{point.X}:{point.Y}");
}

var result = Parallel.For(1, 100, (i, s) => { Squery(i, s); });

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
CancellationToken token = cancellationTokenSource.Token;

token.Register(() => Console.WriteLine("Галя, у нас отмена");

var task = Task.Run(() =>
{
    for(int i = 0; i < 10; i++)
    {
        if (token.IsCancellationRequested)
        {
            Console.WriteLine("Прервана");
            return;
        }
        Console.WriteLine("Привет, Ким");
        Thread.Sleep(1000);
    }
}, token);

Thread.Sleep(4000);
cancellationTokenSource.Cancel();
task.Wait();

void Squery(int i, ParallelLoopState s)
{
    if(i == 5) s.Break();

    Console.WriteLine(i * i);
    Thread.Sleep(2000);
}

class Point(int x, int y){
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}