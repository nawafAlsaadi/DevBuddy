using AutoCRUD.Domain.Enums;

namespace AutoCRUD.Application.Services;

public class UserInputService
{
    public string PromptForEntityName()
    {
        Console.WriteLine("\nPlease provide the name of the entity you want to create templates for (e.g., 'User', 'Order').:");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return input.Trim();
    }

    public string GetReferenceEntityName()
    {
        Console.WriteLine("Please enter the name of the base entity (default: 'Address'). Press Enter to use the default value:");
        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input) ? "Address" : input.Trim();
    }
    public string GetConfigPath()
    {
        Console.WriteLine("configPath plz");
        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input) ? "C:\\Users\\002na\\Documents\\config.json" : input.Trim();
    }
    public string? PromptForTemplateSelection(List<string> templates)
    {
        Console.WriteLine("\nSelect templates to generate:");
        for (int i = 0; i < templates.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {templates[i]}");
        }
        //Console.WriteLine($"{templates.Count + 1}. All Templates");
        Console.WriteLine("Enter your choice (press Enter for all templates ||  or  comma-separated numbers ):");

        return Console.ReadLine();
    }
    public string PromptForRootPath()
    {
        while (true)
        {
            Console.WriteLine("Please specify the root path where your project is located (e.g., 'C:/Users/Nawaf/source/repos/YourAppName/'):");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                //todo fix this:
                return "C:/Users/002na/source/repos/TestingMyCli/";
                //RootPath =   Directory.GetCurrentDirectory();
                //return input.Trim();

                Console.WriteLine("Error: Root path cannot be empty. Please try again.");
                continue;
            }

            if (!Directory.Exists(input))
            {
                Console.WriteLine("Error: The provided path does not exist. Please try again.");
                continue;
            }

            return input.Trim();
        }
    }
}
