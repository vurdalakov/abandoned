#ifndef QVACTIONGROUP_H
#define QVACTIONGROUP_H

#include <QActionGroup>
#include <QMap>

class QVActionGroup : public QActionGroup
{
    Q_OBJECT

    QMap<int, QAction*> m_ids;

public:
    explicit QVActionGroup(QObject *parent = 0);
    ~QVActionGroup();

    QAction* addAction(QAction* action);
    QAction* addAction(const QString& text);
    QAction* addAction(const QIcon& icon, const QString& text);

    QAction* addAction(QAction* action, int id);
    QAction* addAction(const QString& text, int id);
    QAction* addAction(const QIcon& icon, const QString& text, int id);

    QAction* removeAction(QAction* action);
    QAction* removeAction(int id);

    QAction* action(int id) const;
    int id(QAction* action) const;

    void setId(QAction* action, int id);

    int checkedId() const;
    QAction* setCheckedId(int id);

    void clear();

signals:

    void hovered(int id);
    void triggered(int id);

public slots:

    void onHovered(QAction* action);
    void onTriggered(QAction* action);
};

#endif // QVACTIONGROUP_H
