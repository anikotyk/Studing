#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QtWidgets>
#include <tagslist.h>
#include <menuwindow.h>

namespace Ui { class MainWindow; }

class MainWindow : public QMainWindow {
 Q_OBJECT

public:
    QString NoteName;
    QJsonArray JsonObjectTags;
    QJsonObject NoteData;
public:
 explicit MainWindow(QString noteName="", QWidget *parent = 0);
 ~MainWindow();
private slots:
 void open(); //метод для открытия файла
 void setActivenes();
 void setTags();
 void setTagsList(QJsonArray list);
 void closeEvent (QCloseEvent *event);
 void saveFunc();
 void backToMenu();
 void deleteNote();
private:
 Ui::MainWindow *ui;
 QTextEdit *textEdit; //указатель на поле ввода текста
 QAction *setActivenesAction; //и "Выйти"
 QAction *setTagsAction; //и "Выйти"
 QAction *backToMenuAction; //и "Выйти"
 QAction *deleteNoteAction;
 QMenu *fileMenu; //Указатель на меню
};

#endif // MAINWINDOW_H
