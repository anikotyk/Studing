#include <SFML/Graphics.hpp>

int  cirRad = 50;
int  dotRad = 5;

int  trjRad = 20;

int  xpos = 0;
int  ypos = 100;
int  dir = +1;
bool trace = true;

const float pi = 2 * acos(0);

using namespace sf;

int main()
{
	Event event;

	RenderWindow window(sf::VideoMode((2 * pi*cirRad) * 2, 2 * ypos), "cycloid");
	window.setFramerateLimit(120);

	VertexArray trajectory(Lines);

	// define a wheel params
	CircleShape cir;
	cir.setRadius(cirRad);
	cir.setOrigin(cirRad, cirRad);
	cir.setFillColor(Color::Green);

	// define a dot params
	CircleShape dot;
	dot.setRadius(dotRad);
	dot.setFillColor(Color::Red);
	dot.setOrigin(dotRad, dotRad + trjRad);

	// main loop
	while (window.isOpen())
	{
		while (window.pollEvent(event))
		{
			if (event.type == Event::Closed) window.close();
			if (event.type == Event::KeyPressed)
				if (event.key.code == Keyboard::Escape) window.close();
		}

		window.clear(Color(0, 0, 0));

		// draw a wheel
		cir.setPosition(xpos, ypos);

		// draw a dot
		dot.setPosition(xpos, ypos);
		dot.setRotation((180.0f / pi * xpos) / cirRad);

		// draw a dot trajectory
		if (!(xpos % 6) && trace) trajectory.append(Vertex(dot.getTransform().transformPoint(dotRad, dotRad)));

		// the next position
		xpos += dir;

		// check boundaries
		if ((xpos <= 0) || (xpos >= window.getSize().x)) {
			if (trace) {
				trajectory.append(Vertex(dot.getTransform().transformPoint(dotRad, dotRad)));
				trace = false;
			}
			dir *= -1;
		}

		window.draw(cir);
		window.draw(trajectory);
		window.draw(dot);

		window.display();
	}

	return 0;
}
