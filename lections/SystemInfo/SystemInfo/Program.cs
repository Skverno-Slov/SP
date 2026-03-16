using System.Diagnostics;
using System.Management;
using System.Text;

Console.WriteLine(HardwareInfo.GetCpuInfo());
Console.WriteLine(HardwareInfo.GetRamInfo());

public class HardwareInfo
{

    public static string GetCpuInfo()
    {
        StringBuilder sb = new();
        using ManagementObjectSearcher searcher = new("SELECT * FROM Win32_Processor");

        foreach (var obj in searcher.Get())
        {
            sb.Append($"Процессор: {obj["Name"]}\nЯдер: {obj["NumberOfCores"]}\nПотоков: {obj["NumberOfLogicalProcessors"]}\nЧастота: {obj["MaxClockSpeed"]} МГц");
            
        }
        return sb.ToString();
    }

    public static string GetRamInfo()
    {
        StringBuilder sb = new();
        using ManagementObjectSearcher searcher = new("SELECT * FROM Win32_ComputerSystem");

        foreach (var obj in searcher.Get())
        {
            double ram = Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1 << 30);
            sb.Append($"Опративки: {ram:F2} ГБ");

        }
        return sb.ToString();
    }

    //^^^ AdapterRAM - видеокарта(дискретный/встроенный)


    // Size, Model, SerialNumber - диск

    //double speed = Convert.ToDouble(obj["Speed']) / (1000*1000); Speed, Name, MACAddress

    private PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");

    public void GetCurrentPerformance()
    {
        Console.WriteLine(cpuCounter.NextValue());
    }
}
