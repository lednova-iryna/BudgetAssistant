#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS publish

ARG TARGETPLATFORM

WORKDIR /src
COPY . .
RUN dotnet restore "Assistants.Budget.BE.API/Assistants.Budget.BE.API.csproj"

WORKDIR "/src/Assistants.Budget.BE.API"


RUN if [ "$TARGETPLATFORM" = "linux/amd64" ]; then \
    RID=linux-x64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm64" ]; then \
    RID=linux-arm64 ; \
    fi \
    && dotnet publish ./Assistants.Budget.BE.API.csproj -c Release -o /app/publish -r $RID

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assistants.Budget.BE.API.dll"]
