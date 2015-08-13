@echo off

start "Kill Firefox" /WAIT C:\Users\balagour\personal\utils\sysint\pskill.exe firefox.exe

start "Firefox" firefox.exe -purgecaches -console -chromebug
