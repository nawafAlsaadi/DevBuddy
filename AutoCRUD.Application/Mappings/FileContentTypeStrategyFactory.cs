using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Mappings
{
    public class FileContentTypeStrategyFactory
    {
        public static IFileContentTypeStrategy GetStrategy(FileContentType fileContentType)
        {
            return fileContentType switch
            {
                FileContentType.Template => new TemplateStrategy(),
                FileContentType.Resource => new ResourceStrategy(),
                FileContentType.OutputFile => new OutputFileStrategy(),
                FileContentType.ReferenceEntity => new ReferenceEntityStrategy(),
                _ => throw new ArgumentException("Unsupported FileContentType")
            };
        }
    }

}
