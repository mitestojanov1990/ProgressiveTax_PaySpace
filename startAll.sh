#!/bin/bash

dotnet run --project \PaySpace.Calculator.API\ --launch-profile https &

dotnet run --project .\PaySpace.Calculator.Web\ --launch-profile https


@echo off

REM Start the backend in a new command prompt window
start "Backend" cmd /k "dotnet run --project .\PaySpace.Calculator.API\ --launch-profile https"

REM Start the frontend in a new command prompt window
start "Frontend" cmd /k "dotnet run --project .\PaySpace.Calculator.Web\ --launch-profile https"
