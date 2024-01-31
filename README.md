# How to deploy .NET8 WebApp to Azure Container App

The source code is in github repo: https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp

https://github.com/KamalRathnayake/Dapr-Todo-App

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
EXPOSE 80
EXPOSE 443

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

```cshtml
﻿@{
    ViewData["Title"] = "Home Page";
}

<style>
    .todo-item {
        background-color: white;
        border-radius: 5px;
        margin-bottom: 15px;
        padding: 15px;
        padding-left: 20px;
        padding-right: 20px;
        box-shadow: 0px 0px 10px #e2e2e2;
        text-align: left;
        cursor: pointer;
        display:flex;
        font-size:13pt;
    }

    h1 {
        margin-bottom: 50px;
    }
</style>

<div class="text-center">
    <h1 class="display-4">Todos App!</h1>
    <h5>You are viewing your app for the @ViewBag.Counter time</h5>

    <div style="display: flex">
        <div style="flex:1"></div>
        <div style="flex:2">
            @if (ViewBag.Todos == null || ViewBag.Todos.Count == 0)
            {
                <div>No Todos :(</div>
            }
            @foreach (var todo in ViewBag.Todos)
            {
                <div class="todo-item">
                    <div style="flex:12">
                        @todo.Name
                    </div>
                    <div>
                        <img style="width:22px;flex:1" src="~/icons/bin.png" />
                    </div>
                </div>
            }
        </div>
        <div style="flex:1"></div>
    </div>
    <div>
    </div>
</div>
```

We can verify the **index.cshtm** file in the projec folders structure

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/6f471ab7-7bf0-44f3-a5b2-a1f611336cf0)

### 1.3. Add the "bin" icon

We can download the icons from this URL: https://www.freepik.com/icons/bin

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/b0ebf5e0-9c78-4435-8e1e-82cedb8fe10c)

We copy the **bin.png** icon file to the **wwwroot/icons** folder

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/eb997f1e-3b97-4a47-b9b6-cf8c3f6b5210)

### 1.4. Modify the HomeController.cs 

```csharp
using AzureContainerAppWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureContainerAppWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public class Todo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Done { get; set; }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Todos = new List<Todo>()

            {
                new Todo 
                { 
                    Id = 1,
                    Name = "Test1",
                },
                new Todo
                {
                    Id = 1,
                    Name = "Test2",
                }
            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
```

### 1.5 Modify the ports in Dockerfile and launchSettings.json 

We set in the launchSettings.json the ports 80 and 443

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/862ea9f0-6075-4ecd-b9d2-6e2b97f97c61)

**launchSettings.json**

```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "dotnetRunMessages": true,
      "applicationUrl": "http://localhost:5191"
    },
    "https": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "dotnetRunMessages": true,
      "applicationUrl": "https://localhost:7162;http://localhost:5191"
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Docker": {
      "commandName": "Docker",
      "launchBrowser": true,
      "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}",
      "environmentVariables": {
        "ASPNETCORE_HTTPS_PORTS": "443",
        "ASPNETCORE_HTTP_PORTS": "80"
      },
      "publishAllPorts": true,
      "useSSL": true
    }
  },
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:14154",
      "sslPort": 44348
    }
  }
}
```

### 1.6. Build and Run the WebApp

We open a Terminal Window in our soultion in Visual Studio and we create the Docker image 

``` 
docker build -t mywebapi .
```

We run the docker image in Docker Desktop to verify it is working fine

```
docker run -e ASPNETCORE_ENVIRONMENT=Development -p 5191:8080 --name myapp -d mywebapi:latest
```

We verify the docker image and running container in Docker Desktop

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/47e67c4b-a8d6-43b0-87b6-1ef7572ca3fd)

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/daa1f753-ceac-4855-8709-2d72ab05e377)

We also access the application

http://localhost:5191/

![image](https://github.com/luiscoco/Azure_ContainerApp_Deploy_.NET8-WebApp/assets/32194879/00f8e764-f513-43e5-9c17-b9f409fdd4e6)

## 2. Publish the WebApp to Docker Hub

Be sure we already renamed the Docker image according to our Docker Hub repo name

```
docker tag mywebapi luiscoco/mywebapi:latest
```

We log in to Docker and **push the image to Docker Hub**

```
docker login
docker push luiscoco/mywebapi:latest
```




### 2.1. Publish the WebApp to Docker Hub (with Visual Studio)





### 2.2. Publish the WebApp to Docker Hub (with commands)





## 3. Create Azure Container App





## 4. Verify the Web App already deploy to Azure Container App















