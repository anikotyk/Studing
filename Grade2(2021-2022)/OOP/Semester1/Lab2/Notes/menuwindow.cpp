#include "menuwindow.h"
#include "ui_menuwindow.h"


MenuWindow::MenuWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MenuWindow)
{
    jsonManager = JsonManager();
    isActiveNotes=true;
    ui->setupUi(this);
    ShowAllNotes(jsonManager.SortJsonKeysByDate(jsonManager.data.object().keys()));
}

MenuWindow::~MenuWindow()
{
    delete ui;
}


void MenuWindow::on_pushButton_clicked()
{
    openNotes();
}



void MenuWindow::openNotes(QString noteName)
{
    MainWindow *w = new class MainWindow(noteName);
    w->show();
    this->close();
}

#include <QAction>
void MenuWindow::ShowAllNotes(QStringList notes){

    notes = jsonManager.SortJsonKeysByDate(notes);

    QLayoutItem *item;
        while((item = ui->verticalLayout->takeAt(0))) {
            if (item->widget()) {
               delete item->widget();
            }
            delete item;
        }
    QJsonObject recordsObject = jsonManager.data.object();
    foreach(QString note, notes){
        QPushButton *btn = new QPushButton();
        if(recordsObject[note].toObject()["isActive"].toBool()!=isActiveNotes){
            continue;
        }
        btn->setText(recordsObject[note].toObject()["ShortText"].toString());
        QObject::connect(btn, &QPushButton::clicked, [this, note](){openNotes(note);});
        ui->verticalLayout->addWidget(btn);
    }
}


void MenuWindow::on_pushButton_3_clicked()
{
    QString newTag=ui->taginputfield->text();
    searchtags.append(newTag);
    ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags));
    ui->taginputfield->clear();
    QPushButton *btn = new QPushButton();
    btn->setText(newTag);
    QObject::connect(btn, &QPushButton::clicked, [btn, this, newTag](){for(int i=0; i<searchtags.size(); i++){if(searchtags[i]==newTag){searchtags.remove(i);}}; ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags)); delete btn; });
    ui->currentTagsLayout->addWidget(btn);
}


void MenuWindow::on_archiveButton_clicked()
{
    isActiveNotes=!isActiveNotes;
    if(isActiveNotes){
        ui->archiveButton->setText("Archive");
    }else{
        ui->archiveButton->setText("Active notes");
    }
   ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags));

}

