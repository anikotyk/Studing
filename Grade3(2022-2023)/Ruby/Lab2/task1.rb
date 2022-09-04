def calcSquare(*points)
  square = 0.0
  for i in 0...points.length
    i2 = (i+1)%(points.length)
    square+=(points[i][0] + points[i2][0])*(points[i][1] - points[i2][1])
  end
  square = square.abs * 0.5

  return square
end

puts "Answer1: " + calcSquare([69, 43], [110, 46], [147, 50], [186, 45], [238, 37]).to_s
puts "Answer2: " + calcSquare([298, 34], [344, 34], [396, 57], [429, 79], [482, 149]).to_s
puts "Answer3: " + calcSquare([485, 215], [488, 273], [443, 314], [350, 307], [271, 299]).to_s
puts "Answer4: " + calcSquare([207, 275], [177, 206], [136, 168], [97, 141], [47, 95]).to_s
puts "Answer5: " + calcSquare([49, 71], [56, 57]).to_s
