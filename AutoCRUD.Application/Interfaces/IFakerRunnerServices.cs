using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Interfaces
{
    public interface IFakerRunnerServices
    {
        void GenerateFakerRunner(string outputPath, List<string> registeredFakers, List<string> fakerClasses,string UsingModels);

    }
}
