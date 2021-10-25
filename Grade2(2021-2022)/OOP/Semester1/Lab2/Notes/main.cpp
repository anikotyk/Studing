
#include "menuwindow.h"

#include <QApplication>
int main(int argc, char *argv[]) {
 QApplication a(argc, argv);
 a.setWindowIcon(QIcon(":/img/mainicon.png"));
 a.setQuitOnLastWindowClosed(true);
 /*QTranslator qtTranslator;
 qtTranslator.load("qt_" + QLocale::system().name(),
             QLibraryInfo::location(QLibraryInfo::TranslationsPath));
 a.installTranslator(&qtTranslator);*/
 MenuWindow w; //Создаём,
 w.show(); //показываем виджет
 return a.exec(); //и запускаем цикл обработки событий приложения
}
