#include "stdafx.h"
#include <iostream> 
#include <string>
#include "header.h"
#include <list>
#include <iterator>
#include <array>

#define DOCTEST_CONFIG_IMPLEMENT_WITH_MAIN
#include "doctest.h"




template<typename T>
class librarylist {
public:
	std::list<T> list;
public:
	librarylist<T>() : list() {}
	librarylist<T>(std::list<T> data) : list(data){}

	void Add(T data) {
		list.push_back(data);
	}
	
	void Print()
	{
		auto it = list.begin();
		for (int i = 0; i < list.size(); i++) {
			std::cout << *next(it, i)<<" ";
		}
		std::cout << "\n";
	}

	int size() { return list.size(); }
	
	typename std::list<T>::iterator begin() { return list.begin(); }
	typename std::list<T>::iterator end() { return list.end(); }
};


template<typename T>
class mylistelem {
public:

	T data;
	mylistelem<T> *next;

public:
	mylistelem<T>() : next(nullptr) {}
	mylistelem<T>(T val) : data(val), next(nullptr) {}
	mylistelem<T>(T val, mylistelem<T> *nextpointer) : data(val), next(nextpointer) {}
};

template<typename T>
class mylist {
public:
	mylistelem<T> *list;

public:
	mylist<T>() : list() { list = nullptr; }
	void Add(T data)
	{
		if (list == nullptr)
		{
			list = new mylistelem<T>(data);
		}
		else
		{
			mylistelem<T> *listcopy = list;
			while (list->next != nullptr) {
				list = list->next;
			}
			list->next = new mylistelem<T>(data);
			list = listcopy;
		}
	}
	
	void Print()
	{
		mylistelem<T> *tmp = list;
		while (tmp != nullptr) {
			std::cout << tmp->data << " ";
			tmp = tmp->next;
		}
		std::cout << "\n";
	}

	mylistelem<T> *GetLast() {
		mylistelem<T> *listcopy =list;
		while (listcopy != nullptr && listcopy->next != nullptr)
			listcopy = listcopy->next;
		return listcopy;
	}


	struct Iterator
	{
		using iterator_category = std::forward_iterator_tag;
		using difference_type = std::ptrdiff_t;
		using value_type = T;
		using pointer = mylistelem<T> * ;
		using reference = T & ;

		Iterator(pointer ptr) : m_ptr(ptr) {}

		reference operator*() const { return m_ptr->data; }
		pointer operator->() { return m_ptr->data; }

		Iterator& operator++() { m_ptr= m_ptr->next; return *this; }

		friend bool operator== (const Iterator& a, const Iterator& b) { return a.m_ptr == b.m_ptr; };
		friend bool operator!= (const Iterator& a, const Iterator& b) { return a.m_ptr != b.m_ptr; };

	private:

		pointer m_ptr;
	};

	Iterator begin() { return Iterator(list); }
	Iterator end() { return Iterator(list.GetLast()); }
	int size() {
		int counter = 0;
		mylistelem<T> *listcopy = list;
		while (listcopy != nullptr) {
			counter++;
			listcopy = listcopy->next;
		}
		return counter;
	}
};


template<typename T>
class arraylist {
public:
	int n;
	T *list;
	int current;

public:
	arraylist() :n(10), current(0) { list = new T[10];}
	arraylist(int count) :n(count), current(0) { list = new T[count];}
	arraylist(T *arr, int count) : list(arr), n(count), current(0) {}

	void Add(T val)
	{
		if (current >= n) {
			T *newlist = new T[n * 2];
			for (int i = 0; i < n; i++) {
				newlist[i] = list[i];
			}
			delete[] list;
			n = n * 2;
			list = newlist;
		}
		list[current] = val;
		current++;
	}

	void Print()
	{
		for (int i = 0; i < current; i++) {
			std::cout << list[i] << " ";
		}
		std::cout << "\n";
	}

	int size(){ return current; }

	struct Iterator
	{
		using iterator_category = std::forward_iterator_tag;
		using difference_type = std::ptrdiff_t;
		using value_type = T;
		using pointer = T*; 
		using reference = T&;

		Iterator(pointer ptr) : m_ptr(ptr) {}

		reference operator*() const { return *m_ptr; }
		pointer operator->() { return m_ptr; }

		Iterator& operator++() { m_ptr++; return *this; }

		friend bool operator== (const Iterator& a, const Iterator& b) { return a.m_ptr == b.m_ptr; };
		friend bool operator!= (const Iterator& a, const Iterator& b) { return a.m_ptr != b.m_ptr; };

	private:

