#include "terminalinputwidget.h"

#include <QKeyEvent>

TerminalInputWidget::TerminalInputWidget(QWidget* parent) :
    QComboBox(parent)
{
}

void TerminalInputWidget::keyPressEvent(QKeyEvent* event)
{
    switch (event->key())
    {
        case Qt::Key_Return:
        case Qt::Key_Enter:
        {
            QString text = currentText();

            if (!text.isEmpty())
            {
                insertItem(0, text);

                clearEditText();

                emit enterKeyPressed(text);
            }
            break;
        }

        case Qt::Key_Escape:
            clearEditText();
            break;

        default:
            QComboBox::keyPressEvent(event);
            break;
    }
}
