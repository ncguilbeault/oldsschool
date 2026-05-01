using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OldsSchool.App.Models;

namespace OldsSchool.App.Services;

public sealed class CatalogService
{
    private static readonly JsonSerializerOptions Options = new()
    {
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        PropertyNameCaseInsensitive = true,
    };

    public IReadOnlyList<Game> Load(string path)
    {
        using var stream = File.OpenRead(path);
        var doc = JsonSerializer.Deserialize<CatalogFile>(stream, Options);
        return doc?.Games ?? new List<Game>();
    }

    private sealed class CatalogFile
    {
        public List<Game> Games { get; set; } = new();
    }
}
