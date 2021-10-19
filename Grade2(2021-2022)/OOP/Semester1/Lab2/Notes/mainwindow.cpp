#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "jsonmanager.h"
#include <QJsonDocument>

MainWindow::MainWindow(QString noteName, QWidget *parent) :
    QMainWindow(parent),ui(new Ui::MainWindow) {
 ui->setupUi(this);
 //После этой строчки - наши действия!
 setActivenesAction = new QAction(tr("&Архив"), this);
 connect(setActivenesAction, SIGNAL(triggered()), this, SLOT(setActivenes()));
 setTagsAction = new QAction(tr("&Теги"), this);
 connect(setTagsAction, SIGNAL(triggered()), this, SLOT(setTags()));
 backToMenuAction = new QAction(tr("&В меню"), this);
 connect(backToMenuAction, SIGNAL(triggered()), this, SLOT(backToMenu()));

 fileMenu = this->menuBar()->addMenu(tr("&Файл"));
 fileMenu->addAction(setActivenesAction);
 fileMenu->addAction(setTagsAction);
 fileMenu->addSeparator();
 fileMenu->addAction(backToMenuAction);
 textEdit = new QTextEdit();
 setCentralWidget(textEdit);
 setWindowTitle(tr("Блокнотик"));
 NoteName = noteName;
 JsonManager jsonManager;
 NoteData = jsonManager.ReadJson(jsonManager.fileName).object()[NoteName].toObject();
 JsonObjectTags=NoteData["Tags"].toArray();
 if(noteName==""){
     NoteData["isActive"]=true;
 }else{
     open();
 }
 qDebug()<<NoteData["isActive"];
}

MainWindow::~MainWindow() { delete ui; }

//Ниже - наши методы класса
void MainWindow::open() {
 QString fileName =QCoreApplication::applicationDirPath()+"/"+NoteName+".txt";
 QFile file(fileName);
 if (!file.open(QIODevice::ReadOnly)) {
   return;
 }
  QTextStream in(&file);
  textEdit->setText(in.readAll());
  file.close();
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

 void MainWindow::setTags() {
     TagsList *tagslist = new class TagsList(JsonObjectTags);

     tagslist->setModal(true);
     tagslist->show();
     connect(tagslist, SIGNAL(sendTagsList(QJsonArray)), this, SLOT(setTagsList(QJsonArray)));
 }

 void MainWindow::setTagsList(QJsonArray list)
 {
     JsonObjectTags = list;
 }

 void MainWindow::saveFunc(){
     JsonManager jsonManager = JsonManager();
     QJsonObject recordsObject = jsonManager.data.object();
     QString textEditData = textEdit->toPlainText();
     if(textEditData==""){
         if(NoteName!=""){
             QString fileName = QCoreApplication::applicationDirPath()+"/"+NoteName+".txt";
             QFile file(fileName);
             file.remove();
             recordsObject.remove(NoteName);
             jsonManager.data = QJsonDocument(recordsObject);
             jsonManager.WriteJson(jsonManager.fileName, jsonManager.data);
         }
         return;
     }



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

     QString fileName = QCoreApplication::applicationDirPath()+"/"+NoteName+".txt";
     QFile file(fileName);
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
 void MainWindow::closeEvent (QCloseEvent *event)
 {
     event->ignore();
     saveFunc();
     event->accept();
 }

 void MainWindow::backToMenu()
 {
     saveFunc();
     MenuWindow *w = new class MenuWindow();
     w->show();

     this->close();
 }