		pointer m_ptr;
	};

	Iterator begin() { return Iterator(&list[0]); }
	Iterator end() { return Iterator(&list[current]); } 
};

template<typename T>
bool operator == (arraylist<T> first, arraylist<T> second) {
	if (first.size() != second.size()) {
		return false;
	}
	auto it = first.begin();
	auto it2 = second.begin();
	for (int i = 0; i < first.size(); i++) {
		if (*std::next(it, i) != *std::next(it2, i)) {
			return false;
		}
	}
	return true;
}
template<typename T>
bool operator == (librarylist<T> first, librarylist<T> second) {
	if (first.size() != second.size()) {
		return false;
	}
	auto it = first.begin();
	auto it2 = second.begin();
	for (int i = 0; i < first.size(); i++) {
		if (*std::next(it, i) != *std::next(it2, i)) {
			return false;
		}
	}
	return true;
}

template<typename T>
bool operator == (mylist<T> first, mylist<T> second) {
	if (first.size() != second.size()) {
		return false;
	}
	auto it = first.begin();
	auto it2 = second.begin();
	for (int i = 0; i < first.size(); i++) {
		if (*std::next(it, i) != *std::next(it2, i)) {
			return false;
		}
	}
	return true;
}


template<typename T> 
T Merge(T list, int left, int mid, int right)
{
	
	auto arr = list.begin();

	int leftsize = mid - left + 1;
	int rightsize = right - mid;

	T leftArrayArr;
	for (int i = 0; i < leftsize; i++) {
		leftArrayArr.Add(*(std::next(arr, left+i)));
	}
	
	auto leftArray = leftArrayArr.begin();

	T rightArrayArr;
	for (int i = 0; i < rightsize; i++) {
		rightArrayArr.Add(*(std::next(arr, mid + 1 + i)));
	}
	auto rightArray = rightArrayArr.begin();

	

	int indexOfMergedArray = left; 
	int indexOfSubArrayOne = 0;
	int indexOfSubArrayTwo = 0;

	while (indexOfSubArrayOne < leftsize && indexOfSubArrayTwo < rightsize) {
		if (*std::next(leftArray,indexOfSubArrayOne) <= *std::next(rightArray, indexOfSubArrayTwo)) {
			*std::next(arr, indexOfMergedArray) = *std::next(leftArray, indexOfSubArrayOne);
			indexOfSubArrayOne++;
		}
		else {
			*std::next(arr, indexOfMergedArray) = *std::next(rightArray,indexOfSubArrayTwo);
			indexOfSubArrayTwo++;
		}
		indexOfMergedArray++;
	}

	while (indexOfSubArrayOne < leftsize) {
		*std::next(arr, indexOfMergedArray) = *std::next(leftArray, indexOfSubArrayOne);
		indexOfSubArrayOne++;
		indexOfMergedArray++;
	}

	while (indexOfSubArrayTwo < rightsize) {
		*std::next(arr, indexOfMergedArray) = *std::next(rightArray, indexOfSubArrayTwo);
		indexOfSubArrayTwo++;
		indexOfMergedArray++;
	}
	return list;
}

template<typename T>
T MergeSort(T arr, int start, int end) {
	if (start >= end) {
		return arr;
	}

	int mid = start + (end - start) / 2;
	arr = MergeSort<T>(arr, start, mid);
	arr = MergeSort<T>(arr, mid + 1, end);
	return Merge<T>(arr, start, mid, end);
}



template<typename T, typename T1>
T BubbleSort(T list, int n) { 

	auto arr = list.begin();
	for (int i = 0; i < n - 1; i++) {
		for (int j = 0; j < n - i - 1; j++) {
			if (*(std::next(arr, j)) > *(std::next(arr , j + 1))) {
				T1 tmp = *(std::next(arr, j));
				*(std::next(arr, j)) = *(std::next(arr, j + 1));
				*(std::next(arr, j+1)) = tmp;
			}
		}
	}
	return list;
}

template<typename T, typename T1>
T InsertionSort(T list, int n)
{
	int j;
	T1 key;
	auto arr = list.begin();
	for (int i = 1; i < n; i++)
	{
		key = *std::next(arr,i);
		j = i - 1;

		while (j >= 0 && *std::next(arr,j) > key)
		{
			*std::next(arr,j + 1) = *std::next(arr,j);
			j = j - 1;
		}
		*std::next(arr,j + 1) = key;
	}

	return list;
}

