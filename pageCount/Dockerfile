FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set environment variables

ENV usernameHash=""
ENV passwordHash=""
ENV rateLimit=""


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["pageCount.csproj", "./"]
RUN dotnet restore "pageCount.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "pageCount.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pageCount.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pageCount.dll"]
