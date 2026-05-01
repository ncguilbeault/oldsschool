using System.Diagnostics;
using System.Threading.Tasks;
using OldsSchool.App.Models;

namespace OldsSchool.App.Services;

public sealed class LauncherService
{
    public async Task<int> RunAsync(Game game)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "nix",
            UseShellExecute = false,
            CreateNoWindow = false,
        };

        psi.Environment["NIXPKGS_ALLOW_UNFREE"] = "1";

        if (string.IsNullOrEmpty(game.Binary))
        {
            psi.ArgumentList.Add("run");
            psi.ArgumentList.Add("--impure");
            psi.ArgumentList.Add($"nixpkgs#{game.Nixpkg}");
        }
        else
        {
            psi.ArgumentList.Add("shell");
            psi.ArgumentList.Add("--impure");
            psi.ArgumentList.Add($"nixpkgs#{game.Nixpkg}");
            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add(game.Binary);
        }

        using var p = Process.Start(psi)
            ?? throw new System.InvalidOperationException("failed to start nix");
        await p.WaitForExitAsync();
        return p.ExitCode;
    }
}
