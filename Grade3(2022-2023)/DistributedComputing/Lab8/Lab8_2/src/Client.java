import java.io.IOException;
import java.io.PrintWriter;
import java.net.MalformedURLException;
import java.net.Socket;
import java.rmi.Naming;
import java.rmi.NotBoundException;
import java.rmi.RemoteException;
import java.util.ArrayList;

public class Client{
    public Client(){
        StartClientSending();
    }
    private void StartClientSending() {
        Mark mark = new Mark("mark1");
        Mark mark2 = new Mark("mark2");
        Mark mark3 = new Mark("mark3");
        ArrayList<Mark> marks = new ArrayList<>();
        ArrayList<Mark> marks2 = new ArrayList<>();
        marks.add(mark);
        marks.add(mark2);

        marks2.add(mark);
        marks2.add(mark2);

        CarProducer producer = new CarProducer("producer1", marks);
        CarProducer producer2 = new CarProducer("producer2", marks);

        try {
            String url = "//localhost:123/Management";
            Management management = (Management) Naming.lookup(url);
            System.out.println("RMI object found");
            management.AddProducer(producer);
            management.AddProducer(producer2);
            management.PrintAllProducers();
            management.AddMarkToProducer(producer2.name, mark3);
            management.RemoveMarkFromProducer(producer2.name, mark);
            management.ChangeMarkFromProducer(producer2.name, mark2, "newName");
            management.PrintAllProducers();
            management.CalculateMarksCountFromProducer(producer.name);
            management.PrintAllMarks();
            management.PrintAllMarksFromProducers(producer.name);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
