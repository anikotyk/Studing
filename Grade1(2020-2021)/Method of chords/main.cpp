#include <iostream>
#include <SFML/Graphics.hpp>

using namespace sf;

//size of window
int window_width  = 500;
int window_height = 500;

//size of view (for zooming)
int zoomx = 50;
int zoomy = 50;

//interval x
double start_x = -zoomx / 2;
double stop_x  = +zoomx / 2;

//calculation accuracy
double e = 0.0001;


double f(double x) {
	return x*x;
}


const double dx = 1e-6;
// numerical differentiation
double df(double (*f)(double), double x) {
	return (f(x + dx / 2) - f(x - dx / 2)) / dx;
}

void makeGrid(VertexArray &grid);

const float pi = 2 * acos(0);

int main()
{
	RenderWindow window(VideoMode(window_width, window_height), "Newton approximation");

	//zooming
	View view;
	view.reset(FloatRect((window_width - zoomx) / 2, (window_height-zoomy)/2, zoomx, zoomy));

	//creating coordinate axes
	VertexArray grid(Lines);
	makeGrid(grid);

	//graph of user function
	VertexArray func(LinesStrip);
	for (double x = start_x; x < stop_x; x += 0.1) {
		func.append(Vertex(Vector2f(x + window_width / 2,  window_height / 2 - f(x)), Color::Red));
	}

	//creating graph of tangent
	RectangleShape tangent(Vector2f(window_width*2, 0.1));
	tangent.setOrigin(tangent.getSize().x / 2, tangent.getSize().y / 2);
	tangent.setFillColor(Color::Green);
		
	double xo = stop_x, x = 2;

	bool next_step = true;
	while (window.isOpen())
	{
		Event event;
		while (window.pollEvent(event))
		{
			if (event.type == Event::Closed)
				window.close();
			if (Keyboard::isKeyPressed(Keyboard::Right)) next_step = true;
		}

		if (!next_step) continue;

		if (fabs(x - xo) > e)
		{
			double tang = df(f, x);

			
			if (tang != 0) {
				xo = x;
				// Newton method
				x = x - f(x) / df(f, x);

				// draw tangent at point x
				tangent.setPosition(x + window_width / 2, window_height / 2);
				tangent.setRotation(-atan(tang) * 180.0f / pi);

				std::cout << "Approx step: f(" << x << ") = " << f(x) << "\n";
			}
			else {
				if (fabs(f(x)) < e) {
					std::cout << "Root " << x;
				}
				else {
					std::cout << "Can't find root";
				}
			}

			window.clear();

			window.setView(view);
			window.draw(grid);
			window.draw(func);
			window.draw(tangent);

			window.display();

		} else std::cout << "DONE. Root is " << x << "\r";

		next_step = false;
	}
	return 0;
}


void makeGrid(VertexArray &grid)
{
	//creating coordinate axes
	grid.append(Vertex(Vector2f(window_width / 2, 0)));
	grid.append(Vertex(Vector2f(window_width / 2, window_height)));
	grid.append(Vertex(Vector2f(0, window_height / 2)));
	grid.append(Vertex(Vector2f(window_width, window_height / 2)));

	//creating little lines on coordinate axes
	int j, step = 0;

	while (step < window_height) {
		j = window_width / 2 - 0.5;
		grid.append(Vertex(Vector2f(j, step)));
		j = window_width / 2 + 1;
		grid.append(Vertex(Vector2f(j, step)));

		j = window_height / 2 - 0.5;
		grid.append(Vertex(Vector2f(step, j)));
		j = window_height / 2 + 1;
		grid.append(Vertex(Vector2f(step, j)));

		step += 5;
	}
}
