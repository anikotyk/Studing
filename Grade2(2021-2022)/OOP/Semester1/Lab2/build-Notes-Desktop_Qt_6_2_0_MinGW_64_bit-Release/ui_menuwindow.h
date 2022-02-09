/********************************************************************************
** Form generated from reading UI file 'menuwindow.ui'
**
** Created by: Qt User Interface Compiler version 6.2.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MENUWINDOW_H
#define UI_MENUWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QFrame>
#include <QtWidgets/QGridLayout>
#include <QtWidgets/QHBoxLayout>
#include <QtWidgets/QLabel>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QScrollArea>
#include <QtWidgets/QSpacerItem>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QVBoxLayout>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MenuWindow
{
public:
    QWidget *centralwidget;
    QPushButton *pushButton;
    QPushButton *archiveButton;
    QFrame *frame;
    QPushButton *pushButton_3;
    QLineEdit *taginputfield;
    QWidget *horizontalLayoutWidget;
    QHBoxLayout *horizontalLayout;
    QLabel *label;
    QScrollArea *scrollArea;
    QWidget *scrollAreaWidgetContents;
    QHBoxLayout *horizontalLayout_2;
    QHBoxLayout *currentTagsLayout;
    QSpacerItem *horizontalSpacer;
    QScrollArea *scrollArea_2;
    QWidget *scrollAreaWidgetContents_3;
    QVBoxLayout *verticalLayout;
    QGridLayout *gridLayout;
    QSpacerItem *verticalSpacer;
    QSpacerItem *verticalSpacer_2;
    QMenuBar *menubar;
    QStatusBar *statusbar;

    void setupUi(QMainWindow *MenuWindow)
    {
        if (MenuWindow->objectName().isEmpty())
            MenuWindow->setObjectName(QString::fromUtf8("MenuWindow"));
        MenuWindow->resize(800, 620);
        MenuWindow->setMinimumSize(QSize(800, 620));
        MenuWindow->setMaximumSize(QSize(800, 620));
        QFont font;
        font.setPointSize(8);
        MenuWindow->setFont(font);
        MenuWindow->setStyleSheet(QString::fromUtf8("background-color: rgba(34, 30, 30, 1);\n"
"	\n"
"\n"
""));
        centralwidget = new QWidget(MenuWindow);
        centralwidget->setObjectName(QString::fromUtf8("centralwidget"));
        centralwidget->setStyleSheet(QString::fromUtf8("\n"
"QScrollBar:vertical {\n"
"	border: none;\n"
"    background: rgb(45, 45, 68);\n"
"    width: 14px;\n"
"    margin: 15px 0 15px 0;\n"
"	border-radius: 0px;\n"
" }\n"
"\n"
"/*  HANDLE BAR VERTICAL */\n"
"QScrollBar::handle:vertical {	\n"
"	background-color: rgb(80, 80, 122);\n"
"	min-height: 30px;\n"
"	border-radius: 7px;\n"
"}\n"
"QScrollBar::handle:vertical:hover{	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::handle:vertical:pressed {	\n"
"	background-color:rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* BTN TOP - SCROLLBAR */\n"
"QScrollBar::sub-line:vertical {\n"
"	border: none;\n"
"	background-color: rgb(59, 59, 90);\n"
"	height: 15px;\n"
"	border-top-left-radius: 7px;\n"
"	border-top-right-radius: 7px;\n"
"	subcontrol-position: top;\n"
"	subcontrol-origin: margin;\n"
"}\n"
"QScrollBar::sub-line:vertical:hover {	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::sub-line:vertical:pressed {	\n"
"	background-color: rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* BTN BOTTOM "
                        "- SCROLLBAR */\n"
"QScrollBar::add-line:vertical {\n"
"	border: none;\n"
"	background-color: rgb(59, 59, 90);\n"
"	height: 15px;\n"
"	border-bottom-left-radius: 7px;\n"
"	border-bottom-right-radius: 7px;\n"
"	subcontrol-position: bottom;\n"
"	subcontrol-origin: margin;\n"
"}\n"
"QScrollBar::add-line:vertical:hover {	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::add-line:vertical:pressed {	\n"
"	background-color: rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* RESET ARROW */\n"
"QScrollBar::up-arrow:vertical, QScrollBar::down-arrow:vertical {\n"
"	background: none;\n"
"}\n"
"QScrollBar::add-page:vertical, QScrollBar::sub-page:vertical {\n"
"	background: none;\n"
"}\n"
"\n"
"\n"
"QScrollBar:horizontal {\n"
"	border: none;\n"
"    background: rgb(45, 45, 68);\n"
"    height: 14px;\n"
"    margin: 0 15px 0 15px;\n"
"	border-radius: 0px;\n"
" }\n"
"\n"
"/*  HANDLE BAR VERTICAL */\n"
"QScrollBar::handle:horizontal {	\n"
"	background-color: rgb(80, 80, 122);\n"
"	min-width: 30px;\n"
"	border-radius:"
                        " 7px;\n"
