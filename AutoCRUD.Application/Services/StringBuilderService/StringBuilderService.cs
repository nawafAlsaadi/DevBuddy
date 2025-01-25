using AutoCRUD.Domain;
using AutoCRUD.Domain.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AutoCRUD.Domain.Models;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;
using System.IO;
using System.Reflection.Emit;
using AutoCRUD.Domain.Interfaces;
using PropertyInfo = AutoCRUD.Domain.Models.PropertyInfo;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Application.Services.Strategies;
using Humanizer;

namespace AutoCRUD.Application.Services.StringBuilderService
{
    public class StringBuilderService : IStringBuilderService
    {
        private readonly BuildTemplateStrategySelector _strategySelector;

        public StringBuilderService(BuildTemplateStrategySelector strategySelector)
        {
            _strategySelector = strategySelector;
        }

        public string BuildTemplate(string? templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            string input = templateData.TemplateConfig.Name;
            TemplateCategory category = Enum.Parse<TemplateCategory>(input, true);
            var strategy = _strategySelector.GetStrategy(category);
            return strategy.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
        private string GetSimplifiedType(ITypeSymbol? typeSymbol)
        {
            if (typeSymbol == null)
            {
                Console.WriteLine("TypeSymbol is null, unable to resolve type.");
                return "object"; // Default fallback type
            }

            // Use ToDisplayString to get a simplified type name with C# aliases
            var format = SymbolDisplayFormat.CSharpErrorMessageFormat
                .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted) // Remove namespace
                .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.UseSpecialTypes); // Use C# aliases

            return typeSymbol.ToDisplayString(format);
        }
        // public List<PropertyInfo> ExtractPropertiesFromEntity(string entityPath)
        //{
        //    var properties = new List<PropertyInfo>();

        //    // Read the file content
        //    string entityContent = File.ReadAllText(entityPath);

        //    // Parse the entity using Roslyn
        //    var syntaxTree = CSharpSyntaxTree.ParseText(entityContent);
        //    var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

        //    // Get the first class declaration (assuming one class per file)
        //    var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
        //    if (classNode == null)
        //    {
        //        throw new InvalidDataException("No class found in the provided file.");
        //    }

        //    var hasIdProperty = false;

        //    // Extract properties and fields
        //    foreach (var memberNode in classNode.DescendantNodes())
        //    {
        //        // Handle properties
        //        if (memberNode is PropertyDeclarationSyntax propertyNode)
        //        {
        //            var propertyName = propertyNode.Identifier.Text;
        //            var propertyType = propertyNode.Type.ToString();

        //            var attributes = propertyNode.AttributeLists
        //                .SelectMany(attrList => attrList.Attributes)
        //                .Select(attr => attr.ToString())
        //                .ToList();

        //            properties.Add(new PropertyInfo
        //            {
        //                Name = propertyName,
        //                Type = propertyType,
        //                Attributes = attributes
        //            });

        //            if (string.Equals(propertyName, "Id", StringComparison.OrdinalIgnoreCase) ||
        //                string.Equals(propertyType, "Guid", StringComparison.OrdinalIgnoreCase))
        //            {
        //                hasIdProperty = true;
        //            }
        //        }
        //        // Handle fields
        //        else if (memberNode is FieldDeclarationSyntax fieldNode)
        //        {
        //            var variable = fieldNode.Declaration.Variables.FirstOrDefault();
        //            if (variable != null)
        //            {
        //                var fieldName = variable.Identifier.Text;
        //                var fieldType = fieldNode.Declaration.Type.ToString();

        //                properties.Add(new PropertyInfo
        //                {
        //                    Name = fieldName,
        //                    Type = fieldType,
        //                    Attributes = new List<string>() // Fields typically don't have attributes
        //                });
        //            }
        //        }
        //    }

        //    // Add default "Id" property if not present
        //    if (!hasIdProperty)
        //    {
        //        properties.Add(new PropertyInfo
        //        {
        //            Name = "Id",
        //            Type = "string",
        //            Attributes = new List<string>()
        //        });
        //    }

        //    return properties;
        //}

        private int OpeningBraceIndex(string templateContent, string entity, string path)
        {
            // Improved regex to find the class declaration
            var classDeclarationRegex = new Regex(@"public\s+class\s+\w+(\s*:\s*[\w<>\s,]+)?\s*{", RegexOptions.Singleline);
            var match = classDeclarationRegex.Match(templateContent);

            if (!match.Success)
            {
                Console.WriteLine("Template Content: ");
                Console.WriteLine(templateContent);
                throw new InvalidDataException("Class declaration not found in the template. Ensure the template file contains a proper class definition.");
            }

            // Locate the position of the opening curly brace '{'
            int openingBraceIndex = match.Index + match.Length - 1;

            return openingBraceIndex;
            //return   templateContent.Insert(openingBraceIndex + 1, $"\n{PropertiesString}\n");
            // Inject the properties after the opening brace


        }


       
        
    }

}
