require 'set'

class Student
  attr_reader :surname
  attr_reader :street
  attr_reader :house
  attr_reader :apartment

  def initialize(surname, street, house, apartment)
    @surname = surname
    @street = street
    @house = house
    @apartment = apartment
  end
end

def CreateNewStudent()
  print "Enter surname: "
  surname = gets.chomp
  print "Enter street: "
  street = gets.chomp
  print "Enter house: "
  house = gets.chomp
  print "Enter apartment: "
  apartment = gets.chomp
  return Student.new(surname, street, house, apartment)
end

def GetCountStudentsFromAllStreets(students)
  streets = GetAllStreets(students)
  counts = [0]*streets.length
  students.each { |student|
      index = streets.find_index(student.street)
      if(index >= 0)
        counts[index] += 1
      end
   }
   return counts
end

def PrintStreetsCount(students, counts)
  cnt = 0
  streets = GetAllStreets(students)
  if(streets.length>0)
    puts "Streets: "
  else
    puts "Array is empty"
  end
  streets.each { |street|
      puts street.to_s + ": " + counts[cnt].to_s
      cnt+=1
   }
end

def GetStudentsFromHouse(students, house)
  result = []
  students.each { |student|
      if(student.house==house)
        result.append(student)
      end
   }
   return result
end

def GetAllStreets(students)
  streets = []
  students.each { |student|
     streets.append(student.street)
  }
  return streets.to_set.to_a
end

def PrintStudents(students)
  if(students.length>0)
    print "Students: "
  else
    puts "Array is empty"
  end

  students.each { |student|
     print student.surname + " "
  }
end

def WorkWithUser()
  input = ""
  students = []
  while(input!="0")
    print "Enter command, 0 - to exit, 1 - to add student, 2 - to find count of students on each street, 3 - to find all students from house: "
    input = gets.chomp
    if(input=="0")
      break
    elsif (input=="1")
      students.append(CreateNewStudent())
      puts " "
    elsif (input=="2")
      PrintStreetsCount(students, GetCountStudentsFromAllStreets(students))
      puts " "
    elsif (input=="3")
      print "Enter house number: "
      n = gets.chomp
      PrintStudents(GetStudentsFromHouse(students, n))
      puts " "
    end
  end
end

WorkWithUser()
