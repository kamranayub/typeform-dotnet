#!/bin/sh
TAG=$1
KEY=$2

dotnet pack -c Release -o ../dist Typeform/Typeform.csproj -p:PackageVersion=$TAG -p:NuspecFile=../Typeform.nuspec

dotnet nuget push dist/Typeform.$TAG.nupkg -k $KEY -s https://www.nuget.org