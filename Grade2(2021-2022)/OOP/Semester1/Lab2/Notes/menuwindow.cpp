#include "menuwindow.h"
#include "ui_menuwindow.h"
#include <QPushButton>
MenuWindow::MenuWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MenuWindow)
{
    ui->setupUi(this);

    MainWindow = new class MainWindow();
}

MenuWindow::~MenuWindow()
{
    delete ui;
}


void MenuWindow::on_pushButton_clicked()
{
     MainWindow->show();  // Показываем второе окно
         this->close();
}


void MenuWindow::on_pushButton_2_clicked()
{
    QString fileName = QCoreApplication::applicationDirPath()+"/data.json";
    QFile jsonFile(fileName);
    QJsonArray recordsArray;
    QJsonObject alls;
    if (jsonFile.open(QIODevice::ReadOnly))
    {
        QByteArray saveData = jsonFile.readAll();
        jsonFile.close();
        QJsonDocument jsonDocument(QJsonDocument::fromJson(saveData));
        recordsArray = jsonDocument.array();
        alls = jsonDocument.object();
        qDebug() << jsonDocument.toJson()<<"\n\n";
        qDebug() <<"next\n";
    }
    QJsonObject recordObject;
    recordObject.insert("Hola", QJsonValue::fromVariant("John"));
    recordObject.insert("LastName", QJsonValue::fromVariant("Doe"));
    recordObject.insert("Age", QJsonValue::fromVariant(43));

    QJsonObject addressObject;
    addressObject.insert("Street", "Downing Street 10");
    addressObject.insert("City", "London");
    addressObject.insert("Country", "Great Britain");
    recordObject.insert("Address", addressObject);

    QJsonArray phoneNumbersArray;
    phoneNumbersArray.push_back("+44 1234567");
    phoneNumbersArray.push_back("+44 2345678");
    recordObject.insert("Phone Numbers", phoneNumbersArray);

    //recordsArray.push_back(recordObject);

    QJsonObject dd;
    QJsonObject ddq;
    ddq.insert("Name", "a1aa");
    ddq.insert("Surname", "bbob");
    //dd.insert("Note"+QString::number(recordsArray.size()+1), ddq);
  //  alls.insert("Note"+QString::number(alls.size()+1), ddq);
    alls.insert("Note1", ddq);
    QJsonDocument doc(alls);


    if (fileName != "") {
     QFile file(fileName);
     if (!file.open(QIODevice::WriteOnly)) {
      QMessageBox msgBox; msgBox.setText("Не могу записать файл"); msgBox.exec();
     }
     else {
      QTextStream stream(&file);

      stream <<doc.toJson();
      stream.flush();
      file.close();
     }
    }




}

