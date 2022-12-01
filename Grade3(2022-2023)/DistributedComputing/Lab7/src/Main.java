import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;
import java.util.ArrayList;

public class Main {
    private static String pathXML = "data.xml";
    private static String pathXSD = "data.xsd";

    public static void main(String[] args) throws IOException, SAXException, ParserConfigurationException {
        Mark mark1 = new Mark("mark1");
        Mark mark2 = new Mark("mark2");
        Mark mark3 = new Mark("mark3");
        Mark mark4 = new Mark("mark4");
        Mark mark5 = new Mark("mark5");

        ArrayList<Mark> marks = new ArrayList<>();
        marks.add(mark1);
        marks.add(mark2);
        marks.add(mark3);
        CarProducer producer1 = new CarProducer("producer1", marks);

        marks = new ArrayList<>();
        marks.add(mark4);
        marks.add(mark5);
        CarProducer producer2 = new CarProducer("producer2", marks);

        ArrayList<CarProducer> carProducers = new ArrayList<CarProducer>();
        carProducers.add(producer1);
        carProducers.add(producer2);

        XMLController xmlController = new XMLController();

        xmlController.WriteXML(carProducers, pathXML);

        System.out.println("Is XML valid: "+ xmlController.IsXMLValid(pathXSD, pathXML));

        carProducers = xmlController.ReadXML(pathXML);

        for(int i = 0; i < carProducers.size(); i++){
            System.out.println(carProducers.get(i));
        }
    }


}