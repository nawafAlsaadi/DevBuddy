using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Strategies.GeneratingViewModelTemplate
{
    public class ViewModelBuildTemplateStrategy : IBuildTemplateStrategy
    {
        private readonly IBuildTemplate _generatingViewModel;

        public ViewModelBuildTemplateStrategy(GeneratingViewModel generatingViewModel)
        {
            _generatingViewModel = generatingViewModel;
        }

        public bool CanHandle(TemplateCategory templateCategory)
        {
            return templateCategory == TemplateCategory.ViewModel;
        }

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            return _generatingViewModel.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
    }

}
