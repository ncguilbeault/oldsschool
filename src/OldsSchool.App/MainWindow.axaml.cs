using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using OldsSchool.App.Models;
using OldsSchool.App.Services;

namespace OldsSchool.App;

public partial class MainWindow : Window
{
    private readonly LauncherService _launcher = new();
    private readonly CatalogService _catalog = new();
    private List<Game> _all = new();
    private const string AllCategoriesLabel = "all categories";

    public MainWindow()
    {
        InitializeComponent();
        Opened += (_, _) => Load();
        SearchBox.TextChanged += (_, _) => Refresh();
        CategoryBox.SelectionChanged += (_, _) => Refresh();
    }

    private void Load()
    {
        try
        {
            var path = ResolveCatalogPath();
            _all = _catalog.Load(path).ToList();
            var categories = new List<string> { AllCategoriesLabel };
            categories.AddRange(_all.Select(g => g.Category).Distinct().OrderBy(c => c));
            CategoryBox.ItemsSource = categories;
            CategoryBox.SelectedIndex = 0;
            Refresh();
            StatusText.Text = $"{_all.Count} games loaded";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"catalog error: {ex.Message}";
        }
    }

    private static string ResolveCatalogPath()
    {
        var primary = Path.Combine(AppContext.BaseDirectory, "games.jsonc");
        if (File.Exists(primary)) return primary;
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "games.jsonc");
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        throw new FileNotFoundException("games.jsonc not found");
    }

    private void Refresh()
    {
        var query = (SearchBox.Text ?? "").Trim();
        var category = CategoryBox.SelectedItem as string ?? AllCategoriesLabel;
        IEnumerable<Game> view = _all;
        if (category != AllCategoriesLabel)
            view = view.Where(g => g.Category == category);
        if (!string.IsNullOrEmpty(query))
            view = view.Where(g =>
                g.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                g.Blurb.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                g.Category.Contains(query, StringComparison.OrdinalIgnoreCase));
        GameList.ItemsSource = view.ToList();
    }

    private async void OnGameClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: Game game }) return;
        StatusText.Text = $"launching {game.Name}…";
        try
        {
            var exit = await _launcher.RunAsync(game);
            StatusText.Text = exit == 0
                ? $"{game.Name} exited cleanly"
                : $"{game.Name} exited (code {exit})";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"launch failed: {ex.Message}";
        }
    }
}
