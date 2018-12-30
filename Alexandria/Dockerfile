FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["nuget.config", "./"]
COPY Alexandria/Alexandria.csproj Alexandria/
COPY Alexandria.Shared/Alexandria.Shared.csproj Alexandria.Shared/
COPY Alexandria.DTO/Alexandria.DTO.csproj Alexandria.DTO/
COPY Alexandria.EF/Alexandria.EF.csproj Alexandria.EF/
COPY Alexandria.Interfaces/Alexandria.Interfaces.csproj Alexandria.Interfaces/
COPY Alexandria.Orchestration/Alexandria.Orchestration.csproj Alexandria.Orchestration/
RUN dotnet restore Alexandria/Alexandria.csproj
COPY . .
WORKDIR /src/Alexandria
RUN dotnet build Alexandria.csproj -c Release -o /app
COPY ["Alexandria/Alexandria.xml", "./Alexandria"]

FROM build AS publish
RUN dotnet publish Alexandria.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Alexandria.dll"]
