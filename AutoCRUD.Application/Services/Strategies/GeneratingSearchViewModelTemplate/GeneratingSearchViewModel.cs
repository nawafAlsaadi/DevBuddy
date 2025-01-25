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

namespace AutoCRUD.Application.Services.Strategies
{
    public class GeneratingSearchViewModel : IBuildTemplate
    {

        public string BuildTemplate(string templateContent, FileModel templateData2, FileModel outputFileData, List<PropertyInfo> properties)
        {
            var PropertiesString = GeneratePropertiesString(properties);

            templateContent = StringBuilderHelper.RemoveProperties(templateContent);
            templateContent = RemoveAllAfterClassOpeningBrace(templateContent);
            templateContent = templateContent + PropertiesString;
            templateContent = templateContent + GenerateToSearchModel(properties, outputFileData.Entity) + "}}";
            //templateContent = templateContent + GenerateToModelMethod(properties, outputFileData.Entity);
            //templateContent = templateContent + GenerateEmptyConstructor(outputFileData.Entity) + "}}";
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
        @"(public\s+class\s+\w+\s*:\s*SearchViewModelBase<\w+>\s*\{)",
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

        private string GenerateToSearchModel(List<PropertyInfo> properties, string entityName)
        {
            var toModelMethod = new StringBuilder();

            // Start the ToSearchModel method
            toModelMethod.AppendLine($"    public override SearchCriteria<{entityName}> ToSearchModel()");
            toModelMethod.AppendLine("    {");

            // Add property-based filters
            foreach (var property in properties)
            {
                if (IsStringProperty(property))
                {
                    toModelMethod.AppendLine($"        if (!string.IsNullOrWhiteSpace({property.Name}))");
                    toModelMethod.AppendLine("        {");
                    toModelMethod.AppendLine($"            AddAndFilter(a => a.{property.Name} == {property.Name});");
                    toModelMethod.AppendLine("        }");
                }
            }
            // Return the base search model
            toModelMethod.AppendLine("        return base.ToSearchModel();");
            toModelMethod.AppendLine("    }");
            toModelMethod.AppendLine(); // Add a blank line for formatting

            return toModelMethod.ToString();
        }

        private bool IsStringProperty(PropertyInfo property)
        {
            return (property.Type?.Name?.ToLower().Contains("string") ?? false) ||
                   (property.StringType?.ToLower().Contains("string") ?? false);
        }
    }
}
