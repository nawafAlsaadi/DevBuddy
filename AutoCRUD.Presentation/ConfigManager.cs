//using AutoCRUD.Application.Configuration;
//using AutoCRUD.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace AutoCRUD.Presentation
//{
//    public static class ConfigManager
//    {
//        public static List<TemplateConfig> LoadConfiguration(string configPath)
//        {
//            if (!File.Exists(configPath))
//            {
//                throw new FileNotFoundException($"Configuration file not found at {configPath}");
//            }

//            var configContent = File.ReadAllText(configPath);

//            return JsonSerializer.Deserialize<RootConfig>(configContent)?.TemplateCategories
//                   ?? throw new InvalidOperationException("Failed to deserialize configuration file.");
//        }
//    }

//    public class RootConfig
//    {
//        public List<TemplateConfig> TemplateCategories { get; set; }
//    }
//}