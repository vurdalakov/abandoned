@echo off

set version=0.30

set zipper="C:\Program Files\7-Zip\7z.exe"

rem === Cleanup

if exist binview rmdir /s /q binview
if exist *.zip del *.zip

rem === Create package

mkdir binview
xcopy /e /exclude:exclude.txt ..\..\src binview

%zipper% a -tzip -r binview.%version%.src.zip binview

rem === Cleanup

if exist binview rmdir /s /q binview
