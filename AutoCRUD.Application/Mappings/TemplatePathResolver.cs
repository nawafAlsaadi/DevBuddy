using AutoCRUD.Domain;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Mappings
{
    internal static class TemplatePathResolver
    {
        public static string GetTemplatePath(string categoryName, string solutionName, string entityName, FileModel fileDTO)
        {
            // Retrieve the template configuration
            var templateConfig = categoryName.GetTemplateConfig(fileDTO);

            // Build the template path
            var layer = templateConfig.TemplateInfo.ProjectLayer;
            var subFolder = string.IsNullOrEmpty(templateConfig.TemplateInfo.Subfolder) ? string.Empty : $"/{templateConfig.TemplateInfo.Subfolder}";
            var fileName = templateConfig.TemplateInfo.FileNamePattern;

            return $"{solutionName}.{layer}{subFolder}/{fileName}"
                .Replace("{solutionName}", solutionName)
                .Replace("{entityName}", entityName);
        }
        public static TemplateConfig GetTemplateConfig(this string categoryName, FileModel fileDTO)
        {
            return fileDTO.TemplateConfig;
            //var config = configs.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            //var config = fileDTO.TemplateType.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            //if (config == null)
            //{
            //    throw new InvalidOperationException($"Template configuration not found for category '{categoryName}'.");
            //}
            //return config;
        }
    }
}
