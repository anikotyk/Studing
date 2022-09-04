puts "Введіть значення невідомих змінних: "
print "x: "
x = gets.chomp.to_f
print "a: "
a = gets.chomp.to_f
print "b: "
b = gets.chomp.to_f

e = Math.exp(1)

part1 = 6.2 * 10**(2.7) * Math.tan(Math::PI - x**3)
part2 = e + Math.log(Math.acos((b**3).abs), e) ** (1/2)
part3 = Math.atan((10**2 * Math.sqrt(a))/(2*x + 87.2))

L = part1/part2 + part3
puts "Відповідь: #{L}"

print "Натисніть Enter для виходу "
gets
