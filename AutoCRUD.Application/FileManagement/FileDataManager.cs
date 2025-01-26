using AutoCRUD.Application.Application;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.Models.Extensions;
using AutoCRUD.Domain.Models.Attributes;
using AutoCRUD.Application.Mappings;
using System.IO;
using AutoCRUD.Domain;
namespace AutoCRUD.Application.FileManagement
{
    public class FileDataManager
    {
        public List<FileModel> SetFiles(FileModel fileDTO)
        {
            var fileContentTypes = new[]
                  {
                         FileContentType.Template,
                        FileContentType.OutputFile,
                        FileContentType.Resource
                    };

            var list = fileContentTypes
                .Select(contentType => SetFileInformation(fileDTO, contentType))
                .ToList();

            return list;
        }
        public FileModel SetReferenceEntityFile(FileModel fileDTO)
        {

            var temp = fileDTO.TemplateConfig;
            var fileModel = EnumsMapping.GetTemplatePath(fileDTO, FileContentType.ReferenceEntity);

            fileDTO.TemplateConfig = temp;
            return fileModel;
        }
        private FileModel SetFileInformation(FileModel fileDTO, FileContentType fileContentType)
        {

            var strategy = FileContentTypeStrategyFactory.GetStrategy(fileContentType);
            var fileModel = strategy.GetTemplatePath(fileDTO, fileContentType);

            return fileModel;
        }
        public List<FileModel> FetchModelFilesFromDirectory(string folderPath, string model)
        {
            model = model.Trim();
            List<FileModel> modelFiles = new();
            // List to hold the file paths
            List<string> filePaths = new List<string>();
            var modelPath = Path.Combine(folderPath, model);
            try
            {
                // Filter model files if a specific model is provided
                if (model.Equals("all"))
                {
                    modelPath = folderPath;
                    filePaths.AddRange(Directory.GetFiles(modelPath, "*.cs")); 
                }
                else
                {
                    var models = model.Split(',');
                    foreach (var item in models)
                    {
                        filePaths.Add(Path.Combine(folderPath, item + ".cs"));
                    }
                }

                // Get all file paths in the specified folder
                Console.WriteLine("File paths retrieved successfully!");
                Console.WriteLine($"Number of files: {filePaths.Count}");

                // Optionally display the file paths
                foreach (var path in filePaths)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                    var file = new FileModel()
                    {
                        TemplateConfig = new TemplateConfig() { Name = model },
                        Path = path,
                        Entity = fileNameWithoutExtension
                    };
                    modelFiles.Add(file);
                    Console.WriteLine(path);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception($"can not find Models in: {modelPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return modelFiles;
        }
    }
}
