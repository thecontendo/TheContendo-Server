######## BUILD #######
FROM mcr.microsoft.com/dotnet/sdk:5.0.301 AS build
#WORKDIR /app
#RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash -
#RUN apt-get install -y nodejs

COPY ./*.sln .
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore TheContendo.sln

COPY . /TheContendo

####### UI5 BUNDLE ########
#RUN npm install --prefix /TheContendo/Contendo/wwwroot/app
#RUN npm link gulp
#RUN gulp ui5preload --gulpfile /TheContendo/Contendo/wwwroot/app/gulpfile.js
#
RUN dotnet publish /TheContendo/Contendo/Contendo.csproj -c Release -o /app_publish;

###### RUNTIME #######
FROM mcr.microsoft.com/dotnet/aspnet:5.0.7 as runtime
WORKDIR /app
RUN apt-get update; apt-get -y install curl;
COPY --from=build /app_publish .

#HEALTHCHECK --interval=5s --timeout=2s --retries=12 \
#        CMD curl --silent --fail --request GET \
#            localhost:80/health || exit 1
ENTRYPOINT ["dotnet", "Contendo.dll"]