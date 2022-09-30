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
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;

public class TaskA {
    public static void main(String[] args) {
        ArrayList<DataObject> dataObject = new ArrayList<DataObject>();
        FileController fileController = new FileController();

        DataObject obj1 = new DataObject("name1", "phone1");
        DataObject obj2 = new DataObject("name2", "phone2");
        DataObject obj3 = new DataObject("name3", "phone3");
        DataObject obj4 = new DataObject("name4", "phone4");

        MainA mainA = new MainA();

        dataObject.add(obj1);
        dataObject.add(obj2);
        dataObject.add(obj3);
        dataObject.add(obj4);
        fileController.WriteXML(dataObject);
        fileController.ReadXML();

        DataObject obj5 = new DataObject("name5", "phone5");
        new Writer(fileController, mainA, obj5);
        new FinderPhoneByName(fileController, mainA, "name5");
        new Deleter(fileController, mainA, obj2);
        new FinderNameByPhone(fileController, mainA, "phone2");
    }
}

class MainA {
    int countReaders;
    int countWriters;

    public synchronized void ReaderGetPermission(){
        while(countWriters>0){
            try {
                wait();
            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }
        }

        this.countReaders += 1;
        notify();
    }

    public synchronized void ReaderEnded(){
        this.countReaders -= 1;
        notifyAll();
    }

    public synchronized void WriterGetPermission(){
        while(countWriters>0 || countReaders>0){
            try {
                wait();
            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }
        }

        this.countWriters += 1;
        notify();
    }

    public synchronized void WriterEnded(){
        this.countWriters -= 1;
        notifyAll();
    }
}


class FinderPhoneByName implements Runnable {
    FileController fileController;
    MainA mainA;

    String name;

    public FinderPhoneByName(FileController fileController, MainA mainA, String name){
        this.fileController = fileController;
        this.mainA = mainA;
        this.name = name;

        new Thread(this, "FinderPhoneByName").start();
    }

    private String FindPhoneByName(String name){
        ArrayList<DataObject> dataObjects = fileController.ReadXML();
        DataObject obj = dataObjects.stream().filter(dataObject -> dataObject.name.equals(name)).findFirst().orElse(null);
        if(obj==null){
            return "not found";
        }
        return obj.phone;
    }

    @Override
    public void run() {
        mainA.ReaderGetPermission();
        System.out.println("Phone finder: name "+name+" has phone "+ FindPhoneByName(name));
        mainA.ReaderEnded();
    }
}

class FinderNameByPhone implements Runnable {
    FileController fileController;
    MainA mainA;
    String phone;

    public FinderNameByPhone(FileController fileController,  MainA mainA, String phone){
        this.fileController = fileController;
        this.mainA = mainA;
        this.phone = phone;

        new Thread(this, "FinderNameByPhone").start();
    }

    private String FindNameByPhone(String phone){
        ArrayList<DataObject> dataObjects = fileController.ReadXML();
        DataObject obj = dataObjects.stream().filter(dataObject -> dataObject.phone.equals(phone)).findFirst().orElse(null);
        if(obj==null){
            return "not found";
        }
        return obj.name;
    }

    @Override
    public void run() {
        mainA.ReaderGetPermission();
        System.out.println("Name finder: phone "+phone+" has name "+ FindNameByPhone(phone));
        mainA.ReaderEnded();
    }
}

class Deleter implements Runnable {
    FileController fileController;
    MainA mainA;

    DataObject obj;

    public Deleter(FileController fileController,  MainA mainA, DataObject obj){
        this.fileController = fileController;
        this.mainA = mainA;
        this.obj = obj;

        new Thread(this, "Deleter").start();
    }

    private void Delete(DataObject obj){
        ArrayList<DataObject> dataObjects = fileController.ReadXML();
        dataObjects.remove(obj);
        fileController.WriteXML(dataObjects);
    }
    @Override
    public void run() {
        mainA.WriterGetPermission();
        Delete(obj);
        System.out.println("Deleter: deleted person with name "+obj.name);
        mainA.WriterEnded();
    }
}

class Writer implements Runnable {
    FileController fileController;
    MainA mainA;
    DataObject obj;

    public Writer(FileController fileController,  MainA mainA, DataObject obj){
        this.fileController = fileController;
        this.mainA = mainA;
        this.obj = obj;

        new Thread(this, "Writer").start();
    }

    private void Write(DataObject obj){
        ArrayList<DataObject> dataObjects = fileController.ReadXML();
        dataObjects.add(obj);
        fileController.WriteXML(dataObjects);
    }

    @Override
    public void run() {
        mainA.WriterGetPermission();
        Write(obj);
        System.out.println("Writer: write person with name "+obj.name);
        mainA.WriterEnded();
    }
}

class DataObject {
    String name;
    String phone;

    public DataObject(String name, String phone){
        this.name = name;
        this.phone = phone;
    }

    public DataObject(Element element) {
        this.name = element.getElementsByTagName("Name").item(0).getTextContent();
        this.phone = element.getElementsByTagName("Phone").item(0).getTextContent();
    }

    public Element CreateXMLObject(Document doc){
        try{
            Element obj = doc.createElement("Person");

            Element name = doc.createElement("Name");
            name.appendChild(doc.createTextNode(this.name));
            obj.appendChild(name);

            Element phone = doc.createElement("Phone");
            phone.appendChild(doc.createTextNode(this.phone));
            obj.appendChild(phone);

            return obj;
        }catch (Exception e){
            System.out.println("Error creating xml object: "+e);
        }

        return null;
    }
}

class FileController {
    public void WriteXML(ArrayList<DataObject> dataArray){
        DocumentBuilderFactory dFact = DocumentBuilderFactory.newInstance();
        try{
            DocumentBuilder build = dFact.newDocumentBuilder();
            Document doc = build.newDocument();
            Element root = doc.createElement("Data");
            doc.appendChild(root);

            for(int i=0; i < dataArray.size(); i++){
                Element obj = dataArray.get(i).CreateXMLObject(doc);
                root.appendChild(obj);
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

    public ArrayList<DataObject> ReadXML(){
        ArrayList<DataObject> dataArray = new ArrayList<>();

        try
        {
            File file = new File("data.xml");
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            Document doc = db.parse(file);
            doc.getDocumentElement().normalize();
            NodeList nodeList = doc.getElementsByTagName("Person");
            for (int itr = 0; itr < nodeList.getLength(); itr++)
            {
                Node node = nodeList.item(itr);
                if (node.getNodeType() == Node.ELEMENT_NODE)
                {
                    Element eElement = (Element) node;
                    DataObject obj = new DataObject(eElement);
                    dataArray.add(obj);
                }
            }
        }
        catch (Exception e)
        {
            System.out.println("Error reading xml: "+e);
        }

        return dataArray;
    }
}
