FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["lingo/lingo.csproj", "lingo/"]
RUN dotnet restore "lingo/lingo.csproj"
COPY . .
WORKDIR "/src/lingo"
RUN dotnet build "lingo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lingo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lingo.dll"]
