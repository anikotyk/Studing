import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;
import javax.xml.XMLConstants;
import javax.xml.parsers.*;
import javax.xml.stream.XMLEventReader;
import javax.xml.stream.XMLInputFactory;
import javax.xml.stream.XMLStreamException;
import javax.xml.stream.events.EndElement;
import javax.xml.stream.events.StartElement;
import javax.xml.stream.events.XMLEvent;
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
import java.io.*;
import java.util.ArrayList;

public class XMLController {
    public void WriteXML(ArrayList<DanceGroup> danceGroups, String path){
        DocumentBuilderFactory dFact = DocumentBuilderFactory.newInstance();
        try{
            DocumentBuilder build = dFact.newDocumentBuilder();
            Document doc = build.newDocument();
            Element root = doc.createElement("Dance");
            doc.appendChild(root);

            for(int i=0; i < danceGroups.size(); i++){
                Element danceGroup = danceGroups.get(i).CreateXMLObject(doc);
                danceGroup.setAttribute("id", String.valueOf(i));
                root.appendChild(danceGroup);
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
    public ArrayList<DanceGroup> SAXParserXML(String path) throws ParserConfigurationException, SAXException, IOException {
        SAXParserFactory factory = SAXParserFactory.newInstance();
        SAXParser saxParser = factory.newSAXParser();
        DanceGroupHandler danceGroupHandler = new DanceGroupHandler();
        saxParser.parse(path, danceGroupHandler);
        ArrayList<DanceGroup> danceGroups = danceGroupHandler.danceGroups;

        return danceGroups;
    }

    public ArrayList<DanceGroup> DOMParserXML(String path) throws ParserConfigurationException, IOException, SAXException {
        ArrayList<DanceGroup> danceGroups = new ArrayList<DanceGroup>();
        DanceGroup danceGroup = null;
        Dancer dancer = null;

        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        Document document = builder.parse(new File(path));
        document.getDocumentElement().normalize();
        NodeList nList = document.getElementsByTagName("DanceGroup");
        for (int temp = 0; temp < nList.getLength(); temp++)
        {
            Node node = nList.item(temp);
            if (node.getNodeType() == Node.ELEMENT_NODE)
            {
                Element eElement = (Element) node;
                danceGroup = new DanceGroup();
                danceGroup.type = Type.valueOf(eElement.getElementsByTagName("Type").item(0).getTextContent());
                danceGroup.scene = Scene.valueOf(eElement.getElementsByTagName("Scene").item(0).getTextContent());
                danceGroup.numberOfDancers = NumberOfDancers.valueOf(eElement.getElementsByTagName("NumberOfDancers").item(0).getTextContent());
                danceGroup.music = Music.valueOf(eElement.getElementsByTagName("Music").item(0).getTextContent());
                danceGroup.number = Integer.parseInt(eElement.getElementsByTagName("Number").item(0).getTextContent());
                ArrayList<Dancer> dancers = new ArrayList<Dancer>();
                Node dancersNode = eElement.getElementsByTagName("Dancers").item(0);
                NodeList dancersList = dancersNode.getChildNodes();
                for(int i = 0; i < dancersList.getLength(); i++) {
                    Node dancerNode = dancersList.item(i);
                    if (dancerNode.getNodeType() == Node.ELEMENT_NODE)
                    {
                        Element dancerElement = (Element) dancerNode;
                        dancer = new Dancer();
                        dancer.name = dancerElement.getElementsByTagName("Name").item(0).getTextContent();
                        dancer.age = Integer.parseInt(dancerElement.getElementsByTagName("Age").item(0).getTextContent());
                        dancer.experienceYears = Integer.parseInt(dancerElement.getElementsByTagName("ExperienceYears").item(0).getTextContent());

                        dancers.add(dancer);
                    }
                }
                danceGroup.dancers = dancers;

                danceGroups.add(danceGroup);
            }
        }

        return danceGroups;

    }

    public ArrayList<DanceGroup> StAXParserXML(String path) throws FileNotFoundException, XMLStreamException {
        XMLInputFactory xmlInputFactory = XMLInputFactory.newInstance();
        XMLEventReader reader = xmlInputFactory.createXMLEventReader(new FileInputStream(path));

        ArrayList<DanceGroup> danceGroups = new ArrayList<DanceGroup>();
        DanceGroup danceGroup = null;
        ArrayList<Dancer> dancers = null;
        Dancer dancer = null;

        while (reader.hasNext()) {
            XMLEvent nextEvent = reader.nextEvent();
            if (nextEvent.isStartElement()) {
                StartElement startElement = nextEvent.asStartElement();
                switch (startElement.getName().getLocalPart()) {
                    case "DanceGroup":
                        danceGroup = new DanceGroup();
                        break;
                    case "Type":
                        nextEvent = reader.nextEvent();
                        danceGroup.type = Type.valueOf(nextEvent.asCharacters().getData());
                        break;
                    case "Scene":
                        nextEvent = reader.nextEvent();
                        danceGroup.scene = Scene.valueOf(nextEvent.asCharacters().getData());
                        break;
                    case "NumberOfDancers":
                        nextEvent = reader.nextEvent();
                        danceGroup.numberOfDancers = NumberOfDancers.valueOf(nextEvent.asCharacters().getData());
                        break;
                    case "Music":
                        nextEvent = reader.nextEvent();
                        danceGroup.music = Music.valueOf(nextEvent.asCharacters().getData());
                        break;
                    case "Number":
                        nextEvent = reader.nextEvent();
                        danceGroup.number = Integer.parseInt(nextEvent.asCharacters().getData());
                        break;
                    case "Dancers":
                        dancers = new ArrayList<Dancer>();
                        break;
                    case "Dancer":
                        dancer = new Dancer();
                        break;
                    case "Name":
                        nextEvent = reader.nextEvent();
                        dancer.name = nextEvent.asCharacters().getData();
                        break;
                    case "Age":
                        nextEvent = reader.nextEvent();
                        dancer.age = Integer.parseInt(nextEvent.asCharacters().getData());
                        break;
                    case "ExperienceYears":
                        nextEvent = reader.nextEvent();
                        dancer.experienceYears = Integer.parseInt(nextEvent.asCharacters().getData());
                        break;
                }
            }
            if (nextEvent.isEndElement()) {
                EndElement endElement = nextEvent.asEndElement();
                if (endElement.getName().getLocalPart().equals("DanceGroup")) {
                    danceGroups.add(danceGroup);
                }else if (endElement.getName().getLocalPart().equals("Dancer")) {
                    dancers.add(dancer);
                }else if (endElement.getName().getLocalPart().equals("Dancers")) {
                    danceGroup.dancers = dancers;
                }
            }
        }

        return danceGroups;
    }
}
