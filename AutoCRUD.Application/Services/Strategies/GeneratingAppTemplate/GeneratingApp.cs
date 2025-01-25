using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Models;
using ICSharpCode.Decompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Strategies
{
    public class GeneratingApp : IBuildTemplate
    {
        public string BuildTemplate(string templateContent, FileModel ReferenceEntityData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            if (string.IsNullOrEmpty(templateContent) || ReferenceEntityData == null || outputFileData == null || properties == null)
                throw new ArgumentException("Invalid arguments provided.");
            
            var entity = outputFileData.Entity;
            string newLine = $"            services.AddTransient<I{entity}Service, {entity}Service>();";



            // Find the starting index of "Initialize"
            int initializeIndex = templateContent.IndexOf("Initialize");

            // Find the index of the opening brace `{` for the Initialize method
            int openingBraceIndex = templateContent.IndexOf("{", initializeIndex);

            // Insert the new line immediately after the opening brace
            if (openingBraceIndex != -1)
            {
                int insertIndex = openingBraceIndex + 1; // Move one character past the `{`
             return   templateContent = templateContent.Insert(insertIndex, "\n" + newLine); // Add newline for formatting
            }
            // Insert the new line before the closing brace
            //string updatedTemplate = templateContent.Insert(insertIndex, "\n" + newLine);

            return templateContent;


        }



    }
}
