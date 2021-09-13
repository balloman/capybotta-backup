FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CapybottaBot/CapybottaBot.csproj", "CapybottaBot/"]
RUN dotnet restore "CapybottaBot/CapybottaBot.csproj"
COPY . .
WORKDIR "/src/CapybottaBot"
RUN dotnet build "CapybottaBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CapybottaBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CapybottaBot.dll"]
