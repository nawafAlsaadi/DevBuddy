using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Services.StringBuilderService;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Strategies.GeneratingViewModelTemplate
{
    public class GeneratingViewModel : IBuildTemplate
    {

        public string BuildTemplate(string templateContent, FileModel templateData2, FileModel outputFileData, List<PropertyInfo> properties)
        {
            var PropertiesString = GeneratePropertiesString(properties);

            templateContent = StringBuilderHelper.RemoveProperties(templateContent);
            templateContent = RemoveAllAfterClassOpeningBrace(templateContent);
            templateContent = templateContent + PropertiesString;
            templateContent = templateContent + GenerateConstructer(properties, outputFileData.Entity);
            templateContent = templateContent + GenerateToModelMethod(properties, outputFileData.Entity);
            templateContent = templateContent + GenerateEmptyConstructor(outputFileData.Entity) + "}}";
            return templateContent;
        }
        private string GeneratePropertiesString(List<PropertyInfo> properties)
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                
                // Ensure that Attributes is a collection (e.g., List<string>)
                if (property.Attributes is List<string> filteredAttributes)
                {
                    // Filter out ForeignKey attributes
                    var nonForeignKeyAttributes = filteredAttributes
                        .Where(attr => !attr.Contains("ForeignKey"))
                        .ToList();

                    // Add remaining attributes to the property
                    foreach (var attribute in nonForeignKeyAttributes)
                    {
                        sb.AppendLine($"    [{attribute}]");
                    }
                }

                // Add the property declaration
                sb.AppendLine($"    public {property.StringType} {property.Name} {{ get; set; }}");
                sb.AppendLine(); // Add a blank line after each property
            }

            return sb.ToString();
        }

        private string RemoveAllAfterClassOpeningBrace(string templateContent)
        {
            // Regex to match everything up to the opening `{` after the class declaration
            var removeAfterClassRegex = new Regex(
                @"(public\s+class\s+\w+\s*:\s*ViewModelBase<\w+>\s*\{)",
                RegexOptions.Singleline
            );

            // Find the class declaration and opening brace
            var match = removeAfterClassRegex.Match(templateContent);
            if (!match.Success)
            {
                throw new InvalidDataException("Class declaration with opening brace not found.");
            }

            // Get the position of the opening brace and retain content up to it
            int openingBraceIndex = match.Index + match.Length;

            // Return content up to and including the opening brace
            return templateContent.Substring(0, openingBraceIndex + 1);
        }

        private string GenerateEmptyConstructor(string entity)
        {
            var constructorString = new StringBuilder();
            constructorString.AppendLine($"    public {entity}ViewModel ()");
            constructorString.AppendLine("    {");
            constructorString.AppendLine("    }");


            return constructorString.ToString();
        }

        private string GenerateToModelMethod(List<PropertyInfo> properties, string entityName)
        {
            //string className = entityName + TemplateConfig.ViewModel;

            // Generate the ToModel method
            var toModelMethod = new StringBuilder();
            toModelMethod.AppendLine($"    public override {entityName} ToModel()");
            toModelMethod.AppendLine("    {");
            toModelMethod.AppendLine($"        return new {entityName}()");
            toModelMethod.AppendLine("        {");

            // Add properties to the ToModel method
            foreach (var property in properties)
            {
                toModelMethod.AppendLine($"            {property.Name} = {property.Name},");
            }

            // Remove the trailing comma for the last property
            int lastCommaIndex = toModelMethod.ToString().LastIndexOf(',');
            if (lastCommaIndex != -1)
            {
                toModelMethod.Remove(lastCommaIndex, 1);
            }

            toModelMethod.AppendLine("        };");
            toModelMethod.AppendLine("    }");
            toModelMethod.AppendLine(); // Add a blank line for formatting
            return toModelMethod.ToString();

        }
        private string GenerateConstructer(List<PropertyInfo> properties, string entityName)
        {
            //string className = entityName + TemplateConfig.ViewModel;

            // Generate the ToModel method
            var toModelMethod = new StringBuilder();
            toModelMethod.AppendLine($"    public {entityName}ViewModel({entityName}  entity) ");
            toModelMethod.AppendLine("    {");
            toModelMethod.AppendLine(" if (entity == null)  return;");

            // Add properties to the ToModel method
            foreach (var property in properties)
            {
                toModelMethod.AppendLine($"            this.{property.Name} = entity.{property.Name};");
            }
            toModelMethod.AppendLine("        }");
            toModelMethod.AppendLine(); // Add a blank line for formatting
            return toModelMethod.ToString();

        }
    }
}
