namespace OldsSchool.App.Models;

public sealed class Game
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Nixpkg { get; set; } = "";
    public string Binary { get; set; } = "";
    public string Category { get; set; } = "";
    public string Blurb { get; set; } = "";

    public string NixCommand => string.IsNullOrEmpty(Binary)
        ? $"nix run nixpkgs#{Nixpkg}"
        : $"nix shell nixpkgs#{Nixpkg} -c {Binary}";
}
