#include "jsonmanager.h"

JsonManager::JsonManager()
{
    fileName = QCoreApplication::applicationDirPath()+"/data.json";
    data = ReadJson(fileName);
    notescount = data.object().size();
}

QJsonDocument JsonManager::ReadJson(QString filepath){
    QFile jsonFile(filepath);
    QJsonDocument jsonDocument;
    if (jsonFile.open(QIODevice::ReadOnly))
    {
        QByteArray saveData = jsonFile.readAll();
        jsonFile.close();
        jsonDocument = QJsonDocument::fromJson(saveData);

    }

    return jsonDocument;
}

QStringList JsonManager::SortJsonKeysByDate(QJsonDocument jsonDocument, QStringList keys){
    QJsonObject jsonObject = jsonDocument.object();
    int n =jsonObject.size();
    for(int i=0; i<n-1; i++){
        for(int j=0; j<n-i-1; j++){
            QDate Date = QDate::fromString(jsonObject[keys[j]].toObject()["Date"].toString(),"dd.MM.yyyy HH:mm:ss");
            QDate Date2 = QDate::fromString(jsonObject[keys[j+1]].toObject()["Date"].toString(),"dd.MM.yyyy HH:mm:ss");
            if(Date>Date2){
                QString tmp = keys[j];
                keys[j] = keys[j+1];
                keys[j+1]=tmp;
            }
        }
    }
    return keys;
}

QStringList JsonManager::GetJsonKeysByTag(QJsonDocument jsonDocument, QString tag){
    QJsonObject jsonObject = jsonDocument.object();
    QStringList keys = jsonObject.keys();
    QStringList res;
    int n =jsonObject.size();

    for(int i=0; i<n; i++){
        if(jsonObject[keys[i]].toObject()["Tags"].toArray().contains(tag)){
            res.append(keys[i]);
        }
    }
    return res;
}

QStringList JsonManager::AddKeysLists(QList<QStringList> lists){

    QStringList keys;
    foreach(QStringList list, lists){
        foreach(QString str, list){
            keys.append(str);
        }
    }
    return keys;
}

QStringList JsonManager::SubstractKeysLists(QList<QStringList> lists){
    QStringList keys;

    foreach(QString str, lists[0]){
        bool flag=true;
        foreach(QStringList list, lists){
            if(!list.contains(str)){
                flag=false;
                break;
            }
        }
        if(flag){
            keys.append(str);
        }
    }

    return keys;
}

QStringList JsonManager::GetKeysListOfActiveOrArchived(QJsonDocument jsonDocument, bool isActive){
    QJsonObject jsonObject = jsonDocument.object();
    QStringList keys = jsonObject.keys();
    QStringList res;

    for(int i=0; i<jsonObject.size(); i++){
       if(jsonObject[keys[i]].toObject()["isActive"].toBool()==isActive){
           res.append(keys[i]);
       }
    }

    return res;
}

void JsonManager::WriteJson(QString filepath, QJsonDocument data){
    if (filepath != "") {
     QFile file(filepath);
     if (file.open(QIODevice::WriteOnly)) {
      QTextStream stream(&file);
      stream <<data.toJson();
      stream.flush();
      file.close();
     }
    }
}

QJsonObject JsonManager::CreateJsonObject(QString shorttext, QJsonArray jsonObjectTags, bool isActive){
    QJsonObject jsonObject;
    jsonObject.insert("ShortText", shorttext);
    jsonObject.insert("Date", QDateTime::currentDateTime().toString("dd.MM.yyyy HH:mm:ss"));
    jsonObject.insert("isActive", isActive);
    jsonObject.insert("Tags", jsonObjectTags);
    return jsonObject;
}



