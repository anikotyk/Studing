import java.io.Serializable;

public class Mark implements Serializable {
    public String name;

    public Mark(String name){
        this.name = name;
    }

    @Override
    public String toString(){
        return this.name;
    }
}
