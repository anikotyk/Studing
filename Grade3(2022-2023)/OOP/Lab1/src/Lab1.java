import java.util.ArrayList;
import java.util.Scanner;
public class Lab1 {
    public static void main(String[] args) {
        FileController fileController = new FileController();
        ArrayList<Toy> toys =  fileController.Read();

        String input = "";
        Scanner scanner = new Scanner(System.in);

        while(true){
            System.out.println("Enter 0 - to exit, 1 - to add new toy, 2 - to create a room, 3 - to print all toys: ");
            input = scanner.nextLine().trim();

            if(input.equals("0")){
                break;
            }else if(input.equals("1")){
                Toy toy = new Toy();
                toy.InputToy();
                toys.add(toy);
                fileController.Write(toys);
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

    public static boolean isNumeric(String string) {
        int intValue;

        if(string == null || string.equals("")) {
            return false;
        }

        try {
            intValue = Integer.parseInt(string);
            return true;
        } catch (NumberFormatException e) {
        }
        return false;
    }
}
