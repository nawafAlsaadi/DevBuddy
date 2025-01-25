using AutoCRUD.Application.Configuration;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain;
namespace AutoCRUD.Application.Mappings
{
    public class ResourceStrategy : IFileContentTypeStrategy
    {
            public FileModel GetTemplatePath(FileModel fileDTO, FileContentType fileContentType)
            {
                var categoryName = fileDTO.TemplateConfig.ToString(); // Get the category name dynamically
                var solutionName = ConfigHelper.GetSolutionName(fileDTO.RootPath);
                var entityName = fileDTO.BasedEntity;

                // Use TemplatePathResolver to get the path
                string path = Path.Combine(fileDTO.RootPath, TemplatePathResolver.GetTemplatePath(categoryName, solutionName, entityName, fileDTO));
                var normalizedPath = path.Replace('/', '\\');

                return new FileModel
                {
                    Path = normalizedPath,
                    Entity = entityName,
                    FileContentType = fileContentType,
                    TemplateConfig = new TemplateConfig() { Name = fileDTO.TemplateConfig.Name }
                };
            }

    //public string GenerateFileNameWithExtension(FileDTO fileDTO)
    //{
    //    return "CommonText.en.resx";
    //}

    //public string GetSubFolder(TemplateConfig type, string addSubFolder = "")
    //{
    //    return "Resources";
    //}

    //public string GetLayerName(TemplateConfig type, string RootPath)
    //{
    //    var solutionName = ConfigHelper.GetSolutionName(RootPath);
    //    return $"{solutionName}.{ProjectLayer.Core}";
    //}
    //public string getFilePath(FileDTO fileDTO)
    //{
    //    return Path.Combine(fileDTO.RootPath, fileDTO.ProjectLayer, fileDTO.SubFolder, fileDTO.FileName);

    //}
}

}
