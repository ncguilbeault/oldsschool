# oldsschool

An ephemeral, nix-native dashboard for old-school open-source games.

Click a game. It runs. Nothing installs.

## Manifesto

- **No emulation.** Native FOSS games and source ports only — no DOSBox, MAME, RetroArch.
- **No containers.** No Docker, no Flatpak, no AppImage.
- **No orchestration.** No Kubernetes, no Compose, no agents-of-agents.
- **No permanent install.** Game launches use `nix run --impure nixpkgs#<pkg>` (or `nix shell --impure -c`). Store paths are GC'able. The catalog is a JSONC file.

The dashboard itself is .NET 9 + [Avalonia](https://avaloniaui.net) (cross-platform XAML UI, native on Linux).

## Dependencies

Deliberately minimal:

- **Avalonia** + **Avalonia.Desktop** — UI framework + Linux desktop backend.
- **Avalonia.Themes.Fluent** — default control styling.
- `System.Text.Json` (BCL, no NuGet) — reads the JSONC catalog.

Nix flake provides `dotnet-sdk_9`, `fontconfig`, `icu`.

## Prerequisites

You need [Nix](https://nixos.org/download) with flakes enabled, plus a graphical session (X11 or Wayland — the launcher won't render over plain SSH without forwarding).

Enable flakes if you haven't already:

- **NixOS** — add to `configuration.nix`:
  ```nix
  nix.settings.experimental-features = [ "nix-command" "flakes" ];
  ```
- **[Determinate Nix](https://determinate.systems/nix-installer/)** (recommended for non-NixOS Linux + macOS) — flakes are on by default.
- **Upstream Nix installer** — add to `~/.config/nix/nix.conf` (or `/etc/nix/nix.conf`):
  ```
  experimental-features = nix-command flakes
  ```

Supported systems: `x86_64-linux`, `aarch64-linux`, `x86_64-darwin`, `aarch64-darwin`. Tested primarily on Linux/X11.

## Install, build & run

```sh
git clone https://github.com/ncguilbeault/oldsschool.git
cd oldsschool

nix develop                       # enters dev shell: dotnet-sdk_9, fontconfig, icu
cd src/OldsSchool.App
dotnet run                        # opens the launcher window
```

First click on a game fetches the package into `/nix/store` (subsequent launches are instant). The launcher spawns games with `--impure` + `NIXPKGS_ALLOW_UNFREE=1` so freeware classics (e.g. Dwarf Fortress) launch without a manual override.

To wipe every game and dev artifact you've accumulated:

```sh
nix-collect-garbage -d
```

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

v1 ships ~34 entries — predominantly pure FOSS, plus a small whitelist of freeware
classics (Dwarf Fortress is the notable example). Engine-only ports
(gzdoom, vcmi, devilutionx, …) are deliberately excluded — they need
user-supplied original assets.

## Roadmap

| Phase | Scope |
|---|---|
| **v0 (this)** | Avalonia dashboard, JSONC catalog, `nix run` launches, search + category filter. |
| v1 | Cover art, controller nav, recent/favorites, MVVM refactor, packaged via `buildDotnetModule`. |
| v2 | Original in-house game integrated as a first-class catalog entry. |
| v3 | Stripe subscription gating + Web3 token entitlements. |

## License

TBD (default to MIT for the launcher; original game phase will reconsider).
