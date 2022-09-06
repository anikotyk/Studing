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
end

n = 8
matrix = CreateMatrix(n)
