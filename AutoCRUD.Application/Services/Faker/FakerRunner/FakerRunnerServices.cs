using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Faker.FakerRunner
{
    public class FakerRunnerServices : IFakerRunnerServices
    {
        private readonly IFileManager _fileManager;

        public FakerRunnerServices(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public void GenerateFakerRunner(string outputPath, List<string> registeredFakers, List<string> fakerClasses, string UsingModels)
        {

            var existingContent = _fileManager.FileExists(outputPath) ? _fileManager.ReadTemplate(outputPath) : string.Empty;
            if (true)
            //if (!string.IsNullOrEmpty(existingContent))
            {
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "FakerRunner.txt");
                var templateContent = _fileManager.ReadTemplate(templatePath);

                var placeholders = new Dictionary<string, string>
            {
                { "{RegisteredFakers}", string.Join(Environment.NewLine, registeredFakers.Select(f => @$"_fakers.Add(new {f}(""ar""));")) },
                { "{FakerInstances}", string.Join(Environment.NewLine, registeredFakers.Select(f => @$"new {f}(""ar""),")) },
                { "{usingModels}", UsingModels },
                { "{varName}", string.Join(Environment.NewLine, fakerClasses.Select(f => @$"var {f.ToLower().Replace("Faker","")}Faker = new {f}Faker(""ar"");")) },
{
    "{classes}",
    string.Join(Environment.NewLine, fakerClasses.Select(f =>
        @$"  
          var  {f.ToLower()}Data  =   {f.ToLower()}Faker.Generate(recordCount); 
        if({f.ToLower()}Data.FirstOrDefault() != null &&  !AreAllPropertiesNull({f.ToLower()}Data.First())){{
                    dbContext.AddRange({f.ToLower()}Data);  
            }}"
    ))
}            };

                foreach (var placeholder in placeholders)
                {
                    templateContent = templateContent.Replace(placeholder.Key, placeholder.Value);
                }

                var outputFilePath = Path.Combine(outputPath, "FakerRunner.cs");
                _fileManager.WriteToFile(outputFilePath, templateContent);
                Console.WriteLine($"FakerRunner.cs has been generated at: {outputFilePath}");
            }
            else
            {
                var outputFilePath = Path.Combine(outputPath, "FakerRunner.cs");

                string fileContent = File.ReadAllText(outputFilePath);


                string methodPattern = @$"public\s+static\s+void\s+ RunFakerToDatabase \s*\(.*?\)\s*{{(.*?)}}";
                Match match = Regex.Match(fileContent, methodPattern, RegexOptions.Singleline);


            }

        }



    }
}
