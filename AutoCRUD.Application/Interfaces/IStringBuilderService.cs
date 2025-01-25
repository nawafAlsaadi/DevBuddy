using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyInfo = AutoCRUD.Domain.Models.PropertyInfo;

namespace AutoCRUD.Application.Interfaces
{
    public interface IStringBuilderService
    {
        string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties);
        //List<PropertyInfo> ExtractPropertiesFromEntity(string entityPath);// Extract properties from an entity
        //List<PropertyInfo> ExtractPropertiesFromEntityTypeString(string entityPath);// Extract properties from an entity
        //string   plural(string name);
    }
}
