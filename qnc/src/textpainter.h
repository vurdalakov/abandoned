#ifndef TEXTPAINTER_H
#define TEXTPAINTER_H

#include <QPainter>

class TextPainter : public QPainter
{
    static QFont m_font;

    static int m_charHeight;
    static int m_charWidth;

    static void calculate();
    static void calculateIfNeeded();

    explicit TextPainter() : QPainter() {}

public:

    static void setFont(const QFont& font);

    static int charHeight();
    static int charWidth();

    explicit TextPainter(QPaintDevice* device);
    ~TextPainter();
};

#endif // TEXTPAINTER_H
