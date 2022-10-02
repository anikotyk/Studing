import org.w3c.dom.Document;
import org.w3c.dom.Element;
import java.util.ArrayList;

public class DanceGroup {
    public Type type;
    public Scene scene;
    public NumberOfDancers numberOfDancers;
    public Music music;
    public ArrayList<Dancer> dancers;
    public int number;

    public DanceGroup(Type type, Scene scene, NumberOfDancers numberOfDancers, Music music, ArrayList<Dancer> dancers, int number){
        this.type = type;
        this.scene = scene;
        this.numberOfDancers = numberOfDancers;
        this.music = music;
        this.dancers = dancers;
        this.number = number;
    }

    public DanceGroup(){
        this.type = Type.Type1;
        this.scene = Scene.Scene1;
        this.numberOfDancers = NumberOfDancers.NumberOfDancers1;
        this.music = Music.Music1;
        this.dancers = new ArrayList<Dancer>();
        this.number = 0;
    }

    @Override
    public String toString(){
        String dancersString = "";
        for(int i = 0; i < this.dancers.size(); i++){
            dancersString += "\n\t" + this.dancers.get(i);
        }

        return "Type: "+this.type + "\nScene: "+this.scene + "\nNumber of dancers: "+
                this.numberOfDancers + "\nMusic: "+this.music + "\nDancers: "+ dancersString + "\nNumber: "+this.number;
    }
    public Element CreateXMLObject(Document doc){
        try{
            Element danceGroup = (Element) doc.createElement("DanceGroup");


            Element type = doc.createElement("Type");
            type.appendChild(doc.createTextNode(String.valueOf(this.type)));
            danceGroup.appendChild(type);

            Element scene = doc.createElement("Scene");
            scene.appendChild(doc.createTextNode(String.valueOf(this.scene)));
            danceGroup.appendChild(scene);

            Element numberOfDancers = doc.createElement("NumberOfDancers");
            numberOfDancers.appendChild(doc.createTextNode(String.valueOf(this.numberOfDancers)));
            danceGroup.appendChild(numberOfDancers);

            Element music = doc.createElement("Music");
            music.appendChild(doc.createTextNode(String.valueOf(this.music)));
            danceGroup.appendChild(music);

            Element dancers = doc.createElement("Dancers");
            for(int i = 0; i < this.dancers.size(); i++){
                dancers.appendChild(this.dancers.get(i).CreateXMLObject(doc));
            }
            danceGroup.appendChild(dancers);

            Element number = doc.createElement("Number");
            number.appendChild(doc.createTextNode(String.valueOf(this.number)));
            danceGroup.appendChild(number);

            return danceGroup;
        }catch (Exception e){
            System.out.println("Error creating xml object: "+e);
        }

        return null;
    }
}

enum Type{
    Type1,
    Type2
}

enum Scene{
    Scene1,
    Scene2
}

enum NumberOfDancers{
    NumberOfDancers1,
    NumberOfDancers2
}

enum Music{
    Music1,
    Music2
}

class Dancer{
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