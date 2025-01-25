using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.StringBuilderService
{
    public class StringBuilderHelper
    {
     
        public static string ReplaceBetweenTags(string template, string tagName, string newContent)
        {
            // Find the start and end of the tag
            var startTag = $"<{tagName}>";
            var endTag = $"</{tagName}>";

            var startIndex = template.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
            var endIndex = template.IndexOf(endTag, StringComparison.OrdinalIgnoreCase);

            // If the tags are not found, return the original template
            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            {
                return template;
            }

            // Replace the content between the tags
            var beforeTag = template.Substring(0, startIndex + startTag.Length);
            var afterTag = template.Substring(endIndex);

            return beforeTag + Environment.NewLine + newContent + Environment.NewLine + afterTag;
        }
        public static string GetType(dynamic property)
        {
            var propertyType = property?.Type?.ToLowerInvariant();
            propertyType ??= property?.StringType;
            // Shared result for most simple types
            if (propertyType is "string" or "system.string" or "string?"
                 or "int" or "system.int32" or "int?"
                 or "float" or "system.float" or "float?"
                 or "double" or "system.double" or "double?"
                 or "decimal" or "system.decimal" or "decimal?"
                 or "byte" or "system.byte" or "byte?" or "char" or "system.char" or "char?")

                return "String or Nubmer";

            // Special case for DateTime
            else if (propertyType is "datetime" or "system.datetime" or "datetime?")
            {
                return "datetime";
            }
            // Special case for Boolean
            else if (propertyType is "bool" or "system.boolean" or "bool?")
            {
                return "bool";
            }
            // Special case for Char
            else if (propertyType is "char" or "system.char" or "char?")
            {
                return $@"
<div class=""form-group"">
    <label>@Html.DisplayNameFor(a => a.{property.Name})</label>
    <input type=""text"" asp-for=""{property.Name}\"" maxlength=""1"" class=""form-control"" />
    <span asp-validation-for=""{property.Name}\"" class=""text-danger""></span>
</div>";
            }

            // Default for unknown types
            return GenerateDropdownTemplate(property); ;
            //return $"<!-- Template for {property.Type} is not defined -->";
        }
        public static string GenerateDropdownTemplate(dynamic property)
        {
            var entityName = property?.Type; // E.g., "Country"
              entityName ??= property?.StringType; // E.g., "Country"
            var foreignKeyName = $"{property?.Name}Id"; // E.g., "CountryId"
            return "";
            string template = $@"<!--
<div class=""col-md-6"">
    <div class=""form-group"">
        <label asp-for=""{foreignKeyName}""></label>
        <select asp-for=""{foreignKeyName}""
                class=""form-control""
                asp-items=""@(new SelectList({entityName.ToLower()}s, nameof({entityName}.Id), nameof({entityName}.Something)))"">
            <option value="""">@CommonText.Select</option>
        </select>
    </div>
    <span asp-validation-for=""{foreignKeyName}"" />
</div>
-->";

            return CommentOutString(template);

        }
        private static string CommentOutString(string content)
        {
            // Split the content into lines, prefix each with `//`, then join them back
            var commentedLines = content.Split(Environment.NewLine)
                                         .Select(line => $"// {line}")
                                         .ToArray();
            return string.Join(Environment.NewLine, commentedLines);
        }

        public static string RemoveProperties(string templateContent)
        {
            // Regex to match properties with or without attributes, and handle irregular spacing
            var propertiesRegex = new Regex(@"(\s*\[[^\]]+\]\s*)*public\s+[\w<>\?]+\s+\w+\s*{[^}]*}", RegexOptions.Singleline);
            return propertiesRegex.Replace(templateContent, string.Empty);
        }

        public static string plural(string name)
        {

            return name.Pluralize(); 

        }

        //        public static string GenerateTemplateForProperty(dynamic property)
        //        {
        //            var propertyType = property.Type.ToLowerInvariant();

        //            // Shared result for most simple types
        //            if (propertyType is "string" or "system.string" or "string?"
        //                or "int" or "system.int32" or "int?"
        //                or "float" or "system.float" or "float?"
        //                or "double" or "system.double" or "double?"
        //                or "decimal" or "system.decimal" or "decimal?"
        //                or "byte" or "system.byte" or "byte?")
        //            {
        //                return $"<bootstrap-input-form-group for=\"{property.Name}\" />";
        //            }
        //            // Special case for DateTime
        //            else if (propertyType is "datetime" or "system.datetime" or "datetime?")
        //            {
        //                return $@"  <datetime-picker asp-for=""{property.Name}""
        //                                             asp-hijri-default=""false""
        //                                             asp-show-switch=""true""
        //                                             asp-view-mode=""Days""
        //                                             asp-show-close=""false""
        //                                             asp-select-mode=""Date"" />";
        //            }
        //            // Special case for Boolean
        //            else if (propertyType is "bool" or "system.boolean" or "bool?")
        //            {
        //                return $@"
        //<div class=""form-group"">
        //    <label class=""kt-checkbox kt-checkbox--brand"">
        //        <input type=""checkbox"" asp-for=""{property.Name}\"" />
        //        @Html.DisplayNameFor(a => a.{property.Name})
        //        <span></span>
        //        <span asp-validation-for=""{property.Name}\""></span>
        //    </label>
        //</div>";
        //            }
        //            // Special case for Char
        //            else if (propertyType is "char" or "system.char" or "char?")
        //            {
        //                return $@"
        //<div class=""form-group"">
        //    <label>@Html.DisplayNameFor(a => a.{property.Name})</label>
        //    <input type=""text"" asp-for=""{property.Name}\"" maxlength=""1"" class=""form-control"" />
        //    <span asp-validation-for=""{property.Name}\"" class=""text-danger""></span>
        //</div>";
        //            }

        //            // Default for unknown types
        //            return GenerateDropdownTemplate(property); ;
        //            //return $"<!-- Template for {property.Type} is not defined -->";
        //        }
    }

}
