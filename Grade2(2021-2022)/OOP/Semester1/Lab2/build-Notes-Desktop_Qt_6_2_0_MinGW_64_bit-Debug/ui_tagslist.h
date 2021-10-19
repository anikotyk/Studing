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
    QWidget *widget;
    QVBoxLayout *verticalLayout_2;
    QListWidget *tagListWidget;
    QLineEdit *taginput;
    QHBoxLayout *horizontalLayout;
    QPushButton *addbutton;
    QPushButton *deletebutton;
    QPushButton *pushButton;

    void setupUi(QWidget *TagsList)
    {
        if (TagsList->objectName().isEmpty())
            TagsList->setObjectName(QString::fromUtf8("TagsList"));
        TagsList->resize(400, 400);
        widget = new QWidget(TagsList);
        widget->setObjectName(QString::fromUtf8("widget"));
        widget->setGeometry(QRect(30, 30, 325, 339));
        verticalLayout_2 = new QVBoxLayout(widget);
        verticalLayout_2->setObjectName(QString::fromUtf8("verticalLayout_2"));
        verticalLayout_2->setSizeConstraint(QLayout::SetMinimumSize);
        verticalLayout_2->setContentsMargins(0, 0, 0, 0);
        tagListWidget = new QListWidget(widget);
        tagListWidget->setObjectName(QString::fromUtf8("tagListWidget"));

        verticalLayout_2->addWidget(tagListWidget);

        taginput = new QLineEdit(widget);
        taginput->setObjectName(QString::fromUtf8("taginput"));
        QSizePolicy sizePolicy(QSizePolicy::Expanding, QSizePolicy::Fixed);
        sizePolicy.setHorizontalStretch(0);
        sizePolicy.setVerticalStretch(0);
        sizePolicy.setHeightForWidth(taginput->sizePolicy().hasHeightForWidth());
        taginput->setSizePolicy(sizePolicy);

        verticalLayout_2->addWidget(taginput);

        horizontalLayout = new QHBoxLayout();
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        horizontalLayout->setSizeConstraint(QLayout::SetMinimumSize);
        horizontalLayout->setContentsMargins(10, -1, 10, -1);
        addbutton = new QPushButton(widget);
        addbutton->setObjectName(QString::fromUtf8("addbutton"));
        QSizePolicy sizePolicy1(QSizePolicy::Minimum, QSizePolicy::Fixed);
        sizePolicy1.setHorizontalStretch(0);
        sizePolicy1.setVerticalStretch(0);
        sizePolicy1.setHeightForWidth(addbutton->sizePolicy().hasHeightForWidth());
        addbutton->setSizePolicy(sizePolicy1);

        horizontalLayout->addWidget(addbutton);

        deletebutton = new QPushButton(widget);
        deletebutton->setObjectName(QString::fromUtf8("deletebutton"));

        horizontalLayout->addWidget(deletebutton);

        pushButton = new QPushButton(widget);
        pushButton->setObjectName(QString::fromUtf8("pushButton"));

        horizontalLayout->addWidget(pushButton);


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
        pushButton->setText(QCoreApplication::translate("TagsList", "Confirm", nullptr));
    } // retranslateUi

};

namespace Ui {
    class TagsList: public Ui_TagsList {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_TAGSLIST_H
