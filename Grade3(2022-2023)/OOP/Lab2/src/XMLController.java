import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.xml.sax.SAXException;
import javax.xml.XMLConstants;

import javax.xml.parsers.*;

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
import java.util.Collections;

public class XMLController {
    public void WriteXML(ArrayList<DanceGroup> danceGroups){
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
                FileWriter fos = new FileWriter("data.xml");
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

    public void SAXParserXML() throws ParserConfigurationException, SAXException, IOException {
        SAXParserFactory factory = SAXParserFactory.newInstance();
        SAXParser saxParser = factory.newSAXParser();
        DanceGroupHandler danceGroupHandler = new DanceGroupHandler();
        String path = "data.xml";
        saxParser.parse(path, danceGroupHandler);
        ArrayList<DanceGroup> result = danceGroupHandler.danceGroups;
        Collections.sort(result, new DanceGroupSort());
        for(int i = 0; i < result.size(); i++){
            System.out.println(result.get(i));
            System.out.println();
        }

    }
}
