#ifndef QXSYSTEMINFO_H
#define QXSYSTEMINFO_H

#include <QString>

class QxSystemInfo
{

public:
    QxSystemInfo();

    static quint64 getDiskFreeSpace(const QString& directoryOrFilePath);
};

#endif // QXSYSTEMINFO_H
