import java.io.*;
import java.util.ArrayList;

public class FileController {
    public ArrayList<Toy> Read(){
        ArrayList<Toy> toys = new ArrayList<Toy>();

        try (FileInputStream fis = new FileInputStream("toys.dat");
             ObjectInputStream ois = new ObjectInputStream(fis)) {
            int trainCount = ois.readInt();
            for (int i = 0; i < trainCount; i++) {
                toys.add((Toy)ois.readObject());
            }
        } catch (Exception e){

        }

        return toys;
    }

    public void Write(ArrayList<Toy> toys){
        try (FileOutputStream fos = new FileOutputStream("toys.dat");
        ObjectOutputStream oos = new ObjectOutputStream(fos)) {
            oos.writeInt(toys.size());
            for(Toy toy: toys) {
                oos.writeObject(toy);
            }
        } catch (Exception e) {

        }
    }
}
