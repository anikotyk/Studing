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

QStringList JsonManager::SortJsonKeysByDate(QStringList keys){
    QJsonObject jsonObject = data.object();
    int n =keys.size();
    for(int i=0; i<n-1; i++){
        for(int j=0; j<n-i-1; j++){
            QDateTime Date = QDateTime::fromString(jsonObject[keys[j]].toObject()["Date"].toString(),"dd.MM.yyyy HH:mm:ss");
            QDateTime Date2 = QDateTime::fromString(jsonObject[keys[j+1]].toObject()["Date"].toString(),"dd.MM.yyyy HH:mm:ss");
            if(Date<Date2){
                QString tmp = keys[j];
                keys[j] = keys[j+1];
                keys[j+1]=tmp;
            }
        }
    }

    return keys;
}

QStringList JsonManager::GetJsonKeysByTags(QStringList tags){
    QJsonObject jsonObject = data.object();
    QStringList keys = jsonObject.keys();
    QStringList res;
    int n =jsonObject.size();

    for(int i=0; i<n; i++){
        bool flag=true;
        foreach(QString tag, tags){
            if(!jsonObject[keys[i]].toObject()["Tags"].toArray().contains(tag)){
                flag=false;
                break;
            }
        }
        if(flag){
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

QJsonObject JsonManager::CreateJsonObject(QString shorttext, QJsonArray jsonObjectTags, bool isActive, QString date){
    QJsonObject jsonObject;
    jsonObject.insert("ShortText", shorttext);
    jsonObject.insert("Date",date );
    jsonObject.insert("isActive", isActive);
    jsonObject.insert("Tags", jsonObjectTags);
    return jsonObject;
}



