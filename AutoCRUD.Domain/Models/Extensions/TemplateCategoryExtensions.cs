using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Domain.Models.Extensions
{
    public static class TemplateCategoryExtensions
    {
        public static TemplateInfoAttribute GetTemplateInfo(this TemplateCategory category)
        {
            var memberInfo = typeof(TemplateCategory).GetMember(category.ToString()).FirstOrDefault();
            return memberInfo?.GetCustomAttribute<TemplateInfoAttribute>();
        }

        public static string GetTemplatePath(this TemplateCategory category, string solutionName, string entityName)
        {
            var templateInfo = category.GetTemplateInfo();
            if (templateInfo == null)
                throw new InvalidOperationException("TemplateInfoAttribute is missing for the specified category.");

            var layer = templateInfo.Layer.ToString();
            var subFolder = string.IsNullOrEmpty(templateInfo.SubFolder) ? string.Empty : $"/{templateInfo.SubFolder}";
            var fileName = templateInfo.FileNameFormat;

            string pathWithOutRoot = $"{solutionName}.{layer}{subFolder}/{fileName}".Replace("{solutionName}", solutionName)
                .Replace("{entityName}", entityName);

            return pathWithOutRoot;
        }
       
       
        public static string GetFileName(this TemplateCategory category,string entityName)
        {
            var templateInfo = category.GetTemplateInfo();
            if (templateInfo == null)
                throw new InvalidOperationException("TemplateInfoAttribute is missing for the specified category.");
            return  templateInfo.FileNameFormat.Replace("{entityName}", entityName); ;

        }
        public static string GetModelsPath(string solutionName,string root)
        
        {
            TemplateCategory category = TemplateCategory.Model;
            var templateInfo = category.GetTemplateInfo();
            if (templateInfo == null)
                throw new InvalidOperationException("TemplateInfoAttribute is missing for the specified category.");

            var layer = templateInfo.Layer.ToString();
            var subFolder = string.IsNullOrEmpty(templateInfo.SubFolder) ? string.Empty : $"/{templateInfo.SubFolder}";

            string pathWithOutRoot = Path.Combine(root, $"{solutionName}.{layer}{subFolder}".Replace("{solutionName}", solutionName));
                
            return pathWithOutRoot;

        }
        public static string GetDataPath(string solutionName, string root)

        {
            TemplateCategory category = TemplateCategory.Data;
            var templateInfo = category.GetTemplateInfo();
            if (templateInfo == null)
                throw new InvalidOperationException("TemplateInfoAttribute is missing for the specified category.");

            var layer = templateInfo.Layer.ToString();
            var subFolder = string.IsNullOrEmpty(templateInfo.SubFolder) ? string.Empty : $"/{templateInfo.SubFolder}";

            string pathWithOutRoot = Path.Combine(root, $"{solutionName}.{layer}{subFolder}".Replace("{solutionName}", solutionName));

            return pathWithOutRoot;

        }
        
    }
}
