#ifndef FIXEDFONTDIALOG_H
#define FIXEDFONTDIALOG_H

#include <QFontDialog>
#include <QPushButton>

class FixedFontDialog : public QFontDialog
{
    Q_OBJECT

    QPushButton* m_okButton;

public:
    explicit FixedFontDialog(const QFont& initial, QWidget* parent = 0);
    virtual ~FixedFontDialog();

signals:

public slots:
    void currentFontChanged(const QFont& font);
};

#endif // FIXEDFONTDIALOG_H
