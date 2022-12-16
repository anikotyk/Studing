import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;

public class ServerSocketTask5 implements Runnable{
    ArrayList<Book> books;

    public ServerSocketTask5(){
        books = new ArrayList<Book>();

        Thread th = new Thread(this);
        th.start();
    }

    @Override
    public void run() {
        StartServerListening();
    }

    private void StartServerListening(){
        String regexSymbol = "#";
        ServerSocket server = null;
        try {
            server = new ServerSocket(12345);
        } catch (IOException e) {
            e.printStackTrace();
        }
        while (true){
            try {
                Socket sock = null;
                try {
                    sock = server.accept();
                } catch (IOException e) {
                    e.printStackTrace();
                }

                BufferedReader in = new BufferedReader(
                        new InputStreamReader(sock.getInputStream()));
                do{
                    String messageFromClient = in.readLine();

                    String data[] = messageFromClient.split(regexSymbol);
                    if (data[0].equals("1")) {
                        Book book = new Book(data);
                        books.add(book);
                    } else if (data[0].equals("2")) {
                        System.out.println("Books of writer "+data[1]+": ");
                        for(int i = 0; i < books.size(); i++){
                            for(int j = 0; j < books.get(i).writers.size(); j++){
                                if(books.get(i).writers.get(j).equals(data[1])){
                                    System.out.println(books.get(i));
                                    break;
                                }
                            }
                        }
                    } else if (data[0].equals("3")) {
                        System.out.println("Books of publisher "+data[1]+": ");
                        for(int i = 0; i < books.size(); i++){
                            if(books.get(i).publisher.equals(data[1])){
                                System.out.println(books.get(i));
                            }
                        }
                    } else if (data[0].equals("4")) {
                        System.out.println("Books published since "+data[1]+": ");
                        for(int i = 0; i < books.size(); i++){
                            int year = Integer.parseInt(data[1]);
                            if(books.get(i).publishingYear > year){
                                System.out.println(books.get(i));
                            }
                        }
                    }
                }while(in.ready());

            } catch (IOException e) {
                e.printStackTrace();
            }
        }

    }
}
