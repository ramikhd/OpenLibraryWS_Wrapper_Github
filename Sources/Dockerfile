#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OpenLibraryWrapper/OpenLibraryWrapper.csproj", "OpenLibraryWrapper/"]
COPY ["LibraryDTO/LibraryDTO.csproj", "LibraryDTO/"]
COPY ["StubbedDTO/StubbedDTO.csproj", "StubbedDTO/"]
COPY ["JsonReader/JsonReader.csproj", "JsonReader/"]
COPY ["DtoAbstractLayer/DtoAbstractLayer.csproj", "DtoAbstractLayer/"]
COPY ["OpenLibraryClient/OpenLibraryClient.csproj", "OpenLibraryClient/"]
COPY ["MyLibraryManager/MyLibraryManager.csproj", "MyLibraryManager/"]
COPY ["MyLibraryDB/MyLibraryDB.csproj", "MyLibraryDB/"]
COPY ["MyLibraryEntities/MyLibraryEntities.csproj", "MyLibraryEntities/"]
COPY ["StubbedDB/StubbedDB.csproj", "StubbedDB/"]
RUN dotnet restore "OpenLibraryWrapper/OpenLibraryWrapper.csproj"
COPY . .
WORKDIR "/src/OpenLibraryWrapper"
RUN dotnet build "OpenLibraryWrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenLibraryWrapper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenLibraryWrapper.dll"]