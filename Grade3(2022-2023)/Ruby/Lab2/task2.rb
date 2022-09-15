P = 4
t = 32
r = 16

res = (P**r) * (1 - P**(-t)).to_f
puts "Answer: #{res}"
