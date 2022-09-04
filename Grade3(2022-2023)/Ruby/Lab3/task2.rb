x = 3
P = true

e = Math.exp(1)

part1 = Math.log(x, e) / Math.log(1/3, e) > Math.log(0.7, e) / Math.log(1/3, e)
part2 = Math.sqrt(x) > x*x
answer = (part1 && part2 && !P)
puts "Answer: " + answer.to_s
