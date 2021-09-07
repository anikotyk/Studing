#include "stdafx.h"
#include<iostream>

struct elem {
	int coef=1;
	int power=0;
	elem *next = nullptr;
};

elem *addElem(elem *first_elem, elem *value)
{
	elem *pre_cur = nullptr;
	elem *cur = first_elem;
	if (!first_elem) first_elem = value;

	while (cur)
	{
		if (cur->power > value->power) {
			if (cur->next) {           
				pre_cur = cur;
				cur = cur->next;
			}
			else {                    
				cur->next = value;
				break;
			}
		}
		else if (cur->power < value->power) {   
			value->next = cur;
			if (!pre_cur) first_elem = value;
			else pre_cur->next = value;
			break;
		}
		else {
			cur->coef += value->coef;                        
			break;
		}
	}

	return first_elem;
}

int getPolinomValue(elem *head, int x, int A) {
	if (!head) return 0;
	int num = 1;
	for (int i = 1; i <= head->power; i++) {
		num *= x;
		num %= A;
	}
	
	int res=(head->coef*num)%A + getPolinomValue(head->next, x, A);
	std::cout << res<<" " ;
	return res;
}

void print(elem *first_elem, bool isPolynom=true)
{
	while (first_elem) {
		if (isPolynom) std::cout << first_elem->coef << "x^";
		std::cout << first_elem->power;
		first_elem = first_elem->next;
		if (first_elem) std::cout << (isPolynom ? " + " : ", ");
	}
	std::cout << std::endl;
}

void Delete(elem *first_elem) {
	if (first_elem->next) {
		Delete(first_elem->next);
	}
	delete first_elem;
}

int main()
{
	FILE *fp;
	fopen_s(&fp, "../polinom.txt", "r");
	fseek(fp, 0, SEEK_SET);

	elem *first_elem= nullptr;
	int coef;
	int deg;
	while (!feof(fp)) {
		elem *value = new elem;
		fscanf_s(fp, "%i", &value->coef);
		if (!value->coef) continue;
		fscanf_s(fp, "%i", &value->power);
		fscanf_s(fp, "\n");
		first_elem = addElem(first_elem, value);		
	}
	print(first_elem);
	fclose(fp);

	int A;
	std::cout << "Enter a num: \n";
	std::cin >> A;

	elem* osts=nullptr;

	for (int i = 0; i < A; i++) {
		elem* ost = new elem;
		ost->power = getPolinomValue(first_elem, i, A) % A;
		if (!ost->power) continue;
		osts = addElem(osts, ost);
	}
	print(osts, false);
	Delete(first_elem);
	Delete(osts);
	std::cout << "\n";
    return 0;
}
