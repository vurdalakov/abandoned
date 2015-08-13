#ifndef DIRECTORYVIEW_H
#define DIRECTORYVIEW_H

#include <QAbstractScrollArea>
#include "directoryinfo.h"

class DirectoryView : public QAbstractScrollArea
{
    Q_OBJECT

    DirectoryInfo m_dir;

    bool m_active;

    int m_top;
    int m_current;

    void ensureVisible(int index);
    void onEnter();

public:

    explicit DirectoryView(QWidget *parent = 0);

    QString currentDirectory() const;
    void setCurrentDirectory(const QString& currentDirectory);

    void refresh();

    void setActive(bool active);

    QString currentName();
    void setCurrentName(const QString& name);

    void keyPressed(QKeyEvent* event);

    void mousePressed(QMouseEvent* event);
    void mouseDoubleClicked(QMouseEvent* event);

protected:

    virtual void paintEvent(QPaintEvent* event);

    virtual void resizeEvent(QResizeEvent* event);

    virtual void keyPressEvent(QKeyEvent* event);

signals:

    void currentDirectoryChanged(const QString& currentDirectory);

public slots:

};

#endif // DIRECTORYVIEW_H
