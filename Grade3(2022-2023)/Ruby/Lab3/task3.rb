res = 0
for i in 0..8
  res += 1.0/(3**i)
end
puts "2) #{res}"

res = 0
n = 5
for i in 0...n
  res += 2
  res = Math.sqrt(res)
end
puts "5) #{res}"
