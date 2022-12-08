require 'minitest/autorun'

class ProgramTest < Minitest::Test
  def test_ask_returns_an_answer
	phone = Phone.new(380, 95, 3415621)
	email = Email.new("anna", "gmail.com")
	socialMedia = SocialMedia.new("www.instagram.com", "Annnna")
	data = [phone, email, socialMedia]

	anna = Contact.new("Anna", "White", data)

	phone = Phone.new(380, 95, 6712344)
	email = Email.new("dannni", "gmail.com")
	socialMedia = SocialMedia.new("www.instagram.com", "Danchik")
	data = [phone, email, socialMedia]

	dan = Contact.new("Dan", "Apple", data)

	contacts = Contacts.new([anna, dan])


	searchingValue = "Anna Whi"
	searchResult = contacts.searchContacts(searchingValue)
	realResult = [anna]
	assert searchResult==realResult
	
	searchingValue = "Danch"
	searchResult = contacts.searchContacts(searchingValue)
	realResult = [dan]
	assert searchResult==realResult
	
	searchingValue = "+38095"
	searchResult = contacts.searchContacts(searchingValue)
	realResult = [anna, dan]
	assert searchResult==realResult
	

	sortResult = contacts.sortContactsByName()
	realResult = [anna, dan]
	assert sortResult==realResult
	
	sortResult = contacts.sortContactsBySurname()
	realResult = [dan, anna]
	assert sortResult==realResult
  end
end

class Contacts
    @contactsArray
    
    def initialize(contactsArray)
        @contactsArray = contactsArray
    end
    
    def addContact(contact)
        @contactsArray.push(contact)
    end
    
    def printAllContacts()
        @contactsArray.each { |contact| puts contact}
        puts ""
    end
    
    def searchContacts(searchingValue)
        res = []
        
        @contactsArray.each { |contact| 
            if(contact.checkIsFitToSearch(searchingValue))
                res.push(contact)
            end
        }
        
        return res
    end
    
    def sortContactsByName()
       
        return @contactsArray.sort { |contact1, contact2| contact1.name<=>contact2.name }

    end
    
    def sortContactsBySurname()
       
        return @contactsArray.sort { |contact1, contact2| contact1.surname<=>contact2.surname }
        
    end
    
end

class Contact
    attr_reader :name
    attr_reader :surname
    @contactDataArray
    
    def initialize(name, surname, contactDataArray)
        @name = name
        @surname = surname
        @contactDataArray = contactDataArray
    end
    
    def checkIsFitToSearch(searchingValue)
        if(@name.length >= searchingValue.length and @name[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        if(@surname.length >= searchingValue.length and @surname[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
		tmp = @name +" "+@surname
		if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end 
        @contactDataArray.each { |contactData| 
            if(contactData.checkIsFitToSearch(searchingValue))
                return true
            end
        }
        return false
    end
    
    def to_s
        res = @name + " " + @surname+": \n"
        @contactDataArray.each { |data| res += "\t"+data.shortContactData+"\n"}
        return res
    end
end 

class ContactData
    attr_reader :shortContactData
    def checkIsFitToSearch(searchingValue)
        return false
    end
end

class Email < ContactData
    @username 
    @domain 
    
    def initialize(username, domain)
        @username = username
        @domain = domain
        @shortContactData = username+"@"+domain
    end
    
    def checkIsFitToSearch(searchingValue)
        tmp = @shortContactData
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        
        tmp = @username
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        
        return false
    end
end

class Phone < ContactData
    @countryCode 
    @providerCode 
    @phoneNumber 
    
    def initialize(countryCode, providerCode, phoneNumber)
        @countryCode = countryCode
        @providerCode = providerCode
        @phoneNumber = phoneNumber
        @shortContactData = "+"+countryCode.to_s+""+providerCode.to_s+""+phoneNumber.to_s
    end
    
    def checkIsFitToSearch(searchingValue)
        tmp = @shortContactData
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        tmp = @countryCode.to_s+""+@providerCode.to_s+""+@phoneNumber.to_s
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        tmp = @providerCode.to_s+""+@phoneNumber.to_s
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        tmp = @phoneNumber.to_s
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        return false
    end
end

class SocialMedia < ContactData
    @websiteLink 
    @personalIdentifier
    
    def initialize(websiteLink, personalIdentifier)
        @websiteLink = websiteLink
        @personalIdentifier = personalIdentifier
        @shortContactData = websiteLink+"/"+personalIdentifier
    end
    
    def checkIsFitToSearch(searchingValue)
        tmp = @shortContactData
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        
        tmp = @personalIdentifier
        if(tmp.length >= searchingValue.length and tmp[0, searchingValue.length].downcase==searchingValue.downcase)
            return true
        end
        
        return false
    end
end

