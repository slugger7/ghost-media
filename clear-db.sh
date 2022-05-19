#!/bin/bash
echo "Removing previous database"
rm data/Ghost.db
cd Ghost.Data
echo "Running migrations"
~/.dotnet/tools/dotnet-ef database update
cd ../