using System.Diagnostics;

var startInfo = new ProcessStartInfo()
{
    FileName = "cmd.exe",
    Arguments = "/C pi",
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    RedirectStandardInput = true,
    CreateNoWindow = true,
};

using (var process = Process.Start(startInfo))
{
    using (var writer = process.StandardInput)
    {
        writer.WriteLine("ping ya.ru");
    }

    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    process.WaitForExit();

    Console.WriteLine($"Output: {output}");

    Console.WriteLine($"Error: {error}");
}