FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 50
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Dashing.IDP.dll"]
