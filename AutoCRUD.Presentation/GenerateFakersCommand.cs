using AutoCRUD.Application.Application;
using AutoCRUD.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Presentation
{
    public class GenerateFakersCommand
    {
        private readonly FakersGeneratorApp _app;

        public GenerateFakersCommand(FakersGeneratorApp app)
        {
            _app = app;
        }
        public async Task GenerateFakers(string[] args)
        {
            Console.WriteLine("Welcome to AutoFaker! Your code generation assistant.");

            if (_app == null)
            {
                Console.WriteLine("Error: Code generation service is missing.");
                return;
            }

            var rootCommand = BuildFakerCommand(_app);
            await rootCommand.InvokeAsync(args);

        }
        private static Command BuildFakerCommand(FakersGeneratorApp FakersGeneratorApp)
        {
            var modelsPathOption = new Option<string>(aliases: new[] { "-mp", "--modelsPath" }, description: "The path to the models folder.") { IsRequired = true };
            var outputPathOption = new Option<string>(aliases: new[] { "-op", "--outputPath" }, description: "The path where the Fakers classes will be generated.") { IsRequired = true };
            var modelOption = new Option<string>(aliases: new[] { "-m", "--model" }, description: "The name of the model to generate or 'all' for all models.") { IsRequired = true };

            var command = new Command("Fakers", "Generate Fakers classes for models.")
        {
            modelsPathOption,
            outputPathOption,
            modelOption
        };

            command.SetHandler((string modelsPath, string outputPath, string model) =>
            {
                FakersGeneratorApp.GenerateFakersClasses(modelsPath, outputPath, model);
            }, modelsPathOption, outputPathOption, modelOption);

            return command;
        }

    }
}
