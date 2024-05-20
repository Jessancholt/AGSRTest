#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Test.WebAPI/Test.WebAPI.csproj", "Test.WebAPI/"]
COPY ["Test.DataAccess/Test.DataAccess.csproj", "Test.DataAccess/"]
COPY ["Test.BusinessLogic/Test.Core.csproj", "Test.BusinessLogic/"]
RUN dotnet restore "Test.WebAPI/Test.WebAPI.csproj"
COPY . .
WORKDIR "/src/Test.WebAPI"
RUN dotnet build "Test.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.WebAPI.dll"]
