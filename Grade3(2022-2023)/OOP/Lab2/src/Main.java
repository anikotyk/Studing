import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;

public class Main {
    public static void main(String[] args) throws IOException, SAXException, ParserConfigurationException {
        XMLController xmlController = new XMLController();
        WriteTestData();
        System.out.println("is valid "+xmlController.IsXMLValid("data.xsd","data.xml"));
        System.out.println("is valid "+xmlController.IsXMLValid("data2.xsd","data.xml"));


        xmlController.SAXParserXML();
    }

    public static void WriteTestData(){
        XMLController xmlController = new XMLController();
        ArrayList<DanceGroup> danceGroups = new ArrayList<DanceGroup>();
        DanceGroup danceGroup = new DanceGroup();
        danceGroup.number = 2;
        DanceGroup danceGroup2 = new DanceGroup();
        danceGroup2.number = 1;
        Dancer dancer = new Dancer();
        Dancer dancer2 = new Dancer();
        ArrayList<Dancer> dancers = new ArrayList<Dancer>();
        dancers.add(dancer);
        dancers.add(dancer2);
        danceGroup2.dancers = dancers;
        danceGroup.dancers = dancers;
        danceGroups.add(danceGroup);
        danceGroups.add(danceGroup2);
        xmlController.WriteXML(danceGroups);


        Collections.sort(danceGroups, new DanceGroupSort());

        for (int i = 0; i < danceGroups.size(); i++)
            System.out.println(danceGroups.get(i).number);
    }
}
