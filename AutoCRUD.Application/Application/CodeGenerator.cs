using AutoCRUD.Domain.Interfaces;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.FileManager;
using AutoCRUD.Domain.Models;
using AutoCRUD.Application.FileManagement;
using AutoCRUD.Domain.Models.Extensions;
using AutoCRUD.Application.Services.StringBuilderService;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Configuration;
using AutoCRUD.Domain;
using Humanizer;
namespace AutoCRUD.Application.Application;
public class CodeGenerator : ICodeGenerator
{
    #region Private Fields and Constructor
    private readonly IFileManager _fileManager;
    private readonly FileDataManager _fileDataManager;
    private readonly IStringBuilderService _StringBuilderService;
    private readonly IResourceFilesService _resourceService;
    private readonly IPropertyService _propertyService;

    public CodeGenerator(IResourceFilesService resourceService, IFileManager fileManager, IStringBuilderService stringBuilderService, FileDataManager fileDataManager, IPropertyService propertyService)
    {
        _resourceService = resourceService;
        _fileManager = fileManager;
        _StringBuilderService = stringBuilderService;
        _fileDataManager = fileDataManager;
        _propertyService = propertyService;
    }
    #endregion

    public void GenerateTemplatePage(FileModel file, List<TemplateConfig> templateConfigs)
    {
        foreach (var templateConfig in templateConfigs)
        {
            try
            {
                file.TemplateConfig = templateConfig;
                var files = _fileDataManager.SetFiles(file);
                EnsureReferenceEntityFile(file, files, templateConfigs);

                var outputFileData = files.FirstOrDefault(z => z.FileContentType == FileContentType.OutputFile);

                CheckingFiles(files, out var error);

                if (!error)
                {
                    var templateContent = GetTemplateContent(files);
                    if (!templateConfig.Attributes.Contains("ReadOnly")) _fileManager.WriteToFile(outputFileData?.Path!, templateContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing template '{templateConfig.Name}': {ex.Message}");
            }
        }
    }
    private string GetTemplateContent(List<FileModel> files)
    {
        #region Retrieve Required Files
        var templateData = files.FirstOrDefault(f => f.FileContentType == FileContentType.Template);
        var outputFileData = files.FirstOrDefault(f => f.FileContentType == FileContentType.OutputFile);
        var ReferenceEntityData = files.FirstOrDefault(f => f.FileContentType == FileContentType.ReferenceEntity);

        // Validate the presence of required files
        if (templateData == null || outputFileData == null || ReferenceEntityData == null)
        {
            LogMissingFileDetails(templateData!, outputFileData!, ReferenceEntityData!);
            return string.Empty;
        }
        #endregion

        string templateContent = string.Empty;
        var properties = _propertyService.ExtractPropertiesFromEntityTypeString(ReferenceEntityData.Path);

        if (!ConfigManager.TemplateHasAttribute("UnReadable", templateData!.TemplateConfig))
        {    // Read template content if it is not marked as UnReadable
            templateContent = _fileManager.ReadTemplate(templateData.Path);
        }

        if (templateData.TemplateConfig.Name == TemplateCategory.CommonText.ToString())
        {    // Handle CommonText template
            AddingResourse(files, properties);
            return string.Empty;
        }
        else if (ConfigManager.TemplateHasAttribute("HalfTemplate", templateData.TemplateConfig) ||
              (ConfigManager.TemplateHasAttribute("AppendOnly", templateData.TemplateConfig)))
        {       // Logic for handling the specific customization
            templateContent = _StringBuilderService.BuildTemplate(templateContent, templateData, outputFileData!, properties);
        }
        if (ConfigManager.TemplateHasAttribute("FullTemplate", templateData.TemplateConfig) ||
            (ConfigManager.TemplateHasAttribute("HalfTemplate", templateData.TemplateConfig)))
        {
            var placeholders = new Dictionary<string, string>
        {
            { $"{templateData.Entity?.Pluralize()}", $"{outputFileData!.Entity?.Pluralize()}" },
            { $"{templateData.Entity}", $"{outputFileData.Entity}" },
            { $"{templateData.Entity?.ToLowerInvariant()}", $"{outputFileData.Entity?.ToLowerInvariant()}" }
        };
            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace(placeholder.Key, placeholder.Value);
            }
        }
        return templateContent;
    }
    private void LogMissingFileDetails(FileModel templateData, FileModel outputFileData, FileModel referenceEntityData)
    {
        if (templateData == null)
            Console.WriteLine("Template file is missing.");
        if (outputFileData == null)
            Console.WriteLine("Output file is missing.");
        if (referenceEntityData == null)
            Console.WriteLine("Base entity file is missing.");
    }
    private FileModel EnsureReferenceEntityFile(FileModel fileDTO, List<FileModel> files, List<TemplateConfig> templateConfigs)
    {
        if (!files.Any(z => z.FileContentType == FileContentType.ReferenceEntity))
        {
            fileDTO.TemplateConfig = templateConfigs.FirstOrDefault(a => a.Name == "Model")!;
            if (fileDTO.TemplateConfig != null)
            {
                var referenceEntityData = _fileDataManager.SetReferenceEntityFile(fileDTO);
                referenceEntityData.Properties = _propertyService.ExtractPropertiesFromEntityTypeString(referenceEntityData.Path);
                files.Add(referenceEntityData);
                return referenceEntityData;
            }
        }
        throw new InvalidOperationException("Failed to generate base entity file.");
    }
    private void CheckingFiles(List<FileModel> files, out bool error)
    {
        error = false;

        var templateData = files.FirstOrDefault(z => z.FileContentType == FileContentType.Template);
        var outputFileData = files.FirstOrDefault(z => z.FileContentType == FileContentType.OutputFile);
        var referenceEntityData = files.FirstOrDefault(z => z.FileContentType == FileContentType.ReferenceEntity);
        if (referenceEntityData == null)
        {
            Console.WriteLine("Base entity file is missing from config file.");
            error = true;
            return;
        }

        if (templateData == null)
        {
            Console.WriteLine("Template file is missing.");
            error = true;
            return;
        }
        error = CheckReferenceEntityExists(referenceEntityData);

        if (!ConfigManager.TemplateHasAttribute("EmbeddedTemplate", templateData.TemplateConfig))
        {

            _fileManager.ValidateFiles(Error: out error, shouldBeExist: true,
            templateData, $"Missing template at {templateData.Path} Please check and retry");
        }
    }
    private void AddingResourse(List<FileModel> files, List<PropertyInfo> properties)
    {
        FileModel resourceData = files.Where(z => z.FileContentType == FileContentType.Resource).FirstOrDefault()!;
        if (resourceData != null)
        {
            var referenceEntityData = files.Where(z => z.FileContentType == FileContentType.ReferenceEntity).FirstOrDefault();
            string filePath = resourceData.Path;
            _resourceService.AddResource(filePath, referenceEntityData?.Entity, properties);
            filePath = resourceData.Path;
        }
    }

    /// <summary>
    /// Checks if the base entity file exists and validates it.
    /// </summary>
    /// <param name="file">The file model representing the base entity.</param>
    /// <returns>
    /// Returns <c>true</c> if there was an error during validation; otherwise, <c>false</c>.
    /// </returns>
    private bool CheckReferenceEntityExists(FileModel file)
    {
        _fileManager.ValidateFiles(
            out var error,
            shouldBeExist: true,
            fileData: file,
            sorryMessage: $"The base entity '{file.Entity}' is missing in the path: {file.Path}.",
            StopIfFiled: true
        );
        return error;
    }
    private void BuildData(List<FileModel> fileModels, string root)
    {
        // off for now
        string content = string.Empty;
        var solutionName = ConfigHelper.GetSolutionName(root);
        var modelsPath = TemplateCategoryExtensions.GetModelsPath(solutionName, root);
        var dataPath = TemplateCategoryExtensions.GetDataPath(solutionName, root);
        // todo : need to clean all this
        var ModelsFiles = _fileDataManager.FetchModelFilesFromDirectory(modelsPath, "");
        foreach (var ModelFile in ModelsFiles)
        {
            var properties = _propertyService.ExtractPropertiesFromEntity(ModelFile.Path);
            content = _StringBuilderService.BuildTemplate("", ModelFile, null!, properties);
            string rename = StringBuilderHelper.plural(ModelFile.Entity);
            var jsonFilePath = Path.Combine(dataPath, rename + ".json");
            ModelFile.Path = jsonFilePath;
            //todo good massges 
            //make commenmesses
            _fileManager.ValidateFiles(Error: out var error, shouldBeExist: false,
             ModelFile, $"there is json already  {jsonFilePath}.   ");
            if (!error) _fileManager.WriteToFile(jsonFilePath, content);

        }
    }

}