"}\n"
"QScrollBar::handle:horizontal:hover{	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::handle:horizontal:pressed {	\n"
"	background-color:rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* BTN TOP - SCROLLBAR */\n"
"QScrollBar::sub-line:horizontal {\n"
"	border: none;\n"
"	background-color: rgb(59, 59, 90);\n"
"	width: 15px;\n"
"	border-top-left-radius: 7px;\n"
"	border-bottom-left-radius: 7px;\n"
"	subcontrol-position: left;\n"
"	subcontrol-origin: margin;\n"
"}\n"
"QScrollBar::sub-line:horizontal:hover {	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::sub-line:horizontal:pressed {	\n"
"	background-color: rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* BTN BOTTOM - SCROLLBAR */\n"
"QScrollBar::add-line:horizontal {\n"
"	border: none;\n"
"	background-color: rgb(59, 59, 90);\n"
"	width: 15px;\n"
"	border-top-right-radius: 7px;\n"
"	border-bottom-right-radius: 7px;\n"
"	subcontrol-position: right;\n"
"	subcontrol-origin: margin;\n"
"}\n"
"QScrollBar::add-line:horizontal"
                        ":hover {	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::add-line:horizontal:pressed {	\n"
"	background-color: rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* RESET ARROW */\n"
"QScrollBar::left-arrow:horizontal, QScrollBar::right-arrow:horizontal {\n"
"	background: none;\n"
"}\n"
"QScrollBar::add-page:horizontal, QScrollBar::sub-page:horizontal {\n"
"	background: none;\n"
"}"));
        pushButton = new QPushButton(centralwidget);
        pushButton->setObjectName(QString::fromUtf8("pushButton"));
        pushButton->setGeometry(QRect(30, 110, 171, 40));
        QPalette palette;
        QBrush brush(QColor(255, 255, 255, 255));
        brush.setStyle(Qt::SolidPattern);
        palette.setBrush(QPalette::Active, QPalette::WindowText, brush);
        QBrush brush1(QColor(79, 115, 207, 255));
        brush1.setStyle(Qt::SolidPattern);
        palette.setBrush(QPalette::Active, QPalette::Button, brush1);
        palette.setBrush(QPalette::Active, QPalette::Text, brush);
        palette.setBrush(QPalette::Active, QPalette::ButtonText, brush);
        palette.setBrush(QPalette::Active, QPalette::Base, brush1);
        palette.setBrush(QPalette::Active, QPalette::Window, brush1);
        QBrush brush2(QColor(255, 255, 255, 128));
        brush2.setStyle(Qt::NoBrush);
#if QT_VERSION >= QT_VERSION_CHECK(5, 12, 0)
        palette.setBrush(QPalette::Active, QPalette::PlaceholderText, brush2);
#endif
        QBrush brush3(QColor(0, 0, 0, 255));
        brush3.setStyle(Qt::SolidPattern);
        palette.setBrush(QPalette::Inactive, QPalette::WindowText, brush3);
        QBrush brush4(QColor(34, 30, 30, 255));
        brush4.setStyle(Qt::SolidPattern);
        palette.setBrush(QPalette::Inactive, QPalette::Button, brush4);
        palette.setBrush(QPalette::Inactive, QPalette::Text, brush3);
        palette.setBrush(QPalette::Inactive, QPalette::ButtonText, brush3);
        palette.setBrush(QPalette::Inactive, QPalette::Base, brush4);
        palette.setBrush(QPalette::Inactive, QPalette::Window, brush4);
        QBrush brush5(QColor(0, 0, 0, 128));
        brush5.setStyle(Qt::NoBrush);
#if QT_VERSION >= QT_VERSION_CHECK(5, 12, 0)
        palette.setBrush(QPalette::Inactive, QPalette::PlaceholderText, brush5);
