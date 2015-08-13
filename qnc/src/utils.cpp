#include "utils.h"

QString Utils::formatFileSize(qint64 fileSize)
{
    const qint64 kB = 1024;
    const qint64 mB = 1024 * 1024;
    const qint64 tB = 1024 * 1024 * 1024;

    if (fileSize < 100000)
    {
        return QString("%L1").arg(fileSize);
    }
    else if (fileSize < 1000 * kB)
    {
        return QString("%L1 KB").arg(fileSize / kB);
    }
    else if (fileSize < 1000 * mB)
    {
        return QString("%L1 MB").arg(fileSize / mB);
    }
    else
    {
        return QString("%L1 TB").arg(fileSize / tB);
    }
}
