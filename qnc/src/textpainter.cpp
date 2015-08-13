#include "textpainter.h"

#include <QImage>

QFont TextPainter::m_font("Courier New", 12);

int TextPainter::m_charHeight = -1;
int TextPainter::m_charWidth = -1;

void TextPainter::calculate()
{
    QImage image(100, 100, QImage::Format_RGB32);
    QPainter painter;
    painter.begin(&image);
    int textFlags = Qt::AlignHCenter | Qt::AlignVCenter | Qt::TextSingleLine;
    QRect charRect = painter.boundingRect(image.rect(), textFlags, QString(QChar(48)));
    m_charHeight = charRect.height();
    m_charWidth = charRect.width();
    painter.end();
}

void TextPainter::calculateIfNeeded()
{
    if (m_charWidth < 0)
    {
        calculate();
    }
}

void TextPainter::setFont(const QFont& font)
{
    m_font = font;

    m_charWidth = -1; // invalidate
}

int TextPainter::charHeight()
{
    calculateIfNeeded();

    return m_charHeight;
}

int TextPainter::charWidth()
{
    calculateIfNeeded();

    return m_charWidth;
}

TextPainter::TextPainter(QPaintDevice* device) : QPainter(device)
{
}

TextPainter::~TextPainter()
{
}
