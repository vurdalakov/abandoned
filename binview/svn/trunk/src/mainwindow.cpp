#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QFileDialog>
#include <QFileInfo>
#include <QTimer>
#include <QFontDialog>
#include <QScrollBar>
#include <QMessageBox>
#include <QDragEnterEvent>
#include <QUrl>
#include <QClipboard>

#include "gotodialog.h"
#include "aboutdialog.h"
#include "fixedfontdialog.h"

MainWindow::MainWindow(const QString& fileName) :
    QMainWindow(),
    ui(new Ui::MainWindow),
    m_settings(QSettings::IniFormat, QSettings::UserScope, QCoreApplication::organizationDomain(), QCoreApplication::applicationName(), this),
    m_fileSystemWatcher(0)
{
    ui->setupUi(this);

    QIcon icon;
    icon.addFile(":/resources/binview032.png");
    icon.addFile(":/resources/binview128.png");
    ((QApplication*)QApplication::instance())->setWindowIcon(icon);

    setWindowTitle(QCoreApplication::applicationName());

    setAcceptDrops(true);

    // main menu

    connect(&m_recentFilesGroup, SIGNAL(triggered(int)), this, SLOT(onRecentFileTriggered(int)));

    m_radixActionGroup.addAction(ui->actionRadix16, 16);
    m_radixActionGroup.addAction(ui->actionRadix10, 10);
    m_radixActionGroup.addAction(ui->actionRadix8, 8);
    connect(&m_radixActionGroup, SIGNAL(triggered(int)), this, SLOT(onRadixTriggered(int)));

    m_bytesPerLineActionGroup.addAction(ui->bytesPerLine10, 10);
    m_bytesPerLineActionGroup.addAction(ui->bytesPerLine16, 16);
    m_bytesPerLineActionGroup.addAction(ui->bytesPerLine20, 20);
    m_bytesPerLineActionGroup.addAction(ui->bytesPerLine32, 32);
    connect(&m_bytesPerLineActionGroup, SIGNAL(triggered(int)), this, SLOT(onBytesPerLineTriggered(int)));

    m_offsetActionGroup.addAction(ui->actionOffsetHex, 16);
    m_offsetActionGroup.addAction(ui->actionOffsetDec, 10);
    m_offsetActionGroup.addAction(ui->actionOffsetOct, 8);
    m_offsetActionGroup.addAction(ui->actionOffsetNone, 0);
    connect(&m_offsetActionGroup, SIGNAL(triggered(int)), this, SLOT(onOffsetTriggered(int)));

    // status bar

    statusBar()->setMinimumWidth(2); // prevent window resize if e.g. file name does not fit into window

    const int statusBarWidgetFrameStyle = QFrame::Panel | QFrame::Sunken;

    m_radixStatusBarWidget = new QVLabel(statusBar());
    m_radixStatusBarWidget->setFrameStyle(statusBarWidgetFrameStyle);
    m_radixStatusBarWidget->hide();
    connect(m_radixStatusBarWidget, SIGNAL(mousePressed(QMouseEvent*)), this, SLOT(onRadixMousePressed(QMouseEvent*)));
    statusBar()->addWidget(m_radixStatusBarWidget);

    m_bytesPerLineStatusBarWidget = new QVLabel(statusBar());
    m_bytesPerLineStatusBarWidget->setFrameStyle(statusBarWidgetFrameStyle);
    m_bytesPerLineStatusBarWidget->hide();
    connect(m_bytesPerLineStatusBarWidget, SIGNAL(mousePressed(QMouseEvent*)), this, SLOT(onBytesPerLineMousePressed(QMouseEvent*)));
    statusBar()->addWidget(m_bytesPerLineStatusBarWidget);

    m_fileModifiedStatusBarWidget = new QVLabel(statusBar());
    m_fileModifiedStatusBarWidget->setFrameStyle(statusBarWidgetFrameStyle);
    m_fileModifiedStatusBarWidget->hide();
    connect(m_fileModifiedStatusBarWidget, SIGNAL(doubleClicked(QMouseEvent*)), this, SLOT(onFileModifiedDoubleClicked(QMouseEvent*)));
    statusBar()->addPermanentWidget(m_fileModifiedStatusBarWidget);

    m_fileNameStatusBarWidget = new QVLabel(statusBar());
    m_fileNameStatusBarWidget->setFrameStyle(statusBarWidgetFrameStyle);
    m_fileNameStatusBarWidget->setAlignment(Qt::AlignRight);
    connect(m_fileNameStatusBarWidget, SIGNAL(mousePressed(QMouseEvent*)), this, SLOT(onFileNameMousePressed(QMouseEvent*)));
    statusBar()->addPermanentWidget(m_fileNameStatusBarWidget);

    m_fileSizeStatusBarWidget = new QLabel(statusBar());
    m_fileSizeStatusBarWidget->setFrameStyle(statusBarWidgetFrameStyle);
    statusBar()->addPermanentWidget(m_fileSizeStatusBarWidget);

    // central widget

    m_widget = new BinViewWidget(this);
    this->setCentralWidget(m_widget);

    m_widget->setHorizontalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
    m_widget->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOn);

    m_model = 0;

    // restore settings

    m_fileName = m_settings.value("files/last", QDir::homePath()).toString();

    m_recentFiles = m_settings.value("files/recent").toString().split(':', QString::SkipEmptyParts);
    updateRecentFiles();

    resize(m_settings.value("mainwindow/size", QSize(640, 430)).toSize());
    move(m_settings.value("mainwindow/pos", QPoint(0, 22)).toPoint());
    if (m_settings.value("mainwindow/maximized", false).toBool())
    {
        showMaximized();
    }

    int radix = m_settings.value("view/radix", 16).toInt();
    onRadixTriggered(radix);

    int bytesPerLine = m_settings.value("view/bytesPerLine", 16).toInt();
    onBytesPerLineTriggered(bytesPerLine);

    int offsetAction = m_settings.value("view/offsetMode", 16).toInt();
    m_widget->setOffsetMode(offsetAction);
    m_offsetActionGroup.setCheckedId(offsetAction);

    bool upperHex = m_settings.value("view/upperHex", true).toBool();
    m_widget->setUpperHex(upperHex);
    ui->actionHexUpper->setChecked(upperHex);

    if (m_settings.contains("font/family") && m_settings.contains("font/size"))
    {
        QFont font(m_settings.value("font/family").toString(), m_settings.value("font/size").toInt());
        m_widget->setFont(font);
    }


    ui->actionOpenLastFileAtStartup->setChecked(m_settings.value("options/openLastFileAtStartup", false).toBool());

    // disable controls
    enableControls(false);

    // open file

    show();

    if (!fileName.isEmpty())
    {
        openFile(fileName);
    }
    else if (ui->actionOpenLastFileAtStartup->isChecked() && QFile::exists(m_fileName))
    {
        ui->actionOpenLastFileAtStartup->setChecked(true);
        openFile(m_fileName);
    }
    else
    {
        QTimer::singleShot(0, this, SLOT(on_actionOpen_triggered()));
    }
}

