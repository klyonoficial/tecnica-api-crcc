@echo off
REM Build script for SysBoleto.Api

dotnet build --nologo -v q --property WarningLevel=0 /clp:ErrorsOnly ../server/SysConsultas.sln
IF %ERRORLEVEL% EQU 0 (
    ECHO "BUILD SUCCESS"
) ELSE (
    ECHO "BUILD FAILED"
    exit /b 1
)