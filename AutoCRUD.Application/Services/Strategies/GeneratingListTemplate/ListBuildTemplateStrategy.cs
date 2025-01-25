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
    public class ListBuildTemplateStrategy : IBuildTemplateStrategy
    {
        private readonly IBuildTemplate _generatingList;

        public ListBuildTemplateStrategy(GeneratingList generatingList)
        {
            _generatingList = generatingList;
        }

        public bool CanHandle(TemplateCategory templateCategory)
        {
            return templateCategory == TemplateCategory._List;
        }

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            return _generatingList.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
    }

}
