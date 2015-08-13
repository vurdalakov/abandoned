#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "qxsysteminfo.h"
#include "textpainter.h"

#include "createdirectorydialog.h"

#include <QDir>
#include <QScrollBar>
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    m_inputWidget(this), m_outputWidget(this), m_promptWidget(this),
    m_leftPane(this), m_rightPane(this)
{
    ui->setupUi(this);

    m_hiddenCommand = false;

    this->setWindowTitle("QNC");

    ui->mainToolBar->addAction("Clear", this, SLOT(onClearOutput()));

    // status bar

    statusBar()->setMinimumWidth(2); // prevent window resize if e.g. file name does not fit into window

    const int statusBarWidgetFrameStyle = QFrame::Panel | QFrame::Sunken;

    m_driveStatusBarWidget = new QLabel(statusBar());
    m_driveStatusBarWidget->setFrameStyle(statusBarWidgetFrameStyle);
    m_driveStatusBarWidget->hide();
    //connect(m_driveStatusBarWidget, SIGNAL(mousePressed(QMouseEvent*)), this, SLOT(m_driveStatusBarWidget(QMouseEvent*)));
    statusBar()->addPermanentWidget(m_driveStatusBarWidget);

    resize(800, 600);

    //timerEvent(0);
    //m_driveTimer = startTimer(2000);

    m_activePane = &m_leftPane;
    m_activePane->setActive(true);

#if defined(Q_OS_WIN32)
    QFont font("Courier", 10);
#elif defined(Q_OS_MAC)
    QFont font("Courier New", 12);
#endif
    TextPainter::setFont(font);

    m_inputWidget.setFont(font);
    m_promptWidget.setFont(font);
    m_outputWidget.setFont(font);
    m_leftPane.setFont(font);
    m_rightPane.setFont(font);

    m_inputWidget.setEditable(true);
    m_inputWidget.setFrame(false);

    m_inputWidget.installEventFilter(this);

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

    m_leftPane.move(0, 0);
    m_rightPane.move(width() / 2, 0);

    connect(&m_inputWidget, SIGNAL(enterKeyPressed(const QString&)), this, SLOT(onEnterKeyPressed(const QString&)));

    connect(&m_process, SIGNAL(error(QProcess::ProcessError)), this, SLOT(onError(QProcess::ProcessError)));
    connect(&m_process, SIGNAL(finished(int,QProcess::ExitStatus)), this, SLOT(onFinished(int,QProcess::ExitStatus)));
    connect(&m_process, SIGNAL(readyReadStandardOutput()), this, SLOT(onReadyReadStandardOutput()));
    connect(&m_process, SIGNAL(readyReadStandardError()), this, SLOT(onReadyReadStandardError()));
    connect(&m_process, SIGNAL(stateChanged(QProcess::ProcessState)), this, SLOT(onStateChanged(QProcess::ProcessState)));

    connect(&m_leftPane, SIGNAL(currentDirectoryChanged(QString)), this, SLOT(onCurrentDirectoryChanged(QString)));
    connect(&m_rightPane, SIGNAL(currentDirectoryChanged(QString)), this, SLOT(onCurrentDirectoryChanged(QString)));

    connect(&m_leftPane, SIGNAL(currentPaneChanged(DirectoryWidget*)), this, SLOT(onCurrentPaneChanged(DirectoryWidget*)));
    connect(&m_rightPane, SIGNAL(currentPaneChanged(DirectoryWidget*)), this, SLOT(onCurrentPaneChanged(DirectoryWidget*)));

    // to get the current directory
    // TODO: replace with something more reliable (user can type "set PROMPT=something_else")
    QProcessEnvironment processEnvironment = QProcessEnvironment::systemEnvironment();
#if defined(Q_OS_WIN32)
    processEnvironment.insert("PROMPT", "$P$G");
#elif defined(Q_OS_MAC)
    processEnvironment.insert("PS1", "\\w>");
#endif
    m_process.setProcessEnvironment(processEnvironment);

    m_process.setWorkingDirectory(QDir::homePath()); // TODO

    m_leftPane.setCurrentDirectory(QDir::homePath()); // TODO
    m_rightPane.setCurrentDirectory(QDir::homePath()); // TODO

#if defined(Q_OS_WIN32)
    m_process.start("cmd");
#elif defined(Q_OS_MAC)
    m_process.start("bash -i -s");
#endif
}

