class Patient
  attr_reader :id
  attr_reader :surname
  attr_reader :name
  attr_reader :fathername
  attr_reader :adress
  attr_reader :medcardnumber
  attr_reader :diagnosis

  def initialize(id, surname, name, fathername, adress, medcardnumber, diagnosis)
    @id = id
    @surname = surname
    @name = name
    @fathername = fathername
    @adress = adress
    @medcardnumber = medcardnumber
    @diagnosis = diagnosis
  end
  def to_s()
     stringId =  @id.to_s
     medcardnumberStr =  @medcardnumber.to_s
     return stringId +": "+ @surname+" "+@name +" "+@fathername +" "+@adress +" "+medcardnumberStr +" "+@diagnosis
  end
end

class Patients
  attr_reader :patientsList

  def initialize()
    @last_id = 0
    @patientsList = Array.new
  end
  def toString()
    res = ""
    patientsList.each{ |patient|
      res += patient.toString()+"\n"
    }
    return res
  end
  def setTun(patientsList)
    @patientsList = patientsList
  end
  def getTun()
    return patientsList
  end
  def addNewPatient(surname, name, fathername, adress, medcardnumber, diagnosis)
    @last_id = @last_id + 1
    patientsList.push(Patient.new(@last_id, surname, name, fathername, adress, medcardnumber, diagnosis))
  end
  def getPatientsWithDiagnosis(diagnosis)
    puts "Patients with diagnosis "+diagnosis
    patientsList.each{ |patient|
      if(patient.diagnosis==diagnosis)
        puts patient
      end
    }
  end
  def getPatientsWithCardNumberInRange(minNumber, maxNumber)
    puts "Patients with medcard number in range "+minNumber.to_s+" - "+ maxNumber.to_s
    patientsList.each{ |patient|
      if(patient.medcardnumber>=minNumber and patient.medcardnumber<=maxNumber)
        puts patient
      end
    }
  end
end

patients = Patients.new()
patients.addNewPatient("John", "Smith", "Ivanovich", "Street1", 54, "flue")
patients.addNewPatient("Marie", "White", "Ivanovna", "Street2", 10, "flue")
patients.addNewPatient("Bob", "Grey", "Ivanovich", "Street52", 34, "fever")

patients.getPatientsWithDiagnosis("flue")
patients.getPatientsWithCardNumberInRange(12, 60)
