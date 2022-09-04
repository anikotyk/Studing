num = 10000100101

res = 0
deg = 0
num.to_s.split('').reverse.each{ |char|
  res += char.to_i * 2**deg
  deg += 1
}

puts "Answer: #{res}"
