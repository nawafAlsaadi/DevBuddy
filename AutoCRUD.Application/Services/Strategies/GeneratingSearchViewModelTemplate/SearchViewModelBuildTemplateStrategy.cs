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
    public class SearchViewModelBuildTemplateStrategy : IBuildTemplateStrategy
    {
        private readonly IBuildTemplate _generatingSearchViewModel;

        public SearchViewModelBuildTemplateStrategy(GeneratingSearchViewModel  generatingSearchViewModel)
        {
            _generatingSearchViewModel = generatingSearchViewModel;
        }

        public bool CanHandle(TemplateCategory templateCategory)
        {
            return templateCategory == TemplateCategory.SearchViewModel;
        }

        public string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties)
        {
            return _generatingSearchViewModel.BuildTemplate(templateContent, templateData, outputFileData, properties);
        }
    }

}
