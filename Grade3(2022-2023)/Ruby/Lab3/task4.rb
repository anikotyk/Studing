e = 0.00001

def factorial(n)
  return n > 1 ? n * factorial(n - 1) : 1
end

def sumRow(formula, n, eps)
  sum = formula.call(n)
  sumLast = 0
  while (sum - sumLast).abs > eps
    n += 1
    sumLast = sum
    sum += formula.call(n)
   end

  return sum
end

first_lambda = lambda { |n|
    factorial(n - 1) / factorial(n + 1)
}

second_lambda = lambda { |n|
    1.0 / ((4 * n - 1)*(4 * n + 1))
}

third_lambda = lambda { |n|
  (factorial(3 * n - 1) * factorial(2 * n - 1)).to_f / (factorial(3 * n) * 3 ** (2 * n) * factorial(2 * n))
}

puts "First answer : #{sumRow(first_lambda, 2, e)}"
puts "Second answer : #{1.0/2 - Math::PI/8} or #{sumRow(second_lambda, 1, e)}"
puts "Third answer : #{sumRow(third_lambda, 1, e)}"
