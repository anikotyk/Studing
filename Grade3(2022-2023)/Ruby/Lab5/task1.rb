def prm(a, b, n, func)
  h = (b - a) / n
  res = 0
  for i in 1..n
    xi = a + i*h - h/2
    res += func.call(xi)
  end
  res *= h
  return res
end

def trp(a, b, n, func)
  h = (b - a) / n
  res = 0
  res = func.call(a) / 2 + func.call(b) / 2
  for i in 1..(n-1)
    res += func.call(a + i*h)
  end
  res *= h
  return res
end

def f1(x)
  return Math.sqrt(Math.exp(x) - 1)
end

def f2(x)
  return Math.exp(x) * Math.sin(x)
end

F1 = -> (x) {Math.sqrt(Math.exp(x) - 1)}
F2 = -> (x) {Math.exp(x) * Math.sin(x)}

n1 = 10
a1 = 0.2
b1 = 2.1
n2 = 10
a2 = 0.0
b2 = Math::PI / 2

puts "Func1 prm = " + prm(a1, b1, n1, F1).to_s + " trp = " + trp(a1, b1, n1, F1).to_s
puts "Func2 prm = " + prm(a2, b2, n2, F2).to_s + " trp = " + trp(a2, b2, n2, F2).to_s
