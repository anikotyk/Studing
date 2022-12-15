import java.util.ArrayList;

public class CarProducer {
    public String name;
    public ArrayList<Mark> marks;

    public CarProducer(String name, ArrayList<Mark> marks){
        this.name = name;
        this.marks = marks;
    }

    public CarProducer(String[] data){
        this.name = data[1];
        ArrayList<Mark> marks = new ArrayList<Mark>();
        for(int i = 2; i < data.length; i++){
            marks.add(new Mark(data[i]));
        }
        this.marks = marks;
    }

    @Override
    public String toString(){
        String marks = "";
        for (int i = 0; i < this.marks.size(); i++){
            marks += this.marks.get(i)+" ";
        }
        return this.name + ": "+ marks;
    }

    public String toSendFormat(){
        String marks = "";
        for (int i = 0; i < this.marks.size(); i++){
            marks += this.marks.get(i)+"#";
        }
        return this.name + "#"+ marks;
    }

    public Mark FindMarkByName(String name){
        for (int i = 0; i < this.marks.size(); i++){
            if(marks.get(i).name.equals(name)){
                return marks.get(i);
            }
        }

        return null;
    }
}
