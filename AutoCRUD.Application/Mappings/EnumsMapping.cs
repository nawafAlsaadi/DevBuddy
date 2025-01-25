using AutoCRUD.Application.Configuration;
using AutoCRUD.Domain;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Mappings
{
    public static class EnumsMapping
    {
        //public static FileModel GetTemplatePath(FileDTO fileDTO, FileContentType fileContentType)
        //{
        //    var strategy = FileContentTypeStrategyFactory.GetStrategy(fileContentType);
        //    return strategy.GetTemplatePath(fileDTO,fileContentType);
        // }
        public static FileModel GetTemplatePath(FileModel fileDTO, FileContentType fileContentType)
        {
      
            // Get the appropriate strategy for the file content type
            var strategy = FileContentTypeStrategyFactory.GetStrategy(fileContentType);

            // Pass the configuration to the strategy
            return strategy.GetTemplatePath(fileDTO, fileContentType);
        }
    }

}
