import java.io.Serializable;
import java.util.Scanner;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

abstract public class Toy implements Serializable {
    public String name;
    public Type type;
    public Size size;
    public AgeGroup ageGroup;
    public float price;

    public Toy() { }

    public Toy(String name, Type type, Size size, AgeGroup ageGroup, float price) {
        this.name = name;
        this.type = type;
        this.size = size;
        this.ageGroup = ageGroup;
        this.price = price;
    }

    public Toy(Element element) {
        this.name = element.getElementsByTagName("name").item(0).getTextContent();
        this.type = Type.valueOf(element.getTagName());
        this.size = Size.valueOf(element.getElementsByTagName("size").item(0).getTextContent());
        this.ageGroup = AgeGroup.valueOf(element.getElementsByTagName("ageGroup").item(0).getTextContent());
        this.price = Float.parseFloat(element.getElementsByTagName("price").item(0).getTextContent());
    }

    @Override
    public String toString() {
        return this.name + ": " + this.type + ", " +this.size + ", " +this.ageGroup + ", " +this.price + " uah";
    }

    public Element CreateXMLObject(Document doc){
        try{
            Element toy = doc.createElement(String.valueOf(this.type));

            Element name = doc.createElement("name");
            name.appendChild(doc.createTextNode(this.name));
            toy.appendChild(name);

            Element size =  doc.createElement("size");
            size.appendChild(doc.createTextNode(String.valueOf(this.size)));
            toy.appendChild(size);

            Element ageGroup =  doc.createElement("ageGroup");
            ageGroup.appendChild(doc.createTextNode(String.valueOf(this.ageGroup)));
            toy.appendChild(ageGroup);

            Element price =  doc.createElement("price");
            price.appendChild(doc.createTextNode(String.valueOf(this.price)));
            toy.appendChild(price);

            return toy;
        }catch (Exception e){
            System.out.println("Error creating xml object: "+e);
        }

        return null;
    }

    public void InputToy(){
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter toy`s name: ");
        this.name = scanner.nextLine();

        while(true){
            System.out.println("Enter toy`s type, 1 - dall, 2 - ball, 3 - car: ");
            String input = scanner.nextLine().trim();

            if(input.equals("1")){
                this.type = Type.dall;
                break;
            }else if(input.equals("2")){
                this.type = Type.ball;
                break;
            }else if(input.equals("3")){
                this.type = Type.car;
                break;
            }
        }

        while(true){
            System.out.println("Enter toy`s size, 1 - small, 2 - medium, 3 - big: ");
            String input = scanner.nextLine().trim();

            if(input.equals("1")){
                this.size = Size.small;
                break;
            }else if(input.equals("2")){
                this.size = Size.medium;
                break;
            }else if(input.equals("3")){
                this.size = Size.big;
                break;
            }
        }

        while(true){
            System.out.println("Enter toy`s age group, 1 - toddlers, 2 - preschoolers, 3 - teenagers: ");
            String input = scanner.nextLine().trim();

            if(input.equals("1")){
                this.ageGroup = AgeGroup.toddlers;
                break;
            }else if(input.equals("2")){
                this.ageGroup =  AgeGroup.preschoolers;
                break;
            }else if(input.equals("3")){
                this.ageGroup =  AgeGroup.teenagers;
                break;
            }
        }

        while(true){
            System.out.println("Enter toy`s price: ");
            String input = scanner.nextLine().trim();

            if(Lab1.isCanBeParsedToFloat(input)){
                this.price = Float.parseFloat(input);
                if(this.price>=0){
                    break;
                }
            }
        }
    }
}

enum Size {
    small,
    medium,
    big
}

enum Type {
    dall,
    ball,
    car
}
enum AgeGroup {
    toddlers,
    preschoolers,
    teenagers
}