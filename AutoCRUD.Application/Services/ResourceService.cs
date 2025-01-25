using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Models;
using Humanizer;

namespace AutoCRUD.Application.Services
{
    public class ResourceService : IResourceFilesService
    {
        public void AddResource(string filePath, string entityName, List<PropertyInfo> properties)
        {
          
            // Load the .resx file
             var doc = XDocument.Load(filePath);

            // Add entity name variants
            AddResourceIfNotExists(doc, entityName, entityName);
            AddResourceIfNotExists(doc, entityName.Pluralize(), entityName.Pluralize());
                foreach (var property in properties)
                { 
                    // Extract property name and value
                    var resourceName = property.Name;
                    var resourceValue = property.Name; // Assumes static properties

                    if (string.IsNullOrEmpty(resourceValue))
                        continue; // Skip properties with null or empty values

                    AddResourceIfNotExists(doc, resourceName, resourceValue); 
                }  
            // Save changes
            doc.Save(filePath); //this is right 
          //File.SetLastWriteTime(filePath, DateTime.Now);
        }
        private void AddResourceIfNotExists(XDocument doc, string resourceName, string resourceValue)
        {
            // Check if the resource already exists
            var existingElement = doc.Root
                .Elements("data")
                .FirstOrDefault(e => e.Attribute("name")?.Value == resourceName);
            Random random = new Random();
            if (existingElement == null)
            {
                // Add a new resource if it doesn't exist
                var newData = new XElement("data",
                    new XAttribute("name", resourceName),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", resourceValue)  
                );

                doc.Root.Add(newData);
            }
        }
    }
}
