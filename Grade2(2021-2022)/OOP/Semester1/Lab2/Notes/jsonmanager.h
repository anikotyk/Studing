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
private:
    int notescount;
public:
    QString fileName;
    QJsonDocument data;
    JsonManager();
    QJsonDocument ReadJson(QString filepath);
    void WriteJson(QString filepath, QJsonDocument data);
    QJsonObject CreateJsonObject(QString shorttext, QJsonArray jsonObjectTags, bool isActive, QString date);
    QStringList SortJsonKeysByDate(QStringList keys);
    QStringList GetJsonKeysByTags(QStringList tags);
};

#endif // JSONMANAGER_H
