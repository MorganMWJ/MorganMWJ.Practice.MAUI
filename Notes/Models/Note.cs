using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notes.Models;

internal class Note
{
    [JsonIgnore]
    public string Filename { get; set; } = $"{Path.GetRandomFileName()}.notes.txt";

    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool Strikethrough { get; set; } = false;

    public void Save()
    {
        var jsonContent = JsonSerializer.Serialize<Note>(this);
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, Filename), jsonContent);
    }
        

    public void Delete() =>
        File.Delete(Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static Note Load(string filename)
    {
        filename = Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        var fileText = File.ReadAllText(filename);
        var note = JsonSerializer.Deserialize<Note>(fileText) ??
            throw new NullReferenceException("Failed to deserialise file json.");
        note.Filename = Path.GetFileName(filename);

        return note;
    }

    public static IEnumerable<Note> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.notes.txt")

                // Each file name is used to load a note
                .Select(filename => Note.Load(Path.GetFileName(filename)))

                // With the final collection of notes, order them by date
                .OrderByDescending(note => note.Date);
    }
}