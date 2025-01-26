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
    public static string GetRootPath(string rootPath)
    {
        // Check if the provided rootPath is null or empty.
        if (string.IsNullOrEmpty(rootPath))
        {
            // Use the current directory as the default rootPath.
            rootPath = Directory.GetCurrentDirectory();
            Console.WriteLine($"The default directory is set to: {rootPath}");
            Console.WriteLine("Is this correct? (yes/no)");

            // Read the user's response.
            string response = Console.ReadLine();

            // Normalize the response to handle different cases like "YES", "yes", "y", etc.
            if (!string.IsNullOrWhiteSpace(response) && response.Trim().ToLowerInvariant() == "no")
            {
                // If the user says no, ask for the correct directory.
                Console.WriteLine("Please enter the correct directory path:");
                string newUserPath = Console.ReadLine();

                // Validate the new user input and assign it to rootPath if it's not empty.
                if (!string.IsNullOrEmpty(newUserPath))
                {
                    rootPath = newUserPath;
                }
                else
                {
                    Console.WriteLine("No valid directory entered. Using default directory.");
                }
            }
        }

        return rootPath;
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
