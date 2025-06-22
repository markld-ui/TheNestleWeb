#!/bin/bash

# Проверка, что это директория .NET проекта
if [ ! -f *.csproj ]; then
    echo "Ошибка: Не найден .csproj файл. Запустите скрипт в корне проекта."
    exit 1
fi

# Установка основных пакетов для ASP.NET Core Web API
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.3
dotnet add package Microsoft.AspNetCore.Identity --version 2.3.1
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 9.0.3
dotnet add package Microsoft.AspNetCore.Mvc.Versioning --version 5.1.0
dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer --version 5.1.0

# Установка EF Core + PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.3
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.3
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.3
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.3

# Установка пакетов для безопасности
dotnet add package BCrypt.Net-Next --version 4.0.3
dotnet add package Microsoft.IdentityModel.Tokens --version 8.6.1
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.6.1

# Установка утилит
dotnet add package AutoMapper --version 14.0.0
dotnet add package System.Linq.Dynamic.Core --version 1.6.0.2
dotnet add package Serilog.AspNetCore --version 9.0.0
dotnet add package Swashbuckle.AspNetCore --version 8.0.0

echo "Все пакеты успешно установлены!"