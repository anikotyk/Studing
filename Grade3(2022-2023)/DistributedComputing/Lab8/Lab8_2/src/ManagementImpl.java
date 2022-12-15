import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;

public class ManagementImpl extends UnicastRemoteObject implements Management {
    ArrayList<CarProducer> producers;
    public ManagementImpl() throws RemoteException {
        super();
        producers = new ArrayList<CarProducer>();
    }
    private CarProducer FindCarProducer(String name){
        for(int i = 0; i < producers.size(); i++){
            if(producers.get(i).name.equals(name)){
                return producers.get(i);
            }
        }

        return null;
    }

    @Override
    public void AddProducer(CarProducer producer) throws RemoteException {
        producers.add(producer);
    }

    @Override
    public void RemoveProducer(String producer) throws RemoteException {
        producers.remove(producer);
    }

    @Override
    public void AddMarkToProducer(String producer, Mark mark) throws RemoteException {
        CarProducer prod = FindCarProducer(producer);
        prod.marks.add(mark);
    }

    @Override
    public void RemoveMarkFromProducer(String producer, Mark mark) throws RemoteException {
        CarProducer prod = FindCarProducer(producer);
        prod.marks.remove(mark);
    }

    @Override
    public void ChangeMarkFromProducer(String producer, Mark mark, String newMarkName) throws RemoteException {
        CarProducer prod = FindCarProducer(producer);
        prod.FindMarkByName(mark.name).name = newMarkName;
    }

    @Override
    public int CalculateMarksCountFromProducer(String producer) throws RemoteException {
        CarProducer prod = FindCarProducer(producer);
        return prod.marks.size();
    }

    @Override
    public void PrintAllMarks() throws RemoteException {
        System.out.println("All marks: ");
        for (int i = 0; i < producers.size(); i++) {
            for (int j = 0; j < producers.get(i).marks.size(); j++) {
                System.out.println(producers.get(i).marks.get(j) + ": " + producers.get(i).name);
            }
        }
    }

    @Override
    public void PrintAllProducers() throws RemoteException {
        System.out.println("All producers: ");
        for (int i = 0; i < producers.size(); i++) {
            System.out.println(producers.get(i));
        }
    }

    @Override
    public void PrintAllMarksFromProducers(String producer) throws RemoteException {
        CarProducer prod = FindCarProducer(producer);
        System.out.println(prod);
    }
}
