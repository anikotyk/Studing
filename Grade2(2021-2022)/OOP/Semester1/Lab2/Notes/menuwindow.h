#ifndef MENUWINDOW_H
#define MENUWINDOW_H
#include <QMainWindow>
#include <mainwindow.h>
#include <QPushButton>
#include "jsonmanager.h"
#include <QList>

namespace Ui {
class MenuWindow;
}

class MenuWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MenuWindow(QWidget *parent = nullptr);
    ~MenuWindow();
    void ShowAllNotes(QStringList notes);

public slots:
    void on_pushButton_clicked();
    void openNotes(QString noteName="");

private slots:
    void on_pushButton_3_clicked();

    void on_archiveButton_clicked();

private:
    Ui::MenuWindow *ui;
    QStringList searchtags;
    JsonManager jsonManager;
    bool isActiveNotes;
    //MainWindow *MainWindow;

};

#endif // MENUWINDOW_H
