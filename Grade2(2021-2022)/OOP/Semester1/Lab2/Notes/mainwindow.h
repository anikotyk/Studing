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
    QWidget *menu;
    QString NoteName;
    bool isDeleted;
    QJsonArray JsonObjectTags;
    QJsonArray JsonObjectTagsCopy;
    QJsonObject NoteData;
public:
 explicit MainWindow(QString noteName="", QWidget *parent = 0);
 ~MainWindow();
private slots:
 void open();
 void setActivenes();
 void setTags();
 void setTagsList(QJsonArray list);
 void closeEvent (QCloseEvent *event);
 void saveFunc();
 void backToMenu();
 void deleteNote();
 void deleteAndExit();
signals:
    void updateView();
private:
 Ui::MainWindow *ui;
 QTextEdit *textEdit;
 QAction *setActivenesAction;
 QAction *setTagsAction;
 QAction *backToMenuAction;
 QAction *deleteNoteAction;
 QMenu *fileMenu;

};

#endif // MAINWINDOW_H