MainWindow::~MainWindow()
{
    m_process.terminate(); // TODO: does not work

    disconnect(&m_process, SIGNAL(error(QProcess::ProcessError)), this, SLOT(onError(QProcess::ProcessError)));
    disconnect(&m_process, SIGNAL(finished(int,QProcess::ExitStatus)), this, SLOT(onFinished(int,QProcess::ExitStatus)));
    disconnect(&m_process, SIGNAL(readyReadStandardOutput()), this, SLOT(onReadyReadStandardOutput()));
    disconnect(&m_process, SIGNAL(readyReadStandardError()), this, SLOT(onReadyReadStandardError()));
    disconnect(&m_process, SIGNAL(stateChanged(QProcess::ProcessState)), this, SLOT(onStateChanged(QProcess::ProcessState)));

    disconnect(&m_inputWidget, SIGNAL(enterKeyPressed(const QString&)), this, SLOT(onEnterKeyPressed(const QString&)));

    disconnect(&m_leftPane, SIGNAL(currentDirectoryChanged(QString)), this, SLOT(onCurrentDirectoryChanged(QString)));
    disconnect(&m_rightPane, SIGNAL(currentDirectoryChanged(QString)), this, SLOT(onCurrentDirectoryChanged(QString)));

    disconnect(&m_leftPane, SIGNAL(currentPaneChanged(DirectoryWidget*)), this, SLOT(onCurrentPaneChanged(DirectoryWidget*)));
    disconnect(&m_rightPane, SIGNAL(currentPaneChanged(DirectoryWidget*)), this, SLOT(onCurrentPaneChanged(DirectoryWidget*)));

    if (m_driveTimer != 0)
    {
        killTimer(m_driveTimer);
    }

    delete ui;
}

void MainWindow::timerEvent(QTimerEvent* /*event*/)
{
    qint64 freeSpace = QxSystemInfo::getDiskFreeSpace(m_currentDirectory);

    m_driveStatusBarWidget->show();
    m_driveStatusBarWidget->setText(QString("%L1 bytes free").arg(freeSpace));
}

void MainWindow::onClearOutput()
{
    clearOutputText();
}

void MainWindow::on_actionToggle_Directory_Panes_triggered()
{
    toggleDirectoryPanes();
}

void MainWindow::resizeEvent(QResizeEvent* /*event*/)
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

    m_leftPane.resize(width() / 2, m_inputTop);
    m_rightPane.move(width() / 2, 0);
    m_rightPane.resize(width() / 2, m_inputTop);
}

void MainWindow::clearOutputText()
{
    m_outputWidget.clear();
}

void MainWindow::toggleDirectoryPanes()
{
    bool visible = !m_leftPane.isVisible();
    m_leftPane.setVisible(visible);
    m_rightPane.setVisible(visible);
}

void MainWindow::scrollOutputTextToEnd()
{
    QTextCursor textCursor = m_outputWidget.textCursor();
    textCursor.movePosition(QTextCursor::End);
    m_outputWidget.setTextCursor(textCursor);
}

void MainWindow::appendOutputText(const QString& text, bool appendNewLine)
{
    if (m_hiddenCommand)
    {
        return;
    }

    scrollOutputTextToEnd();

    m_outputWidget.insertPlainText(text);

    if (appendNewLine)
    {
        m_outputWidget.insertPlainText("\r\n"); // TODO
    }

    scrollOutputTextToEnd();
}

void MainWindow::onEnterKeyPressed(const QString& text)
{
    if (QProcess::Running == m_process.state())
    {
        appendOutputText('\r' + m_currentDirectory + ">", false);

        m_process.write(text.toAscii());
        m_process.write("\n");
    }
}

void MainWindow::onError(QProcess::ProcessError error)
{
    appendOutputText(QString("Cannot start command interpreter, error %1").arg(error));
}

void MainWindow::onFinished(int /*exitCode*/, QProcess::ExitStatus /*exitStatus*/)
{
//    appendOutputText(QString("Process finished with exit code %1, status %2").arg(exitCode).arg(exitStatus));
}

