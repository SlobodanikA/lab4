using System;
using System.IO;

namespace FileAttributesChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                ShowHelp();
                return;
            }

            string directoryPath = ".";
            string searchPattern = "*";
            FileAttributes? attributesToSet = null;
            FileAttributes? attributesToUnset = null;

            foreach (string arg in args)
            {
                if (arg.StartsWith("-d:"))
                {
                    directoryPath = arg.Substring(3);
                }
                else if (arg.StartsWith("-p:"))
                {
                    searchPattern = arg.Substring(3);
                }
                else if (arg == "+h")
                {
                    attributesToSet = FileAttributes.Hidden;
                }
                else if (arg == "-h")
                {
                    attributesToUnset = FileAttributes.Hidden;
                }
                else if (arg == "+r")
                {
                    attributesToSet = FileAttributes.ReadOnly;
                }
                else if (arg == "-r")
                {
                    attributesToUnset = FileAttributes.ReadOnly;
                }
                else if (arg == "+a")
                {
                    attributesToSet = FileAttributes.Archive;
                }
                else if (arg == "-a")
                {
                    attributesToUnset = FileAttributes.Archive;
                }
                else
                {
                    Console.WriteLine($"Unknown parameter: {arg}");
                    ShowHelp();
                    Environment.Exit(1);
                }
            }

            try
            {
                var files = Directory.GetFiles(directoryPath, searchPattern);
                foreach (var file in files)
                {
                    var attributes = File.GetAttributes(file);
                    Console.WriteLine($"Original attributes for file {file}: {attributes}");

                    if (attributesToSet.HasValue)
                    {
                        attributes |= attributesToSet.Value;
                    }
                    if (attributesToUnset.HasValue)
                    {
                        attributes &= ~attributesToUnset.Value;
                    }

                    File.SetAttributes(file, attributes);
                    Console.WriteLine($"New attributes for file {file}: {attributes}");
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Usage: FileAttributesChanger [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -d:<directory>     Specify the directory to operate on. Default is current directory.");
            Console.WriteLine("  -p:<pattern>       Specify the file pattern to match. Default is *.");
            Console.WriteLine("  +h                 Set Hidden attribute.");
            Console.WriteLine("  -h                 Unset Hidden attribute.");
            Console.WriteLine("  +r                 Set ReadOnly attribute.");
            Console.WriteLine("  -r                 Unset ReadOnly attribute.");
            Console.WriteLine("  +a                 Set Archive attribute.");
            Console.WriteLine("  -a                 Unset Archive attribute.");
            Console.WriteLine("  -h, --help         Show this help message.");
        }
    }
}
