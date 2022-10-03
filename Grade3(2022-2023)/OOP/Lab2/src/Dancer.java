import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class Dancer{
    public String name;
    public int age;
    public int experienceYears;

    public Dancer(String name, int age, int experienceYears){
        this.name = name;
        this.age = age;
        this.experienceYears = experienceYears;
    }

    public Dancer(){
        this.name = "name";
        this.age = 0;
        this.experienceYears = 0;
    }

    @Override
    public String toString(){
        return this.name + " "+ age +" years, "+experienceYears+" experience years";
    }

    @Override
    public boolean equals(Object obj) {
        if (obj == this) {
            return true;
        }

        if (!(obj instanceof Dancer)) {
            return false;
        }

        Dancer dancerObj = (Dancer) obj;

        return this.name.equals(dancerObj.name) && (this.age==dancerObj.age) && (this.experienceYears==dancerObj.experienceYears);
    }
    public Element CreateXMLObject(Document doc){
        try{
            Element dancer = (Element) doc.createElement("Dancer");

            Element name = doc.createElement("Name");
            name.appendChild(doc.createTextNode(String.valueOf(this.name)));
            dancer.appendChild(name);

            Element age = doc.createElement("Age");
            age.appendChild(doc.createTextNode(String.valueOf(this.age)));
            dancer.appendChild(age);

            Element experienceYears = doc.createElement("ExperienceYears");
            experienceYears.appendChild(doc.createTextNode(String.valueOf(this.experienceYears)));
            dancer.appendChild(experienceYears);

            return dancer;
        }catch (Exception e){
            System.out.println("Error creating xml object: "+e);
        }

        return null;
    }
}