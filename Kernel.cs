using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using Sys = Cosmos.System;

namespace openOS
{
    public class Kernel : Sys.Kernel
    {
        string currentPath = "0:\\";
        CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine("Done!");
            wait(1000);
            Console.Clear();
            Console.WriteLine("openOS has booted sucessfully");
        }

        protected override void Run()
        {
            Console.Write(currentPath + "> ");
            var input = Console.ReadLine();
            string[] splitInput = input.Split(" ");
            string command = splitInput[0];

            if (command == "help")
            {
                Console.WriteLine("Available commands:\n" +
                    "help           Shows this list\n" +
                    "ls [dir]       List all files and directories in given directory");
            }

            else if (command == "ls")
            {
                listDir();
            }

            else if (command == "read")
            {
                try
                {
                    if (splitInput.Length < 2)
                    {
                        Console.WriteLine("No file specified");
                        return;
                    }

                    string path = evaluatePath(splitInput[1]);
                    var file = Sys.FileSystem.VFS.VFSManager.GetFile(path);
                    var fileStream = file.GetFileStream();

                    if (fileStream.CanRead)
                    {
                        byte[] textToRead = new byte[fileStream.Length];
                        fileStream.Read(textToRead, 0, (int)fileStream.Length);

                        string fileContent = Encoding.ASCII.GetString(textToRead);
                        Console.WriteLine(fileContent);
                    }
                    else
                    {
                        Console.WriteLine("File is not readable");
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("No argument given");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Path doesn't exist: " + splitInput[1]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unknown exception: " + ex.Message);
                }
            }

            else if (command == "write")
            {

                if (splitInput.Length < 2)
                {
                    Console.WriteLine("No file specified");
                    return;
                }

                string path = evaluatePath(splitInput[1]);
                var file = Sys.FileSystem.VFS.VFSManager.GetFile(path);
                var fileStream = file.GetFileStream();
                if (fileStream.CanWrite)
                {
                    string writeToFile;
                    while (true)
                    {
                        string fileLine = Console.ReadLine();
                        writeToFile = fileLine + "\n";
                        File.WriteAllText(path, writeToFile);
                        if (Console.ReadKey().Key == ConsoleKey.E && (Console.ReadKey().Modifiers & ConsoleModifiers.Control) != 0)
                        {
                            writeToFile();
                            break;
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine("Command not found");
            }
        }
        public void listDir()
        {
            try
            {
                var dirs = Directory.GetDirectories(currentPath);
                foreach (var dir in dirs)
                { 
                    Console.WriteLine("<DIR> " + dir);
                } 
                var files = Directory.GetFiles(currentPath);
                foreach (var file in files)
                { 
                    Console.WriteLine(file);
                } 
            } 
            catch (Exception error)
            { 
                Console.WriteLine("Error listing directory: " + error.Message);
            }
        }
        void wait(int ms)
        {
            var start = DateTime.Now; while ((DateTime.Now - start).TotalMilliseconds < ms)
            {
              
            }
        }
        string evaluatePath(string input)
        {
            if (startsWithNumber(input))
            {
                // Absolute path already
                return input;
            }
            else if (input.StartsWith("\\"))
            {
                // Relative to current drive root
                return currentPath + input;
            }
            else
            {
                // Relative to current directory
                return currentPath + "\\" + input;
            }
        }
        bool startsWithNumber(string num)
        {
            if (num.StartsWith("0") || num.StartsWith("1") || num.StartsWith("2") || num.StartsWith("3") || num.StartsWith("4") || num.StartsWith("5") || num.StartsWith("6") || num.StartsWith("7") || num.StartsWith("8") || num.StartsWith("9"))
            {
                return true;
            }

            return false;
        }
        public void writeToFile(string fileContent, FileStream fileStream)
        {
            byte[] buffer = new byte[fileContent.Length];
            var openFile = fileStream.WriteAsync();
        }
    }
}
