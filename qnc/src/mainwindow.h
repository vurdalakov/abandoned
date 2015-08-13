#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

#include <QLabel>
#include <QProcess>

#include "terminalinputwidget.h"
#include "terminaloutputwidget.h"
#include "directorywidget.h"

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

    Ui::MainWindow *ui;

    TerminalInputWidget m_inputWidget;
    TerminalOutputWidget m_outputWidget;
    QLabel m_promptWidget;

    DirectoryWidget m_leftPane;
    DirectoryWidget m_rightPane;
    DirectoryWidget* m_activePane;

    int m_inputTop;

    QProcess m_process;
    QString m_currentDirectory;
    bool m_hiddenCommand;

    void scrollOutputTextToEnd();
    void appendOutputText(const QString& text, bool appendNewLine = true);
    void processOutput(const QByteArray& byteArray);

    QLabel* m_driveStatusBarWidget;

    int m_driveTimer;

protected:

    virtual void timerEvent(QTimerEvent* event);

    virtual void resizeEvent(QResizeEvent* event);

    bool eventFilter(QObject* object, QEvent* event);

public:

    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

    void clearOutputText();

    void toggleDirectoryPanes();

public slots:

    void onClearOutput();

    void onCurrentDirectoryChanged(const QString& currentDirectory);

    void onEnterKeyPressed(const QString& text);

    void onError(QProcess::ProcessError error);
    void onFinished(int exitCode, QProcess::ExitStatus exitStatus);
    void onReadyReadStandardOutput();
    void onReadyReadStandardError();
    void onStateChanged(QProcess::ProcessState newState);

    void onCurrentPaneChanged(DirectoryWidget* pane);

    void on_actionCreate_Directory_triggered();

private slots:
    void on_actionDelete_triggered();
    void on_actionToggle_Directory_Panes_triggered();
};

#endif // MAINWINDOW_H
