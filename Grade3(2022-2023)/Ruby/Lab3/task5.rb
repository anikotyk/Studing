n = 10.0
c = 5.0
$pi = 3.131

puts("Task 1")
def y(x, n, c)
  ( ((x+c)**(-n/4)*c**(1/n)) / (x**(2-n)*c**(-3/4)) )**(4/3) + (x*x-1)/(x**3-3*x+1/n)
end

step = (n - 1) / (n + c)

(1.0..n).step(step) do |t|
  puts "x = #{t}"
  puts "y = #{y(t, n, c)}"
end


puts("Task 2")
def z(x)
  Math.sin(15/8*$pi - 2*x)**2 - Math.cos(17/8*$pi - 2*x)**2 + Math.cos(x)**(1/x)
end


step = ($pi - $pi/n) / (3/2*n+c)
x = $pi/n
while x <= $pi
  puts "x = #{x}"
  puts "z = #{z(x)}"
  x = x + step
end

puts("Task 3")
x = 2
step = (c - 2)/(2*n)
while x<=c
  puts "x = #{x}"
  if 2<x && x<n
    puts "f = #{y(x,n,c)}"
  elsif n<x && x < (2*n)
    puts "f=#{z(x)}"
  else
    puts "f=#{y(x,n,c)+z(x)}"
  end
  x = x + step
end
