import java.rmi.Remote;
import java.rmi.RemoteException;

public interface Management extends Remote {
    void AddProducer(CarProducer producer) throws RemoteException;
    void RemoveProducer(String producer) throws RemoteException;
    void AddMarkToProducer(String producer, Mark mark) throws RemoteException;
    void RemoveMarkFromProducer(String producer, Mark mark) throws RemoteException;
    void ChangeMarkFromProducer(String producer, Mark mark, String newMarkName) throws RemoteException;
    int CalculateMarksCountFromProducer(String producer) throws RemoteException;
    void PrintAllMarks() throws RemoteException;
    void PrintAllProducers() throws RemoteException;
    void PrintAllMarksFromProducers(String producer) throws RemoteException;
}
