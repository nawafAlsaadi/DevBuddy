using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Strategies
{
    public class DataBuildTemplateStrategy : IBuildTemplateStrategy
    {
        private readonly IBuildTemplate _dataBuildTemplateStrategy;
        public DataBuildTemplateStrategy(GeneratingData generatingData)
        {
            _dataBuildTemplateStrategy = generatingData;
        }

        public bool CanHandle(TemplateCategory templateCategory)
        {
            return templateCategory == TemplateCategory.Data;
        }

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            
            return _dataBuildTemplateStrategy.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
    }


}
