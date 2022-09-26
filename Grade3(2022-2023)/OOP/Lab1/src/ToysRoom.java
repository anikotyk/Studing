import java.util.*;
public class ToysRoom {
    ArrayList<Toy> toys = new ArrayList<Toy>();

    public void CreateRoom(AgeGroup ageGroup, float amount, ArrayList<Toy> allToys){
        this.toys.clear();
        Collections.shuffle(allToys);
        for(Toy toy: allToys) {
            if(toy.ageGroup.equals(ageGroup) && toy.price < amount){
                this.toys.add(toy);
                amount -= toy.price;
            }
        }
    }

    public void InputRoom(ArrayList<Toy> allToys){
        float amount;
        String input;
        Scanner scanner = new Scanner(System.in);
        while(true){
            System.out.println("Enter toy`s price: ");
            input = scanner.nextLine();
            input = input.trim();
            if(Lab1.isCanBeParsedToFloat(input)){
                amount = Float.parseFloat(input);
                if(amount>=0){
                    break;
                }
            }
        }
        AgeGroup ageGroup;
        while(true){
            System.out.println("Enter toy`s age group, 1 - toddlers, 2 - preschoolers, 3 - teenagers: ");
            input = scanner.nextLine();
            if(input.equals("1")){
                ageGroup = AgeGroup.toddlers;
                break;
            }else if(input.equals("2")){
                ageGroup =  AgeGroup.preschoolers;
                break;
            }else if(input.equals("3")){
                ageGroup =  AgeGroup.teenagers;
                break;
            }
        }

        CreateRoom(ageGroup, amount, allToys);
    }
    public void RoomConsoleMenu(){
        String input = "";
        Scanner scanner = new Scanner(System.in);

        while(true){
            System.out.println("Enter 0 - to exit, 1 - to sort, 2 - to get range, 3 - print room toys: ");
            input = scanner.nextLine();
            input = input.trim();
            if(input.equals("0")){
                break;
            }else if(input.equals("1")){
                SortingConsoleMenu();
            }else if(input.equals("2")){
                SearchingConsoleMenu();
            }else if(input.equals("3")){
                Lab1.printArray(this.toys);
            }
        }
    }

    public void SortingConsoleMenu(){
        String input = "";
        Scanner scanner = new Scanner(System.in);

        while(true){
            System.out.println("Enter 0 - to exit, 1 - to sort by type, 2 - to sort by size, 3 - to sort by age group: ");
            input = scanner.nextLine().trim();

            if(input.equals("0")){
                break;
            }else if(input.equals("1")){
                Collections.sort(this.toys, new Comparator<Toy>() {
                    @Override
                    public int compare(Toy o1, Toy o2) {
                        return o1.type.compareTo(o2.type);
                    }
                });
                Lab1.printArray(this.toys);
            }else if(input.equals("2")){
                Collections.sort(this.toys, new Comparator<Toy>() {
                    @Override
                    public int compare(Toy o1, Toy o2) {
                        return o1.size.compareTo(o2.size);
                    }
                });
                Lab1.printArray(this.toys);
            }else if(input.equals("3")){
                Collections.sort(this.toys, new Comparator<Toy>() {
                    @Override
                    public int compare(Toy o1, Toy o2) {
                        return o1.ageGroup.compareTo(o2.ageGroup);
                    }
                });
                Lab1.printArray(this.toys);
            }
        }
    }

    public void SearchingConsoleMenu(){
        String input = "";
        Scanner scanner = new Scanner(System.in);

        ArrayList<Toy> tmpToys = new ArrayList<>(this.toys);

        Type type = null;
        Size size = null;
        AgeGroup ageGroup = null;

        while(true){
            System.out.println("Enter 0 - to exit, 1 - to set type, 2 - to set size, 3 - to set age group: ");
            input = scanner.nextLine().trim();

            if(input.equals("0")){
                break;
            }else if(input.equals("1")){
                if(type!=null){
                    System.out.println("Type is already set.");
                    continue;
                }
                while(true){
                    System.out.println("Enter type, 1 - dall, 2 - ball, 3 - car: ");
                    input = scanner.nextLine().trim();

                    if(input.equals("1")){
                        type = Type.dall;
                        break;
                    }else if(input.equals("2")){
                        type = Type.ball;
                        break;
                    }else if(input.equals("3")){
                        type = Type.car;
                        break;
                    }
                }

                for (Iterator<Toy> it=tmpToys.iterator(); it.hasNext();) {
                    if (!it.next().type.equals(type))
                        it.remove();
                }

                Lab1.printArray(tmpToys);
            }else if(input.equals("2")){
                if(size!=null){
                    System.out.println("Size is already set.");
                    continue;
                }

                while(true){
                    System.out.println("Enter size, 1 - small, 2 - medium, 3 - big: ");
                    input = scanner.nextLine().trim();
                    if(input.equals("1")){
                        size = Size.small;
                        break;
                    }else if(input.equals("2")){
                        size = Size.medium;
                        break;
                    }else if(input.equals("3")){
                        size = Size.big;
                        break;
                    }
                }

                for (Iterator<Toy> it=tmpToys.iterator(); it.hasNext();) {
                    if (!it.next().size.equals(size))
                        it.remove();
                }

                Lab1.printArray(tmpToys);
            }else if(input.equals("3")){
                if(ageGroup!=null){
                    System.out.println("Age group is already set.");
                    continue;
                }
                while(true){
                    System.out.println("Enter age group, 1 - toddlers, 2 - preschoolers, 3 - teenagers: ");
                    input = scanner.nextLine().trim();

                    if(input.equals("1")){
                        ageGroup = AgeGroup.toddlers;
                        break;
                    }else if(input.equals("2")){
                        ageGroup =  AgeGroup.preschoolers;
                        break;
                    }else if(input.equals("3")){
                        ageGroup =  AgeGroup.teenagers;
                        break;
                    }
                }

                for (Iterator<Toy> it=tmpToys.iterator(); it.hasNext();) {
                    if (!it.next().ageGroup.equals(ageGroup))
                        it.remove();
                }

                Lab1.printArray(tmpToys);
            }
        }
    }
}
