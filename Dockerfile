#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BooksAPI/BooksAPI.csproj", "BooksAPI/"]
COPY ["BooksAPI.DataLayer.EF/BooksAPI.DataLayer.EF.csproj", "BooksAPI.DataLayer.EF/"]
COPY ["BooksAPI.DataLayer.Abstractions/BooksAPI.DataLayer.Abstractions.csproj", "BooksAPI.DataLayer.Abstractions/"]
COPY ["BooksAPI.Model/BooksAPI.Model.csproj", "BooksAPI.Model/"]
COPY ["BooksAPI.Services/BooksAPI.Services.csproj", "BooksAPI.Services/"]
RUN dotnet restore "BooksAPI/BooksAPI.csproj"
COPY . .

FROM build AS publish
RUN dotnet publish "BooksAPI/BooksAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooksAPI.dll"]