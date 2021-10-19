#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QtWidgets>

namespace Ui { class MainWindow; }

class MainWindow : public QMainWindow {
 Q_OBJECT

public:
    QString NoteName;
    QJsonArray JsonObjectTags;
    QJsonObject NoteData;
public:
 explicit MainWindow(QApplication *parent = 0, QString noteName="");
 ~MainWindow();
 void AddTag(QString tag);
private slots:
 void open(); //метод для открытия файла
 void save(); //метод для сохранения файла
 void setActivenes();
private:
 Ui::MainWindow *ui;
 QTextEdit *textEdit; //указатель на поле ввода текста
 QAction *openAction; //указатели на действия "Открыть",
 QAction *saveAction; //"Сохранить"
 QAction *setActivenesAction; //и "Выйти"
 QAction *exitAction; //и "Выйти"
 QMenu *fileMenu; //Указатель на меню

};

#endif // MAINWINDOW_H
