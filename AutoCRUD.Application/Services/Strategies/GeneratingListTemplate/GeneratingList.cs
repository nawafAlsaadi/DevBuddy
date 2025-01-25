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
    public class GeneratingList : IBuildTemplate
    {

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            // Extract valid properties (string or int types)
            var validProperties = properties.Where(p =>
                StringBuilderHelper.GetType(p) == "String or Nubmer"
                && !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(p.Name, "Guid", StringComparison.OrdinalIgnoreCase)
                && !HandelAbnormalCases.IsForeignKey(p.Name)
             ).ToList();

            // Build `<thead>` content dynamically
            var headerBuilder = new StringBuilder();
            headerBuilder.AppendLine("<tr>");
            foreach (var property in validProperties)
            {

                headerBuilder.AppendLine("    <th>");
                headerBuilder.AppendLine($"        @CommonText.{property.Name}");
                headerBuilder.AppendLine("    </th>");
            }
            headerBuilder.AppendLine("    <th class=\"text-center\">");
            headerBuilder.AppendLine("        @CommonText.Actions");
            headerBuilder.AppendLine("    </th>");
            headerBuilder.AppendLine("</tr>");

            // Build `<tbody>` content dynamically
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("@foreach (var item in Model.Result)");
            bodyBuilder.AppendLine("{");
            bodyBuilder.AppendLine("    <tr>");
            foreach (var property in validProperties)
            {
                bodyBuilder.AppendLine("        <td>");
                bodyBuilder.AppendLine($"            @item.{property.Name}");
                bodyBuilder.AppendLine("        </td>");
            }
            // Keep the "Actions" column intact
            bodyBuilder.AppendLine("        <td class=\"text-sm-center\">");
            bodyBuilder.AppendLine("            <a asp-action=\"Edit\" asp-route-id=\"@item.Id\" class=\"btn btn-sm btn-brand m-btn m-btn--icon\">");
            bodyBuilder.AppendLine("                <i class=\"fa fa-edit\"></i>");
            bodyBuilder.AppendLine("                @CommonText.Edit");
            bodyBuilder.AppendLine("            </a>");
            bodyBuilder.AppendLine("            <a class=\"btn btn-sm btn-danger m-btn m-btn--icon\" asp-ajax=\"true\" asp-confirm=\"true\" asp-action=\"Delete\" asp-ajax-success-method=\"updateListFromDiv('#list');\" asp-route-id=\"@item.Id\" asp-ajax-block=\"#list\">");
            bodyBuilder.AppendLine("                <i class=\"fa fa-trash\"></i>");
            bodyBuilder.AppendLine("                @CommonText.Delete");
            bodyBuilder.AppendLine("            </a>");
            bodyBuilder.AppendLine("        </td>");
            bodyBuilder.AppendLine("    </tr>");
            bodyBuilder.AppendLine("}");

            // Locate `<thead>` and `<tbody>` in the template and replace content
            templateContent = StringBuilderHelper.ReplaceBetweenTags(templateContent, "thead", headerBuilder.ToString());
            templateContent = StringBuilderHelper.ReplaceBetweenTags(templateContent, "tbody", bodyBuilder.ToString());

            return templateContent;
        }



    }
}
