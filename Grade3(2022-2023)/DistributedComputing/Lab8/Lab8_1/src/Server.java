import com.sun.xml.internal.bind.v2.model.annotation.RuntimeAnnotationReader;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;

public class Server implements Runnable {
    ArrayList<CarProducer> producers;
    public Server(){
        producers = new ArrayList<CarProducer>();

        Thread th = new Thread(this);
        th.start();
    }

    @Override
    public void run() {
        StartServerListening();
    }

    private CarProducer FindProducerWithName(String name){
        for(int i = 0; i < producers.size(); i++){
            if(producers.get(i).name.equals(name)){
                return producers.get(i);
            }
        }

        return null;
    }

    private void StartServerListening(){
        ServerSocket server = null;
        try {
            server = new ServerSocket(12345);
        } catch (IOException e) {
            e.printStackTrace();
        }
        Socket sock = null;
        try {
            sock = server.accept();
        } catch (IOException e) {
            e.printStackTrace();
        }

        try {
            BufferedReader in = new BufferedReader(
                    new InputStreamReader(sock.getInputStream()));

            do{
                String messageFromClient = in.readLine();
                String data[] = messageFromClient.split("#");
                if (data[0].equals("1")) {
                    CarProducer prod = new CarProducer(data);
                    producers.add(prod);
                    System.out.println("Producer added");
                } else if (data[0].equals("2")) {
                    CarProducer prod = FindProducerWithName(data[1]);
                    producers.remove(prod);
                    System.out.println("Producer removed");
                } else if (data[0].equals("3")) {
                    CarProducer prod = FindProducerWithName(data[1]);
                    prod.marks.add(new Mark(data[2]));
                    System.out.println("Mark added");
                } else if (data[0].equals("4")) {
                    CarProducer prod = FindProducerWithName(data[1]);
                    Mark mark = prod.FindMarkByName(data[2]);
                    prod.marks.remove(mark);
                    System.out.println("Mark removed");
                } else if (data[0].equals("5")) {
                    CarProducer prod = FindProducerWithName(data[1]);
                    Mark mark = prod.FindMarkByName(data[2]);
                    mark.name = data[3];
                    System.out.println("Mark changed");
                } else if (data[0].equals("6")) {
                    CarProducer prod = FindProducerWithName(data[1]);
                    System.out.println("Count of " + prod.name + " marks: " + prod.marks.size());
                } else if (data[0].equals("7")) {
                    System.out.println("All marks: ");
                    for (int i = 0; i < producers.size(); i++) {
                        for (int j = 0; j < producers.get(i).marks.size(); j++) {
                            System.out.println(producers.get(i).marks.get(j) + ": " + producers.get(i).name);
                        }
                    }
                } else if (data[0].equals("8")) {
                    System.out.println("All producers: ");
                    for (int i = 0; i < producers.size(); i++) {
                        System.out.println(producers.get(i));
                    }
                } else if (data[0].equals("9")) {
                    CarProducer prod = FindProducerWithName(data[1]);
                    System.out.println(prod);
                }
            }while(in.ready());

        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
