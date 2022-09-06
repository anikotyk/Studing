size = 10
A = []
B = []
C = []

size.times{
  A.push(1 + rand(10))
  B.push(1 + rand(10))
}

print "A array: "
A.each{|x| print x.to_s + " "}
print "\nB array: "
B.each{|x| print x.to_s + " "}

A.each{|x|
  index = B.find_index(x)
  if index != nil
    B.delete_at(index)
    C.append(x)
  end
}

print "\n\nA & B array: "
C.each{|x| print x.to_s+" "}
