import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.ArrayList;

public class Client implements Runnable{
    public Client(){
        Thread th = new Thread(this);
        th.start();
    }

    @Override
    public void run() {
        StartClientSending();
    }

    private void StartClientSending() {
        Socket sock = null;
        try {
            sock = new Socket("localhost", 12345);
        } catch (IOException e) {
            e.printStackTrace();
        }

        Mark mark = new Mark("mark1");
        Mark mark2 = new Mark("mark2");
        Mark mark3 = new Mark("mark3");
        ArrayList<Mark> marks = new ArrayList<>();
        ArrayList<Mark> marks2 = new ArrayList<>();
        marks.add(mark);
        marks.add(mark2);

        marks2.add(mark);
        marks2.add(mark2);
       // marks2.add(mark3);


        CarProducer producer = new CarProducer("producer1", marks);
        CarProducer producer2 = new CarProducer("producer2", marks);

        try {
            PrintWriter out = new PrintWriter(sock.getOutputStream(), true);
            out.println("1#"+producer.toSendFormat());
            out.println("1#"+producer2.toSendFormat());
            out.println("8");
            out.println("3#"+producer2.name+"#"+mark3.name);
            out.println("4#"+producer2.name+"#"+mark.name);
            out.println("5#"+producer2.name+"#"+mark2.name+"#marknew");
            out.println("8");
            out.println("6#"+producer.name);
            out.println("7");
            out.println("9#"+producer.name);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }


}
