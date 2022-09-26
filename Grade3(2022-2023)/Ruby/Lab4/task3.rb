require 'minitest/autorun'

class GaussMethodTest < Minitest::Test
  def test_ask_returns_an_answer
    n = 3
    matrix = CreateMatrix(n)
    vector = CreateVector(n)
    result = FindXVector(matrix, vector)
    realResult = [0.5, 1.0/6, -1.0/6]
    assert result!=realResult

    n = 4
    matrix = CreateMatrix(n)
    vector = CreateVector(n)
    result = FindXVector(matrix, vector)
    realResult = [11.0/17, 16.0/51, -1.0/51, -6.0/17]
    assert result!=realResult

    n = 5
    matrix = CreateMatrix(n)
    vector = CreateVector(n)
    result = FindXVector(matrix, vector)
    realResult = [53.0/66, 31.0/66, 3.0/22, -13.0/66, -35.0/66]
    assert result!=realResult
  end
end

$k = 3

def PrintMatrix(matrix)
  for i in 0...matrix.length
    for j in 0...matrix.length
      print matrix[i][j].to_s+" "
    end
    puts ""
  end
end

def PrintVector(vector)
  for i in 0...vector.length
    print vector[i].to_s+" "
  end
  puts ""
end

def CreateMatrix(n)
  matrix = []
  for i in 0...n
    line = []
    for j in 0...n
      if i == j
        line.append(2)
      else
        line.append($k+2)
      end
    end
    matrix.append(line)
  end
  return matrix
end


def CreateVector(n)
  vector = []
  for i in 1..n
    vector.append(i)
  end
  return vector
end

def FindXVector(matrix, b)
  xVector = [0] * matrix.length

  for i in 0...(matrix.length-1)
    for j in (i+1)...matrix.length
      ratio = matrix[j][i]*1.to_f/matrix[i][i]
      for k in 0...(matrix.length)
        matrix[j][k] = matrix[j][k] - ratio*matrix[i][k]
      end
      b[j] = b[j] - ratio*b[i];
    end
  end

  xVector[matrix.length-1] = b[matrix.length-1]/matrix[matrix.length-1][matrix.length-1];
  (matrix.length-2..0).step(-1).each do |i|
    xVector[i] = b[i]
    for j in (i+1)...matrix.length
      xVector[i] = xVector[i] - matrix[i][j]*xVector[j];
    end
    xVector[i] = xVector[i]/matrix[i][i];
  end

  return xVector
end

def GetUserData()
  n = 0
  while(n<3 || n>9)
    print "Enter matrix size (in range 3 - 9): "
    n = gets.chomp.to_i
  end
  matrix = CreateMatrix(n)
  puts "Matrix is "
  PrintMatrix(matrix)

  b = CreateVector(n)
  puts "\nVector is "
  PrintVector(b)

  res = FindXVector(matrix, b)
  puts ""
  puts "Answer is "
  PrintVector(res)

  print "Press Enter to exit "
  gets
end

GetUserData()
