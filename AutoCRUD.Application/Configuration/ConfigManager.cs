using AutoCRUD.Application.Configuration;
using AutoCRUD.Domain;
using AutoCRUD.Domain.FileManager;
using AutoCRUD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoCRUD.Application.Configuration
{
    public static class ConfigManager
    { 
        public static List<TemplateConfig> LoadConfiguration(string configPath)
        {
            if (string.IsNullOrWhiteSpace(configPath))
            {
                configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            }
            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Configuration file not found at {configPath}. Please enter the correct path:");
                configPath = Console.ReadLine()?.Trim() ?? string.Empty;
                //throw new FileNotFoundException($"Configuration file not found at {configPath}");
            }

            var configContent = File.ReadAllText(configPath);

            return JsonSerializer.Deserialize<RootConfig>(configContent)?.TemplateCategories
                   ?? throw new InvalidOperationException("Failed to deserialize configuration file.");
        }

        public static bool TemplateHasAttribute(string attributeName, TemplateConfig templateConfigs)
        {
            return templateConfigs?.Attributes.Contains(attributeName) ?? false;
        }
    }

}