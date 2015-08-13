#include "qxsysteminfo.h"

#ifdef Q_OS_WIN
#include <windows.h>
#else
//#include <stdio.h>
#include <sys/statvfs.h>
//#include <errno.h>
//#include <fcntl.h>
#endif

#include <QDir>

QxSystemInfo::QxSystemInfo()
{
}

quint64 QxSystemInfo::getDiskFreeSpace(const QString& directoryOrFilePath)
{
    QDir dir(directoryOrFilePath);
    QString directoryPath = dir.absolutePath();

    while (!dir.exists() && (directoryPath.length() > 1))
    {
        directoryPath = QDir::cleanPath(directoryPath + "/../");

        dir.setPath(directoryPath);
    }

#ifdef Q_OS_WIN
    ULARGE_INTEGER freeBytesAvailable;
    if (!GetDiskFreeSpaceExW((LPCWSTR)directoryPath.constData(), &freeBytesAvailable, NULL, NULL))
    {
        return 0;
    }

    quint64 freeSpace = freeBytesAvailable.QuadPart;
#else
    struct statvfs buffer;

    if (statvfs(directoryPath.toLocal8Bit().constData(), &buffer) != 0) // http://www.opengroup.org/onlinepubs/000095399/functions/statvfs.html
    {
        return 0;
    }

    quint64 freeSpace = buffer.f_bavail * buffer.f_frsize;
#endif

    return freeSpace;
}
