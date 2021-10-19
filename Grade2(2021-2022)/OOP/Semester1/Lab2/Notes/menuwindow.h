#ifndef MENUWINDOW_H
#define MENUWINDOW_H
#include <QMainWindow>
#include <mainwindow.h>
#include <QPushButton>
#include "jsonmanager.h"

namespace Ui {
class MenuWindow;
}

class MenuWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MenuWindow(QWidget *parent = nullptr);
    ~MenuWindow();
    void ShowAllNotes();

public slots:
    void on_pushButton_clicked();
    void openNotes(QString noteName="");

private:
    Ui::MenuWindow *ui;
    //MainWindow *MainWindow;

};

#endif // MENUWINDOW_H
