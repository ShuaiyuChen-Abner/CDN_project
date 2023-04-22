@echo off
cd.\CDNServer\bin\Debug\net7.0-windows\
start CDNServer.exe
cd ..\..\..\..\CDNCache\bin\Debug\net7.0-windows\
start CDNCache.exe
cd ..\..\..\..\CDNClient\bin\Debug\net7.0-windows\
start CDNClient.exe