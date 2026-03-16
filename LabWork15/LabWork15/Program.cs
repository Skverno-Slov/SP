using System.Security.Cryptography;

internal class Program
{
    static Test globalRef;

    private static void Main(string[] args)
    {
        byte[] packet =
        {
            0x01, 0x00, // ushort ID = 1
            0x10, 0x27, 0x00, 0x00, // int timestamp = 10000
            0x00, 0x00, 0x48, 0x42, // float temperature = 50.0
            0x01, // byte status
            0xC3, 0x00, 0x00, 0x00 // int checksum
        };

        unsafe
        {
            fixed (byte* ptr = packet)
            {
                var values = new Values();

                int checkSum =0;
                var ptr2 = ptr;

                ushort* idPtr = (ushort*)ptr2;
                values.id = *idPtr;

                ptr2 += sizeof(ushort);

                int* tsPtr = (int*)ptr2;
                values.timeStamp = *tsPtr;

                ptr2 += sizeof(int);

                float* tempPtr = (float*)ptr2;
                values.temperature = *tempPtr;

                ptr2 += sizeof(float);

                bool* statusPtr = (bool*)ptr2;
                values.status = *statusPtr;

                ptr2 += sizeof(bool);

                int* sumPtr = (int*)ptr2;
                values.sum = *sumPtr;

                foreach (byte b in packet.AsSpan(0, 11))
                {
                    checkSum += b;
                }

                Console.WriteLine(values.id);
                Console.WriteLine(values.timeStamp);
                Console.WriteLine(values.temperature);
                Console.WriteLine(values.status);
                Console.WriteLine(values.sum);
                Console.WriteLine(checkSum);

                if (checkSum == values.sum)
                {
                    Console.WriteLine("Совпадают");
                }
                else
                {
                    Console.WriteLine("Не совпадают");
                }
            }
        }


        try
        {
            CreateObject();
            GC.Collect();
            Console.WriteLine(GC.GetGeneration(globalRef));
        }
        catch { }

        var pointC = new PointClass()
        {
            x = 2,
            y = 3
        };

        var pointS = new PointStruct()
        {
            x = 4,
            y = 6
        };


        int[] arr1 = new int[10];
        Console.WriteLine(GC.GetGeneration(arr1));
        int[] arr2 = new int[90000];
        Console.WriteLine(GC.GetGeneration(arr2));
        int[] arr3 = new int[100000];
        Console.WriteLine(GC.GetGeneration(arr3));

        pointC.ModifyClass(pointC);
        pointS.ModifyStruct(ref pointS);

        Console.WriteLine(pointC.x);
        Console.WriteLine(pointS.x);

        try
        {
            //using var fl1 = new DisposeNtFileLogger()
            //{

            //};
            using var fl2 = new FileLogger()
            {

            };

            var fl3 = new FileLogger();
            fl3.Dispose();

            var fl4 = new FileLogger();
            GC.Collect();

            var fl5 = new DisposeNtFileLogger();
        }
        catch { }
    }

    static void CreateObject()
    {
        Test t = new Test();
        globalRef = t;
    }
}

class Test
{
    ~Test()
    {
        Console.WriteLine("Object finalized");
    }
}

public class PointClass()
{
    public decimal x; public decimal y;

    public void ModifyClass(PointClass point)
    {
        point.x = 14;
        point.y = 18;
    }
}

public struct PointStruct()
{
    public decimal x; public decimal y;
    public void ModifyStruct(ref PointStruct point)
    {
        point.x = 14;
        point.y = 18;
    }
}

public struct Values
{
    public ushort id;
    public int timeStamp;
    public float temperature;
    public bool status;
    public int sum;
}

public class FileLogger : IDisposable
{
    StreamWriter _sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file1.txt"), true);

    public void Dispose()
    {
        _sw.Dispose();
    }
}

public class DisposeNtFileLogger
{
    StreamWriter _sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file2.txt"));
}
