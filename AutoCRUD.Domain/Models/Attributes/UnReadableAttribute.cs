using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Domain.Models.Attributes
{

    [AttributeUsage(AttributeTargets.Field)] // Restricts this attribute to enum fields
    public class UnReadableAttribute : Attribute
    {
    }
}
