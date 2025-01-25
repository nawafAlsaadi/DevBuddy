﻿using AutoCRUD.Application.Application;
using AutoCRUD.Application.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Presentation
{
    internal class GenerateCRUDCommand
    {
        private readonly CodeGeneratorApp _app;

        public GenerateCRUDCommand(CodeGeneratorApp codeGeneratorApp)
        {
            _app = codeGeneratorApp;
        }
        public async Task GenerateCRUD(string[] args)
        {
            Console.WriteLine("Welcome to AutoCRUD! Your code generation assistant.");

            if (_app == null)
            {
                Console.WriteLine("Error: Code generation service is missing.");
                return;
            }

            var rootCommand = BuildCodeGenerationCommand(_app);
            await rootCommand.InvokeAsync(args);

        }
        private static Command BuildCodeGenerationCommand(CodeGeneratorApp codeGeneratorApp)
        {

            var entityOption = new Option<string>(new[] { "--entity", "-e" }, "Name of the entity.") { IsRequired = true };
            var referenceEntityOption = new Option<string>(new[] { "--referenceEntity", "-re" }, "Enter the name of the base entity to copy from") { IsRequired = true }; ;
            var templatesOption = new Option<string>(
                aliases: new[] { "-t", "--templates" },
                description: "Specify the template(s) to generate. Use 'all' to generate all available templates, or provide a comma-separated list such as 'Controller,ViewModel'."
            )
            { IsRequired = true };
            var rootPathOption = new Option<string>("--rootPath", "The root path for file generation.");
            var configOption = new Option<string>("--config", "Path to a configuration file (optional).") { IsRequired = true };

            var command = new Command("generate-CRUD", "Generate CRUD for an entity,  using a base entity as a template.")
            {
                entityOption,
            referenceEntityOption,
            templatesOption,
            rootPathOption,
            configOption
        };

            command.SetHandler((string entity, string referenceEntity, string templates, string rootPath, string configPath) =>
            {

                Console.WriteLine($"Loading configuration from {configPath}...");
                var templateConfigs = ConfigManager.LoadConfiguration(configPath);

                codeGeneratorApp.RunCRUDApp(entity, referenceEntity, templates, rootPath, configPath, templateConfigs);


                // Run the application logic
                //referenceEntity = string.IsNullOrWhiteSpace(referenceEntity) ? "Address" : referenceEntity.Trim();
                //codeGeneratorApp.RunCRUDApp(entity, referenceEntity, templates, RootPath);
            }, entityOption, referenceEntityOption, templatesOption, rootPathOption, configOption);

            return command;
        }

    }
}
