#include <iostream>
#include <SFML/Graphics.hpp>

using namespace sf;
//poly funcs

struct elem {
	int coef = 1;
	int power = 0;
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

long long getPolinomValue(elem *head, long long x) {
	if (!head) return 0;
	return head->coef*pow(x, head->power) + getPolinomValue(head->next, x);
}

void print(elem *first_elem, bool isPolynom = true)
{
	bool flag = true;
	while (first_elem) {
		if (!flag) {
			std::cout << abs(first_elem->coef) << "x^";
		}
		else {
			std::cout <<first_elem->coef << "x^";
			flag = false;
		}
		
		std::cout << first_elem->power;
		first_elem = first_elem->next;
		if (first_elem) std::cout << (first_elem->coef>0 ? " + " : " - ");
	}
	std::cout << std::endl;
}

void Delete(elem *first_elem) {
	if (first_elem->next) {
		Delete(first_elem->next);
	}
	delete first_elem;
}
//graph funcs

int *getQueue(int **data, int n) {
	int *edgesCnt = new int[n];
	int *res = new int[n];
	for (int i = 0; i < n; i++) {
		res[i] = i;
		edgesCnt[i] = 0;
	}

	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			edgesCnt[i] += data[i][j];
		}
	}

	//sort
	int temp;

	for (int i = 0; i < n - 1; i++) {
		for (int j = 0; j < n - i - 1; j++) {
			if (edgesCnt[j] < edgesCnt[j + 1]) {
				temp = edgesCnt[j];
				edgesCnt[j] = edgesCnt[j + 1];
				edgesCnt[j + 1] = temp;

				temp = res[j];
				res[j] = res[j + 1];
				res[j + 1] = temp;
			}
		}
	}

	return res;
}

bool isNeibColor(int **data, int *colors, int color, int index, int n) {
	for (int i = 0; i < n; i++) {
		if (data[index][i]) {
			if (colors[i] == color) {
				return true;
			}
		}
	}
	return false;
}

bool isEmpty(int **data, int n) {
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			if (data[i][j] == 1) {
				return false;
			}
		}
	}
	return true;
}

//array funcs

bool isContain(int *data, int n, int val) {
	for (int i = 0; i < n; i++) {
		if (data[i] == val) {
			return true;
		}
	}
	return false;
}

int **CopySquareArr(int **data, int n) {
	int **res = new int*[n];
	for (int i = 0; i < n; i++) {
		res[i] = new int[n];
	}

	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			res[i][j] = data[i][j];
		}
	}

	return res;
}

int max(int *data, int n) {
	int max = 0;
	for (int i = 0; i < n; i++) {
		if (data[i] > max) {
			max = data[i];
		}
	}
	return max;
}

elem *SubPoly(elem *main_poly, elem *poly) {
	while (poly) {
		elem *value = poly;
		value->coef *= -1;
		main_poly=addElem(main_poly, value);
		poly = poly->next;
	}
	return main_poly;
}

int CountNodes(int **data, int n) {
	int cnt = 0;
	for (int i = 0; i < n; i++) {
		if (data[i][0] != -1) {
			cnt++;
		}
	}
	return cnt;
}

int CountEdges(int **data, int n) {
	int cnt = 0;
	for (int i = 0; i < n; i++) {
		if (data[i][0] == -1) {
			continue;
		}
		for (int j = i+1; j < n; j++) {
			if (data[i][j] == 1) {
				cnt++;
			}
		}
	}
	return cnt;
}

int **DeleteEdge(int **data1, int n, int index){
	int **data = CopySquareArr(data1, n);
	for (int i = 0; i < n; i++) {
		if (data[index][i] == 1) {
			data[index][i] = 0;
			data[i][index] = 0;
			break;
		}
	}

	return data;
}
int **MergeNode(int **data1, int n, int index) {
	int **data = CopySquareArr(data1, n);
	int m = -1;
	for (int i = 0; i < n; i++) {
		if (data[index][i] == 1) {
			if (m == -1) {
				m = i;
			}
			else {
				data[m][i] = 1;
				data[i][m] = 1;
			}
			data[i][index] = 0;
		}
		data[index][i] = -1;
	}

	return data;
}

