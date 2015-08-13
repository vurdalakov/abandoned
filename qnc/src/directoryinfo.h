#ifndef DIRECTORYINFO_H
#define DIRECTORYINFO_H

#include <QObject>
#include <QFileInfoList>
#include <QDir>

class DirectoryInfo : public QObject
{
    Q_OBJECT

    QDir m_dir;
    QFileInfoList m_list;

public:

    explicit DirectoryInfo(QObject* parent = 0);

    void go(const QString& path);
    void refresh();

    int count() const;
    QFileInfo at(int i) const;

    int find(const QString& name) const;

    QString name() const;
    QString path() const;

signals:

public slots:

};

#endif // DIRECTORYINFO_H
