FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

## COPY *.csproj ./
COPY ["FoodStore.API/FoodStore.API.csproj","FoodStore.API/"]
COPY ["FoodStore.Core/FoodStore.Core.csproj", "FoodStore.Core/"]
COPY ["FoodStore.Domain/FoodStore.Domain.csproj", "FoodStore.Domain/"]
COPY ["FoodStore.Domain.DataLayer/FoodStore.Domain.DataLayer.csproj", "FoodStore.Domain.DataLayer/"]
RUN dotnet restore "FoodStore.API/FoodStore.API.csproj"

COPY . ./
RUN dotnet publish -c Release -o out /p:EnvironmentName=Production

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet","FoodStore.API.dll"]
