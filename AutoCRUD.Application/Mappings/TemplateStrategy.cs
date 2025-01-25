using AutoCRUD.Application.Configuration;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;

namespace AutoCRUD.Application.Mappings
{
    public class TemplateStrategy : IFileContentTypeStrategy
    {
        public FileModel GetTemplatePath(FileModel fileDTO, FileContentType fileContentType)
        {
            var categoryName = fileDTO.TemplateConfig.ToString(); // Use the template type
            var solutionName = ConfigHelper.GetSolutionName(fileDTO.RootPath);
            var entityName = fileDTO.BasedEntity;

            // Get the path from configuration
            var path = TemplatePathResolver.GetTemplatePath(categoryName, solutionName, entityName,fileDTO);
            var normalizedPath = Path.Combine(fileDTO.RootPath, path).Replace('/', '\\');

            return new FileModel
            {
                Path = normalizedPath,
                Entity = entityName,
                FileContentType = fileContentType,
                TemplateConfig = fileDTO.TemplateConfig
            };
        }
    
    }

}
