@echo off
call "..\..\tools\windows\init.visualstudio.cmd"

powershell .\test.Magick.NET.ps1
if %errorlevel% neq 0 exit /b %errorlevel%