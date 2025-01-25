using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.FileManager;

namespace AutoCRUD.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IFileManager _fileManager;

        public PropertyService(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public List<PropertyInfo> ExtractPropertiesFromEntity(string entityPath)
        {

            if (string.IsNullOrEmpty(entityPath) || !File.Exists(entityPath))
                throw new ArgumentException("Invalid entity file path.");

            var properties = new List<PropertyInfo>();

            // Read the file content
            string entityContent = _fileManager.ReadTemplate(entityPath);

            // Parse the entity using Roslyn
            var syntaxTree = CSharpSyntaxTree.ParseText(entityContent);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            if (root == null)
                throw new InvalidDataException("Invalid or empty syntax tree.");

            var compilation = CSharpCompilation.Create("TempCompilation")
     .AddReferences(
         MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // Core mscorlib
         MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
         MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location) // System.Collections.Generic
     )
     .AddSyntaxTrees(syntaxTree);


            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Get the first class declaration (assuming one class per file)
            var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classNode == null)
                throw new InvalidDataException("No class found in the provided file.");

            var hasIdProperty = false;

            // Process all members (properties and fields)
            foreach (var memberNode in classNode.DescendantNodes())
            {
                if (memberNode is PropertyDeclarationSyntax propertyNode)
                {
                    properties.Add(ProcessProperty(propertyNode, semanticModel, ref hasIdProperty));
                }
                else if (memberNode is FieldDeclarationSyntax fieldNode)
                {
                    properties.Add(ProcessField(fieldNode, semanticModel));
                }
            }

            // Ensure the "Id" property exists
            if (!hasIdProperty)
            {
                properties.Add(new PropertyInfo
                {
                    Name = "Id",
                    Type = typeof(string), // Default to string for the "Id" property
                    Attributes = new List<string>()
                });
            }

            return properties;
        }
        public List<PropertyInfo> ExtractPropertiesFromEntityTypeString(string entityPath)
        {
            // this could be in stratgy or somthing
            var properties = new List<PropertyInfo>();
            // Read the file content
            string entityContent = _fileManager.ReadTemplate(entityPath);

            // Parse the entity using Roslyn
            var syntaxTree = CSharpSyntaxTree.ParseText(entityContent);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            // Get the first class declaration (assuming one class per file)
            var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classNode == null)
            {
                throw new InvalidDataException("No class found in the provided file.");
            }
            var hasIdProperty = false;
            // Extract properties
            foreach (var propertyNode in classNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                var propertyName = propertyNode.Identifier.Text;
                var propertyType = propertyNode.Type.ToString();

                var attributes = propertyNode.AttributeLists
                    .SelectMany(attrList => attrList.Attributes)
                    .Select(attr => attr.ToString())
                    .ToList();

                properties.Add(new PropertyInfo
                {
                    Name = propertyName,
                    StringType = propertyType,
                    Attributes = attributes
                });
                if (string.Equals(propertyName, "Id", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(propertyType, "Guid", StringComparison.OrdinalIgnoreCase))
                {
                    hasIdProperty = true;
                }
            }

            if (!hasIdProperty)
            {
                properties.Add(new PropertyInfo
                {
                    Name = "Id",
                    StringType = "string",
                    Attributes = new List<string>()
                });
            }
            return properties;
        }

        private PropertyInfo ProcessProperty(PropertyDeclarationSyntax propertyNode, SemanticModel semanticModel, ref bool hasIdProperty)
        {
            var propertyName = propertyNode.Identifier.Text;
            var typeSymbol = semanticModel.GetTypeInfo(propertyNode.Type).Type;
            var resolvedType = ResolveType(typeSymbol);

            var attributes = propertyNode.AttributeLists
                .SelectMany(attrList => attrList.Attributes)
                .Select(attr => attr.ToString())
                .ToList();

            if (string.Equals(propertyName, "Id", StringComparison.OrdinalIgnoreCase) || resolvedType == typeof(Guid))
            {
                hasIdProperty = true;
            }

            return new PropertyInfo
            {
                Name = propertyName,
                Type = resolvedType,
                Attributes = attributes
            };
        }
        private Type? ResolveType(ITypeSymbol? typeSymbol)
        {
            if (typeSymbol == null)
            {
                Console.WriteLine("TypeSymbol is null, unable to resolve type.");

                return null;
            }

            // Handle primitive and common types
            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_Boolean:
                    return typeof(bool);
                case SpecialType.System_Char:
                    return typeof(char);
                case SpecialType.System_SByte:
                    return typeof(sbyte);
                case SpecialType.System_Byte:
                    return typeof(byte);
                case SpecialType.System_Int16:
                    return typeof(short);
                case SpecialType.System_UInt16:
                    return typeof(ushort);
                case SpecialType.System_Int32:
                    return typeof(int);
                case SpecialType.System_UInt32:
                    return typeof(uint);
                case SpecialType.System_Int64:
                    return typeof(long);
                case SpecialType.System_UInt64:
                    return typeof(ulong);
                case SpecialType.System_Single:
                    return typeof(float);
                case SpecialType.System_Double:
                    return typeof(double);
                case SpecialType.System_Decimal:
                    return typeof(decimal);
                case SpecialType.System_String:
                    return typeof(string);
                case SpecialType.System_DateTime:
                    return typeof(DateTime);
                case SpecialType.System_Object:
                    return typeof(object);

            }
            if (typeSymbol.ToDisplayString() == "System.Guid")
            {
                return typeof(Guid);

            }

            // Handle nullable types
            if (typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                var underlyingType = ((INamedTypeSymbol)typeSymbol).TypeArguments.FirstOrDefault();

                if (underlyingType != null)
                {
                    return ResolveType(underlyingType);
                }

                throw new InvalidOperationException("Nullable type has no underlying type.");
            }

            //// Handle nullable types
            //if (typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            //{
            //    var underlyingType = ((INamedTypeSymbol)typeSymbol).TypeArguments.FirstOrDefault();
            //    return ResolveType(underlyingType);
            //}

            // Handle array types
            if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
            {
                var elementType = ResolveType(arrayTypeSymbol.ElementType);
                return elementType?.MakeArrayType();
            }

            // Handle generic types
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                // Check if the generic type is List<>
                if (namedTypeSymbol.ConstructedFrom.ToDisplayString() == "System.Collections.Generic.List<T>")
                {
                    // Resolve the type arguments for List<>
                    var elementType = ResolveType(namedTypeSymbol.TypeArguments.FirstOrDefault());
                    if (elementType != null)
                    {
                        return typeof(List<>).MakeGenericType(elementType);
                    }
                }

                // Handle other generic types
                var genericTypeName = namedTypeSymbol.ConstructedFrom.ToDisplayString();
                var genericTypeDefinition = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName == genericTypeName);

                if (genericTypeDefinition != null)
                {
                    var genericArguments = namedTypeSymbol.TypeArguments
                        .Select(ResolveType)
                        .Where(t => t != null)
                        .ToArray();

                    return genericTypeDefinition.MakeGenericType(genericArguments!);
                }

                //Console.WriteLine($"Could not resolve generic type: {genericTypeName}");
            }

            // Attempt to resolve custom or external types
            return Type.GetType(typeSymbol.ToDisplayString());
        }

        private PropertyInfo ProcessField(FieldDeclarationSyntax fieldNode, SemanticModel semanticModel)
        {
            var variable = fieldNode.Declaration.Variables.FirstOrDefault();
            if (variable == null) throw new InvalidDataException("Field node does not contain a variable.");

            var fieldName = variable.Identifier.Text;
            var typeSymbol = semanticModel.GetTypeInfo(fieldNode.Declaration.Type).Type;
            var resolvedType = ResolveType(typeSymbol);

            return new PropertyInfo
            {
                Name = fieldName,
                Type = resolvedType,
                Attributes = new List<string>() // Fields typically don't have attributes
            };
        }
    }
}
