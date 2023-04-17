import org.w3c.dom.Document;
import org.w3c.dom.Element;

import java.util.Scanner;

public class Ball extends Toy{
    public Type type = Type.ball;
    public Color color;

    public Ball() { }

    public Ball(String name, Type type, Size size, AgeGroup ageGroup, Color color, float price) {
        this.name = name;
        this.type = Type.ball;
        this.size = size;
        this.ageGroup = ageGroup;
        this.color = color;
        this.price = price;
    }

    public Ball(Element element) {
        this.name = element.getElementsByTagName("name").item(0).getTextContent();
        this.type = Type.ball;
        this.size = Size.valueOf(element.getElementsByTagName("size").item(0).getTextContent());
        this.ageGroup = AgeGroup.valueOf(element.getElementsByTagName("ageGroup").item(0).getTextContent());
        this.color = Color.valueOf(element.getElementsByTagName("color").item(0).getTextContent());
        this.price = Float.parseFloat(element.getElementsByTagName("price").item(0).getTextContent());
    }

    @Override
    public String toString() {
        return this.name + ": " + this.type + ", " +this.size + ", " +this.ageGroup + ", "+this.color + ", "  +this.price + " uah";
    }

    @Override
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

            Element color =  doc.createElement("color");
            color.appendChild(doc.createTextNode(String.valueOf(this.color)));
            toy.appendChild(color);

            Element price =  doc.createElement("price");
            price.appendChild(doc.createTextNode(String.valueOf(this.price)));
            toy.appendChild(price);

            return toy;
        }catch (Exception e){
            System.out.println("Error creating xml object: "+e);
        }

        return null;
    }

    @Override
    public void InputToy(){
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter ball`s name: ");
        this.name = scanner.nextLine();

        while(true){
            System.out.println("Enter ball`s size, 1 - small, 2 - medium, 3 - big: ");
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
            System.out.println("Enter ball`s age group, 1 - toddlers, 2 - preschoolers, 3 - teenagers: ");
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
            System.out.println("Enter ball`s color, 1 - red, 2 - green, 3 - blue: ");
            String input = scanner.nextLine().trim();

            if(input.equals("1")){
                this.color = Color.red;
                break;
            }else if(input.equals("2")){
                this.color =  Color.green;
                break;
            }else if(input.equals("3")){
                this.color =  Color.blue;
                break;
            }
        }

        while(true){
            System.out.println("Enter ball`s price: ");
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

enum Color {
    red,
    green,
    blue
}