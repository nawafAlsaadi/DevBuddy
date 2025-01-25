using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.StringBuilderService
{
    internal static class HandelAbnormalCases
    {
        internal static string HandelIntId(string propertyType, string propertyName)
        {
            if (propertyName.ToLower().Equals("id"))
            {
                return $"      int id = 1;{Environment.NewLine} RuleFor(e => e.Id, _ => id++)"; // Create forCollection Randomizer instance"
            }
            return "f.Random.Int(0, 100)";
        }
        internal static bool HandleIdProperty(PropertyInfo property, int index, Dictionary<string, object> randomData)
        { // this will removed
            //if (property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (property.Type.Contains("int"))
            //    {
            //        randomData[property.Name] = index + 1; // Sequential int ID
            //    }
            //    else if (property.Type.Contains("string"))
            //    {
            //        randomData[property.Name] = $"0000-0000000-000000-0000{index + 1}"; // Sequential string ID
            //    }
            //    return true; // Handled
            //}
            return false; // Not handled
        }

        internal static bool HandleForeignKeyProperty(PropertyInfo property, Bogus.Faker faker, Dictionary<string, object> randomData, int max)
        { // this will removed
            //if (StringBuilderHelper.IsForeignKey(property.Name))
            //{
            //    if (property.Type.Contains("int"))
            //    {
            //        randomData[property.Name] = faker.Random.Int(1, max - 1); // Foreign key as int
            //    }
            //    else if (property.Type.Contains("string"))
            //    {
            //        randomData[property.Name] = $"0000-0000000-000000-0000{faker.Random.Int(1, max - 1)}"; // Foreign key as string
            //    }
            //    return true; // Handled
            //}
            return false; // Not handled
        }
        internal static bool IsForeignKey(string propertyName)
        {
            return propertyName.EndsWith("Id", StringComparison.Ordinal) && propertyName != "Id";
        }
        internal static bool IsIdKey(string propertyName)
        {
            return propertyName.Equals("id", StringComparison.OrdinalIgnoreCase);
        }
    }
}
