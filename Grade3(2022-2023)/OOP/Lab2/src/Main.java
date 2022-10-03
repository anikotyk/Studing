import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import javax.xml.stream.XMLStreamException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;

public class Main {
    private static String pathXML = "data.xml";
    private static String pathXSD = "data.xsd";
    private static String pathWrongXSD = "data2.xsd";
    public static void main(String[] args) throws IOException, SAXException, ParserConfigurationException, XMLStreamException {
        XMLController xmlController = new XMLController();
        WriteTestData();

        System.out.println("Is XML valid: "+ xmlController.IsXMLValid(pathXSD, pathXML));
        System.out.println("Is XML valid by wrong XSD: "+xmlController.IsXMLValid(pathWrongXSD, pathXML));
        System.out.println();

        ArrayList<DanceGroup> danceGroups;
        danceGroups = xmlController.SAXParserXML(pathXML);
        //danceGroups = xmlController.DOMParserXML(pathXML);
        //danceGroups = xmlController.StAXParserXML(pathXML);

        Collections.sort(danceGroups, new DanceGroupSort());
        for(int i = 0; i < danceGroups.size(); i++){
            System.out.println(danceGroups.get(i));
            System.out.println();
        }
    }

    public static void WriteTestData(){
        XMLController xmlController = new XMLController();
        ArrayList<DanceGroup> danceGroups = new ArrayList<DanceGroup>();

        //Create first dance group
        ArrayList<Dancer> dancers1 = new ArrayList<Dancer>();;
        Dancer dancer11 = new Dancer("John", 20, 5);
        Dancer dancer12 = new Dancer("Sarah", 30, 10);
        dancers1.add(dancer11);
        dancers1.add(dancer12);
        DanceGroup danceGroup1 = new DanceGroup(Type.Folk, Scene.Street, NumberOfDancers.Pair, Music.Phonogram, dancers1, 2 );
        danceGroups.add(danceGroup1);

        //Create second dance group
        ArrayList<Dancer> dancers2 = new ArrayList<Dancer>();;
        Dancer dancer21 = new Dancer("Robert", 40, 25);
        Dancer dancer22 = new Dancer("Lili", 14, 10);
        Dancer dancer23 = new Dancer("Bob", 27, 9);
        Dancer dancer24 = new Dancer("Mike", 17, 2);
        dancers2.add(dancer21);
        dancers2.add(dancer22);
        dancers2.add(dancer23);
        dancers2.add(dancer24);
        DanceGroup danceGroup2 = new DanceGroup(Type.Pop, Scene.Television, NumberOfDancers.Mass, Music.Live, dancers2, 1 );
        danceGroups.add(danceGroup2);

        xmlController.WriteXML(danceGroups, pathXML);
    }
}
