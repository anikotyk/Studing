#include "tagslist.h"
#include "ui_tagslist.h"

TagsList::TagsList(QJsonArray tagsliststart, QWidget *parent) :
    QDialog(parent),
    ui(new Ui::TagsList)
{
    ui->setupUi(this);
    tagslist=tagsliststart;
    AddTagsToWidget();
}

TagsList::~TagsList()
{
    delete ui;
}

void TagsList::on_addbutton_clicked()
{
    QLineEdit *tagdata =  ui->taginput;
    qDebug()<<tagdata->text();
    if(!tagslist.contains(tagdata->text())){
        tagslist.append(tagdata->text());
        AddTagsToWidget();
    }
    tagdata->clear();
}

void TagsList::AddTagsToWidget(){
    ui->tagListWidget->clear();
    qDebug()<<"Now the list is\n";
    foreach(QJsonValue tag, tagslist){
      //  QListWidgetItem *it = new QListWidgetItem(tag);
        qDebug()<<tag.toString()<<"\n";
        ui->tagListWidget->addItem(tag.toString());
    }
}

void TagsList::on_deletebutton_clicked()
{
    QString str =ui->tagListWidget->currentItem()->text();
    for(int i=0; i<tagslist.size(); i++){
        if(tagslist[i].toString()==str){
            tagslist.removeAt(i);
            break;
        }
    }
    AddTagsToWidget();
}

void TagsList::closeEvent(QCloseEvent *)
{
    emit sendTagsList(tagslist);
    qDebug()<<"Added new tags";
    this->close();
}

void TagsList::on_pushButton_clicked()
{
    emit sendTagsList(tagslist);
    this->close();
}
