using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Domain.Models
{
    public class PropertyInfo
    {
        public string Name { get; set; }
        public Type? Type { get; set; }
        public string? StringType { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();


    }
}
