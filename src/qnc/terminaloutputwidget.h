#ifndef TERMINALOUTPUTWIDGET_H
#define TERMINALOUTPUTWIDGET_H

#include <QPlainTextEdit>

class TerminalOutputWidget : public QPlainTextEdit
{
    Q_OBJECT

protected:

    void contextMenuEvent(QContextMenuEvent* event);

public:

    explicit TerminalOutputWidget(QWidget *parent = 0);

signals:

public slots:

};

#endif // TERMINALOUTPUTWIDGET_H
