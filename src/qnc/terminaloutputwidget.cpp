#include "terminaloutputwidget.h"

#include <QMenu>

TerminalOutputWidget::TerminalOutputWidget(QWidget *parent) :
    QPlainTextEdit(parent)
{
}

void TerminalOutputWidget::contextMenuEvent(QContextMenuEvent* event)
{
    QMenu menu;

    menu.addAction("Copy", this, SLOT(copy()))->setEnabled(textCursor().hasSelection());
    menu.addAction("Select All", this, SLOT(selectAll()));
    menu.addSeparator();
    menu.addAction("Clear", this, SLOT(clear()));

    menu.exec(mapToGlobal(event->pos()));
}
