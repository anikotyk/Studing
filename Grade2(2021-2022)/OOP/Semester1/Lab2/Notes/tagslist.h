#ifndef TAGSLIST_H
#define TAGSLIST_H

#include <QDialog>
#include <QJsonArray>
namespace Ui {
class TagsList;
}

class TagsList : public QDialog
{
    Q_OBJECT

public:  
    explicit TagsList(QJsonArray tagsliststart = QJsonArray(), QString noteName="", QWidget *parent = nullptr);
    ~TagsList();
private slots:
    void on_addbutton_clicked();
    void AddTagsToWidget();
    void on_deletebutton_clicked();
    void on_pushButton_clicked();
    void closeEvent(QCloseEvent *);
signals:
    void sendTagsList(QJsonArray list, QString NoteName);
private:
    Ui::TagsList *ui;
    QJsonArray tagslist;
    QString NoteName;
};

#endif // TAGSLIST_H
