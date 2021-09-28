#include "stdafx.h"
#include <iostream> 
#include "datetime.h"
#include <list>
#include <iterator>
#include <array>
#include <string>
#include <vector>

//#define DOCTEST_CONFIG_IMPLEMENT_WITH_MAIN
#define DOCTEST_CONFIG_IMPLEMENT
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
class linkedlistelem {
public:

	T data;
	linkedlistelem<T> *next;

public:
	linkedlistelem<T>() : next(nullptr) {}
	linkedlistelem<T>(T val) : data(val), next(nullptr) {}
	linkedlistelem<T>(T val, linkedlistelem<T> *nextpointer) : data(val), next(nextpointer) {}
};

template<typename T>
class linkedlist {
public:
	linkedlistelem<T> *list;

public:
	linkedlist<T>() : list() { list = nullptr; }
	void Add(T data)
	{
		if (list == nullptr)
		{
			list = new linkedlistelem<T>(data);
		}
		else
		{
			linkedlistelem<T> *listcopy = list;
			while (list->next != nullptr) {
				list = list->next;
			}
			list->next = new linkedlistelem<T>(data);
			list = listcopy;
		}
	}
	
	void Print()
	{
		linkedlistelem<T> *tmp = list;
		while (tmp != nullptr) {
			std::cout << tmp->data << " ";
			tmp = tmp->next;
		}
		std::cout << "\n";
	}

	linkedlistelem<T> *GetLast() {
		linkedlistelem<T> *listcopy =list;
		while (listcopy != nullptr && listcopy->next != nullptr)
			listcopy = listcopy->next;
		return listcopy;
	}


