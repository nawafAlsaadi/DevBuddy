using System.CommandLine;
using AutoCRUD.Application.Application;
using AutoCRUD.Application.FileManagement;
using AutoCRUD.Application.FileManager;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Services;
using AutoCRUD.Application.Services.Strategies;
using AutoCRUD.Application.Services.Strategies.GeneratingViewModelTemplate;
using AutoCRUD.Application.Services.StringBuilderService;
using AutoCRUD.Domain.FileManager;
using AutoCRUD.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using AutoCRUD.Application.Configuration;
using AutoCRUD.Application.Services.Faker.FakerRunner;
using AutoCRUD.Application.Services.Faker;
using Microsoft.Extensions.Logging;

namespace AutoCRUD.Presentation;

class Program
{


    static async Task Main(string[] args)
    {

        Console.WriteLine("Warning: This tool creates and may overwrites existing files. Ensure you have committed your code in Git or a similar version control system to avoid loss of work.");
        Console.WriteLine("Do you want to continue? (yes/no)");

        string? response = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(response) || response.ToLower() != "yes")
        {
            Console.WriteLine("Operation cancelled by the user.");
            return;
        }


        if (args.Length == 0 || (args[0] != "generate-CRUD" && args[0] != "generate-Fakers"))
        {
            Console.WriteLine("Error: You must specify either 'generate-CRUD' or 'generate-Fakers'. Use --help for details.");
            return;
        }
        var services = ConfigureServices();
        ServiceProvider serviceProvider;
        serviceProvider = services.BuildServiceProvider();

        try
        {

            if (args[0] == "generate-CRUD")
            {
                var codeGeneratorApp = serviceProvider.GetService<CodeGeneratorApp>();
                var command = new GenerateCRUDCommand(codeGeneratorApp!);
                if (codeGeneratorApp == null)
                {
                    Console.WriteLine("Error: Code generation service is missing.");
                    return;
                }
                    await command.GenerateCRUD(args);
            }
            else if (args[0] == "generate-Fakers")
            {

                var FakersGeneratorApp = serviceProvider.GetService<FakersGeneratorApp>();
                var FakersCommand = new GenerateFakersCommand(FakersGeneratorApp!);

                if (FakersGeneratorApp == null)
                {
                    Console.WriteLine("Error: Fakers generation service is missing.");
                    return;
                }
                await FakersCommand.GenerateFakers(args);
            }
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while executing the command.");
        }

    }



    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();


        // Domain-level and core services
        services.AddSingleton<CodeGeneratorApp>();

        services.AddSingleton<IFileManager, FileManager>();
        services.AddSingleton<FileDataManager>();
        services.AddSingleton<ICodeGenerator, CodeGenerator>();
        services.AddSingleton<CodeGeneratorApp>();
        services.AddSingleton<FakersGeneratorApp>();
        services.AddSingleton<UserInputService>();
        services.AddSingleton<TemplateSelectionService>();

        // StringBuilder and template generation services
        services.AddScoped<IStringBuilderService, StringBuilderService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IBuildTemplate, GeneratingForm>();
        services.AddScoped<IBuildTemplate, GeneratingViewModel>();
        services.AddScoped<IBuildTemplate, GeneratingSearchViewModel>();
        services.AddScoped<IBuildTemplate, GeneratingList>();
        services.AddScoped<IBuildTemplate, GeneratingApp>();
        services.AddScoped<IBuildTemplate, GeneratingData>();
        services.AddScoped<IBuildTemplateStrategy, FormBuildTemplateStrategy>();
        services.AddScoped<IBuildTemplateStrategy, ViewModelBuildTemplateStrategy>();
        services.AddScoped<IBuildTemplateStrategy, SearchViewModelBuildTemplateStrategy>();
        services.AddScoped<IBuildTemplateStrategy, ListBuildTemplateStrategy>();
        services.AddScoped<IBuildTemplateStrategy, AppBuildTemplateStrategy>();
        services.AddScoped<IBuildTemplateStrategy, DataBuildTemplateStrategy>();
        services.AddScoped<BuildTemplateStrategySelector>();
        services.AddScoped<GeneratingForm>();

        services.AddScoped<GeneratingViewModel>();
        services.AddScoped<GeneratingSearchViewModel>();
        services.AddScoped<GeneratingData>();
        services.AddScoped<GeneratingList>();
        services.AddScoped<GeneratingApp>();

        services.AddScoped<IResourceFilesService, ResourceService>();
        services.AddScoped<IFakerRunnerServices, FakerRunnerServices>();
        services.AddScoped<IFakerServices, FakerServices>();

        return services;
    }
}
