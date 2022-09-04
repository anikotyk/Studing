A = false
B = true
C = true

X = 90
Y = -1
Z = 5

puts "1) a) "+(!(A||B) && (A && !B)).to_s
puts "b) "+((Z != Y) && (6 >= Y) && A || B && C && (X >= 1.5)).to_s
puts "c) "+(((8 - X*2) <= Z) && (X**2 <= Y**2) || (Z >= 15)).to_s
puts "d) "+((X > 0) && (Y < 0) || (Z >= (X*Y - Y/X - Z))).to_s
puts "e) "+(!(A || B && !(C || (!A || B)))).to_s
puts "f) "+((X**2 + Y**2) >= 1 || X >= 0 || Y>=0).to_s
puts "g) "+((A && (C && B != B || A) || C) && B).to_s

x = 3
P = true
e = Math.exp(1)

part1 = Math.log(x, e) / Math.log(1.0/3, e) > Math.log(0.7, e) / Math.log(1.0/3, e)
part2 = Math.sqrt(x) > x*x
answer = (part1 && part2 && !P)
puts "\n2) " + answer.to_s
