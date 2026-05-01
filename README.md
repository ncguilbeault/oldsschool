# oldsschool

An ephemeral, nix-native dashboard for old-school open-source games.

Click a game. It runs. Nothing installs.

## Manifesto

- **No emulation.** Native FOSS games and source ports only — no DOSBox, MAME, RetroArch.
- **No containers.** No Docker, no Flatpak, no AppImage.
- **No orchestration.** No Kubernetes, no Compose, no agents-of-agents.
- **No permanent install.** Game launches use `nix run nixpkgs#<pkg>` (or `nix shell -c`). Store paths are GC'able. The catalog is a JSONC file.

The dashboard itself is .NET 9 + [Avalonia](https://avaloniaui.net) (cross-platform XAML UI, native on Linux).

## Dependencies

Deliberately minimal:

- **Avalonia** + **Avalonia.Desktop** — UI framework + Linux desktop backend.
- **Avalonia.Themes.Fluent** — default control styling.
- `System.Text.Json` (BCL, no NuGet) — reads the JSONC catalog.

Nix flake provides `dotnet-sdk_9`, `fontconfig`, `icu`.

## Run it

```sh
nix develop                    # drops you in a shell with dotnet-sdk_9
cd src/OldsSchool.App
dotnet run
```

First launch of any game pulls the package into `/nix/store` (cached afterwards).

## Catalog

Edit [`games.jsonc`](games.jsonc). Each entry:

```jsonc
{
  "id": "xonotic",
  "name": "Xonotic",
  "nixpkg": "xonotic",          // attr in nixpkgs
  "binary": "",                  // optional override; "" → use meta.mainProgram
  "category": "Arena FPS",
  "blurb": "..."
}
```

v1 ships ~34 pure-FOSS titles. Engine-only ports (gzdoom, vcmi, devilutionx, …) are
deliberately excluded — they need user-supplied original assets.

## Roadmap

| Phase | Scope |
|---|---|
| **v0 (this)** | Avalonia dashboard, JSONC catalog, `nix run` launches, search + category filter. |
| v1 | Cover art, controller nav, recent/favorites, MVVM refactor, packaged via `buildDotnetModule`. |
| v2 | Original in-house game integrated as a first-class catalog entry. |
| v3 | Stripe subscription gating + Web3 token entitlements. |

## License

TBD (default to MIT for the launcher; original game phase will reconsider).
