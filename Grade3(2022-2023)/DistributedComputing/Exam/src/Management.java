import java.rmi.Remote;
import java.rmi.RemoteException;

public interface Management extends Remote {
    void AddBook(Book book) throws RemoteException;
    void FindAllBooksOfWriter(String writer) throws RemoteException;
    void FindAllBooksOfPublisher(String publisher) throws RemoteException;
    void FindAllBooksPublishedSinceYear(int year) throws RemoteException;
}
