FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Alexandria.Admin/Alexandria.Admin.csproj", "Alexandria.Admin/"]
RUN dotnet restore "Alexandria.Admin/Alexandria.Admin.csproj"
COPY . .
WORKDIR "/src/Alexandria.Admin"
RUN dotnet build "Alexandria.Admin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Alexandria.Admin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Alexandria.Admin.dll"]