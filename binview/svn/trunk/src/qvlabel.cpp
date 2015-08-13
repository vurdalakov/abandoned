#include "qvlabel.h"

QVLabel::QVLabel(QWidget *parent) :
    QLabel(parent)
{
}

void QVLabel::mouseDoubleClickEvent(QMouseEvent* mouseEvent)
{
    emit doubleClicked(mouseEvent);
}

void QVLabel::mousePressEvent(QMouseEvent* mouseEvent)
{
    emit mousePressed(mouseEvent);
}
