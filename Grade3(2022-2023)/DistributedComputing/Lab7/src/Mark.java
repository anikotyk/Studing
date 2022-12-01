import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class Mark {
    public String name;

    public Mark(String name){
        this.name = name;
    }

    public Element CreateXMLObject(Document doc) {
        try {
            Element mark = doc.createElement("Mark");
            Element markName = doc.createElement("Name");
            markName.appendChild(doc.createTextNode(this.name));
            mark.appendChild(markName);
            return mark;
        } catch (Exception var9) {
            System.out.println("Error creating xml object: " + var9);
            return null;
        }
    }

    @Override
    public String toString(){
        return this.name;
    }
}
