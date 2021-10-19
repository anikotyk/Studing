//#include "mainwindow.h"
#include "menuwindow.h"

#include <QApplication>
int main(int argc, char *argv[]) {
 QApplication a(argc, argv);
 a.setQuitOnLastWindowClosed(true);
 QTranslator qtTranslator;
 qtTranslator.load("qt_" + QLocale::system().name(),
             QLibraryInfo::location(QLibraryInfo::TranslationsPath));
 a.installTranslator(&qtTranslator);
  //При закрытии последнего окна освободить ресурсы приложения
  //и закрыть его
// MainWindow w; //Создаём,
 MenuWindow w; //Создаём,
 w.show(); //показываем виджет
 return a.exec(); //и запускаем цикл обработки событий приложения
}
