#ifndef UTILS_H
#define UTILS_H

#include <QString>

class Utils
{
    Utils() {} // static class

public:

    static QString formatFileSize(qint64 fileSize);

};

#endif // UTILS_H
