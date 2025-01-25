using Microsoft.Extensions.Configuration;

namespace AutoCRUD.Application.Configuration;

public static class ConfigHelper
{
    private static IConfiguration? _configuration;
    public static string _solutionName { get; set; }

    public static string GetSolutionName(string rootPath)
    {
        if (!string.IsNullOrEmpty(_solutionName)) 
            return _solutionName;
        //var RootPath = CodeGeneratorApp.RootPath;

        // Ensure the root path exists
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"The specified root path does not exist: {rootPath}");
        }

        // Look for a .sln file in the root directory
        var solutionFiles = Directory.GetFiles(rootPath, "*.sln", SearchOption.TopDirectoryOnly);

        // Ensure at least one .sln file is found
        if (solutionFiles.Length == 0)
        {
            Console.WriteLine($"No .sln file found in the directory: {rootPath}");
            Console.Write("Please enter the name of the .sln file: ");
            var solutionName = Console.ReadLine();

            // Validate the user input
            if (!string.IsNullOrEmpty(solutionName))
            {
                _solutionName = solutionName;
                 return _solutionName;
            }
            else
            {
                throw new FileNotFoundException($"No .sln file was provided. Unable to proceed.");
            }
        }
        _solutionName = Path.GetFileNameWithoutExtension(solutionFiles[0]);
        return _solutionName;
    }

    #region will be deleted
    public static IConfiguration InitializeConfiguration()
    {
        if (_configuration != null)
        {
            return _configuration;
        }

        // Load configuration from appsettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
        return _configuration;
    }

    public static string Get(string key, string defaultValue = "")
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration has not been initialized. Call InitializeConfiguration first.");
        }

        return _configuration[key] ?? defaultValue;
    }
    #endregion
}
