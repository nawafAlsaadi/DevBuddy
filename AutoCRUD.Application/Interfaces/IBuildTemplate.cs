using AutoCRUD.Domain.Models;

namespace AutoCRUD.Application.Interfaces
{
    public interface IBuildTemplate
    {
        string BuildTemplate(string templateContent, FileModel templateData, FileModel outputFileData, List<PropertyInfo> properties);

    }
}