MainWindow::~MainWindow()
{
    closeFile();

    disconnect(&m_radixActionGroup, SIGNAL(triggered(int)), this, SLOT(onRadixTriggered(int)));
    disconnect(&m_bytesPerLineActionGroup, SIGNAL(triggered(int)), this, SLOT(onBytesPerLineTriggered(int)));
    disconnect(&m_offsetActionGroup, SIGNAL(triggered(int)), this, SLOT(onOffsetTriggered(int)));

    disconnect(m_fileModifiedStatusBarWidget, SIGNAL(doubleClicked(QMouseEvent*)), this, SLOT(onFileModifiedDoubleClicked(QMouseEvent*)));

    m_settings.setValue("mainwindow/size", size());
    m_settings.setValue("mainwindow/pos", pos());
    m_settings.setValue("mainwindow/maximized", isFullScreen());

    delete m_widget;
    delete m_model;

    delete ui;
}

void MainWindow::enableControls(bool enable)
{
    ui->actionReopen->setEnabled(enable);
    ui->actionClose->setEnabled(enable);
    ui->actionGoto->setEnabled(enable);

    if (!enable)
    {
        m_fileModifiedStatusBarWidget->hide();
    }

    m_fileNameStatusBarWidget->setVisible(enable);
    m_fileSizeStatusBarWidget->setVisible(enable);

    m_radixStatusBarWidget->setVisible(enable);
    m_bytesPerLineStatusBarWidget->setVisible(enable);
}

