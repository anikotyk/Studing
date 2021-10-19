#include "menuwindow.h"
#include "ui_menuwindow.h"


MenuWindow::MenuWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MenuWindow)
{
    ui->setupUi(this);
    ShowAllNotes();
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
void MenuWindow::ShowAllNotes(){
    JsonManager jsonManager = JsonManager();
    QJsonObject recordsObject = jsonManager.data.object();
    QStringList keys = recordsObject.keys();
    QString a="";
    foreach(QString note, keys){
        QPushButton *btn = new QPushButton();
        btn->setText(recordsObject[note].toObject()["ShortText"].toString());
        QObject::connect(btn, &QPushButton::clicked, [this, note](){openNotes(note);});
        ui->verticalLayout->addWidget(btn);
    }
}

