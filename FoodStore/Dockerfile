FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

## COPY *.csproj ./
COPY ["FoodStore/FoodStore.csproj","FoodStore/"]
COPY ["FoodStore.Core/FoodStore.Core.csproj", "FoodStore.Core/"]
COPY ["FoodStore.Services/FoodStore.Services.csproj", "FoodStore.Core/"]
COPY ["FoodStore.Domain/FoodStore.Domain.csproj", "FoodStore.Domain/"]
COPY ["FoodStore.Domain.DataLayer/FoodStore.Domain.DataLayer.csproj", "FoodStore.Domain.DataLayer/"]
RUN dotnet restore "FoodStore/FoodStore.csproj"

COPY . ./
RUN dotnet publish -c Release -o out /p:EnvironmentName=Production

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet","FoodStore.dll"]
