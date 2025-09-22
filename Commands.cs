namespace CommandsSpace;
using System.Diagnostics;
using AN;
using System.IO;


public class Commands
{

    public static readonly string[] commands = { "ls", "cd", "mktxt" };

    public static string[] ValidateInput(string input)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return Array.Empty<string>();

        else
        { 
            if (input.Contains('"')) // if input like 'cd "/path/to/dir" -param'
            {
                var res = input.Trim().Split('"');
                
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = res[i].Trim();
                }
                res[0] = res[0].ToLower(); // Command
                return res;
            }
            else
            {
                var res = input.Trim().Split(' ');

                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = res[i].Trim();
                }
                res[0] = res[0].ToLower(); // Command
                return res;
            }
        }
    }

   public static bool Run(string command, string[] arguments)
    {
        try
        {
            if (!commands.Contains(command)) 
                return SystemRun(command, arguments);
            
            bool theresArguments = false;
            if (arguments.Length > 0) theresArguments = true;

            switch (command)
            {
                case "ls":
                    return Ls(arguments);

                case "cd":
                    if (theresArguments) return Cd(arguments[0]);
                    else return Cd();
                case "mktxt":
                    return MkText(arguments);

                default:
                    return false;
            }
        }
        catch (Exception ex)
        {
            console.Error($"\n  {ex.Message.ToString()}\n\n");
            return false;
        }
    }

    // Run System Command
    public static bool SystemRun(string command, string[] arguments)
    {
        try
        {
            var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = string.Join(" ", arguments);

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                console.Error(error);
                return false;
            }


            Console.WriteLine($"\n{output}\n");
            return true;
        }
        catch (Exception ex)
        {
            console.Error($"\n  {ex.Message.ToString()}\n\n");
            return false;
        }
    }

    // command: ls
    public static bool Ls(string[] arguments)
    {
        try
        {
            if (arguments.Length > 1) throw new ArgumentException("ls Command NOT Accept More Than One Argument!");
            else
            {
                string currentDir = Directory.GetCurrentDirectory();
                bool showHidden = false;

                if (arguments.Contains("-all")) showHidden = true; 

                Console.WriteLine("");

                // المجلدات
                string[] dirs = Directory.GetDirectories(currentDir);
                Array.Sort(dirs);
                foreach (var dir in dirs)
                {
                    string dirName = Path.GetFileName(dir).Trim();

                    if (dirName.StartsWith('.'))
                    {
                        if (!showHidden) continue;
                        else Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    console.Text_1("🗀  " + dirName);
                    Console.ResetColor();
                }

                Console.WriteLine("");

                // الملفات
                string[] files = Directory.GetFiles(currentDir);
                Array.Sort(files);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.StartsWith('.'))
                    {
                        if (!showHidden) continue;
                        else Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    console.Text_1("🗅  " + fileName);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            return false;
        }
    }

    // command: cd
    public static bool Cd(string newPath = "~")
    {
        try
        {
        if (newPath == "~") newPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Directory.SetCurrentDirectory(newPath);
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            return false;
        }
    }

    // command: mktxt
    public static bool MkText(string[] arguments)
    {
        try
        {
            string? fileName;
            if (arguments.Length > 0)
            {
                fileName = arguments[0];
            }
            else
            {
                Console.Write("Enter The Text File Name: ");
                fileName = console.Input().Trim();
                int counter = 0;
                while (string.IsNullOrEmpty(fileName) && counter < 4)
                {
                    Console.Write("Enter The Text File Name: ");
                    fileName = console.Input().Trim();
                    counter++;
                }
                if (string.IsNullOrEmpty(fileName) && counter == 4) fileName = "TextFile.txt";
            }

            if (!fileName.EndsWith(".txt")) fileName += ".txt";

            string path = $"{Directory.GetCurrentDirectory()}/{fileName}";

            Console.Write("Enter The File Content: ");
            string content = console.GetMultiLines();
            
            File.WriteAllText(path, content);
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            return false;
        }
    }
}
