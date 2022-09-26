$e = 0.001

def sumRowEps(formula, n, x)
  sum = formula.call(n, x)
  sumLast = 0
  while (sum - sumLast).abs > $e
    n += 1
    sumLast = sum
    sum += formula.call(n, x)
   end

  return sum
end

def sumRowN(formula, n, maxN, x)
  sum = formula.call(n, x)
  sumLast = 0
  while n<maxN
    n += 1
    sumLast = sum
    sum += formula.call(n, x)
   end

  return sum
end


$func = lambda { |i, x|
    (-1)**i * (x**(2*i+1)*1.0/(2*i+1))
}

x = 0
while(x<0.1 || x>1)
  print "Enter x in range [0.1, 1]: "
  x = gets.chomp.to_f
end

maxN = 0
while(maxN<10 || maxN>58)
  print "Enter count values of row in range [10, 58]: "
  maxN = gets.chomp.to_i
end

puts "Answer for calculation with epsilon: "+sumRowEps($func , 0, x).to_s
puts "Answer for calculation with count: "+sumRowN($func , 0, maxN, x).to_s

print "Press enter to exit "
gets
