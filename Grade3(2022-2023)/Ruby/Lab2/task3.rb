num = 10000100101

res = 0
deg = num.to_s.length - 1
num.to_s.split('').each{ |char|
  res += char.to_i * 2**deg
  deg -= 1
}

puts "Answer: #{res}"
