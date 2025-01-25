using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Services.StringBuilderService;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Strategies
{
    public class GeneratingForm : IBuildTemplate
    {
        public string BuildTemplate(string templateContent, FileModel referenceEntityData, FileModel outputFileData, List<PropertyInfo> properties)
        { 
            var entity = outputFileData.Entity;

            // Initialize the template
            var template = new StringBuilder();
            template.AppendLine($"@model {entity}ViewModel");
            template.AppendLine($"@inject I{entity}Service {entity}Service;\r\n");
            template.AppendLine("@{");
            template.AppendLine("}");
            template.AppendLine();
            template.AppendLine("<input type=\"hidden\" asp-for=\"Id\"/>");

            // Generate input groups
            for (int i = 0; i < properties.Count; i += 2)
            {
                var firstProperty = properties[i];
                var secondProperty = i + 1 < properties.Count ? properties[i + 1] : null;

                // Check if the first and second properties are valid
                bool isFirstValid = firstProperty.Name != "Id" && !HandelAbnormalCases.IsForeignKey(firstProperty.Name);
                bool isSecondValid = secondProperty != null && secondProperty.Name != "Id" && !HandelAbnormalCases.IsForeignKey(secondProperty.Name);

                // Create a row only if there is at least one valid property
                if (isFirstValid || isSecondValid)
                {
                    template.AppendLine("<div class=\"row\">");

                    // Add the first property
                    if (isFirstValid)
                    {
                        template.AppendLine("    <div class=\"col-md-6\">");
                        template.AppendLine(GenerateTemplateForProperty(firstProperty));
                        template.AppendLine("    </div>");
                    }

                    // Add the second property, if valid
                    if (isSecondValid)
                    {
                        template.AppendLine("    <div class=\"col-md-6\">");
                        template.AppendLine(GenerateTemplateForProperty(secondProperty));
                        template.AppendLine("    </div>");
                    }

                    template.AppendLine("</div>");
                }
            }

            return template.ToString();
        }

        private static string GenerateTemplateForProperty(dynamic property)
        {
            var propertyType = property.Type?.ToLowerInvariant();
            propertyType ??= property.StringType;
            // Shared result for most simple types
            if (StringBuilderHelper.GetType(property) == "String or Nubmer")
            {
                return $"<bootstrap-input-form-group for=\"{property.Name}\" />";
            }
            // Special case for DateTime
            else if (StringBuilderHelper.GetType(property) == "datetime")
            {
                return $@"  <datetime-picker asp-for=""{property.Name}""
                                             asp-hijri-default=""false""
                                             asp-show-switch=""true""
                                             asp-view-mode=""Days""
                                             asp-show-close=""false""
                                             asp-select-mode=""Date"" />";
            }
            // Special case for Boolean
            else if (StringBuilderHelper.GetType(property) == "bool")
            {
                return $@"
<div class=""form-group"">
    <label class=""kt-checkbox kt-checkbox--brand"">
        <input type=""checkbox"" asp-for=""{property.Name}"" />
        @Html.DisplayNameFor(a => a.{property.Name})
        <span></span>
        <span asp-validation-for=""{property.Name}""></span>
    </label>
</div>";
            }
            // Special case for Char
            //            else if (propertyType is "char" or "system.char" or "char?")
            //            {
            //                return $@"
            //<div class=""form-group"">
            //    <label>@Html.DisplayNameFor(a => a.{property.Name})</label>
            //    <input type=""text"" asp-for=""{property.Name}\"" maxlength=""1"" class=""form-control"" />
            //    <span asp-validation-for=""{property.Name}\"" class=""text-danger""></span>
            //</div>";
            //            }

            // Default for unknown types
            return StringBuilderHelper.GenerateDropdownTemplate(property); ;
            //return $"<!-- Template for {property.Type} is not defined -->";
        }

    }
}
