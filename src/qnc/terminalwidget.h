#ifndef TERMINALWIDGET_H
#define TERMINALWIDGET_H

#include <QWidget>
#include <QPlainTextEdit>
#include <QLabel>
#include <QProcess>

#include "terminalinputwidget.h"
#include "terminaloutputwidget.h"

class TerminalWidget : public QWidget
{
    Q_OBJECT

    TerminalInputWidget m_inputWidget;
    TerminalOutputWidget m_outputWidget;
    QLabel m_promptWidget;

    int m_inputTop;

    QProcess m_process;
    QString m_currentDirectory;

    void scrollOutputTextToEnd();
    void appendOutputText(const QString& text, bool appendNewLine = true);
    void processOutput(const QByteArray& byteArray);

protected:

    virtual void resizeEvent(QResizeEvent* event);

public:

    explicit TerminalWidget(QWidget *parent = 0);
    ~TerminalWidget();

    void clearOutputText();

public slots:

    void onEnterKeyPressed(const QString& text);

    void onError(QProcess::ProcessError error);
    void onFinished(int exitCode, QProcess::ExitStatus exitStatus);
    void onReadyReadStandardOutput();
    void onReadyReadStandardError();
    void onStateChanged(QProcess::ProcessState newState);

signals:

    void currentDirectoryChanged(const QString& currentDirectory);
};

#endif // TERMINALWIDGET_H
