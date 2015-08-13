#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    this->setWindowTitle("QNC");

    ui->mainToolBar->addAction("Clear", this, SLOT(onClearOutput()));

    m_terminalWidget = new TerminalWidget(this);
    setCentralWidget(m_terminalWidget);

    resize(800, 600);

    connect(m_terminalWidget, SIGNAL(currentDirectoryChanged(const QString&)), this, SLOT(onCurrentDirectoryChanged(const QString&)));
}

MainWindow::~MainWindow()
{
    disconnect(m_terminalWidget, SIGNAL(currentDirectoryChanged(const QString&)), this, SLOT(onCurrentDirectoryChanged(const QString&)));

    delete ui;
}

void MainWindow::onClearOutput()
{
    m_terminalWidget->clearOutputText();
}

void MainWindow::onCurrentDirectoryChanged(const QString& currentDirectory)
{
    this->setWindowTitle(QString("{%1} - QNC").arg(currentDirectory));
}
