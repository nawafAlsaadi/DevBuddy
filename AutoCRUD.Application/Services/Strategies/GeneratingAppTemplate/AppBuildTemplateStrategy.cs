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
    public class AppBuildTemplateStrategy : IBuildTemplateStrategy
    {
        private readonly IBuildTemplate _appBuildTemplateStrategy;
        public AppBuildTemplateStrategy(GeneratingApp generatingApp)
        {
            _appBuildTemplateStrategy = generatingApp;
        }

        public bool CanHandle(TemplateCategory templateCategory)
        {
            return templateCategory == TemplateCategory.App;
        }

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            return _appBuildTemplateStrategy.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
    }


}