	struct Iterator
	{
		using iterator_category = std::forward_iterator_tag;
		using difference_type = std::ptrdiff_t;
		using value_type = T;
		using pointer = linkedlistelem<T> * ;
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
		linkedlistelem<T> *listcopy = list;
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
bool operator == (linkedlist<T> first, linkedlist<T> second) {
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
	
	auto listiter = list.begin();

	int leftsize = mid - left + 1;
	int rightsize = right - mid;

	T leftArrayArr;
	for (int i = 0; i < leftsize; i++) {
		leftArrayArr.Add(*(std::next(listiter, left+i)));
	}
	
	auto leftArray = leftArrayArr.begin();

	T rightArrayArr;
	for (int i = 0; i < rightsize; i++) {
		rightArrayArr.Add(*(std::next(listiter, mid + 1 + i)));
	}
	auto rightArray = rightArrayArr.begin();

	

	int indexOfMergedArray = left; 
	int indexOfSubArrayOne = 0;
	int indexOfSubArrayTwo = 0;

	while (indexOfSubArrayOne < leftsize && indexOfSubArrayTwo < rightsize) {
		if (*std::next(leftArray,indexOfSubArrayOne) <= *std::next(rightArray, indexOfSubArrayTwo)) {
			*std::next(listiter, indexOfMergedArray) = *std::next(leftArray, indexOfSubArrayOne);
			indexOfSubArrayOne++;
		}
		else {
			*std::next(listiter, indexOfMergedArray) = *std::next(rightArray,indexOfSubArrayTwo);
			indexOfSubArrayTwo++;
		}
		indexOfMergedArray++;
	}

	while (indexOfSubArrayOne < leftsize) {
		*std::next(listiter, indexOfMergedArray) = *std::next(leftArray, indexOfSubArrayOne);
		indexOfSubArrayOne++;
		indexOfMergedArray++;
	}

	while (indexOfSubArrayTwo < rightsize) {
		*std::next(listiter, indexOfMergedArray) = *std::next(rightArray, indexOfSubArrayTwo);
		indexOfSubArrayTwo++;
		indexOfMergedArray++;
	}
	return list;
}

template<typename T>
T MergeSort(T list, int start, int end) {
	if (start >= end) {
		return list;
	}

	int mid = start + (end - start) / 2;
	list = MergeSort<T>(list, start, mid);
	list = MergeSort<T>(list, mid + 1, end);
	return Merge<T>(list, start, mid, end);
}



template<typename T, typename T1>
T BubbleSort(T list, int n) { 

	auto listiter = list.begin();
	for (int i = 0; i < n - 1; i++) {
		for (int j = 0; j < n - i - 1; j++) {
			if (*(std::next(listiter, j)) > *(std::next(listiter , j + 1))) {
				T1 tmp = *(std::next(listiter, j));
				*(std::next(listiter, j)) = *(std::next(listiter, j + 1));
				*(std::next(listiter, j+1)) = tmp;
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
	auto listiter = list.begin();
	for (int i = 1; i < n; i++)
	{
		key = *std::next(listiter, i);
		j = i - 1;

		while (j >= 0 && *std::next(listiter,j) > key)
		{
			*std::next(listiter,j + 1) = *std::next(listiter,j);
			j = j - 1;
		}
		*std::next(listiter,j + 1) = key;
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

	librarylist<std::string> list4;
	list4.Add("aaaa"); list4.Add("aaaab"); list4.Add("aaa"); list4.Add("bubble"); list4.Add("sort"); list4.Add("sort");

	librarylist<std::string> listres4;
	listres4.Add("aaa"); listres4.Add("aaaa"); listres4.Add("aaaab"); listres4.Add("bubble"); listres4.Add("sort"); listres4.Add("sort");

	CHECK(listres4 == QuickSort<librarylist<std::string>, std::string>(list4, 0, list4.size() - 1));
	CHECK(listres4 == BubbleSort<librarylist<std::string>, std::string>(list4, list4.size()));
	CHECK(listres4 == MergeSort<librarylist<std::string>>(list4, 0, list4.size() - 1));
	CHECK(listres4 == InsertionSort<librarylist<std::string>, std::string>(list4, list4.size()));

	librarylist<std::vector<int>> list5;
	list5.Add({ 4,5,6 }); list5.Add({ 1,2,6 }); list5.Add({ 1,2,6,7 });
	librarylist<std::vector<int>> listres5;
	listres5.Add({ 1,2,6 }); listres5.Add({ 1,2,6,7 }); listres5.Add({ 4,5,6 });

	CHECK(listres5 == QuickSort<librarylist<std::vector<int>>, std::vector<int>>(list5, 0, list5.size() - 1));
	CHECK(listres5 == BubbleSort<librarylist<std::vector<int>>, std::vector<int>>(list5, list5.size()));
	CHECK(listres5 == MergeSort<librarylist<std::vector<int>>>(list5, 0, list5.size() - 1));
	CHECK(listres5 == InsertionSort<librarylist<std::vector<int>>, std::vector<int>>(list5, list5.size()));

	librarylist<std::vector<std::vector<datetime>>> list6;
	list6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	list6.Add({ { datetime(1972, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	list6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2005, 7, 11) } });


	librarylist<std::vector<std::vector<datetime>>> listres6;
	listres6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	listres6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2005, 7, 11) } });
	listres6.Add({ { datetime(1972, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });

	CHECK(listres6 == QuickSort<librarylist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, 0, list6.size() - 1));
	CHECK(listres6 == BubbleSort<librarylist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, list6.size()));
	CHECK(listres6 == MergeSort<librarylist<std::vector<std::vector<datetime>>>>(list6, 0, list6.size() - 1));
	CHECK(listres6 == InsertionSort<librarylist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, list6.size()));
	
}


TEST_CASE("testing linked list sorts") {

	linkedlist<int> list;
	list.Add(7); list.Add(4); list.Add(7); list.Add(-2);  list.Add(3); list.Add(0);
	linkedlist<int> listres;
	listres.Add(-2); listres.Add(0); listres.Add(3); listres.Add(4);  listres.Add(7); listres.Add(7);

	CHECK(listres == QuickSort<linkedlist<int>, int>(list, 0, list.size() - 1));
	CHECK(listres == BubbleSort<linkedlist<int>, int>(list, list.size()));
	CHECK(listres == MergeSort<linkedlist<int>>(list, 0, list.size() - 1));
	CHECK(listres == InsertionSort<linkedlist<int>, int>(list, list.size()));

	
	linkedlist<double> list2;
	list2.Add(7.2); list2.Add(4.5); list2.Add(7.2); list2.Add(-2.0);  list2.Add(3.8); list2.Add(0);
	linkedlist<double> listres2;
	listres2.Add(-2.0); listres2.Add(0); listres2.Add(3.8); listres2.Add(4.5);  listres2.Add(7.2); listres2.Add(7.2);

	CHECK(listres2 == QuickSort<linkedlist<double>, double>(list2, 0, list2.size() - 1));
	CHECK(listres2 == BubbleSort<linkedlist<double>, double>(list2, list2.size()));
	CHECK(listres2 == MergeSort<linkedlist<double>>(list2, 0, list2.size() - 1));
	CHECK(listres2 == InsertionSort<linkedlist<double>, double>(list2, list2.size()));

	
	linkedlist<datetime> list3;

	list3.Add(datetime(1990, 4, 5)); list3.Add(datetime(1983, 8, 9, 3, 15, 45)); list3.Add(datetime(2003, 13, 7));
	list3.Add(datetime(2020, 29, 2)); list3.Add(datetime(1971, 1, 1)); list3.Add(datetime(1971, 3, 5, 20, 15, 7));


	linkedlist<datetime> listres3;
	listres3.Add(datetime(1971, 1, 1)); listres3.Add(datetime(1971, 3, 5, 20, 15, 7)); listres3.Add(datetime(1983, 8, 9, 3, 15, 45));
	listres3.Add(datetime(1990, 4, 5)); listres3.Add(datetime(2003, 13, 7)); listres3.Add(datetime(2020, 29, 2));

	CHECK(listres3 == QuickSort<linkedlist<datetime>, datetime>(list3, 0, list3.size() - 1));
	CHECK(listres3 == BubbleSort<linkedlist<datetime>, datetime>(list3, list3.size()));
	CHECK(listres3 == MergeSort<linkedlist<datetime>>(list3, 0, list3.size() - 1));
	CHECK(listres3 == InsertionSort<linkedlist<datetime>, datetime>(list3, list3.size()));

	linkedlist<std::string> list4;
	list4.Add("aaaa"); list4.Add("aaaab"); list4.Add("aaa"); list4.Add("bubble"); list4.Add("sort"); list4.Add("sort");

	linkedlist<std::string> listres4;
	listres4.Add("aaa"); listres4.Add("aaaa"); listres4.Add("aaaab"); listres4.Add("bubble"); listres4.Add("sort"); listres4.Add("sort");

	CHECK(listres4 == QuickSort<linkedlist<std::string>, std::string>(list4, 0, list4.size() - 1));
	CHECK(listres4 == BubbleSort<linkedlist<std::string>, std::string>(list4, list4.size()));
	CHECK(listres4 == MergeSort<linkedlist<std::string>>(list4, 0, list4.size() - 1));
	CHECK(listres4 == InsertionSort<linkedlist<std::string>, std::string>(list4, list4.size()));

	linkedlist<std::vector<int>> list5;
	list5.Add({ 4,5,6 }); list5.Add({ 1,2,6 }); list5.Add({ 1,2,6,7 });
	linkedlist<std::vector<int>> listres5;
	listres5.Add({ 1,2,6 }); listres5.Add({ 1,2,6,7 }); listres5.Add({ 4,5,6 });

	CHECK(listres5 == QuickSort<linkedlist<std::vector<int>>, std::vector<int>>(list5, 0, list5.size() - 1));
	CHECK(listres5 == BubbleSort<linkedlist<std::vector<int>>, std::vector<int>>(list5, list5.size()));
	CHECK(listres5 == MergeSort<linkedlist<std::vector<int>>>(list5, 0, list5.size() - 1));
	CHECK(listres5 == InsertionSort<linkedlist<std::vector<int>>, std::vector<int>>(list5, list5.size()));

	linkedlist<std::vector<std::vector<datetime>>> list6;
	list6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	list6.Add({ { datetime(1972, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	list6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2005, 7, 11) } });


	linkedlist<std::vector<std::vector<datetime>>>  listres6;
	listres6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	listres6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2005, 7, 11) } });
	listres6.Add({ { datetime(1972, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });

	CHECK(listres6 == QuickSort<linkedlist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, 0, list6.size() - 1));
	CHECK(listres6 == BubbleSort<linkedlist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, list6.size()));
	CHECK(listres6 == MergeSort<linkedlist<std::vector<std::vector<datetime>>>>(list6, 0, list6.size() - 1));
	CHECK(listres6 == InsertionSort<linkedlist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, list6.size()));

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

	arraylist<std::string> list4;
	list4.Add("aaaa"); list4.Add("aaaab"); list4.Add("aaa"); list4.Add("bubble"); list4.Add("sort"); list4.Add("sort");

	arraylist<std::string> listres4;
	listres4.Add("aaa"); listres4.Add("aaaa"); listres4.Add("aaaab"); listres4.Add("bubble"); listres4.Add("sort"); listres4.Add("sort");

	CHECK(listres4 == QuickSort<arraylist<std::string>, std::string>(list4, 0, list4.size() - 1));
	CHECK(listres4 == BubbleSort<arraylist<std::string>, std::string>(list4, list4.size()));
	CHECK(listres4 == MergeSort<arraylist<std::string>>(list4, 0, list4.size() - 1));
	CHECK(listres4 == InsertionSort<arraylist<std::string>, std::string>(list4, list4.size()));

	arraylist<std::vector<int>> list5;
	list5.Add({ 4,5,6 }); list5.Add({ 1,2,6 }); list5.Add({ 1,2,6,7 });
	arraylist<std::vector<int>> listres5;
	listres5.Add({ 1,2,6 }); listres5.Add({ 1,2,6,7 }); listres5.Add({ 4,5,6 });

	CHECK(listres5 == QuickSort<arraylist<std::vector<int>>, std::vector<int>>(list5, 0, list5.size() - 1));
	CHECK(listres5 == BubbleSort<arraylist<std::vector<int>>, std::vector<int>>(list5, list5.size()));
	CHECK(listres5 == MergeSort<arraylist<std::vector<int>>>(list5, 0, list5.size() - 1));
	CHECK(listres5 == InsertionSort<arraylist<std::vector<int>>, std::vector<int>>(list5, list5.size()));

	arraylist<std::vector<std::vector<datetime>>> list6;
	list6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	list6.Add({ { datetime(1972, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	list6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2005, 7, 11) } });


	arraylist<std::vector<std::vector<datetime>>>  listres6;
	listres6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });
	listres6.Add({ { datetime(1971, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2005, 7, 11) } });
	listres6.Add({ { datetime(1972, 1, 1), datetime(1971, 1, 2), datetime(2001, 2, 11) },{ datetime(2001, 8, 11), datetime(2002, 8, 11) } });

	CHECK(listres6 == QuickSort<arraylist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, 0, list6.size() - 1));
	CHECK(listres6 == BubbleSort<arraylist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, list6.size()));
	CHECK(listres6 == MergeSort<arraylist<std::vector<std::vector<datetime>>>>(list6, 0, list6.size() - 1));
	CHECK(listres6 == InsertionSort<arraylist<std::vector<std::vector<datetime>>>, std::vector<std::vector<datetime>>>(list6, list6.size()));
}


int main(int argc, char** argv) {
	doctest::Context ctx;
	ctx.setOption("abort-after", 5);  // default - stop after 5 failed asserts
	ctx.applyCommandLine(argc, argv); // apply command line - argc / argv
	ctx.setOption("no-breaks", true); // override - don't break in the debugger
	int res = ctx.run();              // run test cases unless with --no-run
	if (ctx.shouldExit())              // query flags (and --exit) rely on this
		return res;

	UserInterfaceDateTime();

	return res;
}

//UserInterfaceDateTime();