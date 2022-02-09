#include "menuwindow.h"
#include "ui_menuwindow.h"


MenuWindow::MenuWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MenuWindow)
{
    jsonManager = JsonManager();
    isActiveNotes=true;
    ui->setupUi(this);
    ShowAllNotes(jsonManager.data.object().keys());
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
    /*MainWindow *w = new class MainWindow(noteName, this);
    w->show();
    this->setVisible(false);
    connect(w, SIGNAL(updateView()), this, SLOT(ShowAllNotes(jsonManager.GetJsonKeysByTags(searchtags))));
*/
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
        QLabel *datelabel=new QLabel();
        datelabel->setText(recordsObject[note].toObject()["Date"].toString());
        datelabel->setStyleSheet(
                    "QLabel {"
                    "color:rgba(246, 237, 237, 1);"
                    "font-family: Proxima Nove;"
                    "font-size:10px;"
                    "}"
                    );
        QLabel *shorttextnote=new QLabel();
        shorttextnote->setText(recordsObject[note].toObject()["ShortText"].toString());
        shorttextnote->setStyleSheet(
                    "QLabel {"
                    "color:rgba(246, 237, 237, 1);"
                    "font-family: Proxima Nove;"
                    "font-size:13px;"
                    "min-height:50px;"
                    "min-width:200px;"
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
                               "QPushButton::hover{"
                               "background-color: rgba(79, 115, 180, 1);"
                               "color: rgba(255, 255, 255, 1);"
                               "border: none;"
                                "}"
                               "QPushButton::pressed{"
                               "background-color: rgba(79, 115, 207, 0.7);"
                               "color: rgba(255, 255, 255, 1);"
                               "border: none;"
                                "}"
                    );
        QObject::connect(btnopen, &QPushButton::clicked, [this, note](){openNotes(note);});
        layout->addWidget(datelabel);
        layout->addWidget(shorttextnote);
        layout->addWidget(btnopen);
        layout->setAlignment(shorttextnote, Qt::AlignVCenter);
        layout->setAlignment(shorttextnote, Qt::AlignLeft);
        layout->setAlignment(datelabel, Qt::AlignTop);
        layout->setAlignment(datelabel, Qt::AlignRight);
        layout->setAlignment(btnopen, Qt::AlignHCenter);

        ui->gridLayout->addWidget(framenote, row, column);
        column=column+1;
        if(column==3){
            row++;
        }
        column=column%3;
    }
    QLabel *l=new QLabel();
    ui->gridLayout->addWidget(l, row, column);

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
                       "QPushButton::hover{"
                       "background-color: rgba(79, 115, 180, 1);"
                       "color: rgba(255, 255, 255, 1);"
                       "border: none;"
                        "}"
                       "QPushButton::pressed{"
                       "background-color: rgba(79, 115, 207, 0.7);"
                       "color: rgba(255, 255, 255, 1);"
                       "border: none;"
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

