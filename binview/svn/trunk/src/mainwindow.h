#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSettings>
#include <QLabel>
#include <QFileSystemWatcher>

#include "binviewwidget.h"
#include "binviewmodel.h"

#include "qvactiongroup.h"
#include "qvlabel.h"

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(const QString& fileName);
    ~MainWindow();

protected:

    virtual void dragEnterEvent(QDragEnterEvent* event);
    virtual void dropEvent(QDropEvent* event);

private:
    Ui::MainWindow* ui;

    QVLabel* m_fileNameStatusBarWidget;
    QLabel* m_fileSizeStatusBarWidget;
    QVLabel* m_fileModifiedStatusBarWidget;
    QVLabel* m_radixStatusBarWidget;
    QVLabel* m_bytesPerLineStatusBarWidget;

    QVActionGroup m_radixActionGroup;
    QVActionGroup m_bytesPerLineActionGroup;
    QVActionGroup m_offsetActionGroup;
    QVActionGroup m_recentFilesGroup;

    QSettings m_settings;

    BinViewWidget* m_widget;
    BinViewModel* m_model;

    QString m_fileName;
    QStringList m_recentFiles;

    QFileSystemWatcher* m_fileSystemWatcher;

    void enableControls(bool enable);
    void openFile(const QString& fileName);
    void closeFile();

    void updateRecentFiles();
    void setFont(const QFont& font);

private slots:
    void on_actionClose_triggered();
    void on_actionReopen_triggered();
    void on_actionExit_triggered();
    void on_action_About_triggered();
    void on_actionGoto_triggered();
    void on_actionHexUpper_triggered();
    void on_actionFontReset_triggered();
    void on_actionFontSelect_triggered();
    void on_actionOpenLastFileAtStartup_triggered();
    void on_actionOpen_triggered();

    void onRadixTriggered(int id);
    void onBytesPerLineTriggered(int id);
    void onOffsetTriggered(int id);
    void onRecentFileTriggered(int id);
    void onFileChanged(const QString& path);
    void onFileModifiedDoubleClicked(QMouseEvent* mouseEvent);
    void onFileNameMousePressed(QMouseEvent* mouseEvent);
    void onRadixMousePressed(QMouseEvent* mouseEvent);
    void onBytesPerLineMousePressed(QMouseEvent* mouseEvent);
};

#endif // MAINWINDOW_H
