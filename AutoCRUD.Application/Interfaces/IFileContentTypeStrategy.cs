using AutoCRUD.Domain;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;

namespace AutoCRUD.Application.Interfaces
{
    public interface IFileContentTypeStrategy
    {
        FileModel GetTemplatePath(FileModel fileDTO, FileContentType fileContentType ); 
       
    }

}
