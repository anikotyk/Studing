import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.ArrayList;

public class ClientSocketTask5 implements Runnable{

    public ClientSocketTask5(){
        Thread th = new Thread(this);
        th.start();
    }

    @Override
    public void run() {
        Socket sock = ConnectToServer();
        if(sock!=null){
            SendRequests(sock);
        }
    }

    private Socket ConnectToServer() {
        Socket sock = null;
        try {
            sock = new Socket("localhost", 12345);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return sock;
    }

    private void SendRequests(Socket sock){
        String regexSymbol = "#";

        ArrayList<String> writers1 = new ArrayList<>();
        writers1.add("writer1");
        writers1.add("writer2");
        writers1.add("writer3");
        ArrayList<String> writers2 = new ArrayList<>();
        writers2.add("writer1");
        writers2.add("writer3");
        ArrayList<String> writers3 = new ArrayList<>();
        writers3.add("writer2");
        Book book1 = new Book(0, "book1", "publisher1", 1940, 50, 750, "type1", writers1);
        Book book2 = new Book(1, "book2", "publisher2", 2005, 150, 1000, "type2", writers2);
        Book book3 = new Book(2, "book3", "publisher1", 2000, 100, 600, "type1", writers3);


        try {
            PrintWriter out = new PrintWriter(sock.getOutputStream(), true);

            out.println("1"+regexSymbol+book1.toSendFormat(regexSymbol));
            out.println("1"+regexSymbol+book2.toSendFormat(regexSymbol));
            out.println("1"+regexSymbol+book3.toSendFormat(regexSymbol));

            out.println("2"+regexSymbol+"writer1");
            out.println("3"+regexSymbol+"publisher1");
            out.println("4"+regexSymbol+"1950");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}
