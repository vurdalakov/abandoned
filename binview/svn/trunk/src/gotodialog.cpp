#include "gotodialog.h"
#include "ui_gotodialog.h"

#include <QPushButton>

GotoDialog::GotoDialog(int radix, const QString& text, QWidget* parent) :
    QDialog(parent),
    m_buttonGroup(this),
    ui(new Ui::GotoDialog)
{
    ui->setupUi(this);

    m_offset = -1;

    m_buttonGroup.addButton(ui->radioButtonPercent, 0);
    m_buttonGroup.addButton(ui->radioButtonHex, 16);
    m_buttonGroup.addButton(ui->radioButtonDec, 10);
    m_buttonGroup.addButton(ui->radioButtonOct, 8);

    connect(&m_buttonGroup, SIGNAL(buttonClicked(int)), this, SLOT(onRadioButtonClicked(int)));

    ui->lineEdit->setText(text);
    ui->lineEdit->selectAll();

    QAbstractButton* button = m_buttonGroup.button(radix);
    if (0 == button)
    {
        button = ui->radioButtonHex;
    }
    button->setChecked(true);

    calculateOffset();
}

GotoDialog::~GotoDialog()
{
    disconnect(&m_buttonGroup, SIGNAL(buttonClicked(int)), this, SLOT(onRadioButtonClicked(int)));

    delete ui;
}

int GotoDialog::radix() const
{
    return m_buttonGroup.checkedId();
}

qint64 GotoDialog::offset() const
{
    return m_offset;
}

QString GotoDialog::text() const
{
    return ui->lineEdit->text();
}

void GotoDialog::calculateOffset()
{
    QString number = text().trimmed();
    int radix = m_buttonGroup.checkedId();

    if (number.contains('%'))
    {
        number = number.remove('%');
        radix = 0;
    }
    else if (number.startsWith("0x", Qt::CaseInsensitive) || number.contains(QRegExp("[ABCDEFabcdef]")))
    {
        number = number.mid(2);
        radix = 16;
    }
    else if (number.startsWith('0') && (number.length() > 1))
    {
        number = number.mid(1);
        radix = 8;
    }
    else if (number.startsWith('o', Qt::CaseInsensitive) || number.startsWith('q', Qt::CaseInsensitive))
    {
        number = number.mid(1);
        radix = 8;
    }
    else if (number.startsWith("0o", Qt::CaseInsensitive))
    {
        number = number.mid(2);
        radix = 8;
    }
    else if ((0 == radix) && (number.length() > 2))
    {
        radix = 10;
    }

    m_buttonGroup.button(radix)->setChecked(true);

    bool ok;
    m_offset = number.toLongLong(&ok, 0 == radix ? 10 : radix);

    ui->buttonBox->button(QDialogButtonBox::Ok)->setEnabled(ok);
}

void GotoDialog::on_lineEdit_textEdited(const QString& /*text*/)
{
    calculateOffset();
}

void GotoDialog::onRadioButtonClicked(int /*id*/)
{
    calculateOffset();
}
