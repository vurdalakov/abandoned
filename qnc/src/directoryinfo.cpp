#include "directoryinfo.h"

DirectoryInfo::DirectoryInfo(QObject* parent) :
    QObject(parent)
{
}

void DirectoryInfo::go(const QString& path)
{
    m_dir.setPath(path);

    refresh();
}

void DirectoryInfo::refresh()
{
    m_list.clear();

    m_list = m_dir.entryInfoList(QDir::Dirs | QDir::Files | QDir::NoDot | QDir::Hidden, QDir::Name | QDir::IgnoreCase | QDir::DirsFirst);

    for (int i = 0; i < m_list.size(); i++)
    {
        if (".." == m_list.at(i).fileName())
        {
            QFileInfo fileInfo = m_list.at(i);
            m_list.removeAt(i);
            if (!m_dir.isRoot())
            {
                m_list.insert(0, fileInfo);
            }
            break;
        }
     }
}

int DirectoryInfo::count() const
{
    return m_list.count();
}

QFileInfo DirectoryInfo::at(int i) const
{
    return m_list.at(i);
}

int DirectoryInfo::find(const QString& name) const
{
    for (int i = 0; i < m_list.size(); i++)
    {
        if (name == m_list.at(i).fileName())
        {
            return i;
        }
    }

    return -1;
}

QString DirectoryInfo::name() const
{
    return m_dir.dirName();
}

QString DirectoryInfo::path() const
{
    return m_dir.path();
}
