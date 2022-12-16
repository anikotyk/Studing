import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class ServerRmiTask5
{
    public ServerRmiTask5(){
        try {
            ManagementImpl managementImpl = new ManagementImpl();
            Registry registry = LocateRegistry.createRegistry(123);
            registry.rebind("Management", managementImpl);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
