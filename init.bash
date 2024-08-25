
# Clone Dalamud
git clone --recurse-submodules -j8 https://github.com/goatcorp/Dalamud.git

# Build Dalamud
cd Dalamud && dotnet restore && dotnet build

# Build AuroraClient
cd ../AuroraClient && dotnet restore && dotnet build --configuration Debug
