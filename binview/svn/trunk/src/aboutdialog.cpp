#include "aboutdialog.h"
#include "ui_aboutdialog.h"

AboutDialog::AboutDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AboutDialog)
{
    ui->setupUi(this);

    setFixedSize(size());

    ui->labelTitle->setText(QString("%1 %2").arg(QCoreApplication::applicationName()).arg(QCoreApplication::applicationVersion()));

    ui->labelQtCt->setText(QString("Qt version %1 at compile time").arg(QT_VERSION_STR));
    ui->labelQtRt->setText(QString("Qt version %1 at run time").arg(qVersion()));
    ui->labelPid->setText(QString("Application process id: %1").arg(QCoreApplication::applicationPid()));
}

AboutDialog::~AboutDialog()
{
    delete ui;
}
