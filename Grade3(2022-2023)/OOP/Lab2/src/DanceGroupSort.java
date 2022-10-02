import java.util.Comparator;

public class DanceGroupSort implements Comparator<DanceGroup> {
    public int compare(DanceGroup a, DanceGroup b)
    {
        return a.number - b.number;
    }
}