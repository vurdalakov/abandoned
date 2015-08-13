#include "fixedfontdialog.h"

#include <QDialogButtonBox>

FixedFontDialog::FixedFontDialog(const QFont& initial, QWidget* parent) :
    QFontDialog(initial, parent)
{
    m_okButton = 0;

    for (int i = 0; i < children().count(); i++)
    {
        QObject* child = children().at(i);
        if (child->inherits("QDialogButtonBox"))
        {
            QDialogButtonBox* dialogButtonBox = (QDialogButtonBox*)child;

            m_okButton = dialogButtonBox->button(QDialogButtonBox::Ok);

            break;
        }
    }

    connect(this, SIGNAL(currentFontChanged(QFont)), this, SLOT(currentFontChanged(QFont)));
}

FixedFontDialog::~FixedFontDialog()
{
    disconnect(this, SIGNAL(currentFontChanged(QFont)), this, SLOT(currentFontChanged(QFont)));
}

void FixedFontDialog::currentFontChanged(const QFont& font)
{
    QFontInfo fontInfo(font);
    m_okButton->setEnabled(fontInfo.fixedPitch());
}