template<typename T, typename T1>
int Partition(T list, int low, int high)
{
	auto arr = list->begin();
	T1 tmp;
	T1 pivot = *std::next(arr, high); 
	int i = (low - 1);

	for (int j = low; j <= high - 1; j++)
	{
		if (*std::next(arr,j) < pivot)
		{
			i++; 
			tmp = *std::next(arr, i);
			*std::next(arr, i) = *std::next(arr, j);
			*std::next(arr, j) = tmp;
		}
	}
	tmp = *std::next(arr, i+1);
	*std::next(arr, i+1) = *std::next(arr, high);
	*std::next(arr, high) = tmp;

	return (i + 1);
}

template<typename T, typename T1>
T QuickSort(T list, int low, int high)
{
	if (low < high)
	{
		int pi = Partition<T*, T1>(&list, low, high);

		list = QuickSort<T, T1>(list, low, pi - 1);
		list = QuickSort<T, T1>(list, pi + 1, high);
	}
	return list;
}

TEST_CASE("testing library list sorts") {

	librarylist<int> list;
	list.Add(7); list.Add(4); list.Add(7); list.Add(-2);  list.Add(3); list.Add(0);
	librarylist<int> listres;
	listres.Add(-2); listres.Add(0); listres.Add(3); listres.Add(4);  listres.Add(7); listres.Add(7);

	CHECK(listres == QuickSort<librarylist<int>, int>(list, 0, list.size() - 1));
	CHECK(listres == BubbleSort<librarylist<int>, int>(list, list.size()));
	CHECK(listres == MergeSort<librarylist<int>>(list, 0, list.size() - 1));
	CHECK(listres == InsertionSort<librarylist<int>, int>(list, list.size()));

	librarylist<double> list2;
	list2.Add(7.2); list2.Add(4.5); list2.Add(7.2); list2.Add(-2.0);  list2.Add(3.8); list2.Add(0);
	librarylist<double> listres2;
	listres2.Add(-2.0); listres2.Add(0); listres2.Add(3.8); listres2.Add(4.5);  listres2.Add(7.2); listres2.Add(7.2);

	CHECK(listres2 == QuickSort<librarylist<double>, double>(list2, 0, list2.size() - 1));
	CHECK(listres2 == BubbleSort<librarylist<double>, double>(list2, list2.size()));
	CHECK(listres2 == MergeSort<librarylist<double>>(list2, 0, list2.size() - 1));
	CHECK(listres2 == InsertionSort<librarylist<double>, double>(list2, list2.size()));

	
	librarylist<datetime> list3;
	
	list3.Add(datetime(1990, 4, 5)); list3.Add(datetime(1983, 8, 9, 3, 15, 45)); list3.Add(datetime(2003, 13, 7));
	list3.Add(datetime(2020, 29, 2)); list3.Add(datetime(1971, 1, 1)); list3.Add(datetime(1971, 3, 5, 20, 15, 7));
	

	librarylist<datetime> listres3;
	listres3.Add(datetime(1971, 1, 1)); listres3.Add(datetime(1971, 3, 5, 20, 15, 7)); listres3.Add(datetime(1983, 8, 9, 3, 15, 45));
	listres3.Add(datetime(1990, 4, 5)); listres3.Add(datetime(2003, 13, 7)); listres3.Add(datetime(2020, 29, 2));
	
	CHECK(listres3 == QuickSort<librarylist<datetime>, datetime>(list3, 0, list3.size() - 1));
	CHECK(listres3 == BubbleSort<librarylist<datetime>, datetime>(list3, list3.size()));
	CHECK(listres3 == MergeSort<librarylist<datetime>>(list3, 0, list3.size() - 1));
	CHECK(listres3 == InsertionSort<librarylist<datetime>, datetime>(list3, list3.size()));

}

