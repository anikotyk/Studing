import org.w3c.dom.Document;
import org.w3c.dom.Element;

import java.util.ArrayList;

public class CarProducer {
    private String name;
    public ArrayList<Mark> marks;

    public CarProducer(String name, ArrayList<Mark> marks){
        this.name = name;
        this.marks = marks;
    }

    public Element CreateXMLObject(Document doc) {
        try {
            Element carProducer = doc.createElement("CarProducer");
            Element producerName = doc.createElement("Name");
            producerName.appendChild(doc.createTextNode(this.name));
            carProducer.appendChild(producerName);

            Element producerMarks = doc.createElement("Marks");
            for(int i = 0; i < this.marks.size(); ++i) {
                producerMarks.appendChild((this.marks.get(i)).CreateXMLObject(doc));
            }
            carProducer.appendChild(producerMarks);

            return carProducer;
        } catch (Exception var9) {
            System.out.println("Error creating xml object: " + var9);
            return null;
        }
    }

    @Override
    public String toString(){
        String marks = "";
        for (int i = 0; i < this.marks.size(); i++){
            marks += this.marks.get(i)+" ";
        }
        return this.name + ": "+ marks;
    }
}
