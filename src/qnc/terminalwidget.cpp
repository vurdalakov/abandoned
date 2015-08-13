#include "terminalwidget.h"

#include <QDir>
#include <QScrollBar>

TerminalWidget::TerminalWidget(QWidget *parent) :
    QWidget(parent),
    m_inputWidget(this), m_outputWidget(this), m_promptWidget(this)
{
    m_currentDirectory = QDir::currentPath();

#if defined(Q_OS_WIN32)
    QFont font("Courier", 10);
#elif defined(Q_OS_MAC)
    QFont font("Courier New", 12);
#endif
    m_inputWidget.setFont(font);
    m_promptWidget.setFont(font);
    m_outputWidget.setFont(font);

    m_inputWidget.setEditable(true);
    m_inputWidget.setFrame(false);

    m_promptWidget.setFrameShape(QFrame::NoFrame);
    QPalette palette = m_inputWidget.palette();
    palette.setColor(m_promptWidget.backgroundRole(), Qt::white); // TODO: change Qt::white to common style
    m_promptWidget.setPalette(palette);
    m_promptWidget.setAutoFillBackground(true);

    m_outputWidget.setFrameShape(QFrame::NoFrame);
    m_outputWidget.setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOn);
    m_outputWidget.setReadOnly(true);
    m_outputWidget.setEnabled(true);
    m_outputWidget.setFocusPolicy(Qt::NoFocus);
    m_outputWidget.setLineWrapMode(QPlainTextEdit::NoWrap);
    m_outputWidget.setMaximumBlockCount(4096); // TODO: customize

    connect(&m_inputWidget, SIGNAL(enterKeyPressed(const QString&)), this, SLOT(onEnterKeyPressed(const QString&)));

    connect(&m_process, SIGNAL(error(QProcess::ProcessError)), this, SLOT(onError(QProcess::ProcessError)));
    connect(&m_process, SIGNAL(finished(int,QProcess::ExitStatus)), this, SLOT(onFinished(int,QProcess::ExitStatus)));
    connect(&m_process, SIGNAL(readyReadStandardOutput()), this, SLOT(onReadyReadStandardOutput()));
    connect(&m_process, SIGNAL(readyReadStandardError()), this, SLOT(onReadyReadStandardError()));
    connect(&m_process, SIGNAL(stateChanged(QProcess::ProcessState)), this, SLOT(onStateChanged(QProcess::ProcessState)));

    // to get the current directory
    // TODO: replace with something more reliable (user can type "set PROMPT=something_else")
    QProcessEnvironment processEnvironment = QProcessEnvironment::systemEnvironment();
#if defined(Q_OS_WIN32)
    processEnvironment.insert("PROMPT", "$P$G");
#elif defined(Q_OS_MAC)
    processEnvironment.insert("PS1", "\\w>");
#endif
    m_process.setProcessEnvironment(processEnvironment);

#if defined(Q_OS_WIN32)
    m_process.start("cmd");
#elif defined(Q_OS_MAC)
    m_process.start("bash -i -s");
#endif
}

TerminalWidget::~TerminalWidget()
{
    m_process.terminate(); // TODO: does not work

    disconnect(&m_process, SIGNAL(error(QProcess::ProcessError)), this, SLOT(onError(QProcess::ProcessError)));
    disconnect(&m_process, SIGNAL(finished(int,QProcess::ExitStatus)), this, SLOT(onFinished(int,QProcess::ExitStatus)));
    disconnect(&m_process, SIGNAL(readyReadStandardOutput()), this, SLOT(onReadyReadStandardOutput()));
    disconnect(&m_process, SIGNAL(readyReadStandardError()), this, SLOT(onReadyReadStandardError()));
    disconnect(&m_process, SIGNAL(stateChanged(QProcess::ProcessState)), this, SLOT(onStateChanged(QProcess::ProcessState)));

    disconnect(&m_inputWidget, SIGNAL(enterKeyPressed(const QString&)), this, SLOT(onEnterKeyPressed(const QString&)));
}

