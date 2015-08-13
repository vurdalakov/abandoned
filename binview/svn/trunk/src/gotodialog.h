#ifndef GOTODIALOG_H
#define GOTODIALOG_H

#include <QDialog>
#include <QButtonGroup>

namespace Ui {
    class GotoDialog;
}

class GotoDialog : public QDialog
{
    Q_OBJECT

    QButtonGroup m_buttonGroup;

    qint64 m_offset;

    void calculateOffset();

public:
    explicit GotoDialog(int radix, const QString& text, QWidget* parent = 0);
    ~GotoDialog();

    int radix() const;

    qint64 offset() const;

    QString text() const;

private:
    Ui::GotoDialog *ui;

private slots:
    void on_lineEdit_textEdited(const QString& text);
    void onRadioButtonClicked(int id);
};

#endif // GOTODIALOG_H
