using AutoCRUD.Domain.FileManager;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.FileManager
{
    public class FileManager : IFileManager
    {
        public string ReadTemplate(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Template file not found at {path}");
            }
            return File.ReadAllText(path);
        }

        public void WriteToFile(string path, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

#if DEBUG
            // In debug mode, always overwrite the file
            File.WriteAllText(path, content);
                Console.WriteLine($"[DEBUG MODE]File saved at {path}");
#else
    // In release mode, ask for confirmation if the file exists
    if (File.Exists(path))
    {
        Console.WriteLine($"The file '{path}' already exists. Do you want to overwrite it? (y/n): ");
        var userInput = Console.ReadLine()?.Trim().ToLower();
        if (userInput != "y" && userInput != "yes")
        {
            Console.WriteLine("Skipping file overwrite.");
            return; // Exit without writing
        }
    }
    File.WriteAllText(path, content);
Console.WriteLine($"File saved at {path}.");
#endif
            }
        }
        public void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        public void ValidateFiles(out bool Erorr,bool shouldBeExist, FileModel fileData, string sorryMessage = "", bool StopIfFiled = false)
        {
            Erorr = false;

            var find = FileExists(fileData.Path);
            if (!shouldBeExist)
            {
                if (find) // to avoid dublcated 
                {
                    Console.WriteLine(sorryMessage);
                    Exception(StopIfFiled);
                    Erorr = true;
                    return ;
                }
            }
            else if (shouldBeExist)
            {
 
                if (!find) // to avoid reading for null file
                {
                    Erorr = true;

                    Console.WriteLine(sorryMessage);
                    Exception(StopIfFiled);
                    return;
                }
 
            }
        }
        private void Exception(bool StopIfFiled)
        {
            if (StopIfFiled)            
                Environment.Exit(0);            
        }
        public bool FileExists(string? path)
        {
            
            return File.Exists(path);

        }
    }

}
