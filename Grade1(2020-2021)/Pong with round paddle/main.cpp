#include <SFML/Graphics.hpp>
#include <iostream>
#include <stdlib.h>

using namespace sf;

#define PI 3.14159265 

int window_width = 1000;
int window_height = 800;

float margin = window_height / 60;
float pong_rad = window_height / 100;
float paddle_rad = window_height / 10;
float player_paddle_x = window_width / 12 + paddle_rad;
float bot_paddle_x = window_width - player_paddle_x;

Vector3f changeDirection(Vector3f ball, float x, Vector3f wall) {
	float cosin, sinus, newx, newy, newz;

	
	cosin = (wall.x*ball.x + wall.y*ball.y) / (sqrt(wall.x*wall.x + wall.y*wall.y)*sqrt(ball.x*ball.x + ball.y*ball.y));
	cosin = acos(cosin);
	cosin = abs(cos((PI / 2 - cosin) * 2));
	sinus = sqrt(1 - cosin * cosin);

	//std::cout << "Can be " << ball.x * cosin + ball.y * sinus << " " << -ball.x * sinus + ball.y * cosin <<" "<< (-ball.x * sinus + ball.y * cosin) * (ball.x*x + ball.z) / ball.y - (ball.x * cosin + ball.y * sinus) * x << "\n";
	//std::cout << "Can be too " << ball.x * cosin - ball.y * sinus << " " << ball.x * sinus + ball.y * cosin << " " << (ball.x * sinus + ball.y * cosin) * (ball.x*x + ball.z) / ball.y - (ball.x * cosin - ball.y * sinus) * x << "\n";
	
	newx = ball.x * cosin - ball.y * sinus;
	newy = ball.x * sinus + ball.y * cosin;
	
	if(abs(ball.x)-abs(newx)>0.1 || abs(ball.y)-abs(newy)>0.1){
		newx = ball.x * cosin + ball.y * sinus;
		newy = -ball.x * sinus + ball.y * cosin;
	}
	newz = newy * (ball.x*x + ball.z) / ball.y - newx * x;

	//std::cout << newx << " " << newy << " " << newz << "\n";

	ball = Vector3f(newx, newy, newz);
	return ball;
}

Vector3f bounce(Vector3f ball, CircleShape pong, CircleShape paddle) {
	float a, b, c, x1, y1, x2, y2;
	a = 2 * (paddle.getPosition().x - pong.getPosition().x);
	b = 2 * (pong.getPosition().y - paddle.getPosition().y);
	c = pong.getPosition().x*pong.getPosition().x + (window_height - pong.getPosition().y)*(window_height - pong.getPosition().y) + paddle_rad * paddle_rad - pong_rad * pong_rad - paddle.getPosition().x*paddle.getPosition().x - (window_height - paddle.getPosition().y)*(window_height - paddle.getPosition().y);
	a = a / (-b);
	b = c / (-b);

	/*b = a * pong.getPosition().x - pong.getPosition().y;
	ball = changeDirection(ball, pong.getPosition().x, Vector3f(a, 1, b));
	return ball;*/

	double k, l, m;
	k = pow(a, 2.) + 1;
	l = 2 * a * (b - (window_height - paddle.getPosition().y)) - 2 * paddle.getPosition().x;
	m = pow(paddle.getPosition().x, 2.) - pow(paddle_rad, 2.) + pow(b - (window_height - paddle.getPosition().y), 2.);
	double D = pow(l, 2.) - 4. * k * m;

	if (D >= 0)
	{
		x1 = (-l - sqrt(D)) / (2. * k);
		y1 = a * x1 + b;
		x2 = (-l + sqrt(D)) / (2. * k);
		y2 = a * x2 + b;
		x1 = (x1 + x2) / 2;
		y1 = (y1 + y2) / 2;
	}
	else {
		return ball;
	}
	k = (window_height - pong.getPosition().y - y1) / (pong.getPosition().x - x1);
	b = y1 - k * x1;
	ball = Vector3f(k, 1, b);


	//ball = changeDirection(ball, x1, Vector3f(a, 1, b));
	return ball;
}

