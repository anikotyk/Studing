#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "jsonmanager.h"
#include <QJsonDocument>

MainWindow::MainWindow(QString noteName, QWidget *parent) :
    QMainWindow(parent),ui(new Ui::MainWindow) {
 ui->setupUi(this);

 setActivenesAction = new QAction(tr("&Archive"), this);
 connect(setActivenesAction, SIGNAL(triggered()), this, SLOT(setActivenes()));
 setTagsAction = new QAction(tr("&Tags"), this);
 connect(setTagsAction, SIGNAL(triggered()), this, SLOT(setTags()));
 backToMenuAction = new QAction(tr("&Menu"), this);
 connect(backToMenuAction, SIGNAL(triggered()), this, SLOT(backToMenu()));
 deleteNoteAction = new QAction(tr("&Delete"), this);
 connect(deleteNoteAction, SIGNAL(triggered()), this, SLOT(deleteAndExit()));

 fileMenu = this->menuBar()->addMenu(tr("&File"));
 fileMenu->addAction(setActivenesAction);
 fileMenu->addAction(setTagsAction);
 fileMenu->addSeparator();
 fileMenu->addAction(backToMenuAction);
 fileMenu->addSeparator();
 fileMenu->addAction(deleteNoteAction);
 textEdit = new QTextEdit();
 setCentralWidget(textEdit);
 setWindowTitle(tr("Notes"));
 NoteName = noteName;
 JsonManager jsonManager;
 NoteData = jsonManager.ReadJson(jsonManager.fileName).object()[NoteName].toObject();
 JsonObjectTags=NoteData["Tags"].toArray();
 isDeleted=false;
 menu=parent;
 if(noteName==""){
     NoteData["isActive"]=true;
 }else{
     open();
 }

}

MainWindow::~MainWindow() { delete ui; }

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
         question="Add note to archive?";
     }else{
         question="Remove note from archive?";
     }

  QMessageBox::StandardButton reply = QMessageBox::question(this, "Archive", question, QMessageBox::Yes| QMessageBox::No );
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


     QString textEditData = textEdit->toPlainText();
     textEditData=textEditData.simplified();
     if(textEditData==""){
         if(NoteName!=""){
            deleteNote();
         }
         return;
     }

     bool flagChangeData=true;
     JsonManager jsonManager = JsonManager();
     QJsonObject recordsObject = jsonManager.data.object();


     if(NoteName==""){
         int lastnumber=0;
         for(int i=0; i<jsonManager.data.object().size(); i++){
             qDebug()<<(jsonManager.data.object().keys()[i]).mid(4);
             int number = (jsonManager.data.object().keys()[i]).mid(4).toInt();
             if(number>lastnumber){
                 lastnumber=number;
             }
         }
         NoteName="Note"+QString::number(lastnumber+1);
     }

     QString fileName = QCoreApplication::applicationDirPath()+"/"+NoteName+".txt";
     QFile file(fileName);
     if (file.open(QIODevice::ReadOnly)) {
         QTextStream in(&file);
         if(in.readAll()!=textEditData){
             flagChangeData=false;
         }
         file.close();
     }
     if (!file.open(QIODevice::WriteOnly)) {
      QMessageBox msgBox; msgBox.setText("Can't write file"); msgBox.exec();
     }
     else {
      QTextStream stream(&file);
      stream << textEditData;
      stream.flush();
      file.close();
     }

     if(JsonObjectTags!=JsonObjectTagsCopy){
         flagChangeData=true;
     }

     int stringsize=textEditData.size();
     QString afterstring="";
     if(stringsize>100){
       afterstring="...";
       stringsize=100;
     }
     QString date;
     if(flagChangeData){
         date= QDateTime::currentDateTime().toString("dd.MM.yyyy HH:mm:ss");
     }else{
         date = NoteData["Date"].toString();
     }

     recordsObject.insert(NoteName, jsonManager.CreateJsonObject(textEditData.mid(0,stringsize)+afterstring, JsonObjectTags, NoteData["isActive"].toBool(), date));
     jsonManager.data = QJsonDocument(recordsObject);
     jsonManager.WriteJson(jsonManager.fileName, jsonManager.data);


 }
 void MainWindow::closeEvent (QCloseEvent *event)
 {
     event->ignore();
     if(!isDeleted){
             saveFunc();
          }
          event->accept();
 }

 void MainWindow::backToMenu()
 {
     if(!isDeleted){
         saveFunc();
     }
     MenuWindow *w = new class MenuWindow();
     w->show();
     //menu->show();
     //emit updateView();
     //menu->ShowAllNotes(menu->jsonManager.GetJsonKeysByTags(menu->searchtags));
     this->close();
 }

 void MainWindow::deleteNote(){
     isDeleted=true;
     JsonManager jsonManager = JsonManager();
     QJsonObject recordsObject = jsonManager.data.object();

     QString fileName = QCoreApplication::applicationDirPath()+"/"+NoteName+".txt";
     qDebug()<<fileName;
     QFile::remove(fileName);
     recordsObject.remove(NoteName);
     jsonManager.data = QJsonDocument(recordsObject);
     jsonManager.WriteJson(jsonManager.fileName, jsonManager.data);
 }

 void MainWindow::deleteAndExit(){
     deleteNote();
     backToMenu();
 }
