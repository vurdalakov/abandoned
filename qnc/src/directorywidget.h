#ifndef DIRECTORYWIDGET_H
#define DIRECTORYWIDGET_H

#include <QWidget>
#include <QLabel>

#include "directoryview.h"

class DirectoryWidget : public QWidget
{
    Q_OBJECT

    QLabel m_pathPane;
    DirectoryView m_view;

public:

    explicit DirectoryWidget(QWidget *parent = 0);
    ~DirectoryWidget();

    QString currentDirectory() const;
    void setCurrentDirectory(const QString& currentDirectory);

    void refresh();

    void setActive(bool active);

    QString getCurrentFilePath();

    QString currentName();
    void setCurrentName(const QString& name);

    void keyPressed(QKeyEvent* event);

protected:

    virtual void resizeEvent(QResizeEvent* event);

    virtual void mousePressEvent(QMouseEvent* event);
    virtual void mouseMoveEvent(QMouseEvent* event);
    virtual void mouseReleaseEvent(QMouseEvent* event);
    virtual void mouseDoubleClickEvent(QMouseEvent* event);

signals:

    void currentDirectoryChanged(const QString& currentDirectory);
    void currentPaneChanged(DirectoryWidget* pane);

public slots:

    void onCurrentDirectoryChanged(const QString& currentDirectory);
};

#endif // DIRECTORYWIDGET_H