void MainWindow::updateRecentFiles()
{
    ui->menuRecent->clear();
    m_recentFilesGroup.clear();

    int count = m_recentFiles.count();

    for (int i = 0; i < count; i++)
    {
        QAction* action = ui->menuRecent->addAction(m_recentFiles.at(i));
        m_recentFilesGroup.addAction(action, i);
    }

    ui->menuRecent->setEnabled(count > 0);
}

void MainWindow::openFile(const QString& fileName)
{
    closeFile();

    m_model = new BinViewModel();
    if (!m_model->open(fileName))
    {
        QMessageBox::critical(this, "Error", "An error occured while opening file: \n\n" + m_model->errorString());
        return;
    }

    m_fileName = fileName;

    m_widget->setModel(m_model);

    enableControls(true);

    int index = m_recentFiles.indexOf(m_fileName);
    if (index >= 0)
    {
        m_recentFiles.removeAt(index);
    }
    m_recentFiles.insert(0, m_fileName);
    while (m_recentFiles.count() > 5)
    {
        m_recentFiles.removeLast();
    }

    updateRecentFiles();

    m_settings.setValue("files/last", m_fileName);
    m_settings.setValue("files/recent", m_recentFiles.join(":"));

    QString shortFileName = fileName;
    int slash1 = shortFileName.indexOf('/', 1);
    if (slash1 > 1)
    {
        slash1 = shortFileName.indexOf('/', slash1 + 1);
    }
    int slash2 = shortFileName.lastIndexOf('/');
    if (slash2 > 1)
    {
        slash2 = shortFileName.lastIndexOf('/', slash2 - 1);
    }
    if ((slash2 - slash1) > 2)
    {
        shortFileName.remove(slash1 + 1, slash2 - slash1 - 1);
        shortFileName.insert(slash1 + 1, "...");
    }
    m_fileNameStatusBarWidget->setText(shortFileName);

    m_fileNameStatusBarWidget->setToolTip(fileName);

    QFileInfo fileInfo(fileName);
    m_fileSizeStatusBarWidget->setText(QString("%L1 bytes").arg(fileInfo.size()));
    m_fileSizeStatusBarWidget->setToolTip("0x" + m_widget->changeHexCase(QString("%L1").arg(fileInfo.size(), 0, 16)) + " bytes");

    setWindowTitle(QString("%1 - %2").arg(fileInfo.fileName()).arg(QCoreApplication::applicationName()));

    m_fileSystemWatcher = new QFileSystemWatcher();
    m_fileSystemWatcher->addPath(m_fileName);
    connect(m_fileSystemWatcher, SIGNAL(fileChanged(const QString&)), this, SLOT(onFileChanged(const QString&)));
}

void MainWindow::closeFile()
{
    enableControls(false);

    m_widget->setModel(0);

    if (m_model != 0)
    {
        m_model->close();
        m_model = 0;
    }

    if (m_fileSystemWatcher != 0)
    {
        disconnect(m_fileSystemWatcher, SIGNAL(fileChanged(const QString&)), this, SLOT(onFileChanged(const QString&)));
        delete m_fileSystemWatcher;
        m_fileSystemWatcher = 0;
    }

    setWindowTitle(QCoreApplication::applicationName());
}

void MainWindow::dragEnterEvent(QDragEnterEvent* event)
{
    if (event->mimeData()->hasUrls())
    {
        event->acceptProposedAction();
    }
}

void MainWindow::dropEvent(QDropEvent* event)
{
    QList<QUrl> urls = event->mimeData()->urls();
    if (urls.count() > 0)
    {
        openFile(urls.at(0).toLocalFile());
    }

    event->setDropAction(Qt::CopyAction);
    event->accept();
}

void MainWindow::on_actionOpen_triggered()
{
    QString fileName = QFileDialog::getOpenFileName(this, tr("Open File"), m_fileName);

    if (fileName.isEmpty())
    {
        return;
    }

    openFile(fileName);
}

void MainWindow::on_actionReopen_triggered()
{
    openFile(m_fileName);
}

void MainWindow::on_actionClose_triggered()
{
    closeFile();
}

void MainWindow::onRecentFileTriggered(int id)
{
    openFile(m_recentFiles.at(id));
}

