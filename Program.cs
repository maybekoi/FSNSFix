using System.Text;

Console.Write("Enter the folder path: ");
string folderPath = Console.ReadLine() ?? string.Empty;

if (!Directory.Exists(folderPath))
{
    Console.WriteLine("Folder does not exist!");
    return;
}

string[] csFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

foreach (string file in csFiles)
{
    string[] lines = File.ReadAllLines(file);
    if (!lines.Any(line => line.Trim() == "namespace Celeste;")) continue; // Monocle, FMOD, FMOD.Studio

    StringBuilder newContent = new StringBuilder();
    bool foundNamespace = false;
    bool inUsingBlock = true;

    foreach (string line in lines)
    {
        string trimmedLine = line.Trim();
        
        if (trimmedLine == "namespace Celeste;")  // Monocle, FMOD, FMOD.Studio
        {
            newContent.AppendLine("namespace Celeste");  // Monocle, FMOD, FMOD.Studio
            newContent.AppendLine("{");
            foundNamespace = true;
            inUsingBlock = false;
            continue;
        }

        if (!foundNamespace)
        {
            newContent.AppendLine(line);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                newContent.AppendLine("    " + line);
            }
            else
            {
                newContent.AppendLine();
            }
        }
    }

    if (foundNamespace)
    {
        newContent.AppendLine("}");
        File.WriteAllText(file, newContent.ToString());
        Console.WriteLine($"Modified: {file}");
    }
}

Console.WriteLine("Processing complete!");