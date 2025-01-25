using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Domain
{
    public class TemplateConfig
    {
        public string Name { get; set; }                  // Name of the template
        public List<string> Attributes { get; set; }     // List of attributes like "ReadOnly", "HalfTemplate"
        public PathInfo TemplateInfo { get; set; }   // Info about the template file
    }

   
}