void TerminalWidget::resizeEvent(QResizeEvent* /*event*/)
{
    int inputHeight = m_inputWidget.height();
    m_inputTop = height() - inputHeight;

    m_promptWidget.move(0, m_inputTop);
    m_promptWidget.resize(m_promptWidget.width(), inputHeight);

    int promptWidth = m_promptWidget.width();
    m_inputWidget.move(promptWidth, m_inputTop);
    m_inputWidget.resize(width() - promptWidth, inputHeight);

    m_outputWidget.move(0, 0);
    m_outputWidget.resize(width(), m_inputWidget.pos().y());
}


void TerminalWidget::clearOutputText()
{
    m_outputWidget.clear();
}

void TerminalWidget::scrollOutputTextToEnd()
{
    QTextCursor textCursor = m_outputWidget.textCursor();
    textCursor.movePosition(QTextCursor::End);
    m_outputWidget.setTextCursor(textCursor);
}

void TerminalWidget::appendOutputText(const QString& text, bool appendNewLine)
{
    scrollOutputTextToEnd();

    m_outputWidget.insertPlainText(text);

    if (appendNewLine)
    {
        m_outputWidget.insertPlainText("\r\n"); // TODO
    }

    scrollOutputTextToEnd();
}

void TerminalWidget::onEnterKeyPressed(const QString& text)
{
    if (QProcess::Running == m_process.state())
    {
        appendOutputText(m_currentDirectory + ">", false);

        m_process.write(text.toAscii());
        m_process.write("\n");
    }
}

void TerminalWidget::onError(QProcess::ProcessError error)
{
    appendOutputText(QString("Cannot start command interpreter, error %1").arg(error));
}

void TerminalWidget::onFinished(int /*exitCode*/, QProcess::ExitStatus /*exitStatus*/)
{
//    appendOutputText(QString("Process finished with exit code %1, status %2").arg(exitCode).arg(exitStatus));
}

void TerminalWidget::processOutput(const QByteArray& byteArray)
{
    QString line;
    for (int i = 0; i < byteArray.count(); i++)
    {
        char c = byteArray.at(i);

        if (10 == c)
        {
            appendOutputText(line);
            line.clear();
        }
        else if (12 == c)
        {
            clearOutputText();
            line.clear();
        }
        else if (c >= 32)
        {
            line += c;
        }
        else
        {
            line += '?';
        }
    }

    if (line.endsWith('>'))
    {
        QString currentDirectory = line.mid(0, line.length() - 1);

        if (0 == m_currentDirectory.compare(currentDirectory, Qt::CaseInsensitive))
        {
            return;
        }

        m_currentDirectory = currentDirectory;

        m_promptWidget.setText(m_currentDirectory + ">");
        m_promptWidget.adjustSize();

        int promptWidth = m_promptWidget.width();
        int inputHeight = m_inputWidget.height();

        m_promptWidget.resize(promptWidth, inputHeight);

        m_inputWidget.move(promptWidth, m_inputTop);
        m_inputWidget.resize(width() - promptWidth, inputHeight);

        emit currentDirectoryChanged(m_currentDirectory);
    }
    else if (line.length() > 0)
    {
        appendOutputText(line);
    }
}

void TerminalWidget::onReadyReadStandardOutput()
{
    QByteArray byteArray = m_process.readAllStandardOutput();
    qDebug(byteArray.constData());

    processOutput(byteArray);
}

void TerminalWidget::onReadyReadStandardError()
{
    QByteArray byteArray = m_process.readAllStandardError();
    qDebug(byteArray.constData());

    processOutput(byteArray);
}

void TerminalWidget::onStateChanged(QProcess::ProcessState /*newState*/)
{
//    appendOutputText(QString("Process state %1").arg(newState));
}
