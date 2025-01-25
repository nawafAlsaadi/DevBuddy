using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Interfaces
{
    public  interface IFakerServices
    {
        string GeneratePropertyRules(FileModel file, string existingContent, List<string> fakersClasses, Bogus.Faker faker);
        string CreateNewFakerClasse(string UsingModels, FileModel file, List<string> fakersClasses, Bogus.Faker faker);
    }
}
