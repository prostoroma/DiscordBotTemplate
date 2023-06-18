FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DiscordBotTemplate/DiscordBotTemplate.csproj", "DiscordBotTemplate/"]
RUN dotnet restore "DiscordBotTemplate/DiscordBotTemplate.csproj"
COPY . .
WORKDIR "/src/DiscordBotTemplate"
RUN dotnet build "DiscordBotTemplate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordBotTemplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordBotTemplate.dll"]
