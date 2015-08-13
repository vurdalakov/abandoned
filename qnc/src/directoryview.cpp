#include "directoryview.h"

#include <QPainter>
#include <QScrollBar>
#include <QKeyEvent>
#include <QDateTime>
#include <QDesktopServices>
#include <QUrl>

#include "utils.h"
#include "textpainter.h"

DirectoryView::DirectoryView(QWidget *parent) :
    QAbstractScrollArea(parent), m_active(false)
{
    m_dir.go(QDir::homePath());
    m_current = 0;

    setHorizontalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
    setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOn);

    setCursor(Qt::ArrowCursor);
}

QString DirectoryView::currentDirectory() const
{
    return m_dir.path();
}

void DirectoryView::setCurrentDirectory(const QString& currentDirectory)
{
    m_dir.go(currentDirectory);

    verticalScrollBar()->setSliderPosition(0);

    viewport()->update();
}

void DirectoryView::refresh()
{
    QString name = currentName();
    int pos = m_current - m_top;

    m_dir.refresh();

    if (!name.isEmpty())
    {
        int current = m_dir.find(name);
        if (current >= 0)
        {
            m_current = current;
            m_top = m_current - pos;
        }
    }
}

void DirectoryView::setActive(bool active)
{
    if (m_active != active)
    {
        m_active = active;

        viewport()->update();
    }
}

QString DirectoryView::currentName()
{
    if ((m_current >= 0) && (m_current < m_dir.count()))
    {
        QFileInfo fileInfo = m_dir.at(m_current);

        return fileInfo.fileName();
    }
    else
    {
        return "";
    }
}

void DirectoryView::setCurrentName(const QString& name)
{
    int i = m_dir.find(name);
    if (i >= 0)
    {
        m_current = i;

        viewport()->update();

        ensureVisible(i);
    }
}

void DirectoryView::keyPressed(QKeyEvent* event)
{
    keyPressEvent(event);
}

void DirectoryView::paintEvent(QPaintEvent* /*event*/)
{
    TextPainter painter(viewport());

    painter.setPen(Qt::black);
    painter.setBrush(QBrush(Qt::white));
    painter.drawRect(rect());

    int textFlags = /*Qt::AlignHCenter |*/ Qt::AlignVCenter | Qt::TextSingleLine;

    int rowHeight = painter.charHeight();
    int rows = rect().height() / rowHeight;
    int charWidth = painter.charWidth();
    int charsInRow = rect().width() / charWidth;

    int rowCount = m_dir.count();
    verticalScrollBar()->setRange(0, qMax<qint64>(0, rowCount - rows));

    qint64 firstRow = verticalScrollBar()->sliderPosition();
    qint64 maxRow = qMin<qint64>(rowCount, firstRow + rows);

    for (qint64 row = firstRow; row < maxRow; row++)
    {
        int i = row - firstRow;

        QRect r(0, i * rowHeight, rect().width(), rowHeight);
        if (m_current == row)
        {
            painter.setPen(Qt::white);
            painter.setBrush(QBrush(m_active ? Qt::blue : Qt::gray));
            painter.drawRect(r);
        }
        else
        {
            painter.setPen(Qt::black);
            painter.setBrush(QBrush(Qt::white));
        }

        QFileInfo fileInfo = m_dir.at(row);

        r.setLeft(rect().left());
        r.setRight(r.left() + (charsInRow - 28) * charWidth);
        painter.drawText(r, textFlags, fileInfo.fileName());

        r.setLeft(r.right());
        r.setRight(r.left() + charWidth * 8);
        painter.drawText(r, textFlags | Qt::AlignRight, " " + (fileInfo.isDir() ? " <DIR>" : Utils::formatFileSize(fileInfo.size())));

        r.setLeft(r.right());
        r.setRight(r.left() + charWidth * 20);
        painter.drawText(r, textFlags, fileInfo.lastModified().toString(" dd.MM.yyyy hh:mm:ss"));
    }
}

void DirectoryView::resizeEvent(QResizeEvent* /*event*/)
{
    viewport()->update();
}

void DirectoryView::keyPressEvent(QKeyEvent* event)
{
    switch (event->key())
    {
        case Qt::Key_Up:
            if (m_current > 0)
            {
                m_current--;
            }
            break;
        case Qt::Key_Down:
            if (m_current < (m_dir.count() - 1))
            {
                m_current++;
            }
            break;
        case Qt::Key_Return:
            onEnter();
            break;
        default:
            return;
    }

    ensureVisible(m_current);

    viewport()->update();
}

void DirectoryView::mousePressed(QMouseEvent* event)
{
    int top = verticalScrollBar()->sliderPosition();

    int current = top + event->y() / TextPainter::charHeight();

    if (current < m_dir.count())
    {
        m_current = current;
        viewport()->update();
    }

    setActive(true);
}

void DirectoryView::mouseDoubleClicked(QMouseEvent* /*event*/)
{
    onEnter();
}

void DirectoryView::ensureVisible(int index)
{
    int top = verticalScrollBar()->sliderPosition();

    if (index < top)
    {
        verticalScrollBar()->setSliderPosition(index);
        return;
    }

    int rowCount = height() / TextPainter::charHeight();

    if (index >= (top + rowCount))
    {
        top = index - rowCount + 1;
        if (top < 0)
        {
            top = 0;
        }
        verticalScrollBar()->setSliderPosition(top);
        return;
    }
}

void DirectoryView::onEnter()
{
    QFileInfo fileInfo = m_dir.at(m_current);

    if (fileInfo.isDir())
    {
        QString dirName = ".." == fileInfo.fileName() ? m_dir.name() : "";

        m_dir.go(fileInfo.absoluteFilePath());

        m_current = 0;
        if (!dirName.isEmpty())
        {
            for (int i = 0; i < m_dir.count(); i++)
            {
                if (m_dir.at(i).fileName() == dirName)
                {
                    m_current = i;
                    break;
                }
            }
        }

        verticalScrollBar()->setSliderPosition(0);

        viewport()->update();

        emit currentDirectoryChanged(m_dir.path());
    }
    else
    {
        QDesktopServices::openUrl(QUrl(QString("file://%1").arg(fileInfo.filePath())));
    }
}
