import org.w3c.dom.Document;
import org.w3c.dom.Element;

import java.util.Scanner;

public class Dall extends Toy{
    public Type type = Type.dall;
    public Haircut haircut;

    public Dall() { }

    public Dall(String name, Type type, Size size, AgeGroup ageGroup, Haircut haircut, float price) {
        this.name = name;
        this.type = Type.dall;
        this.size = size;
        this.ageGroup = ageGroup;
        this.haircut = haircut;
        this.price = price;
    }

    public Dall(Element element) {
        this.name = element.getElementsByTagName("name").item(0).getTextContent();
        this.type = Type.dall;
        this.size = Size.valueOf(element.getElementsByTagName("size").item(0).getTextContent());
        this.ageGroup = AgeGroup.valueOf(element.getElementsByTagName("ageGroup").item(0).getTextContent());
        this.haircut = Haircut.valueOf(element.getElementsByTagName("haircut").item(0).getTextContent());
        this.price = Float.parseFloat(element.getElementsByTagName("price").item(0).getTextContent());
    }

    @Override
    public String toString() {
        return this.name + ": " + this.type + ", " +this.size + ", " +this.ageGroup + ", "+this.haircut + ", "  +this.price + " uah";
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

            Element haircut =  doc.createElement("haircut");
            haircut.appendChild(doc.createTextNode(String.valueOf(this.haircut)));
            toy.appendChild(haircut);


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
        System.out.println("Enter dall`s name: ");
        this.name = scanner.nextLine();

        while(true){
            System.out.println("Enter dall`s size, 1 - small, 2 - medium, 3 - big: ");
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
            System.out.println("Enter dall`s age group, 1 - toddlers, 2 - preschoolers, 3 - teenagers: ");
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
            System.out.println("Enter dall`s haircut, 1 - shortHair, 2 - mediumHair, 3 - longHair: ");
            String input = scanner.nextLine().trim();

            if(input.equals("1")){
                this.haircut = Haircut.shortHair;
                break;
            }else if(input.equals("2")){
                this.haircut =  Haircut.mediumHair;
                break;
            }else if(input.equals("3")){
                this.haircut =  Haircut.longHair;
                break;
            }
        }

        while(true){
            System.out.println("Enter dall`s price: ");
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

enum Haircut {
    shortHair,
    mediumHair,
    longHair
}