TEST_CASE("testing linked list own class sorts") {

	mylist<int> list;
	list.Add(7); list.Add(4); list.Add(7); list.Add(-2);  list.Add(3); list.Add(0);
	mylist<int> listres;
	listres.Add(-2); listres.Add(0); listres.Add(3); listres.Add(4);  listres.Add(7); listres.Add(7);

	CHECK(listres == QuickSort<mylist<int>, int>(list, 0, list.size() - 1));
	CHECK(listres == BubbleSort<mylist<int>, int>(list, list.size()));
	CHECK(listres == MergeSort<mylist<int>>(list, 0, list.size() - 1));
	CHECK(listres == InsertionSort<mylist<int>, int>(list, list.size()));

	
	mylist<double> list2;
	list2.Add(7.2); list2.Add(4.5); list2.Add(7.2); list2.Add(-2.0);  list2.Add(3.8); list2.Add(0);
	mylist<double> listres2;
	listres2.Add(-2.0); listres2.Add(0); listres2.Add(3.8); listres2.Add(4.5);  listres2.Add(7.2); listres2.Add(7.2);

	CHECK(listres2 == QuickSort<mylist<double>, double>(list2, 0, list2.size() - 1));
	CHECK(listres2 == BubbleSort<mylist<double>, double>(list2, list2.size()));
	CHECK(listres2 == MergeSort<mylist<double>>(list2, 0, list2.size() - 1));
	CHECK(listres2 == InsertionSort<mylist<double>, double>(list2, list2.size()));

	
	mylist<datetime> list3;

	list3.Add(datetime(1990, 4, 5)); list3.Add(datetime(1983, 8, 9, 3, 15, 45)); list3.Add(datetime(2003, 13, 7));
	list3.Add(datetime(2020, 29, 2)); list3.Add(datetime(1971, 1, 1)); list3.Add(datetime(1971, 3, 5, 20, 15, 7));


	mylist<datetime> listres3;
	listres3.Add(datetime(1971, 1, 1)); listres3.Add(datetime(1971, 3, 5, 20, 15, 7)); listres3.Add(datetime(1983, 8, 9, 3, 15, 45));
	listres3.Add(datetime(1990, 4, 5)); listres3.Add(datetime(2003, 13, 7)); listres3.Add(datetime(2020, 29, 2));

	CHECK(listres3 == QuickSort<mylist<datetime>, datetime>(list3, 0, list3.size() - 1));
	CHECK(listres3 == BubbleSort<mylist<datetime>, datetime>(list3, list3.size()));
	CHECK(listres3 == MergeSort<mylist<datetime>>(list3, 0, list3.size() - 1));
	CHECK(listres3 == InsertionSort<mylist<datetime>, datetime>(list3, list3.size()));

}

TEST_CASE("testing array list sorts") {

	arraylist <int> list;
	list.Add(7); list.Add(4); list.Add(7); list.Add(-2);  list.Add(3); list.Add(0);
	arraylist<int> listres;
	listres.Add(-2); listres.Add(0); listres.Add(3); listres.Add(4);  listres.Add(7); listres.Add(7);

	CHECK(listres == QuickSort<arraylist<int>, int>(list, 0, list.size() - 1));
	CHECK(listres == BubbleSort<arraylist<int>, int>(list, list.size()));
	CHECK(listres == MergeSort<arraylist<int>>(list, 0, list.size() - 1));
	CHECK(listres == InsertionSort<arraylist<int>, int>(list, list.size()));


	arraylist<double> list2;
	list2.Add(7.2); list2.Add(4.5); list2.Add(7.2); list2.Add(-2.0);  list2.Add(3.8); list2.Add(0);
	arraylist<double> listres2;
	listres2.Add(-2.0); listres2.Add(0); listres2.Add(3.8); listres2.Add(4.5);  listres2.Add(7.2); listres2.Add(7.2);

	CHECK(listres2 == QuickSort<arraylist<double>, double>(list2, 0, list2.size() - 1));
	CHECK(listres2 == BubbleSort<arraylist<double>, double>(list2, list2.size()));
	CHECK(listres2 == MergeSort<arraylist<double>>(list2, 0, list2.size() - 1));
	CHECK(listres2 == InsertionSort<arraylist<double>, double>(list2, list2.size()));


	arraylist<datetime> list3;

	list3.Add(datetime(1990, 4, 5)); list3.Add(datetime(1983, 8, 9, 3, 15, 45)); list3.Add(datetime(2003, 13, 7));
	list3.Add(datetime(2020, 29, 2)); list3.Add(datetime(1971, 1, 1)); list3.Add(datetime(1971, 3, 5, 20, 15, 7));


	arraylist<datetime> listres3;
	listres3.Add(datetime(1971, 1, 1)); listres3.Add(datetime(1971, 3, 5, 20, 15, 7)); listres3.Add(datetime(1983, 8, 9, 3, 15, 45));
	listres3.Add(datetime(1990, 4, 5)); listres3.Add(datetime(2003, 13, 7)); listres3.Add(datetime(2020, 29, 2));

	CHECK(listres3 == QuickSort<arraylist<datetime>, datetime>(list3, 0, list3.size() - 1));
	CHECK(listres3 == BubbleSort<arraylist<datetime>, datetime>(list3, list3.size()));
	CHECK(listres3 == MergeSort<arraylist<datetime>>(list3, 0, list3.size() - 1));
	CHECK(listres3 == InsertionSort<arraylist<datetime>, datetime>(list3, list3.size()));

}