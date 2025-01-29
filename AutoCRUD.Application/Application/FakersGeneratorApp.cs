using AutoCRUD.Application.Configuration;
using AutoCRUD.Application.FileManagement;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Services;
using AutoCRUD.Domain.Models.Extensions;
using AutoCRUD.Domain.Models;
using Bogus;
using AutoCRUD.Domain.FileManager;
using System;
namespace AutoCRUD.Application.Application
{
    public class FakersGeneratorApp
    {
        // Fields
        private readonly UserInputService _userInputService;
        private readonly FileDataManager _fileDataManager;
        private readonly IFileManager _fileManager;
        private readonly IFakerRunnerServices _fakerRunnerServices;
        private readonly IPropertyService _propertyService;
        private readonly IFakerServices _fakerServices;

        // Static Properties
        public static string? RootPath { get; private set; }
        public static string SolutionName { get; private set; }
        public static string ModelsPath { get; private set; }
        public static string UsingModels { get; private set; }

        // Constructor
        public FakersGeneratorApp(UserInputService userInputService, FileDataManager fileDataManager, IFileManager fileManager, IFakerRunnerServices fakerRunnerServices, IPropertyService propertyService, IFakerServices fakerServices)
        {
            _fileDataManager = fileDataManager;
            _userInputService = userInputService;
            _fileManager = fileManager;
            _fakerRunnerServices = fakerRunnerServices;
            _propertyService = propertyService;
            _fakerServices = fakerServices;

        }

        // Initialization Methods
        /// <summary>
        /// Initializes root path and other derived paths.
        /// </summary>
        private void InitializePaths(string modelsPath)
        {

            RootPath = string.IsNullOrEmpty(RootPath)
                ? Directory.GetCurrentDirectory()
                : RootPath;

            SolutionName = ConfigHelper.GetSolutionName(RootPath);
            ModelsPath = string.IsNullOrEmpty(modelsPath) ? TemplateCategoryExtensions.GetModelsPath(SolutionName, RootPath) : modelsPath;
            UsingModels = ModelsPath.Replace(RootPath, string.Empty)
                                    .TrimStart(Path.DirectorySeparatorChar)
                                    .Replace(Path.DirectorySeparatorChar, '.')
                                    .Replace(Path.AltDirectorySeparatorChar, '.')
                                    .Trim('.');
        }

        // Public Methods
        /// <summary>
        /// Main method to generate Faker classes and the Faker runner.
        /// </summary>
        public void GenerateFakersClasses(string modelsPath, string outputPath, string model)
        {
            try
            {

                InitializePaths(modelsPath);
                var faker = new Faker();
                var fakerClasses = new List<string>();
                var modelFiles = _fileDataManager.FetchModelFilesFromDirectory(modelsPath, model);

                foreach (var modelFile in modelFiles)
                {
                    var properties = _propertyService.ExtractPropertiesFromEntity(modelFile.Path);
                    if (properties != null)
                    {
                        modelFile.Properties = properties;
                        fakerClasses.Add(modelFile.Entity);
                    }
                }
                var generatedClasses = SaveFakerClasses(modelFiles, outputPath, faker, fakerClasses);
                _fakerRunnerServices.GenerateFakerRunner(outputPath, generatedClasses, fakerClasses, UsingModels);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Private Methods
        private List<string> SaveFakerClasses(List<FileModel> files, string outputPath, Faker faker, List<string> fakersClasses)
        {
            Directory.CreateDirectory(outputPath);
            return GenerateFakerClasses(files, outputPath, faker, fakersClasses);
        }

        private List<string> GenerateFakerClasses(List<FileModel> files, string outputPath, Faker faker, List<string> fakersClasses)
        {
            var classes = new List<string>();
            foreach (var file in files)
            {
                file.Path = outputPath;
                var content = GenerateFakerClassContent(file, faker, fakersClasses);
                if(!string.IsNullOrEmpty(content))
                {
                    var fileName = $"{file.Entity}Faker.cs";
                    var filePath = Path.Combine(outputPath, fileName);
                    _fileManager.WriteToFile(filePath, content);
                    Console.WriteLine($"Generated: {fileName}");
                    classes.Add($"{file.Entity}Faker");
                }
          
            }
            return classes;
        }


        private string GenerateFakerClassContent(FileModel file, Faker faker, List<string> fakersClasses)
        {
            if (file.Properties != null)
            {

                var filePath = Path.Combine(file.Path, $"{file.Entity}Faker.cs");
                var existingContent = _fileManager.FileExists(filePath) ? _fileManager.ReadTemplate(filePath) : string.Empty;

                if (!string.IsNullOrEmpty(existingContent))
                {
                    return _fakerServices.GeneratePropertyRules(file, existingContent, fakersClasses, faker);
                }
                return _fakerServices.CreateNewFakerClasse(UsingModels, file, fakersClasses, faker);
            }
            return string.Empty;
        }

    }
}
