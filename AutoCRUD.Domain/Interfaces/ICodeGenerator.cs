using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;

namespace AutoCRUD.Domain.Interfaces;

public interface ICodeGenerator
{
    void GenerateTemplatePage(FileModel file, List<TemplateConfig> templateConfigs);
}