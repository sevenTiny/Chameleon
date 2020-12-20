FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ARG source
WORKDIR /publish
EXPOSE 80
COPY /. /publish
ENTRYPOINT ["dotnet", "Chameleon.Development.dll"]
