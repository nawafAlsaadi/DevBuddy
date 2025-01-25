using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Domain.Models
{
    public class PathInfo
    {
        public string ProjectLayer { get; set; }         // Layer (e.g., "Domain", "Web", etc.)
        public string Subfolder { get; set; }           // Subfolder path for the template
        public string FileNamePattern { get; set; }     // Filename pattern (e.g., "{entityName}.cs")
    }
}
