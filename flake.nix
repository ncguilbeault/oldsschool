{
  description = "oldsschool — ephemeral nix-native retro game launcher";

  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";

  outputs = { self, nixpkgs }:
    let
      systems = [ "x86_64-linux" "aarch64-linux" ];
      forAllSystems = nixpkgs.lib.genAttrs systems;
    in {
      devShells = forAllSystems (system:
        let pkgs = nixpkgs.legacyPackages.${system}; in {
          default = pkgs.mkShell {
            packages = with pkgs; [
              dotnet-sdk_9
              fontconfig
              icu
            ];
            shellHook = ''
              export DOTNET_ROOT=${pkgs.dotnet-sdk_9}/share/dotnet
              export DOTNET_CLI_TELEMETRY_OPTOUT=1
              export DOTNET_NOLOGO=1
              echo "oldsschool dev shell — dotnet $(dotnet --version)"
              echo "  → cd src/OldsSchool.App && dotnet run"
            '';
          };
        });
    };
}