int main()
{
	int game;
	std::cout << "Choose game type: \n1 - Player with player (use up arrow, down arrow and w, s to control paddle)\n2 - Player with computer\n3 - Computer with computer\n";
	std::cin >> game;
	if (game < 1 || game>3) {
		game = 2;
	}


	CircleShape player_paddle_cir;
	player_paddle_cir.setRadius(paddle_rad);
	player_paddle_cir.setOrigin(paddle_rad, paddle_rad);
	player_paddle_cir.setFillColor(Color(0, 204, 255));
	player_paddle_cir.setPosition(player_paddle_x, window_height / 2 );

	CircleShape bot_paddle_cir;
	bot_paddle_cir.setRadius(paddle_rad);
	bot_paddle_cir.setOrigin(paddle_rad, paddle_rad);
	bot_paddle_cir.setFillColor(Color(255, 51, 153));
	bot_paddle_cir.setPosition(bot_paddle_x, window_height / 2);


	CircleShape pong;
	pong.setRadius(pong_rad);
	pong.setFillColor(Color::Color(51,255,0));
	pong.setOrigin(pong_rad, pong_rad);
	pong.setPosition(window_width / 2, window_height / 2);
	
	float pong_speed = 1.5;

	Vector3f ball = Vector3f(rand() % 4 + 1, rand() % 4 + 1, 0);

	ball.z = ball.y* (window_height / 2) - ball.x* (window_width / 2);

	srand(time(NULL));
	
	float y;
	float x = window_width /2;

	float step = 1;
	int target = -1;
	int target2 = -1;

	float cosin = (ball.y) / sqrt(ball.x*ball.x + ball.y*ball.y);

	bool isStart=false;
	
	float dif1, dif2;

	RenderWindow window(VideoMode(window_width, window_height), "Pong");
	while (window.isOpen())
	{
		window.clear(Color::Color(153,153,204));
		
		window.draw(player_paddle_cir);
		window.draw(bot_paddle_cir);
		window.draw(pong);
		

		window.display();
		Event event;
		
		while (window.pollEvent(event))
		{
			if (event.type == Event::Closed)
				window.close();
			else if (event.type == Event::KeyPressed) {
				if (game == 3) {
					isStart = true;
					continue;
				}
				if(Keyboard::isKeyPressed(Keyboard::Up)){
					if (player_paddle_cir.getPosition().y >= margin + paddle_rad) {
						isStart = true;
						player_paddle_cir.move(0, -10);
					}
				}
				else if (Keyboard::isKeyPressed(Keyboard::Down)) {
					if (player_paddle_cir.getPosition().y <= window_height - margin - paddle_rad) {
						isStart = true;
						player_paddle_cir.move(0, 10);
					}
				}

				if (game==1 && Keyboard::isKeyPressed(Keyboard::W)) {
					if (bot_paddle_cir.getPosition().y >= margin + paddle_rad) {
						isStart = true;
						bot_paddle_cir.move(0, -10);
					}
				}
				else if (game == 1 && Keyboard::isKeyPressed(Keyboard::S)) {
					if (bot_paddle_cir.getPosition().y <= window_height - margin - paddle_rad) {
						isStart = true;
						bot_paddle_cir.move(0, 10);
					}
				}
			}
		}
		if (!isStart) {continue;}
		
		x += step*abs(cosin)*pong_speed;
		y = (ball.x*x + ball.z) / ball.y;
		pong.setPosition(x , window_height - y);

		if (abs(x - window_width) <= 1 || abs(x) <= 1) {
			player_paddle_cir.setPosition(player_paddle_x, window_height / 2);
			bot_paddle_cir.setPosition(bot_paddle_x, window_height / 2);
			pong.setPosition(window_width / 2, window_height / 2);

			ball = Vector3f(rand() % 4 + 1, rand() % 4 + 1, 0);
			ball.z = ball.y* (window_height / 2) - ball.x* (window_width / 2);
			step = abs(x - window_width) <= 1 ? -1 : 1;
			x = window_width / 2 ;
			target = -1;
			target2 = -1;
			
			cosin = (ball.y) / sqrt(ball.x*ball.x + ball.y*ball.y);
			isStart = false;
		}

		else if (abs(y - window_height) <= pong_rad) {
			target = -1;
			target2 = -1;
			ball = changeDirection(ball, x, Vector3f(0, 1, window_height));
			cosin = (ball.y) / sqrt(ball.x*ball.x + ball.y*ball.y);
			
			y -= pong_rad - abs(y - window_height);
			x = (ball.y*y - ball.z) / ball.x;
		}
		else if (y <= pong_rad) {
			target = -1;
			target2 = -1;
			ball = changeDirection(ball, x, Vector3f(0, 1, 0));
			cosin = (ball.y) / sqrt(ball.x*ball.x + ball.y*ball.y);
		
			y += pong_rad - y + 0.1;
			x = (ball.y*y - ball.z) / ball.x;	
		}
		else if (sqrt(pow(pong.getPosition().x - player_paddle_cir.getPosition().x, 2) + pow(pong.getPosition().y - player_paddle_cir.getPosition().y, 2)) <=  pong_rad + paddle_rad) {
				ball = bounce(ball, pong, player_paddle_cir);
				cosin = (ball.y) / sqrt(ball.x*ball.x + ball.y*ball.y);
				dif1 = sqrt(pow((x + 1 - player_paddle_cir.getPosition().x), 2) + pow((ball.x*(x + 1) + ball.z) / ball.y - window_height + player_paddle_cir.getPosition().y, 2));
				dif2 = sqrt(pow((x - 1 - player_paddle_cir.getPosition().x), 2) + pow((ball.x*(x - 1) + ball.z) / ball.y - window_height + player_paddle_cir.getPosition().y, 2));
				
				if(dif1<dif2){
					step = -1;
				}
				else {
					step = 1;
				}
				target = -1;
				target2 = -1;
				pong.setFillColor(Color::Color(0, 51, 255));
		}
		else if (sqrt(pow(pong.getPosition().x - bot_paddle_cir.getPosition().x, 2) + pow(pong.getPosition().y - bot_paddle_cir.getPosition().y, 2)) <= pong_rad + paddle_rad) {
				ball = bounce(ball, pong, bot_paddle_cir);
				cosin = (ball.y) / sqrt(ball.x*ball.x + ball.y*ball.y);
				dif1 = sqrt(pow((x + 1 - bot_paddle_cir.getPosition().x), 2) + pow((ball.x*(x + 1) + ball.z) / ball.y - window_height + bot_paddle_cir.getPosition().y, 2));
				dif2 = sqrt(pow((x - 1 - bot_paddle_cir.getPosition().x), 2) + pow((ball.x*(x - 1) + ball.z) / ball.y - window_height + bot_paddle_cir.getPosition().y, 2));
				if(dif1<dif2){
				
					step = -1;
				}
				else {
					step = 1;
				}
				target = -1;
				target2 = -1;
				pong.setFillColor(Color::Color(255, 0, 51));
		}


		if (game != 1) {
			if (target == -1) {
				//float zal = rand() % (int)(paddle_rad / 2);
				float zal = -paddle_rad ;
				//float zal = 0;
				if ((ball.x*(x + 1) + ball.z) / ball.y < (ball.x*(x - 1) + ball.z) / ball.y) {
					zal *= -1;
				}

				target = (ball.x*bot_paddle_cir.getPosition().x + ball.z) / ball.y+zal;
				target2 = (ball.x*player_paddle_cir.getPosition().x + ball.z) / ball.y - zal;
			}
			else {
				if (window_height - bot_paddle_cir.getPosition().y - margin > paddle_rad && target < window_height - bot_paddle_cir.getPosition().y) {
					bot_paddle_cir.move(0, 1);
				}
				else if (window_height - bot_paddle_cir.getPosition().y + margin<window_height-paddle_rad && target >  window_height - bot_paddle_cir.getPosition().y) {
					bot_paddle_cir.move(0, -1);
				}
				
				if (game == 3) {
					if (window_height - player_paddle_cir.getPosition().y - margin >paddle_rad && target2 < window_height - player_paddle_cir.getPosition().y) {
						player_paddle_cir.move(0, 1);
					}
					else if (window_height - player_paddle_cir.getPosition().y + margin<window_height - paddle_rad && target2 >  window_height - player_paddle_cir.getPosition().y) {
						player_paddle_cir.move(0, -1);
					}
				}
			}
		}
		sleep(milliseconds(1));
	}

	return 0;
}
