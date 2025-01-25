using AutoCRUD.Domain.Enums;

namespace AutoCRUD.Domain.Models
{
    public class FileModel
    {
        public FileContentType FileContentType { get; set; }
        public TemplateConfig TemplateConfig { get; set; }        
        public string Entity { get; set; }
        public string Path { get; set; }
        public string RootPath { get; set; }
        public string BasedEntity { get; set; }
     
        public List<PropertyInfo> Properties { get; set; }

        public FileModel()
        {

        }
        public FileModel(string rootPath, TemplateConfig templateType, string entity, string basedEntity)
        {
            RootPath = rootPath;
            Entity = entity;
            BasedEntity = basedEntity;
            TemplateConfig = templateType;
        }
    }
}
