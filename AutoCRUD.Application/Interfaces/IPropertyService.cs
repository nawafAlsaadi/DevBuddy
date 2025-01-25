using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Interfaces
{
    public interface IPropertyService
    {
        List<PropertyInfo> ExtractPropertiesFromEntity(string entityPath);
        List<PropertyInfo> ExtractPropertiesFromEntityTypeString(string entityPath);
    }
}
