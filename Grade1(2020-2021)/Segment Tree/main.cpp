#include "stdafx.h"
#include <iostream>

struct tree {
	int sum;
	tree *left=nullptr;
	tree *right=nullptr;
};

int getSum(tree *head, int start, int end, int a, int b) {
	if (a <=start && b >= end) {
		return head->sum;
	}
	else if(start>b || end<a){
		return 0;
	}
	else {
		if (end - start == 1) {
			return 0;
		}
		else {
			int center = start + (end - start) / 2;
			return getSum(head->left, start, center, a, b) + getSum(head->right, center, end, a, b);
		}
	}
}

tree *change(tree *head, int index, int val, int a, int b) {
	if (b - a == 1) {
		if (a == index) {
			tree *cur = new tree;
			cur->sum = val;
			return cur;
		}
		else {
			return head;
		}
	}else if(index<a || index>b){
		return head;
	}
	else {
		int c = a + (b - a) / 2;
		head->left = change(head->left, index, val, a, c);
		head->right = change(head->right, index, val, c, b);
		head->sum = head->left->sum + head->right->sum;
	}

	return head;
}

tree *calctree(int values[], int a, int b) {
	tree *cur = new tree;
	if (b - a == 1) {
		cur->sum = values[a];
	}
	else {
		int c = a + (b - a)/2;
		cur->left = calctree(values, a, c);
		cur->right = calctree(values, c, b);
		cur->sum = cur->left->sum + cur->right->sum;
	}
	
	return cur;
}

void Delete(tree *head) {
	if (!head->left) {
		delete head;
		return;
	}
	else {
		Delete(head->left);
		Delete(head->right);
		delete head;
	}
	
}

int main()
{
	FILE *fp;
	fopen_s(&fp, "../tree.txt", "r");
	fseek(fp, 0, SEEK_SET);

	int values[100];
	int count = 0;
	while (!feof(fp)) {
		fscanf_s(fp, "%d", &values[count]);
		count++;
	}
	fclose(fp);
	tree *head = calctree(values, 0, count);

	int command;
	int a;
	int b;
	char help[] = "Enter '1' to get the sum of a segment \nEnter '2' to change one element of the tree \nEnter '-1' to exit\n";
	std::cout << help;
	std::cout << ">>> ";
	std::cin >> command;
	while (command != -1) {
		if (command == 1) {
			std::cout << "Enter the start index of the segment: ";
			std::cin >> a;
			std::cout << "Enter the end index of the segment: ";
			std::cin >> b;

			std::cout << getSum(head, 0, count, a, b) << "\n";
		}
		else if (command == 2) {
			std::cout << "Enter the index of the element you want to change: ";
			std::cin >> a;
			std::cout << "Enter new value of the element: ";
			std::cin >> b;
			head = change(head, a, b, 0, count);
		}
		else {
			std::cout << help;
		}
		std::cout << ">>> ";
		std::cin >> command;
	}

	Delete(head);

    return 0;
}

