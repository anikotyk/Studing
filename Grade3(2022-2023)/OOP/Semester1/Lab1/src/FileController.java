import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.*;
import java.util.ArrayList;

public class FileController {
    public void WriteXML(ArrayList<Toy> toys){
        DocumentBuilderFactory dFact = DocumentBuilderFactory.newInstance();
        try{
            DocumentBuilder build = dFact.newDocumentBuilder();
            Document doc = build.newDocument();
            Element root = doc.createElement("Toys");
            doc.appendChild(root);

            for(int i=0; i < toys.size(); i++){
                Element toy = toys.get(i).CreateXMLObject(doc);
                root.appendChild(toy);
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

                e.printStackTrace();
            }

        }catch(Exception e){
            System.out.println("Error writing xml: "+e);
        }
    }

    public ArrayList<Toy> ReadXML(){
        ArrayList<Toy> toys = new ArrayList<>();

        try
        {
            File file = new File("data.xml");
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            Document doc = db.parse(file);
            doc.getDocumentElement().normalize();
            NodeList nodeList = doc.getElementsByTagName(String.valueOf(Type.dall));
            for (int itr = 0; itr < nodeList.getLength(); itr++)
            {
                Node node = nodeList.item(itr);
                if (node.getNodeType() == Node.ELEMENT_NODE)
                {
                    Element eElement = (Element) node;
                    Dall dall = new Dall(eElement);
                    toys.add(dall);
                }
            }

            nodeList = doc.getElementsByTagName(String.valueOf(Type.ball));
            for (int itr = 0; itr < nodeList.getLength(); itr++)
            {
                Node node = nodeList.item(itr);
                if (node.getNodeType() == Node.ELEMENT_NODE)
                {
                    Element eElement = (Element) node;
                    Ball ball = new Ball(eElement);
                    toys.add(ball);
                }
            }

            nodeList = doc.getElementsByTagName(String.valueOf(Type.car));
            for (int itr = 0; itr < nodeList.getLength(); itr++)
            {
                Node node = nodeList.item(itr);
                if (node.getNodeType() == Node.ELEMENT_NODE)
                {
                    Element eElement = (Element) node;
                    Car car = new Car(eElement);
                    toys.add(car);
                }
            }
        }
        catch (Exception e)
        {
            System.out.println("Error reading xml: "+e);
        }

        return toys;
    }
}
