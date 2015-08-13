@echo off

echo *** 1. Initializing

set addon=rslinks
set version=0.3.4

set zipper="C:\Program Files\WinRAR\WinRAR.exe" a -afzip -r 

set jar=%addon%.jar
set xpi=%addon%_%version%.xpi

set jar_path=install

echo *** 2. Cleaning

if exist %xpi% del %xpi%
if exist %jar_path%\%jar% del %jar_path%\%jar%

echo *** 3. Creating [%jar%]

cd source
%zipper% %jar% *.*
move %jar% ..\%jar_path%
cd ..

echo *** 4. Creating [%xpi%]

cd install
%zipper% %xpi% *.*
move %xpi% ..
cd ..

del %jar_path%\%jar%
