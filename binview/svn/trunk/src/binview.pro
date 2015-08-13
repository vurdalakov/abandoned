
VERSION = 0.30

QT += core gui

TARGET = binview
TEMPLATE = app
RESOURCES = binview.qrc

DEFINES += APP_VERSION=\\\"$$VERSION\\\"

win32{
    message(Win32 build)
    OPERATINGSYSTEM = win32
    RC_FILE = binview.rc
}

macx{
    message(MacOS X build)
    OPERATINGSYSTEM = osx
    ICON = resources/binview.icns
}

unix:!macx{
    message(Linux build)
    OPERATINGSYSTEM = lnx
}

CONFIG(release, release|debug){
    message(Release build)
    BUILDTYPE = release
}

CONFIG(debug, release|debug){
    message(Debug build)
    BUILDTYPE = debug
}

DESTDIR = ./../bin/$${OPERATINGSYSTEM}/$${BUILDTYPE}
OBJECTS_DIR = ./../obj/$${OPERATINGSYSTEM}/$${BUILDTYPE}
MOC_DIR = ./../obj/$${OPERATINGSYSTEM}/$${BUILDTYPE}
UI_DIR = ./../obj/$${OPERATINGSYSTEM}/$${BUILDTYPE}
RCC_DIR = ./../obj/$${OPERATINGSYSTEM}/$${BUILDTYPE}

SOURCES += main.cpp\
        mainwindow.cpp \
    qvactiongroup.cpp \
    gotodialog.cpp \
    aboutdialog.cpp \
    binviewmodel.cpp \
    binviewwidget.cpp \
    fixedfontdialog.cpp \
    qvlabel.cpp

HEADERS  += mainwindow.h \
    binviewmodel.h \
    binviewwidget.h \
    qvactiongroup.h \
    gotodialog.h \
    aboutdialog.h \
    fixedfontdialog.h \
    qvlabel.h

FORMS    += mainwindow.ui \
    gotodialog.ui \
    aboutdialog.ui

RESOURCES += \
    binview.qrc

OTHER_FILES += \
    resources/binview.icns \
    resources/binview128.png \
    resources/binview048.png \
    resources/binview032.png \
    resources/binview016.png \
    resources/binview.ico \
    binview.rc \
    resources/binview064.png