int getLast(int **data, int n, int *queue) {
	for (int i = n - 1; i >= 0; i--) {
		int cnt = 0;
		for (int j = 0; j < n; j++) {
			if (data[queue[i]][i] == 1) {
				return i;
			}
		}
	}
	return 0;
}


elem *getPoly(int **data, int n, int *queue) {
	if (CountEdges(data, n) == 0) {
		elem *val = new elem;
		val->power = CountNodes(data, n);
		return val;
	}
	queue = getQueue(data, n);

	return SubPoly(getPoly(DeleteEdge(data, n,queue[getLast(data, n, queue)]), n, queue), getPoly(MergeNode(data, n, queue[getLast(data, n, queue)]), n, queue));
}



int main()
{
	FILE *fp;
	fopen_s(&fp, "../data.txt", "r");

	int n;
	fscanf_s(fp, "%d", &n);
	int **data = new int*[n];
	for (int i = 0; i < n; i++) {
		data[i] = new int[n];
	}

	for (int i = 0; i<n; i++) {
		for (int j = 0; j < n; j++) {
			fscanf_s(fp, "%d", &data[i][j]);
		}
	}
	fclose(fp);

	int *queue = getQueue(data, n);

	int *colors = new int[n];
	for (int i = 0; i < n; i++) {
		colors[i] = 0;
	}

	int k = 0;
	while (isContain(colors, n, 0)) {
		k++;
		for (int i = 0; i < n; i++) {
			if (colors[queue[i]] == 0) {
				if (!isNeibColor(data, colors, k, queue[i], n)) {
					colors[queue[i]] = k;
				}
			}
		}
	}

	std::cout << "Chromatic number of the graph is: " << k << "\n";

	

	// chromatic poly
	elem *poly = nullptr;

	int **copy_data = CopySquareArr(data, n);
	poly = getPoly(copy_data, n, getQueue(data, n));

	std::cout << "\nChromatic polynomial: ";
	print(poly);

	std::cout << "\nNumber of the graph colorings: " << getPolinomValue(poly, k) << "\n";
	Delete(poly);
	delete[] queue;


	//draw
	int window_size = 500;
	int cirRad = window_size/20;
	int xpos= 50+2*window_size/n;
	int ypos= 50+window_size/n;
	
	sf::RenderWindow window(sf::VideoMode(window_size+100, window_size+100), "Graph");

	CircleShape *graph_nodes=new CircleShape[n];

	srand(time(NULL));
	int count_colors = max(colors, n);
	Color *sfml_colors = new Color[count_colors];
	for (int i = 0; i < count_colors; i++) {
		sfml_colors[i] = Color(rand() % 255, rand() % 255, rand() % 255);
	}

	for (int i = 0; i < n; i++) {
		graph_nodes[i].setRadius(cirRad);
		//graph_nodes[i].setOrigin(cirRad, cirRad);
		graph_nodes[i].setPosition(xpos, ypos);
		graph_nodes[i].setFillColor(sfml_colors[colors[i]]);

		if (i % 2 == 1) {
			ypos +=2* window_size / n;
			if (i < n / 2-1) {
				xpos += 2*window_size / n;
			}
			else if (i>n/2-1) {
				xpos -= 2*window_size / n;
			}
			
		}
		
		xpos = (window_size+50  - xpos);
		
		
	}
	delete[] colors;
	delete[] sfml_colors;

	int p = 0;
	VertexArray lines(Lines, n*n);

	for (int i = 0; i < n; i++)
	{
		for (int j = i+1; j < n; j++) {
			if (data[i][j] == 1) {
				lines[p].position= graph_nodes[i].getTransform().transformPoint(cirRad,cirRad);
				p++;
				lines[p].position = graph_nodes[j].getTransform().transformPoint(cirRad, cirRad);
				p++;
			}
		}
	}
	for (int i = 0; i < n; i++) {
		delete[] data[i];
		delete[] copy_data[i];
	}
	delete[] data;
	delete[] copy_data;

	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
		}

		window.draw(lines);
		for (int i = 0; i < n; i++) {
			window.draw(graph_nodes[i]);
		}
		
		window.display();
	}

	delete[] graph_nodes;
	return 0;
}
