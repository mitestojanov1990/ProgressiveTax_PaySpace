# PaySpace.Calculator.API Dockerfile

# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the certificate
RUN mkdir /https
RUN chown 777 /https
COPY aspnetapp1.pfx /https/aspnetapp1.pfx
RUN chmod 644 /https/aspnetapp1.pfx

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["PaySpace.Calculator.API/PaySpace.Calculator.API.csproj", "PaySpace.Calculator.API/"]
COPY ["PaySpace.Calculator.Application/PaySpace.Calculator.Application.csproj", "PaySpace.Calculator.Application/"]
COPY ["PaySpace.Calculator.Infrastructure/PaySpace.Calculator.Infrastructure.csproj", "PaySpace.Calculator.Infrastructure/"]
COPY ["PaySpace.Calculator.Services/PaySpace.Calculator.Services.csproj", "PaySpace.Calculator.Services/"]

RUN dotnet restore "PaySpace.Calculator.API/PaySpace.Calculator.API.csproj"

# Copy the rest of the application and build it
COPY . .
WORKDIR "/src/PaySpace.Calculator.API"
RUN dotnet build "PaySpace.Calculator.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "PaySpace.Calculator.API.csproj" -c Release -o /app/publish

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaySpace.Calculator.API.dll"]
