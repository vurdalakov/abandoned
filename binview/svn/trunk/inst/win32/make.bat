@echo off

set version=0.30

set zipper="C:\Program Files\7-Zip\7z.exe"

rem === Cleanup

if exist binview rmdir /s /q binview
if exist *.zip del *.zip

rem === Create package without Qt runtime libraries

mkdir binview
copy ..\..\bin\win32\release\binview.exe binview

%zipper% a -tzip binview.%version%.win32.zip binview

rem === Create package with Qt runtime libraries

copy C:\Qt\4.7.1\bin\QtCore4.dll binview
copy C:\Qt\4.7.1\bin\QtGui4.dll binview

%zipper% a -tzip binview.%version%.win32.qt.zip binview

rem === Cleanup

if exist binview rmdir /s /q binview
