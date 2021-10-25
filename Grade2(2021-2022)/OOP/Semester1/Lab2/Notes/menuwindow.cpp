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
        while((item = ui->gridLayout->takeAt(0))) {
            if (item->widget()) {
               delete item->widget();
            }
            delete item;
        }
    int column=0;
    int row=0;
    QJsonObject recordsObject = jsonManager.data.object();


    foreach(QString note, notes){
      QPushButton *btn = new QPushButton();
        if(recordsObject[note].toObject()["isActive"].toBool()!=isActiveNotes){
            continue;
        }

        QFrame *framenote = new QFrame();
        framenote->setStyleSheet("QFrame {background-color: rgba(104, 93, 93, 1);"
                               //  "min-height:100px;"
                                // "max-height:100px;"
                                 "max-width:220px;"
                                 "padding:3px;"
                                 "}");

        framenote->setSizePolicy(QSizePolicy(QSizePolicy::Minimum, QSizePolicy::Fixed));

        QVBoxLayout *layout = new QVBoxLayout();
        framenote->setLayout(layout);
        QLabel *shorttextnote=new QLabel();
        shorttextnote->setText(recordsObject[note].toObject()["ShortText"].toString());
        shorttextnote->setStyleSheet(
                    "QLabel {"
                    "color:rgba(246, 237, 237, 1);"
                    "font-family: Proxima Nove;"
                    "font-size:13px;"
                    "min-height:50px;"
                    "max-width:200px;"
                    "}"
                    );
        shorttextnote->setWordWrap(true);
        QPushButton *btnopen =new QPushButton();
        btnopen->setSizePolicy(QSizePolicy(QSizePolicy::Fixed, QSizePolicy::Fixed));
        btnopen->setText("Open");
        btnopen->setStyleSheet("QPushButton {background-color: rgba(79, 115, 207, 1);"
                               "border: none;"
                               "color: rgba(255, 255, 255, 1);"
                               "font-family: Proxima Nove;"
                               "font-size:13px;"
                               "padding:5px 10px;"
                               "}"
                    );
        QObject::connect(btnopen, &QPushButton::clicked, [this, note](){openNotes(note);});
        layout->addWidget(shorttextnote);
        layout->addWidget(btnopen);
        layout->setAlignment(shorttextnote, Qt::AlignTop);
        layout->setAlignment(btnopen, Qt::AlignHCenter);

        ui->gridLayout->addWidget(framenote, row, column);
        column=column+1;
        if(column==3){
            row++;
        }
        column=column%3;
    }

}


void MenuWindow::on_pushButton_3_clicked()
{
    QString newTag=ui->taginputfield->text();
    if(newTag=="" || searchtags.contains(newTag)){
        return;
    }
    searchtags.append(newTag);
    ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags));
    ui->taginputfield->clear();

    QPushButton *btn = new QPushButton();

   btn->setText(" #"+newTag+"  ");
    btn->setIcon(QIcon(":/img/close.png"));

    btn->setStyleSheet(
                       "QPushButton {background-color: rgba(79, 115, 207, 1);"
                       "border: none;"
                       "color: rgba(255, 255, 255, 1);"
                       "border-radius:13px;"
                       "font-family: Proxima Nove;"
                       "font-size:15px;"
                       "padding:5px 20px;"
                       "}"
                       );
    btn->setSizePolicy(QSizePolicy(QSizePolicy::Fixed, QSizePolicy::Fixed));
    QObject::connect(btn, &QPushButton::clicked, [btn, this, newTag](){for(int i=0; i<searchtags.size(); i++){if(searchtags[i]==newTag){searchtags.remove(i);}}; ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags)); delete btn; });
    ui->currentTagsLayout->addWidget(btn);
}


void MenuWindow::on_archiveButton_clicked()
{
    isActiveNotes=!isActiveNotes;
    if(isActiveNotes){
        ui->archiveButton->setText("Archive");
        ui->label->setText("Notes");
    }else{
        ui->archiveButton->setText("Notes");
        ui->label->setText("Archive");
    }
   ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags));

}

