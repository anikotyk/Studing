import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;

public class ManagementImpl extends UnicastRemoteObject implements Management {
    ArrayList<Book> books;
    public ManagementImpl() throws RemoteException {
        super();
        books = new ArrayList<Book>();
    }


    @Override
    public void AddBook(Book book) throws RemoteException {
        books.add(book);
    }

    @Override
    public void FindAllBooksOfWriter(String writer) throws RemoteException {
        System.out.println("Books of writer "+writer+": ");
        for(int i = 0; i < books.size(); i++){
            for(int j = 0; j < books.get(i).writers.size(); j++){
                if(books.get(i).writers.get(j).equals(writer)){
                    System.out.println(books.get(i));
                    break;
                }
            }
        }
    }

    @Override
    public void FindAllBooksOfPublisher(String publisher) throws RemoteException {
        System.out.println("Books of publisher "+publisher+": ");
        for(int i = 0; i < books.size(); i++){
            if(books.get(i).publisher.equals(publisher)){
                System.out.println(books.get(i));
            }
        }
    }

    @Override
    public void FindAllBooksPublishedSinceYear(int year) throws RemoteException {
        System.out.println("Books published since "+year+": ");
        for(int i = 0; i < books.size(); i++){
            if(books.get(i).publishingYear > year){
                System.out.println(books.get(i));
            }
        }
    }
}
