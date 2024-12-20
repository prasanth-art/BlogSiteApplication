#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlogSiteApplication.csproj", "."]
RUN dotnet restore "./BlogSiteApplication.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlogSiteApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogSiteApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLs=http://*:5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogSiteApplication.dll"]