#include "menuwindow.h"
#include <QApplication>

int main(int argc, char *argv[]) {
 QApplication a(argc, argv);
 a.setWindowIcon(QIcon(":/img/mainicon.png"));
 a.setQuitOnLastWindowClosed(true);
 MenuWindow w;
 w.show();
 return a.exec();
}
