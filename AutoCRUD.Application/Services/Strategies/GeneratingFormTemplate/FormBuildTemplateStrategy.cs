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
    public class FormBuildTemplateStrategy : IBuildTemplateStrategy
    {
        private readonly IBuildTemplate _generatingForm;

        public FormBuildTemplateStrategy(GeneratingForm generatingForm)
        {
            _generatingForm = generatingForm;
        }

        public bool CanHandle(TemplateCategory templateCategory)
        {
            return templateCategory == TemplateCategory._Form;
        }

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            return _generatingForm.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
    }


}
