
# DevBuddy

**DevBuddy** is a command-line tool designed to streamline development by automating repetitive tasks like generating CRUD templates and Faker classes. It helps developers focus on building features instead of boilerplate code.

---

## Features

- Generate CRUD templates for entities.
- Generate Faker classes for data seeding.
- Simple, flexible, and easy to use.

---

## Installation

Install DevBuddy as a .NET global tool by running:

```bash
dotnet tool install -g DevBuddy
```

---

## Getting Started

DevBuddy provides two main commands:

1. **Generate CRUD Templates**
2. **Generate Faker Classes**

---

### 1. Generate CRUD Templates

The `generate-CRUD` command generates CRUD templates for a specified entity.

**Command Syntax:**

```bash
devbuddy generate-CRUD --entity <EntityName> --referenceEntity <ReferenceEntity> --templates <Templates> --config <ConfigPath> [--rootPath <RootPath>]
```

**Options:**

- `--entity, -e` (required): The name of the entity for which CRUD templates will be generated.  
  **Examples:** `Product`, `Order`, `Customer`.

- `--referenceEntity, -re` (required): The name of the existing entity to use as a reference.  
  **Example:** If your `Customer` entity follows the same structure as `User`, use `--referenceEntity User`.

- `--templates, -t` (required): Specify the templates to generate.  
  Options:
  - Use `all` to generate all templates.
  - Specify templates as a comma-separated list, such as `Controller`, `ViewModel`, `Service`.

- `--rootPath` (optional): The root folder where files will be generated. Defaults to the current directory.

- `--config` (required): Path to a configuration file for custom settings (e.g., paths and default templates).  
  **Example:** `--config ./config.json`.

## Configuration File: `config.json`
The `config.json` file plays a crucial role in the DevBuddy tool by defining the templates and settings used for code generation. This configuration ensures that the tool generates code that is tailored to the specific architecture and naming conventions of your project. Explore the configuration file at this URL, where you are welcome to view and adjust it to better align with your specific needs.

### Structure:
- **TemplateCategories**: An array of different templates that AutoCRUD can generate. Each category is specified with attributes and a template information block that includes the project layer, subfolder path, and filename pattern.
### Example:

```json
{
    "TemplateCategories": [
      {
        "Name": "CountryController",
        "Attributes":["FullTemplate"],
        "TemplateInfo": {
          "ProjectLayer": "Web",
          "Subfolder": "Controllers/Api",
          "FileNamePattern": "{entityName}Controller.cs"
        }
      },
      {
        "Name": "Model",
        "Attributes": ["ReadOnly"],
        "TemplateInfo": {
          "ProjectLayer": "Domain",
          "Subfolder": "Models",
          "FileNamePattern": "{entityName}.cs"
        }
      },
   
      {
        "Name": "Controller",
        "Attributes": ["FullTemplate"],
        "TemplateInfo": {
          "ProjectLayer": "Web",
          "Subfolder": "Controllers",
          "FileNamePattern": "{entityName}Controller.cs"
        }
      }
    ]
}
  
```

### Customizing Configurations:

You can customize the tool's behavior by modifying the `TemplateCategories` entries:
- **Name**: The identifier for the template category.
- **Attributes**: Modifiers such as `FullTemplate`, `ReadOnly`., that influence how the template is processed.
- **TemplateInfo**: Details where and how the template files should be structured within your project.
### File Patterns:
The `FileNamePattern` provides a template for the filenames that AutoCRUD will generate. For example:
- `"FileNamePattern": "{entityName}Controller.cs"` would result in filenames like `UserAccountController.cs`, where `{entityName}` is replaced by the actual entity name.

### Usage:

To adapt the AutoCRUD tool to your project's needs, modify the `config.json` entries to match your desired output file structure and naming conventions. Ensure that each path and filename pattern correctly reflects the target directory structure in your project.

---

**Examples:**

- Generate all templates for an entity using a reference entity:

  ```bash
  devbuddy generate-CRUD --entity Product --referenceEntity Customer --templates all --config ./config.json
  ```

- Generate specific templates for an entity:

  ```bash
  devbuddy generate-CRUD --entity Order --referenceEntity User --templates Controller,ViewModel --config C:\Users\Documents\config.json
  ```

---

---

### 2. Generate Faker Classes

The `generate-Fakers` command generates Faker classes to help seed your database with dummy data.

**Command Syntax:**

```bash
devbuddy generate-Fakers --model <ModelName> --modelsPath <ModelsPath> --outputPath <OutputPath>
```

**Options:**

- `--model, -m` (required): The name of the model for which Faker classes will be generated, or 'all' to generate for all models.
  **Example:** `Product`, `User`.

- `--modelsPath` (required): The directory path where the model definitions are located.
  **Example:** `./Models`.

- `--outputPath` (required): The folder where the Faker class files will be generated.
  **Example:** `./Fakers`.

---

**Examples:**

- Generate Faker classes for all models:

  ```bash
  devbuddy generate-Fakers --model all --modelsPath ./Models --outputPath ./Fakers
  ```

- Generate a Faker class for a `Product` model:

  ```bash
  devbuddy generate-Fakers --model Product --modelsPath ./Models --outputPath ./Fakers
  ```

---

## Summary

DevBuddy simplifies repetitive coding tasks, allowing developers to save time and focus on building features. With its ability to generate CRUD templates and Faker classes, it’s an essential tool for any project.

---

## Important Note

- **Version Control:** This application creates and overwrites files, so ensure you have your code committed in Git or a similar version control system to avoid any loss of work.
