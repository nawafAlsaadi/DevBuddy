using AutoCRUD.Application.Configuration;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.Models.Attributes;
using AutoCRUD.Domain.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Mappings
{
    public class OutputFileStrategy : IFileContentTypeStrategy
    {
        public FileModel GetTemplatePath(FileModel fileDTO, FileContentType fileContentType)
        {
            var categoryName = fileDTO.TemplateConfig.ToString(); // Get the category name dynamically
            var solutionName = ConfigHelper.GetSolutionName(fileDTO.RootPath);
            var entityName = fileDTO.Entity;

            // Use TemplatePathResolver to get the path
            string path = Path.Combine(fileDTO.RootPath, TemplatePathResolver.GetTemplatePath(categoryName, solutionName, entityName, fileDTO));
            var normalizedPath = path.Replace('/', '\\');

            return new FileModel
            {
                Path = normalizedPath,
                Entity = entityName,
                FileContentType = fileContentType,
                TemplateConfig = fileDTO.TemplateConfig,
            };
        }

        public string GetTemplatePath(string categoryName, string solutionName, string entityName, List<TemplateConfig> configs)
        {
            throw new NotImplementedException();
        }
        //public string GenerateFileNameWithExtension(FileDTO fileDTO)
        //{
        //    var type = fileDTO.TemplateConfig;
        //    var typeName = type.ToString();
        //    var extension = EnumExtensions.GetFileExtension(type);

        //    if (type == TemplateConfig.App)
        //    {
        //        //return $"{solutionName}App.cs";

        //    }
        //    // Custom logic for OutputFile
        //    return $"{fileDTO.Entity}{extension}";
        //}

        //public string GetSubFolder(TemplateConfig type, string addSubFolder = "")
        //{
        //    // Custom subfolder logic for OutputFile
        //    if (type.HasAttribute<UnReadableAttribute>())
        //    {
        //        //type = TemplateConfig.View;
        //    }

        //    return type switch
        //    {
        //        TemplateConfig.Controller => "Controllers",
        //        TemplateConfig.ViewModel => "ViewModels",
        //        TemplateConfig.SearchViewModel => "ViewModels/Search",
        //        TemplateConfig.Service => "Services",
        //        TemplateConfig.IService => "Services",
        //        TemplateConfig.Model => "Models",
        //        //TemplateConfig.View => $"Views/{addSubFolder}",      

        //        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        //    };
        //}

        //public string GetLayerName(TemplateConfig type, string RootPath)
        //{
        //    var solutionName = ConfigHelper.GetSolutionName(RootPath);
        //    if (type.HasAttribute<UnReadableAttribute>())
        //    {
        //        //type = TemplateConfig.View;
        //    }
        //    return type switch
        //    {
        //        TemplateConfig.Controller => $"{solutionName}.{ProjectLayer.Web.ToString()}",
        //        TemplateConfig.ViewModel => $"{solutionName}.{ProjectLayer.Web.ToString()}",
        //        TemplateConfig.SearchViewModel => $"{solutionName}.{ProjectLayer.Web.ToString()}",
        //        TemplateConfig.Service => $"{solutionName}.{ProjectLayer.Application.ToString()}",
        //        TemplateConfig.IService => $"{solutionName}.{ProjectLayer.Domain.ToString()}",
        //        //TemplateConfig.View => $"{solutionName}.{ProjectLayer.Web.ToString()}",
        //        //TemplateConfig.Resource => $"{solutionName}.{ProjectLayer.Core.ToString()}",
        //        TemplateConfig.App => $"{solutionName}.{ProjectLayer.Application.ToString()}",

        //        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        //    };
        //}
        //public string getFilePath(FileDTO fileDTO)
        //{
        //    return Path.Combine(fileDTO.RootPath, fileDTO.ProjectLayer, fileDTO.SubFolder, fileDTO.FileName);

        //}
    }

}
