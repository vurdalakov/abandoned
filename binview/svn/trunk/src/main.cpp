#include <QtGui/QApplication>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
    QCoreApplication::setApplicationName("binview");
    QCoreApplication::setApplicationVersion(APP_VERSION);
    QCoreApplication::setOrganizationDomain("vurdalakov.net");

    QApplication a(argc, argv);

    QString fileName;
    if (argc > 1)
    {
        fileName = argv[1];
    }

    MainWindow w(fileName);
    w.show();

    return a.exec();
}
