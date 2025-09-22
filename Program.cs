using AN;
using CommandsSpace;
using System.Text;

// Configs
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Vars //
string userName = Environment.UserName;
string deviceName = Environment.MachineName;
string currentDirectory = Directory.GetCurrentDirectory();
string? input;
bool isLastCommandSuccess = true;

// Funcs //
void beforeClose()
{
    Console.Clear();
}

// Title //
console.TextHead("AN Terminal | Beta Version");
Console.Title = "AN Terminal";

// Events
// يتنفذ لما يضغط Ctrl+C أو Ctrl+Break
Console.CancelKeyPress += (sender, e) => beforeClose();

// يتنفذ عند أي إغلاق (X، Alt+F4، أو حتى kill)
AppDomain.CurrentDomain.ProcessExit += (sender, e) => beforeClose();

// The Programe //
while (true)
{
    try
    {
        currentDirectory = Directory.GetCurrentDirectory();
        

        if (isLastCommandSuccess) Console.ForegroundColor = ConsoleColor.Green;
        else Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"\n{userName}@{deviceName}");
        Console.ResetColor();
        Console.Write(":");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{currentDirectory} -> ");

        input = console.Input().Trim();

        // Input Validate
        string[] words = Commands.ValidateInput(input);

        string command = words[0];
        string[] arguments = words[1..];

        isLastCommandSuccess = Commands.Run(command, arguments);
    }
    catch (Exception e)
    {
        console.Error(e.Data.ToString() + "");
    }
    finally
    {
        Console.ResetColor();
    }
}