A = false
B = true
C = true

X = 90
Y = -1
Z = 5

puts "a) "+(!(A||B) && (A && !B)).to_s
puts "b) "+((Z != Y) && (6 >= Y) && A || B && C && (X >= 1.5)).to_s
puts "c) "+(((8 - X*2) <= Z) && (X**2 <= Y**2) || (Z >= 15)).to_s
puts "d) "+((X > 0) && (Y < 0) || (Z >= (X*Y - Y/X - Z))).to_s
puts "e) "+(!(A || B && !(C || (!A || B)))).to_s
puts "f) "+((X**2 + Y**2) >= 1 || X >= 0 || Y>=0).to_s
puts "g) "+((A && (C && B != B || A) || C) && B).to_s
