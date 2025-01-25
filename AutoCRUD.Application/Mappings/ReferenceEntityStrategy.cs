using AutoCRUD.Application.Configuration;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.Models.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace AutoCRUD.Application.Mappings
{
    public class ReferenceEntityStrategy : IFileContentTypeStrategy
    {
        public FileModel GetTemplatePath(FileModel fileDTO, FileContentType fileContentType)
        {
            // Use JSON configuration to get the template path
            var categoryName = TemplateCategory.Model.ToString(); // Example: Fixed to "Model"
            var solutionName = ConfigHelper.GetSolutionName(fileDTO.RootPath);
            var entityName = fileDTO.Entity; 
            var path = TemplatePathResolver.GetTemplatePath(categoryName, solutionName, entityName, fileDTO);
            var normalizedPath = Path.Combine(fileDTO.RootPath, path).Replace('/', '\\');

            return new FileModel
            {
                Path = normalizedPath,
                Entity = entityName,
                FileContentType = fileContentType,
               TemplateConfig = categoryName.GetTemplateConfig(fileDTO), 
            };
        }
    }
  

}
