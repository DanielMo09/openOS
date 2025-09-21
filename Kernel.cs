using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;

namespace openOS
{
    public class Kernel : Sys.Kernel
    {

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("openOS has booted sucessfully");
        }

        protected override void Run()
        {
            Console.Write("> ");
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

            else
            {
                Console.WriteLine("Command not found");
            }
        }
        public void listDir()
        {

        }
    }
}
