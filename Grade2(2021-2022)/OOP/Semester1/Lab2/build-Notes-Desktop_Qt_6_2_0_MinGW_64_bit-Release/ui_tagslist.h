/********************************************************************************
** Form generated from reading UI file 'tagslist.ui'
**
** Created by: Qt User Interface Compiler version 6.2.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_TAGSLIST_H
#define UI_TAGSLIST_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QHBoxLayout>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QListWidget>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QVBoxLayout>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_TagsList
{
public:
    QWidget *layoutWidget;
    QVBoxLayout *verticalLayout_2;
    QListWidget *tagListWidget;
    QLineEdit *taginput;
    QHBoxLayout *horizontalLayout;
    QPushButton *addbutton;
    QPushButton *deletebutton;

    void setupUi(QWidget *TagsList)
    {
        if (TagsList->objectName().isEmpty())
            TagsList->setObjectName(QString::fromUtf8("TagsList"));
        TagsList->resize(400, 400);
        TagsList->setMinimumSize(QSize(400, 400));
        TagsList->setMaximumSize(QSize(400, 400));
        TagsList->setStyleSheet(QString::fromUtf8("QWidget{\n"
"\n"
"background-color: rgba(34, 30, 30, 1);\n"
"}\n"
"\n"
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
"	border-radius: 0px;\n"
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
"	border-top-left-radius: 0px;\n"
"	border-top-right-radius: 0px;\n"
"	subcontrol-position: top;\n"
"	subcontrol-origin: margin;\n"
"}\n"
"QScrollBar::sub-line:vertical:hover {	\n"
"	background-color: rgba(79, 115, 207, 1);\n"
"}\n"
"QScrollBar::sub-line:vertical:pressed {	\n"
"	backg"
                        "round-color: rgba(79, 115, 207, 0.7);\n"
"}\n"
"\n"
"/* BTN BOTTOM - SCROLLBAR */\n"
"QScrollBar::add-line:vertical {\n"
"	border: none;\n"
"	background-color: rgb(59, 59, 90);\n"
"	height: 15px;\n"
"	border-bottom-left-radius: 0px;\n"
"	border-bottom-right-radius: 0px;\n"
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
""));
        layoutWidget = new QWidget(TagsList);
        layoutWidget->setObjectName(QString::fromUtf8("layoutWidget"));
        layoutWidget->setGeometry(QRect(40, 30, 325, 339));
        verticalLayout_2 = new QVBoxLayout(layoutWidget);
        verticalLayout_2->setObjectName(QString::fromUtf8("verticalLayout_2"));
        verticalLayout_2->setSizeConstraint(QLayout::SetMinimumSize);
        verticalLayout_2->setContentsMargins(0, 0, 0, 0);
        tagListWidget = new QListWidget(layoutWidget);
        tagListWidget->setObjectName(QString::fromUtf8("tagListWidget"));
        QFont font;
        font.setFamilies({QString::fromUtf8("Proxima Nova")});
        font.setPointSize(15);
        tagListWidget->setFont(font);
        tagListWidget->setStyleSheet(QString::fromUtf8("background-color: rgba(104, 93, 93, 1);\n"
"border:none;\n"
"color:white;\n"
""));
        tagListWidget->setSpacing(3);

        verticalLayout_2->addWidget(tagListWidget);

        taginput = new QLineEdit(layoutWidget);
        taginput->setObjectName(QString::fromUtf8("taginput"));
        QSizePolicy sizePolicy(QSizePolicy::Expanding, QSizePolicy::Fixed);
        sizePolicy.setHorizontalStretch(0);
        sizePolicy.setVerticalStretch(0);
        sizePolicy.setHeightForWidth(taginput->sizePolicy().hasHeightForWidth());
        taginput->setSizePolicy(sizePolicy);
        QFont font1;
        font1.setFamilies({QString::fromUtf8("Proxima Nova")});
        font1.setPointSize(12);
        taginput->setFont(font1);
        taginput->setStyleSheet(QString::fromUtf8("height:35px;\n"
"border:0px;\n"
"background-color:white"));
        taginput->setMaxLength(30);

        verticalLayout_2->addWidget(taginput);

        horizontalLayout = new QHBoxLayout();
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        horizontalLayout->setSizeConstraint(QLayout::SetMinimumSize);
        horizontalLayout->setContentsMargins(10, -1, 10, -1);
        addbutton = new QPushButton(layoutWidget);
        addbutton->setObjectName(QString::fromUtf8("addbutton"));
        QSizePolicy sizePolicy1(QSizePolicy::Minimum, QSizePolicy::Fixed);
        sizePolicy1.setHorizontalStretch(0);
        sizePolicy1.setVerticalStretch(0);
        sizePolicy1.setHeightForWidth(addbutton->sizePolicy().hasHeightForWidth());
        addbutton->setSizePolicy(sizePolicy1);
        QFont font2;
        font2.setPointSize(12);
        addbutton->setFont(font2);
        addbutton->setStyleSheet(QString::fromUtf8(":active{\n"
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

        horizontalLayout->addWidget(addbutton);

        deletebutton = new QPushButton(layoutWidget);
        deletebutton->setObjectName(QString::fromUtf8("deletebutton"));
        deletebutton->setFont(font2);
        deletebutton->setStyleSheet(QString::fromUtf8(":active{\n"
"background-color: rgba(79, 115, 207, 1);\n"
"color: rgba(255, 255, 255, 1);\n"
"border: none;\n"
" }\n"
"\n"
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

        horizontalLayout->addWidget(deletebutton);


        verticalLayout_2->addLayout(horizontalLayout);


        retranslateUi(TagsList);

        QMetaObject::connectSlotsByName(TagsList);
    } // setupUi

    void retranslateUi(QWidget *TagsList)
    {
        TagsList->setWindowTitle(QCoreApplication::translate("TagsList", "Form", nullptr));
        taginput->setText(QString());
        taginput->setPlaceholderText(QCoreApplication::translate("TagsList", "Enter tag here...", nullptr));
        addbutton->setText(QCoreApplication::translate("TagsList", "Add", nullptr));
        deletebutton->setText(QCoreApplication::translate("TagsList", "Delete", nullptr));
    } // retranslateUi

};

namespace Ui {
    class TagsList: public Ui_TagsList {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_TAGSLIST_H
