FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /source
COPY ./CapybottaBot ./build/
WORKDIR /source/build
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app ./
RUN apt update
RUN apt install -y ffmpeg
RUN apt install -y libsodium-dev
ENTRYPOINT [ "dotnet", "CapybottaBot.dll" ]