#endif
        QBrush brush6(QColor(120, 120, 120, 255));
        brush6.setStyle(Qt::SolidPattern);
        palette.setBrush(QPalette::Disabled, QPalette::WindowText, brush6);
        palette.setBrush(QPalette::Disabled, QPalette::Button, brush4);
        palette.setBrush(QPalette::Disabled, QPalette::Text, brush6);
        palette.setBrush(QPalette::Disabled, QPalette::ButtonText, brush6);
        palette.setBrush(QPalette::Disabled, QPalette::Base, brush4);
        palette.setBrush(QPalette::Disabled, QPalette::Window, brush4);
        QBrush brush7(QColor(0, 0, 0, 128));
        brush7.setStyle(Qt::NoBrush);
#if QT_VERSION >= QT_VERSION_CHECK(5, 12, 0)
        palette.setBrush(QPalette::Disabled, QPalette::PlaceholderText, brush7);
#endif
        pushButton->setPalette(palette);
        QFont font1;
        font1.setFamilies({QString::fromUtf8("Proxima Nova")});
        font1.setPointSize(15);
        font1.setBold(false);
        font1.setUnderline(false);
        font1.setStrikeOut(false);
        pushButton->setFont(font1);
        pushButton->setLayoutDirection(Qt::LeftToRight);
        pushButton->setStyleSheet(QString::fromUtf8(":active{\n"
"background-color: rgba(79, 115, 207, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }\n"
":hover{\n"
"background-color: rgba(79, 115, 180, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }\n"
":pressed{\n"
"background-color: rgba(79, 115, 207, 0.7);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }"));
        archiveButton = new QPushButton(centralwidget);
        archiveButton->setObjectName(QString::fromUtf8("archiveButton"));
        archiveButton->setGeometry(QRect(620, 110, 151, 40));
        QFont font2;
        font2.setFamilies({QString::fromUtf8("Proxima Nova")});
        font2.setPointSize(15);
        archiveButton->setFont(font2);
        archiveButton->setStyleSheet(QString::fromUtf8(":active{\n"
"background-color: rgba(79, 115, 207, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }\n"
":hover{\n"
"background-color: rgba(79, 115, 180, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }\n"
":pressed{\n"
"background-color: rgba(79, 115, 207, 0.7);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }"));
        frame = new QFrame(centralwidget);
        frame->setObjectName(QString::fromUtf8("frame"));
        frame->setGeometry(QRect(220, 110, 381, 40));
        frame->setStyleSheet(QString::fromUtf8("background-color:white;\n"
""));
        frame->setFrameShape(QFrame::StyledPanel);
        frame->setFrameShadow(QFrame::Raised);
        pushButton_3 = new QPushButton(frame);
        pushButton_3->setObjectName(QString::fromUtf8("pushButton_3"));
        pushButton_3->setGeometry(QRect(290, -9, 91, 61));
        QSizePolicy sizePolicy(QSizePolicy::Minimum, QSizePolicy::Fixed);
        sizePolicy.setHorizontalStretch(0);
        sizePolicy.setVerticalStretch(0);
        sizePolicy.setHeightForWidth(pushButton_3->sizePolicy().hasHeightForWidth());
        pushButton_3->setSizePolicy(sizePolicy);
        pushButton_3->setFont(font2);
        pushButton_3->setStyleSheet(QString::fromUtf8(":active{\n"
"border: none;\n"
"background-color: rgba(79, 115, 207, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
" }\n"
":hover{\n"
"background-color: rgba(79, 115, 180, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }\n"
":pressed{\n"
"background-color: rgba(79, 115, 207, 0.7);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }"));
        taginputfield = new QLineEdit(frame);
        taginputfield->setObjectName(QString::fromUtf8("taginputfield"));
        taginputfield->setGeometry(QRect(0, 0, 291, 40));
        taginputfield->setFont(font2);
        taginputfield->setStyleSheet(QString::fromUtf8("height:50px;\n"
"border:0px;\n"
"background-color:white"));
        horizontalLayoutWidget = new QWidget(centralwidget);
        horizontalLayoutWidget->setObjectName(QString::fromUtf8("horizontalLayoutWidget"));
        horizontalLayoutWidget->setGeometry(QRect(0, 0, 801, 104));
        horizontalLayout = new QHBoxLayout(horizontalLayoutWidget);
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        horizontalLayout->setContentsMargins(0, 0, 0, 0);
        label = new QLabel(horizontalLayoutWidget);
        label->setObjectName(QString::fromUtf8("label"));
        QFont font3;
        font3.setFamilies({QString::fromUtf8("Proxima Nova Lt")});
        font3.setPointSize(50);
        label->setFont(font3);
        label->setStyleSheet(QString::fromUtf8("color:rgba(255, 255, 255, 1);"));
        label->setAlignment(Qt::AlignCenter);

        horizontalLayout->addWidget(label);

        scrollArea = new QScrollArea(centralwidget);
        scrollArea->setObjectName(QString::fromUtf8("scrollArea"));
        scrollArea->setGeometry(QRect(20, 160, 761, 81));
        scrollArea->setStyleSheet(QString::fromUtf8("border:none;"));
        scrollArea->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
        scrollArea->setHorizontalScrollBarPolicy(Qt::ScrollBarAsNeeded);
        scrollArea->setWidgetResizable(true);
        scrollAreaWidgetContents = new QWidget();
        scrollAreaWidgetContents->setObjectName(QString::fromUtf8("scrollAreaWidgetContents"));
        scrollAreaWidgetContents->setGeometry(QRect(0, 0, 761, 81));
        horizontalLayout_2 = new QHBoxLayout(scrollAreaWidgetContents);
        horizontalLayout_2->setObjectName(QString::fromUtf8("horizontalLayout_2"));
        currentTagsLayout = new QHBoxLayout();
        currentTagsLayout->setObjectName(QString::fromUtf8("currentTagsLayout"));
        currentTagsLayout->setSizeConstraint(QLayout::SetFixedSize);
        horizontalSpacer = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);

        currentTagsLayout->addItem(horizontalSpacer);


        horizontalLayout_2->addLayout(currentTagsLayout);

        scrollArea->setWidget(scrollAreaWidgetContents);
        scrollArea_2 = new QScrollArea(centralwidget);
        scrollArea_2->setObjectName(QString::fromUtf8("scrollArea_2"));
        scrollArea_2->setGeometry(QRect(20, 250, 761, 321));
        scrollArea_2->setStyleSheet(QString::fromUtf8("QScrollArea{\n"
"border:none;\n"
"}\n"
""));
        scrollArea_2->setVerticalScrollBarPolicy(Qt::ScrollBarAsNeeded);
        scrollArea_2->setHorizontalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
        scrollArea_2->setWidgetResizable(true);
        scrollAreaWidgetContents_3 = new QWidget();
        scrollAreaWidgetContents_3->setObjectName(QString::fromUtf8("scrollAreaWidgetContents_3"));
        scrollAreaWidgetContents_3->setGeometry(QRect(0, 0, 761, 321));
        verticalLayout = new QVBoxLayout(scrollAreaWidgetContents_3);
        verticalLayout->setObjectName(QString::fromUtf8("verticalLayout"));
        gridLayout = new QGridLayout();
        gridLayout->setSpacing(10);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        gridLayout->setSizeConstraint(QLayout::SetDefaultConstraint);
        verticalSpacer = new QSpacerItem(20, 40, QSizePolicy::Minimum, QSizePolicy::Expanding);

        gridLayout->addItem(verticalSpacer, 0, 0, 1, 1);


        verticalLayout->addLayout(gridLayout);

        verticalSpacer_2 = new QSpacerItem(20, 40, QSizePolicy::Minimum, QSizePolicy::Expanding);

        verticalLayout->addItem(verticalSpacer_2);

        scrollArea_2->setWidget(scrollAreaWidgetContents_3);
        MenuWindow->setCentralWidget(centralwidget);
        menubar = new QMenuBar(MenuWindow);
        menubar->setObjectName(QString::fromUtf8("menubar"));
        menubar->setGeometry(QRect(0, 0, 800, 26));
        MenuWindow->setMenuBar(menubar);
        statusbar = new QStatusBar(MenuWindow);
        statusbar->setObjectName(QString::fromUtf8("statusbar"));
        MenuWindow->setStatusBar(statusbar);

        retranslateUi(MenuWindow);

        QMetaObject::connectSlotsByName(MenuWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MenuWindow)
    {
        MenuWindow->setWindowTitle(QCoreApplication::translate("MenuWindow", "Notes", nullptr));
        pushButton->setText(QCoreApplication::translate("MenuWindow", "New note", nullptr));
        archiveButton->setText(QCoreApplication::translate("MenuWindow", "Archive", nullptr));
        pushButton_3->setText(QCoreApplication::translate("MenuWindow", "Search", nullptr));
        taginputfield->setPlaceholderText(QCoreApplication::translate("MenuWindow", "Enter tag...", nullptr));
        label->setText(QCoreApplication::translate("MenuWindow", "Notes", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MenuWindow: public Ui_MenuWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MENUWINDOW_H
