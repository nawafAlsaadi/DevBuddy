using AutoCRUD.Domain.Enums;

namespace AutoCRUD.Application.Services;

public class TemplateSelectionService
{

    public List<string> SelectTemplatesFromInput(string? userInput, List<string> templates)
    {
        if (string.IsNullOrWhiteSpace(userInput))
        {
            return templates; // Default to all templates
        }

        var selectedTemplates = new List<string>();
        var choices = userInput.Split(',');

        foreach (var choice in choices)
        {
            if (int.TryParse(choice.Trim(), out var index) && index > 0 && index <= templates.Count)
            {
                selectedTemplates.Add(templates[index - 1]);
            }
            else
            {
                Console.WriteLine($"Invalid choice: {choice}. Skipping.");
                throw new Exception("Invalid choice: {choice}. Skipping.");
            }
        }

        return selectedTemplates.Count > 0 ? selectedTemplates : templates; // Default to all templates if none are valid
    }
}
