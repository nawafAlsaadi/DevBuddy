using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Interfaces;
using AutoCRUD.Application.Services;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain;
using AutoCRUD.Application.Configuration;

namespace AutoCRUD.Application.Application;

public class CodeGeneratorApp
{
    #region Private Fields and Constructor
    private readonly ICodeGenerator _codeGenerator;
    private readonly UserInputService _userInputService;
    private readonly TemplateSelectionService _templateSelectionService;
    public static string? RootPath { get; set; }
    private List<TemplateConfig> _templateConfigs { get; set; }

    public CodeGeneratorApp(
        ICodeGenerator codeGenerator,
        UserInputService userInputService,
        TemplateSelectionService templateSelectionService)
    {
            _codeGenerator = codeGenerator;
        _userInputService = userInputService;
        _templateSelectionService = templateSelectionService;
    }
    #endregion

    private List<TemplateConfig> TemplatesToList(string templatesInput, List<TemplateConfig> templateConfigs)
    {
        if (templatesInput.Equals("all", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(templatesInput))
        {
            // Return the Name property of all TemplateConfig objects
            return templateConfigs.ToList();
        }
        else
        {
            var templates = templatesInput.Split(',').Select(x => x.Trim()).ToList();
            return templateConfigs.Where(a => templates.Contains(a.Name, StringComparer.OrdinalIgnoreCase)).ToList();
        }
    }
    public void RunCRUDApp(string entityName, string referenceEntity, string templatesInput, string rootPath, string configPath, List<TemplateConfig> templateConfigs)
    {
         // Automatically set the root path to the current working directory
        rootPath = String.IsNullOrEmpty(rootPath) ? Directory.GetCurrentDirectory() : rootPath;
        
        Console.WriteLine($"Root Path Selected: {rootPath}. This will be the base location for all generated files. {rootPath}");

        var templates = TemplatesToList(templatesInput, templateConfigs);
        Console.WriteLine($"Generating   for {entityName}, based on {referenceEntity}");
        try
        {
            FileModel file = new FileModel(rootPath: rootPath, null, entity: entityName, basedEntity: referenceEntity);
            _codeGenerator.GenerateTemplatePage(file, templateConfigs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating  : {ex.Message}");
        }
        ExitApplication();   
    }


    private void ExitApplication()
    {
        Console.WriteLine("Thanks for chilling while I do the heavy lifting! Catch you later, My Buddy!");
        Environment.Exit(0);
    }

 
}