void MainWindow::processOutput(const QByteArray& byteArray)
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
        m_hiddenCommand = false;

        QString currentDirectory = line.mid(0, line.length() - 1);

        if (currentDirectory.contains('~'))
        {
            currentDirectory = currentDirectory.replace('~', QDir::homePath());
        }

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

        m_activePane->setCurrentDirectory(m_currentDirectory);
    }
    else if (line.length() > 0)
    {
        appendOutputText(line, false);
    }
}

void MainWindow::onReadyReadStandardOutput()
{
    QByteArray byteArray = m_process.readAllStandardOutput();
    qDebug("O(%i): %s", byteArray.count(), byteArray.constData());

    processOutput(byteArray);
}

void MainWindow::onReadyReadStandardError()
{
    QByteArray byteArray = m_process.readAllStandardError();
    qDebug("E(%i): %s", byteArray.count(), byteArray.constData());

    processOutput(byteArray);
}

void MainWindow::onStateChanged(QProcess::ProcessState newState)
{
    qDebug("Process state %i", newState);
}

void MainWindow::onCurrentPaneChanged(DirectoryWidget* pane)
{
    if (m_activePane != pane)
    {
        m_activePane->setActive(false);
        m_activePane = pane;
        m_activePane->setActive(true);

        onCurrentDirectoryChanged(m_activePane->currentDirectory());
    }

    m_inputWidget.setFocus(Qt::OtherFocusReason);
}

void MainWindow::onCurrentDirectoryChanged(const QString& currentDirectory)
{
    if (m_currentDirectory.compare(currentDirectory, Qt::CaseInsensitive) != 0)
    {
        m_hiddenCommand = true;
        m_process.write(QString("cd %1 \n").arg(currentDirectory).toAscii());
    }

    this->setWindowTitle(QString("{%1} - QNC").arg(currentDirectory));
}

bool MainWindow::eventFilter(QObject* /*object*/, QEvent* event)
{
    if (m_leftPane.isVisible() && (QEvent::KeyPress == event->type()))
    {
        QKeyEvent* keyEvent = static_cast<QKeyEvent*>(event);
        switch (keyEvent->key())
        {
            case Qt::Key_Tab:
                onCurrentPaneChanged(m_activePane == &m_leftPane ? &m_rightPane : &m_leftPane);
                return true;
            case Qt::Key_Up:
            case Qt::Key_Down:
                m_activePane->keyPressed(keyEvent);
                return true;
            case Qt::Key_Return:
            case Qt::Key_Enter:
                if (Qt::ControlModifier == (keyEvent->modifiers() & Qt::ControlModifier))
                {
                    QString currentFileName = m_activePane->currentName();
                    if (!currentFileName.isEmpty())
                    {
                        QString currentText = m_inputWidget.currentText();
                        if (!currentText.isEmpty() && !currentText.endsWith(' '))
                        {
                            currentText += ' ';
                        }
                        m_inputWidget.setEditText(currentText + currentFileName + ' ');
                    }
                    return true;
                }
                if (0 == m_inputWidget.currentText().length())
                {
                    m_activePane->keyPressed(keyEvent);
                    return true;
                }
                else
                {
                    return false;
                }
//            case Qt::Key_F7:
//                if (0 == keyEvent->modifiers())
//                {
//                    QString currentFilePath = m_activePane->getCurrentFilePath();
//                    if (!currentFilePath.isEmpty())
//                    {
//                        CreateDirectoryDialog createDirectoryDialog(this);
//                        createDirectoryDialog.showNormal();
//                    }
//                }
//                break;
            default:
                return false;
        }
    }
    return false;
}

void MainWindow::on_actionCreate_Directory_triggered()
{
    if (!m_currentDirectory.isEmpty())
    {
        CreateDirectoryDialog createDirectoryDialog(this);
        if (QDialog::Accepted == createDirectoryDialog.exec())
        {
            QString name = createDirectoryDialog.name();

            QDir dir(m_currentDirectory);
            if (!dir.mkdir(name))
            {
                QMessageBox::warning(this, "Error", "Cannot create directory:\n" + name);
            }
            else
            {
                m_activePane->refresh();
                m_activePane->setCurrentName(name);
            }
        }
    }
}

void MainWindow::on_actionDelete_triggered()
{
    //QString filePath = m_activePane->currentPath();
    QString fileName = m_activePane->currentName();
    QMessageBox::question(this, "Delete", "Do you want to delete " + fileName + "?");
}
