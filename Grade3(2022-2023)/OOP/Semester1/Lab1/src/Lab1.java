import java.util.ArrayList;
import java.util.Scanner;
public class Lab1 {
    public static void main(String[] args) {
        FileController fileController = new FileController();
        ArrayList<Toy> toys = fileController.ReadXML();

        String input = "";
        Scanner scanner = new Scanner(System.in);

        while(true){
            System.out.println("Enter 0 - to exit, 1 - to add new toy, 2 - to create a room, 3 - to print all toys: ");
            input = scanner.nextLine().trim();

            if(input.equals("0")){
                break;
            }else if(input.equals("1")){
                Toy toy;
                while(true){
                    System.out.println("Enter toy`s type, 1 - dall, 2 - ball, 3 - car: ");
                    input = scanner.nextLine().trim();

                    if(input.equals("1")){
                        toy = new Dall();
                        break;
                    }else if(input.equals("2")){
                        toy = new Ball();
                        break;
                    }else if(input.equals("3")){
                        toy = new Car();
                        break;
                    }
                }
                toy.InputToy();
                toys.add(toy);
                fileController.WriteXML(toys);
            }else if(input.equals("2")){
                ToysRoom room = new ToysRoom();
                room.InputRoom(toys);
                room.RoomConsoleMenu();
            }else if(input.equals("3")){
                printArray(toys);
            }
        }
    }

    public static void printArray(ArrayList<Toy> toys){
        if(toys.size()<1){
            System.out.println("Array is empty");
        }
        for(Toy toy: toys) {
            System.out.println(toy.toString());
        }
    }

    public static boolean isCanBeParsedToFloat(String string) {
        float floatValue;

        if(string == null || string.equals("")) {
            return false;
        }

        try {
            floatValue = Float.parseFloat(string);
            return true;
        } catch (NumberFormatException e) {

        }
        return false;
    }
}