void MainWindow::on_actionExit_triggered()
{
    close();
}

void MainWindow::on_actionGoto_triggered()
{
    GotoDialog gotoDialog(m_settings.value("edit/gotoRadix", 16).toInt(), m_settings.value("edit/gotoText", "0").toString(), this);

    if (QDialog::Accepted != gotoDialog.exec())
    {
        return;
    }

    m_settings.setValue("edit/gotoRadix", gotoDialog.radix());
    m_settings.setValue("edit/gotoText", gotoDialog.text());

    qint64 row = gotoDialog.offset();

    if (0 == gotoDialog.radix())
    {
        row = row * m_widget->bytesCount() / 100;
    }

    row /= m_widget->bytesPerLine();

    if (row < m_widget->rowCount())
    {
        m_widget->verticalScrollBar()->setSliderPosition(row);
    }
}

void MainWindow::onRadixTriggered(int id)
{
    m_widget->setRadix(id);

    m_radixActionGroup.setCheckedId(id);
    m_radixStatusBarWidget->setText(m_radixActionGroup.checkedAction()->text());

    m_settings.setValue("view/radix", id);
}

void MainWindow::onBytesPerLineTriggered(int id)
{
    m_widget->setBytesPerLine(id);

    m_bytesPerLineActionGroup.setCheckedId(id);
    m_bytesPerLineStatusBarWidget->setText(m_bytesPerLineActionGroup.checkedAction()->text() + " bytes per line");

    m_settings.setValue("view/bytesPerLine", id);
}

void MainWindow::onOffsetTriggered(int id)
{
    m_widget->setOffsetMode(id);
    m_offsetActionGroup.setCheckedId(id);
    m_settings.setValue("view/offsetMode", id);
}

void MainWindow::on_actionOpenLastFileAtStartup_triggered()
{
    m_settings.setValue("options/openLastFileAtStartup", ui->actionOpenLastFileAtStartup->isChecked());
}

void MainWindow::setFont(const QFont& font)
{
    m_settings.setValue("font/family", font.family());
    m_settings.setValue("font/size", font.pointSize());

    m_widget->setFont(font);
}

void MainWindow::onFileChanged(const QString& /*path*/)
{
    if (QFile::exists(m_fileName))
    {
        m_fileModifiedStatusBarWidget->setText("Modified");
        m_fileModifiedStatusBarWidget->setToolTip("File was modified.\nDouble-click to reload.");
    }
    else
    {
        m_fileModifiedStatusBarWidget->setText("Deleted");
        m_fileModifiedStatusBarWidget->setToolTip("File was deleted.\nDouble-click to close.");
    }

    m_fileModifiedStatusBarWidget->show();
}

void MainWindow::onFileModifiedDoubleClicked(QMouseEvent* /*mouseEvent*/)
{
    if (QFile::exists(m_fileName))
    {
        openFile(m_fileName);
    }
    else
    {
        closeFile();
    }
}

void MainWindow::on_actionFontSelect_triggered()
{
    FixedFontDialog fontDialog(m_widget->font(), this);

    if (QDialog::Accepted != fontDialog.exec())
    {
        return;
    }

    setFont(fontDialog.selectedFont());
}

void MainWindow::on_actionFontReset_triggered()
{
    setFont(QFont("Courier", 12));
}

void MainWindow::on_actionHexUpper_triggered()
{
    bool upperHex = ui->actionHexUpper->isChecked();

    m_settings.setValue("view/upperHex", upperHex);

    m_widget->setUpperHex(upperHex);
}

void MainWindow::on_action_About_triggered()
{
    AboutDialog aboutDialog(this);

    aboutDialog.exec();
}

void MainWindow::onFileNameMousePressed(QMouseEvent* mouseEvent)
{
    QMenu menu(this);
    menu.addAction("Copy");

    if (menu.exec(mouseEvent->globalPos()) != 0)
    {
        QApplication::clipboard()->setText(m_fileName);
    }
}

void MainWindow::onRadixMousePressed(QMouseEvent* mouseEvent)
{
    ui->menu_Radix->exec(mouseEvent->globalPos());
}

void MainWindow::onBytesPerLineMousePressed(QMouseEvent* mouseEvent)
{
    ui->menu_Bytes_Per_Line->exec(mouseEvent->globalPos());
}
