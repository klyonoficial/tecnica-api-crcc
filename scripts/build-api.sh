#!/bin/bash
# Build script for Tecnica API

echo "Building Tecnica API..."

# Change to the API directory
cd "../src/api"

# Build the project
dotnet build --nologo -v q --property WarningLevel=0 /clp:ErrorsOnly

# Check if build was successful
if [ $? -eq 0 ]; then
    echo "BUILD SUCCESS"
else
    echo "BUILD FAILED"
    exit 1
fi