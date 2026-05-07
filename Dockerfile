# ==========================================
# ETAPA 1: BASE (El motor mínimo para correr)
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
 
# ==========================================
# ETAPA 2: BUILD (La Fábrica de Compilación)
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
 
# 1. Copiamos TODOS los archivos de proyecto primero (Para aprovechar la caché de Docker)
COPY ["WebApplication1/GestionITM.Api/GestionITM.Api.csproj", "WebApplication1/GestionITM.Api/"]
COPY ["GestionITM.Domain/GestionITM.Domain.csproj", "GestionITM.Domain/"]
COPY ["WebApplication1/GestionITM.Infrastructure/GestionITM.Infrastructure.csproj", "WebApplication1/GestionITM.Infrastructure/"]
 
# 2. Restauramos las dependencias (NuGets)
RUN dotnet restore "WebApplication1/GestionITM.Api/GestionITM.Api.csproj"
 
# 3. Copiamos el resto del código fuente real
COPY . .
WORKDIR "/src/WebApplication1/GestionITM.Api"
 
# 4. Compilamos el proyecto en modo Release (Optimizado)
RUN dotnet build "GestionITM.Api.csproj" -c Release -o /app/build
 
# ==========================================
# ETAPA 3: PUBLISH (Empacar el producto final)
# ==========================================
FROM build AS publish
RUN dotnet publish "GestionITM.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
 
# ==========================================
# ETAPA 4: PRODUCCIÓN (El Food Truck listo)
# ==========================================
FROM base AS final
WORKDIR /app
# Solo copiamos el resultado de la etapa 'publish', dejando la "fábrica" atrás
COPY --from=publish /app/publish .
 
# Comando de arranque cuando el contenedor encienda
ENTRYPOINT ["dotnet", "GestionITM.Api.dll"]