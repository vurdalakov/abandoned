#include "qvactiongroup.h"

QVActionGroup::QVActionGroup(QObject *parent) :
    QActionGroup(parent)
{
    connect(this, SIGNAL(hovered(QAction*)), this, SLOT(onHovered(QAction*)));
    connect(this, SIGNAL(triggered(QAction*)), this, SLOT(onTriggered(QAction*)));
}

QVActionGroup::~QVActionGroup()
{
    disconnect(this, SIGNAL(hovered(QAction*)), this, SLOT(onHovered(QAction*)));
    disconnect(this, SIGNAL(triggered(QAction*)), this, SLOT(onTriggered(QAction*)));
}

void QVActionGroup::onHovered(QAction* action)
{
    int i = id(action);

    if (i >= 0)
    {
        emit hovered(i);
    }
}

void QVActionGroup::onTriggered(QAction* action)
{
    int i = id(action);

    if (i >= 0)
    {
        emit triggered(i);
    }
}

QAction* QVActionGroup::addAction(QAction* action)
{
    return QActionGroup::addAction(action);
}

QAction* QVActionGroup::addAction(const QString& text)
{
    return QActionGroup::addAction(text);
 }

QAction* QVActionGroup::addAction(const QIcon& icon, const QString& text)
{
    return QActionGroup::addAction(icon, text);
}

QAction* QVActionGroup::addAction(QAction* action, int id)
{
    addAction(action);

    setId(action, id);

    return action;
}

QAction* QVActionGroup::addAction(const QString& text, int id)
{
    QAction* action = addAction(text);

    setId(action, id);

    return action;
}

QAction* QVActionGroup::addAction(const QIcon& icon, const QString& text, int id)
{
    QAction* action = addAction(icon, text);

    setId(action, id);

    return action;
}

QAction* QVActionGroup::removeAction(QAction* action)
{
    if (action != 0)
    {
        QActionGroup::removeAction(action);

        m_ids.remove(id(action));
    }

    return action;
}

QAction* QVActionGroup::removeAction(int id)
{
    QAction* a = action(id);

    if (a != 0)
    {
        QActionGroup::removeAction(a);
    }

    m_ids.remove(id);

    return a;
}

QAction* QVActionGroup::action(int id) const
{
    return m_ids.value(id, 0);
}

int QVActionGroup::id(QAction* action) const
{
    return m_ids.key(action, -1);
}

void QVActionGroup::setId(QAction* action, int id)
{
    m_ids.insert(id, action);
}

int QVActionGroup::checkedId() const
{
    QAction* action = checkedAction();

    return 0 == action ? -1 : id(action);
}

QAction* QVActionGroup::setCheckedId(int id)
{
    QAction* a = action(id);

    if (a != 0)
    {
        a->setChecked(true);
    }

    return a;
}

void QVActionGroup::clear()
{
    QList<QAction*> list = actions();
    QAction* action;
    foreach (action, list)
    {
        removeAction(action);
        delete action;
    }
}
