using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Services.StringBuilderService.Faker;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Faker
{
    public class FakerServices : IFakerServices
    {
        public string GeneratePropertyRules(FileModel file, string existingContent, List<string> fakersClasses, Bogus.Faker faker)
        {
            var properties = file.Properties
                    .Where(property => !existingContent.Contains(property.Name))
                    .ToList();

            var constructorMarker = $"public {file.Entity}Faker()";
            var insertionIndex = existingContent.IndexOf(constructorMarker);
            if (insertionIndex >= 0)
            {
                var braceIndex = existingContent.IndexOf("{", insertionIndex) + 1;
                var rules = FakerHelper.GetRolesForProperties(properties, fakersClasses, faker);
              return  existingContent = existingContent.Insert(braceIndex, rules.ToString());
            }

            return "";

        }
        public string CreateNewFakerClasse(string UsingModels , FileModel file, List<string> fakersClasses, Bogus.Faker faker)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Bogus;");
            sb.AppendLine("using System;");
            sb.AppendLine($"using {UsingModels};");
            sb.AppendLine();
            sb.AppendLine($"public class {file.Entity}Faker : Faker<{file.Entity}>");
            sb.AppendLine("{");
            sb.AppendLine($"    public {file.Entity}Faker (string locale) : base(locale)");
            sb.AppendLine("    {");
            sb.Append(FakerHelper.GetRolesForProperties(file.Properties, fakersClasses, faker));
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
      
    }
}
