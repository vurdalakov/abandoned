#ifndef TERMINALINPUTWIDGET_H
#define TERMINALINPUTWIDGET_H

#include <QComboBox>

class TerminalInputWidget : public QComboBox
{
    Q_OBJECT

protected:

    virtual void keyPressEvent(QKeyEvent* event);

public:

    explicit TerminalInputWidget(QWidget* parent = 0);

signals:

    void enterKeyPressed(const QString& text);

public slots:

};

#endif // TERMINALINPUTWIDGET_H
