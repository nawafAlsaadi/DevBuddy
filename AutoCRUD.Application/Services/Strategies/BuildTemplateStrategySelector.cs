using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Services.Strategies
{
    public class BuildTemplateStrategySelector
    {
        private readonly IEnumerable<IBuildTemplateStrategy> _strategies;

        public BuildTemplateStrategySelector(IEnumerable<IBuildTemplateStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IBuildTemplateStrategy GetStrategy(TemplateCategory templateCategory)
        {
            return _strategies.FirstOrDefault(s => s.CanHandle(templateCategory))
                ?? throw new ArgumentException($"No strategy found for template category: {templateCategory}");
        }
    }

}
