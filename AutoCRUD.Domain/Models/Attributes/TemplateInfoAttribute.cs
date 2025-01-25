// TemplateInfoAttribute.cs
using AutoCRUD.Domain.Enums;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class TemplateInfoAttribute : Attribute
{
    public ProjectLayer Layer { get; }
    public string SubFolder { get; }
    public string FileNameFormat { get; }

    public TemplateInfoAttribute(ProjectLayer layer, string subFolder, string fileNameFormat)
    {

        Layer = layer;
        SubFolder = subFolder;
        FileNameFormat = fileNameFormat;
    }
}
