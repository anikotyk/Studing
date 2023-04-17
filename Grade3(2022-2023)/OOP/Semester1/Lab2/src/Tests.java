import static org.junit.jupiter.api.Assertions.assertEquals;

import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import javax.xml.stream.XMLStreamException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;

public class Tests {
    static XMLController xmlController;
    static ArrayList<DanceGroup> danceGroups;
    static String pathXML = "data.xml";

    @BeforeAll
    static void setUp() {
        xmlController = new XMLController();
        danceGroups = new ArrayList<DanceGroup>();

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

    @Test
    @DisplayName("XML Validation should work")
    void testValidation() throws IOException, SAXException {
        assertEquals(true, xmlController.IsXMLValid("data.xsd", pathXML));
        assertEquals(false, xmlController.IsXMLValid("data2.xsd", pathXML));
    }

    @Test
    @DisplayName("Sort should work")
    void testSort() {
        ArrayList<DanceGroup> danceGroupsSorted = new ArrayList<DanceGroup>();
        ArrayList<DanceGroup> danceGroupsNotSorted = new ArrayList<DanceGroup>();
        DanceGroup danceGroup1 = new DanceGroup();
        danceGroup1.number = 1;
        DanceGroup danceGroup2 = new DanceGroup();
        danceGroup2.number = 2;

        danceGroupsSorted.add(danceGroup1);
        danceGroupsSorted.add(danceGroup2);

        danceGroupsNotSorted.add(danceGroup2);
        danceGroupsNotSorted.add(danceGroup1);

        Collections.sort(danceGroupsNotSorted, new DanceGroupSort());

        assertEquals(danceGroupsSorted, danceGroupsNotSorted);
    }

    @Test
    @DisplayName("SAX parsing should work")
    void testSAXParsing() throws ParserConfigurationException, IOException, SAXException {
        ArrayList<DanceGroup> result = xmlController.SAXParserXML(pathXML);
        assertEquals(danceGroups, result);
    }

    @Test
    @DisplayName("DOM parsing should work")
    void testDOMParsing() throws ParserConfigurationException, IOException, SAXException {
        ArrayList<DanceGroup> result = xmlController.DOMParserXML(pathXML);
        assertEquals(danceGroups, result);
    }

    @Test
    @DisplayName("StAX parsing should work")
    void testStAXParsing() throws IOException, XMLStreamException {
        ArrayList<DanceGroup> result = xmlController.StAXParserXML(pathXML);
        assertEquals(danceGroups, result);
    }
}
