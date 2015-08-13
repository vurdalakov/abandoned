
declare appName=binview
declare appVersion=0.30

declare dmgName="$appName.$appVersion.dmg"
declare dmgTemp="$appName.temp.dmg"

rm -f $dmgName
rm -f $dmgTemp
rm -rf temp

mkdir temp

echo $dmgTemp

#hdiutil create -fs HFSX -layout SPUD -size 40m $dmgTemp -srcfolder temp -format UDRW -volname "$appName"

bunzip2 -k $dmgTemp.bz2

hdiutil attach $dmgTemp -noautoopen -mountpoint temp

ditto -rsrc "../../bin/osx/release/binview.app" "temp/binview.app"

mkdir -p temp/binview.app/Contents/Frameworks/QtCore.framework/Versions/4.0
cp -R /Library/Frameworks/QtCore.framework/Versions/4.0/QtCore temp/binview.app/Contents/Frameworks/QtCore.framework/Versions/4.0

mkdir -p temp/binview.app/Contents/Frameworks/QtGui.framework/Versions/4.0
cp -R /Library/Frameworks/QtGui.framework/Versions/4.0/QtGui temp/binview.app/Contents/Frameworks/QtGui.framework/Versions/4.0

install_name_tool -change /Library/Frameworks/QtCore.framework/Versions/4.0/QtCore @executable_path/../Frameworks/QtCore.framework/Versions/4.0/QtCore temp/binview.app/Contents/MacOS/binview
install_name_tool -change /Library/Frameworks/QtGui.framework/Versions/4.0/QtGui @executable_path/../Frameworks/QtGui.framework/Versions/4.0/QtGui temp/binview.app/Contents/MacOS/binview

install_name_tool -change /Library/Frameworks/QtCore.framework/Versions/4.0/QtCore @executable_path/../Frameworks/QtCore.framework/Versions/4.0/QtCore temp/binview.app/Contents/Frameworks/QtGui.framework/Versions/4.0/QtGui

hdiutil detach temp -force

hdiutil convert $dmgTemp -format UDZO -imagekey zlib-level=9 -o $dmgName

rm -f $dmgTemp
rm -rf temp
