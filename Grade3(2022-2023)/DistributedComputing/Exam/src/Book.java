import java.io.Serializable;
import java.util.ArrayList;

public class Book implements Serializable {
    public int id;
    public String name;
    public String publisher;
    public int publishingYear;
    public int pagesCount;
    public int price;
    public String bindingType;
    public ArrayList<String> writers;

    public Book(int id, String name, String publisher, int publishingYear, int pagesCount, int price, String bindingType, ArrayList<String> writers){
        this.id = id;
        this.name = name;
        this.publisher = publisher;
        this.publishingYear = publishingYear;
        this.pagesCount = pagesCount;
        this.price = price;
        this.bindingType = bindingType;
        this.writers = writers;
    }

    public Book (String[] sendFormat){
        this.id = Integer.parseInt(sendFormat[1]);
        this.name = sendFormat[2];
        this.publisher = sendFormat[3];
        this.publishingYear = Integer.parseInt(sendFormat[4]);
        this.pagesCount = Integer.parseInt(sendFormat[5]);
        this.price = Integer.parseInt(sendFormat[6]);
        this.bindingType = sendFormat[7];
        this.writers = new ArrayList<>();
        for(int i = 8; i < sendFormat.length; i++){
            this.writers.add(sendFormat[i]);
        }
    }

    public String toSendFormat(String regexSymbol){
        String sendFormat = this.id+regexSymbol+this.name+regexSymbol+this.publisher+regexSymbol+this.publishingYear+regexSymbol+this.pagesCount+regexSymbol+this.price+regexSymbol+this.bindingType;
        for(int i = 0; i < writers.size(); i++){
            sendFormat += regexSymbol + writers.get(i);
        }
        return sendFormat;
    }

    @Override
    public String toString() {
        String res = this.id+" "+this.name+" "+this.publisher+" "+this.publishingYear+" "+this.pagesCount+" "+this.price+" "+this.bindingType;
        for(int i = 0; i < writers.size(); i++){
            res += " " + writers.get(i);
        }
        return res;
    }
}
