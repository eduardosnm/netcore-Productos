FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Products.csproj", "./"]
RUN dotnet restore "./Products.csproj"
COPY . .
RUN dotnet build "Products.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Products.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Products.dll"]
