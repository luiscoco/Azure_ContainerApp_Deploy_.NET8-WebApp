# How to deploy .NET8 WebApp to Azure Container App

## 1. Create the .NET8 WebApp 

### 1.1. Create the .NET8 WebApp with Visual Studio 2022 Community Edition

We create a new project with Visual Studio 2022

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/49b480dd-0132-4ad7-84d2-5eb1e7d0f6b4)

We select the project template

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/3ca91018-38ae-4ce0-9158-5b368cec93aa)

We input the project name and location

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/96854cf3-ba71-4c3b-b84a-cfc36c31ece9)

We select the main project features

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/4784a189-cdfe-4bfe-b2a4-43d9697f32d7)

We add the Docker support to the project and automaticallly the Dockerfile will be created

**Dockerfile**

```
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AzureContainerAppWebApp.csproj", "."]
RUN dotnet restore "./././AzureContainerAppWebApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./AzureContainerAppWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AzureContainerAppWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureContainerAppWebApp.dll"]
```

We verify the project folders structure

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/d3f1372f-e48f-4a42-bdfa-fe9a414c81cc)

### 1.2. Modify the Index.cshtml





### 1.3. Add the "bin" icon




### 1.4. Modify the HomeController.cs 





### 1.5 Modify the ports in Dockerfile and launchSetting.json 






### 1.6. Build and Run the WebApp





## 2. Publish the WebApp to Docker Hub






### 2.1. Publish the WebApp to Docker Hub (with Visual Studio)





### 2.2. Publish the WebApp to Docker Hub (with commands)





## 3. Create Azure Container App





## 4. Verify the Web App already deploy to Azure Container App















