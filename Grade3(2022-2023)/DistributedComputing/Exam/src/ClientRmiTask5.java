import java.rmi.Naming;
import java.util.ArrayList;

public class ClientRmiTask5 implements Runnable{
    public ClientRmiTask5(){
        Thread th = new Thread(this);
        th.start();
    }

    @Override
    public void run() {
        StartClientSending();
    }

    private void StartClientSending() {
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
            String url = "//localhost:123/Management";
            Management management = (Management) Naming.lookup(url);
            management.AddBook(book1);
            management.AddBook(book2);
            management.AddBook(book3);

            management.FindAllBooksOfWriter("writer1");
            management.FindAllBooksOfPublisher("publisher1");
            management.FindAllBooksPublishedSinceYear(1950);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
