require 'minitest/autorun'

class MatrixTransposeTest < Minitest::Test
  def test_ask_returns_an_answer
    matrix = [[1,2,3], [4, 5, 6], [7, 8, 9]]
    result =  MatrixTranspose(matrix)
    realResult = [[1,4,7], [2, 5, 8], [3, 6, 9]]
    assert result==realResult

    matrix = [[1,2], [4, 5]]
    result =  MatrixTranspose(matrix)
    realResult = [[1,4], [2, 5]]
    assert result==realResult
  end
end


def CreateMatrix(n)
  matrix = []
  for i in 0...n
    line = []
    for j in 0...n
      if i == j
        line.append(1)
      else
        line.append(rand(10))
      end
    end
    matrix.append(line)
  end
  return matrix
end

def MatrixTranspose(matrix)
  for i in 0...matrix.length
    for j in 0...i
      tmp = matrix[j][i]
      matrix[j][i] = matrix[i][j]
      matrix[i][j] = tmp
    end
  end
  return matrix
end


def PrintMatrix(matrix)
  for i in 0...matrix.length
    for j in 0...matrix.length
      print matrix[i][j].to_s+" "
    end
    puts ""
  end
end

def GetUserData()
  n = 8

  matrix = CreateMatrix(n)
  puts "Matrix is "
  PrintMatrix(matrix)

  matrix = MatrixTranspose(matrix)
  puts ""
  PrintMatrix(matrix)

  print "Press Enter to exit "
  gets
end

#GetUserData()
