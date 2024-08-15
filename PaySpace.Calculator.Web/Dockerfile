# PaySpace.Calculator.Web Dockerfile

# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the certificate
RUN mkdir /https
RUN chown 777 /https
COPY aspnetapp2.pfx /https/aspnetapp2.pfx
RUN chmod 644 /https/aspnetapp2.pfx

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["PaySpace.Calculator.Web/PaySpace.Calculator.Web.csproj", "PaySpace.Calculator.Web/"]
COPY ["PaySpace.Calculator.Application/PaySpace.Calculator.Application.csproj", "PaySpace.Calculator.Application/"]
COPY ["PaySpace.Calculator.Infrastructure/PaySpace.Calculator.Infrastructure.csproj", "PaySpace.Calculator.Infrastructure/"]
COPY ["PaySpace.Calculator.Services/PaySpace.Calculator.Services.csproj", "PaySpace.Calculator.Services/"]

RUN dotnet restore "PaySpace.Calculator.Web/PaySpace.Calculator.Web.csproj"

# Copy the rest of the application and build it
COPY . .

# Build Tailwind CSS if needed
WORKDIR /src/PaySpace.Calculator.Web
RUN apt-get update && apt-get install -y curl gnupg
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - && apt-get install -y nodejs
RUN npm install
RUN npm run css:build


WORKDIR "/src/PaySpace.Calculator.Web"
RUN dotnet build "PaySpace.Calculator.Web.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "PaySpace.Calculator.Web.csproj" -c Release -o /app/publish

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaySpace.Calculator.Web.dll"]