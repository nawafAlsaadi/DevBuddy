using AutoCRUD.Domain.Models.Attributes;

namespace AutoCRUD.Domain.Enums
{ // all this need to be deleted 
    public enum TemplateCategory 
    {
        [ReadOnly]
        [TemplateInfo(ProjectLayer.Domain, "Models", "{entityName}.cs")]
        Model,

        [TemplateInfo(ProjectLayer.Persistance, "Data", "{entityName}s.json")]
        //[HalfTemplate] // off on 
        [UnReadable]
        Data, 

        [HalfTemplate]
        [TemplateInfo(ProjectLayer.Web, "ViewModels", "{entityName}ViewModel.cs")]
        ViewModel,

        [TemplateInfo(ProjectLayer.Web, "ViewModels/Search", "{entityName}SearchViewModel.cs")]
        SearchViewModel,

        [HalfTemplate]
        [TemplateInfo(ProjectLayer.Web, "Views/{entityName}", "_List.cshtml")]
        _List,

        [HalfTemplate]
        [TemplateInfo(ProjectLayer.Web, "Views/{entityName}", "_Form.cshtml")]
        _Form,

        [TemplateInfo(ProjectLayer.Core, "Resources", "CommonText.resx")]
        CommonText,

        [AppendOnly]
        [TemplateInfo(ProjectLayer.Application, "", "{solutionName}App.cs")] // No subfolder for 'App'
        App

    }
   
    public enum ProjectLayer
    {
        Web,
        Application,
        Domain,
        Core,
        Persistance
    } 
   public enum FileContentType
    {
        Template,
        OutputFile,
        ReferenceEntity,
        Resource
    } 
   

}
