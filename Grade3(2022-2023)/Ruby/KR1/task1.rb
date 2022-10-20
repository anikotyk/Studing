require 'minitest/autorun'

class FunctionFTest < Minitest::Test
  def test_ask_returns_an_answer
    result = F(1, -1, 2, 1)
    realResult = 2
    assert result==realResult

    result = F(1, 1, 2, 0)
    realResult = -1
    assert result==realResult

    result = F(1, -1, 2, 0)
    realResult = -1
    assert result==realResult
  end
end


def F(x, a, b, c)
  if(a < 0 and c != 0)
    return a*x*x + b*x + c
  elsif (a > 0 and c == 0)
    return -1 * a / (x - c)
  else
    return a * (x + c)
  end
end

def WorkWithUser()
  puts "Enter a: "
  a = gets.chomp.to_f
  puts "Enter b: "
  b = gets.chomp.to_f
  puts "Enter c: "
  c = gets.chomp.to_f
  puts "Enter x start: "
  xStart = gets.chomp.to_f
  puts "Enter x end: "
  xEnd = gets.chomp.to_f
  puts "Enter dX: "
  dX = gets.chomp.to_f
  puts ""

  xCurrent = xStart

  while xCurrent<=xEnd
    if(a.to_i and (b.to_i or c.to_i))
      puts "x: "+xCurrent.to_s+", y: "+F(xCurrent, a, b, c).to_s
    else
      puts "x: "+xCurrent.to_i.to_s+", y: "+F(xCurrent.to_i, a, b, c).to_s
    end
    xCurrent += dX
  end
end

#WorkWithUser()
#gets
