#ifndef QVLABEL_H
#define QVLABEL_H

#include <QLabel>

class QVLabel : public QLabel
{
    Q_OBJECT

public:
    explicit QVLabel(QWidget *parent = 0);

protected:
    virtual void mouseDoubleClickEvent(QMouseEvent* mouseEvent);
    virtual void mousePressEvent(QMouseEvent* ev);

signals:

    void doubleClicked(QMouseEvent* mouseEvent);
    void mousePressed(QMouseEvent* mouseEvent);

public slots:

};

#endif // QVLABEL_H
