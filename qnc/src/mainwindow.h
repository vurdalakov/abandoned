#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

#include "terminalwidget.h"

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

    Ui::MainWindow *ui;

    TerminalWidget* m_terminalWidget;

public:

    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

public slots:

    void onClearOutput();

    void onCurrentDirectoryChanged(const QString& currentDirectory);
};

#endif // MAINWINDOW_H
