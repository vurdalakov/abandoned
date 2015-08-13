#include "directorywidget.h"

DirectoryWidget::DirectoryWidget(QWidget *parent) :
    QWidget(parent), m_view(this), m_pathPane(this)
{
    m_pathPane.setAutoFillBackground(true);
    m_pathPane.setAlignment(Qt::AlignHCenter | Qt::AlignVCenter);
    //m_pathPane.setFrameStyle(QFrame::Panel);

    connect(&m_view, SIGNAL(currentDirectoryChanged(QString)), this, SLOT(onCurrentDirectoryChanged(QString)));
}

DirectoryWidget::~DirectoryWidget()
{
    disconnect(&m_view, SIGNAL(currentDirectoryChanged(QString)), this, SLOT(onCurrentDirectoryChanged(QString)));
}

void DirectoryWidget::resizeEvent(QResizeEvent* /*event*/)
{
    m_pathPane.move(0, 0);
    m_pathPane.resize(width(), m_pathPane.height());

    m_view.move(0, m_pathPane.height());
    m_view.resize(width(), height() - m_pathPane.height());
}

void DirectoryWidget::mousePressEvent(QMouseEvent* event)
{
    m_view.mousePressed(event);

    emit currentPaneChanged(this);
}

void DirectoryWidget::mouseMoveEvent(QMouseEvent* /*event*/)
{
}

void DirectoryWidget::mouseReleaseEvent(QMouseEvent* /*event*/)
{
}

void DirectoryWidget::mouseDoubleClickEvent(QMouseEvent* event)
{
    m_view.mouseDoubleClicked(event);
}

void DirectoryWidget::onCurrentDirectoryChanged(const QString& currentDirectory)
{
    m_pathPane.setText(currentDirectory);

    emit currentDirectoryChanged(currentDirectory);
}

QString DirectoryWidget::currentDirectory() const
{
    return m_view.currentDirectory();
}

void DirectoryWidget::setCurrentDirectory(const QString& currentDirectory)
{
    m_pathPane.setText(currentDirectory);

    m_view.setCurrentDirectory(currentDirectory);
}

void DirectoryWidget::refresh()
{
    m_view.refresh();
}

void DirectoryWidget::setActive(bool active)
{
    m_view.setActive(active);
}

QString DirectoryWidget::getCurrentFilePath()
{
    return m_view.currentDirectory() + '/' + m_view.currentName(); // TODO
}

QString DirectoryWidget::currentName()
{
    return m_view.currentName();
}

void DirectoryWidget::setCurrentName(const QString& name)
{
    m_view.setCurrentName(name);
}

void DirectoryWidget::keyPressed(QKeyEvent* event)
{
    m_view.keyPressed(event);
}
