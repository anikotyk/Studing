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
        this.type = Type.Ballet;
        this.scene = Scene.Auditorium;
        this.numberOfDancers = NumberOfDancers.Mass;
        this.music = Music.Phonogram;
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

    @Override
    public boolean equals(Object obj) {
        if (obj == this) {
            return true;
        }

        if (!(obj instanceof DanceGroup)) {
            return false;
        }

        DanceGroup danceGroupObj = (DanceGroup) obj;

        return this.type.equals(danceGroupObj.type) && this.scene.equals(danceGroupObj.scene) &&
                this.numberOfDancers.equals(danceGroupObj.numberOfDancers) && this.music.equals(danceGroupObj.music) &&
                this.dancers.equals(danceGroupObj.dancers) && (this.number == danceGroupObj.number);
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
    Ballet,
    Folk,
    Pop,
    East
}

enum Scene{
    Auditorium,
    Street,
    Television
}

enum NumberOfDancers{
    Mass,
    Solo,
    Pair
}

enum Music{
    Phonogram,
    Live
}