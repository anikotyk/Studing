num = 451
res = ""

while num > 0
  res += (num % 2).to_s
  num /= 2
end

res.reverse!
puts res
