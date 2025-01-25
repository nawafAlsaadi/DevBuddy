using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Domain.FileManager
{
    public interface IFileManager
    {
        string ReadTemplate(string path);
        void WriteToFile(string path, string content);
        void EnsureDirectoryExists(string directoryPath);
        bool FileExists(string? path);
        void ValidateFiles(out bool Error,bool shouldBeExist, FileModel fileData, string sorryMessage = "", bool StopIfFiled = false);
    }

}
