@echo off

call 1_make.bat

echo *** 5. Installing add-on

start "Install add-on" firefox.exe %xpi%
