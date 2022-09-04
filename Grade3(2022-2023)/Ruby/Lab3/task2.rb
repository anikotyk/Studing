x = 1.0
y = 0

if x > -4 && x <= 0
  y = ((x - 2).abs / (x**2 * Math.cos(x)))**(1.0/7)
elsif x > 0 && x <= 12
  y = 1.0 / ((Math.tan(x + 1 / Math.exp(x)) / Math.sin(x)**2) ** 2.0 / 7)
elsif x < -4 || x > 12
  y = 1 / (1 + x / (1 + x / (1 + x)))
end

case x
when -4..0
  y = ((x - 2).abs / (x**2 * Math.cos(x)))**(1.0/7)
when 0..12
  y = 1.0 / ((Math.tan(x + 1.0 / Math.exp(x)) / Math.sin(x)**2) ** 2.0 / 7)
else
  y = 1.0 / (1 + x / (1 + x / (1 + x)))
end

puts "Answer: #{y}"
