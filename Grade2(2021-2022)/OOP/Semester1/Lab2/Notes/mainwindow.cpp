#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "jsonmanager.h"
#include <QJsonDocument>

MainWindow::MainWindow(QApplication *parent, QString noteName) :
    QMainWindow(),ui(new Ui::MainWindow) {
 ui->setupUi(this);
 //После этой строчки - наши действия!
 openAction = new QAction(tr("&Открыть"), this);
 connect(openAction, SIGNAL(triggered()), this, SLOT(open()));
 saveAction = new QAction(tr("&Сохранить"), this);
 connect(saveAction, SIGNAL(triggered()), this, SLOT(save()));
 setActivenesAction = new QAction(tr("&Архив"), this);
 connect(setActivenesAction, SIGNAL(triggered()), this, SLOT(setActivenes()));
 exitAction = new QAction(tr("&Выход"), this);
 connect(exitAction, SIGNAL(triggered()), this, SLOT(close()));
 fileMenu = this->menuBar()->addMenu(tr("&Файл"));
 fileMenu->addAction(openAction);
 fileMenu->addAction(saveAction);
 fileMenu->addAction(setActivenesAction);
 fileMenu->addSeparator();
 fileMenu->addAction(exitAction);
 textEdit = new QTextEdit();
 setCentralWidget(textEdit);
 setWindowTitle(tr("Блокнотик"));
 NoteName = noteName;
 JsonManager jsonManager;
 NoteData = jsonManager.ReadJson(jsonManager.fileName).object()[NoteName].toObject();
 JsonObjectTags=NoteData["Tags"].toArray();
 if(noteName==""){
     NoteData["isActive"]=true;
 }
 qDebug()<<NoteData["isActive"];
}

MainWindow::~MainWindow() { delete ui; }

//Ниже - наши методы класса
void MainWindow::open() {
 QString fileName = QFileDialog::getOpenFileName(this,
  tr("Открыть файл"), "",
  tr("Текстовые файлы (*.txt);;Файлы C++ (*.cpp *.h)"));
 if (fileName != "") {
  QFile file(fileName);
  if (!file.open(QIODevice::ReadOnly)) {
   QMessageBox::critical(this, tr("Ошибка"), tr("Не могу открыть файл"));
   return;
  }
  QTextStream in(&file);
  textEdit->setText(in.readAll());
  file.close();
 }
}

void MainWindow::save() {
 QString fileName = QFileDialog::getSaveFileName(this,
  tr("Сохранить файл"), "",
  tr("Текстовые файлы (*.txt);;Файлы C++ (*.cpp *.h)"));
 if (fileName != "") {
  QFile file(fileName);
  JsonManager jsonManager = JsonManager();
  QJsonObject recordsObject = jsonManager.data.object();
  QString textEditData = textEdit->toPlainText();

  int stringsize=textEditData.size();
  QString afterstring="";
  if(stringsize>20){
    afterstring="...";
    stringsize=20;
  }
  if(NoteName==""){
      NoteName="Note"+QString::number(jsonManager.notescount+1);
  }

  recordsObject.insert(NoteName, jsonManager.CreateJsonObject(textEditData.mid(0,stringsize)+afterstring, JsonObjectTags, NoteData["isActive"].toBool()));
  jsonManager.data = QJsonDocument(recordsObject);
  jsonManager.WriteJson(jsonManager.fileName, jsonManager.data);

  if (!file.open(QIODevice::WriteOnly)) {
   QMessageBox msgBox; msgBox.setText("Не могу записать файл"); msgBox.exec();
  }
  else {
   QTextStream stream(&file);
   stream << textEditData;
   stream.flush();
   file.close();
  }
 }
}

 void MainWindow::AddTag(QString tag) {
     JsonObjectTags.push_back(tag);
 }

 void MainWindow::setActivenes() {
     QString question;
     if(NoteData["isActive"].toBool()){
         question="Добавить запись в архив?";
     }else{
         question="Убрать запись из архива?";
     }

  QMessageBox::StandardButton reply = QMessageBox::question(this, "Архив", question, QMessageBox::Yes| QMessageBox::No );
  if(reply==QMessageBox::Yes){
    NoteData["isActive"]=!NoteData["isActive"].toBool();
  }
 }
