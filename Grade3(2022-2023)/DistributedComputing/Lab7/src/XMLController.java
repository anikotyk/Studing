import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.XMLConstants;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Source;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;
import javax.xml.validation.Schema;
import javax.xml.validation.SchemaFactory;
import javax.xml.validation.Validator;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;

public class XMLController {
    public void WriteXML(ArrayList<CarProducer> carProducers, String path){
        DocumentBuilderFactory dFact = DocumentBuilderFactory.newInstance();
        try{
            DocumentBuilder build = dFact.newDocumentBuilder();
            Document doc = build.newDocument();
            Element root = doc.createElement("Autosalon");
            doc.appendChild(root);

            for(int i=0; i < carProducers.size(); i++){
                Element carProducer = carProducers.get(i).CreateXMLObject(doc);
                carProducer.setAttribute("id", String.valueOf(i));
                root.appendChild(carProducer);
            }

            TransformerFactory tranFactory = TransformerFactory.newInstance();
            Transformer aTransformer = tranFactory.newTransformer();

            aTransformer.setOutputProperty(OutputKeys.ENCODING, "ISO-8859-1");

            aTransformer.setOutputProperty(
                    "{http://xml.apache.org/xslt}indent-amount", "4");
            aTransformer.setOutputProperty(OutputKeys.INDENT, "yes");



            DOMSource source = new DOMSource(doc);
            try {
                FileWriter fos = new FileWriter(path);
                StreamResult result = new StreamResult(fos);
                aTransformer.transform(source, result);

            } catch (IOException e) {
                System.out.println("Error writing xml: "+e);
            }

        }catch(Exception e){
            System.out.println("Error writing xml: "+e);
        }
    }

    public ArrayList<CarProducer> ReadXML(String path) throws ParserConfigurationException, IOException, SAXException {
        ArrayList<CarProducer> carProducers = new ArrayList<CarProducer>();
        CarProducer carProducer = null;
        Mark mark = null;

        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        Document document = builder.parse(new File(path));
        document.getDocumentElement().normalize();
        NodeList nList = document.getElementsByTagName("CarProducer");
        for (int temp = 0; temp < nList.getLength(); temp++)
        {
            Node node = nList.item(temp);
            if (node.getNodeType() == Node.ELEMENT_NODE)
            {
                Element eElement = (Element) node;

                String name = eElement.getElementsByTagName("Name").item(0).getTextContent();

                ArrayList<Mark> marks = new ArrayList<Mark>();
                Node marksNode = eElement.getElementsByTagName("Marks").item(0);
                NodeList marksList = marksNode.getChildNodes();
                for(int i = 0; i < marksList.getLength(); i++) {
                    Node markNode = marksList.item(i);
                    if (markNode.getNodeType() == Node.ELEMENT_NODE)
                    {
                        Element markElement = (Element) markNode;
                        mark = new Mark( markElement.getElementsByTagName("Name").item(0).getTextContent());
                        marks.add(mark);
                    }
                }

                carProducer = new CarProducer(name, marks);

                carProducers.add(carProducer);
            }
        }

        return carProducers;
    }

    public boolean IsXMLValid(String xsdPath, String xmlPath) throws IOException, org.xml.sax.SAXException {
        SchemaFactory factory = SchemaFactory.newInstance(XMLConstants.W3C_XML_SCHEMA_NS_URI);
        Source schemaFile = new StreamSource(new File(xsdPath));
        Schema schema = factory.newSchema(schemaFile);
        Validator validator = schema.newValidator();

        try {
            validator.validate(new StreamSource(new File(xmlPath)));
            return true;
        } catch (org.xml.sax.SAXException e) {
            return false;
        }
    }
}
