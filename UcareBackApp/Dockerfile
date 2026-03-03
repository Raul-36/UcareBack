FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UcareBackApp/UcareBackApp.csproj", "UcareBackApp/"]
RUN dotnet restore "UcareBackApp/UcareBackApp.csproj"
COPY . .
WORKDIR "/src/UcareBackApp"
RUN dotnet build "UcareBackApp.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN mkdir -p wwwroot/images
COPY --from=build /src/UcareBackApp/appsettings.json .
COPY --from=build /src/UcareBackApp/appsettings.Development.json .
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "UcareBackApp.dll"]