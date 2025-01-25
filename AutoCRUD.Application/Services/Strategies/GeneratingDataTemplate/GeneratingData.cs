using AutoCRUD.Application.Interfaces;
using AutoCRUD.Domain.Enums;
using AutoCRUD.Domain.Models;
using AutoCRUD.Domain.Models.Extensions;
using Bogus;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AutoCRUD.Application.Services.StringBuilderService.Faker;
namespace AutoCRUD.Application.Services.Strategies
{
    public class GeneratingData : IBuildTemplate
    {
        public const int Max = 21;
        public string BuildTemplate(string templateContent, FileModel referenceEntityData, FileModel outputFileData, List<PropertyInfo> properties)
        {

            var faker = new Bogus.Faker("ar");
            var randomDataList = FakerHelper.GetRandomData(faker,properties,Max) ;
           
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) // Prevent escaping Unicode
            };

            string jsonData = JsonSerializer.Serialize(randomDataList, options);
            string fileName = TemplateCategoryExtensions.GetFileName(TemplateCategory.Data, referenceEntityData.Entity);
            string result = @$"{jsonData}";

            return result;


        }

    }


}