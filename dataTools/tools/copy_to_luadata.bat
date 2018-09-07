@echo off
set SourcePath=..\..\RPG\Assets\Resources\Lua\Data\
rd /q /s %SourcePath%
xcopy /S /E /Y ..\lua %SourcePath%
echo "Explorer to lua path"
pause 
explorer %SourcePath%