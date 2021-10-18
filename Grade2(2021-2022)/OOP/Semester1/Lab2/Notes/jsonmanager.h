#ifndef JSONMANAGER_H
#define JSONMANAGER_H

#include <QString>
#include <QJsonDocument>
#include <QFile>
#include <QJsonArray>
#include <QJsonObject>
#include <QCoreApplication>


class JsonManager
{
public:
    int notescount;
    QString fileName;
    QJsonDocument data;
public:
    JsonManager();
    QJsonDocument ReadJson(QString filepath);
    void WriteJson(QString filepath, QJsonDocument data);
    QJsonObject CreateJsonObject(QString shorttext, QJsonArray jsonObjectTags, bool isActive);
    QStringList SortJsonKeysByDate(QJsonDocument jsonDocument, QStringList keys);
    QStringList GetJsonKeysByTag(QJsonDocument jsonDocument, QString tag);
    QStringList AddKeysLists(QList<QStringList> lists);
    QStringList SubstractKeysLists(QList<QStringList> lists);
    QStringList GetKeysListOfActiveOrArchived(QJsonDocument jsonDocument, bool isActive=true);
};

#endif // JSONMANAGER_H
