import java.util.concurrent.BrokenBarrierException;
import java.util.concurrent.CyclicBarrier;

public class TaskB {
    public static void main(String[] args) {
        StringChanger[] stringChangers = new StringChanger[4];
        CyclicBarrier cyclicBarrier = new CyclicBarrier(stringChangers.length, new CheckerForStop(stringChangers));

        stringChangers[0] = new StringChanger("ABCD", cyclicBarrier);
        stringChangers[1] = new StringChanger("CDCD", cyclicBarrier);
        stringChangers[2] = new StringChanger("ABAD", cyclicBarrier);
        stringChangers[3] = new StringChanger("AABB", cyclicBarrier);
    }
}

class StringChanger implements  Runnable{
    private String string;
    private Thread th;

    private CyclicBarrier cyclicBarrier;

    public StringChanger(String str, CyclicBarrier cyclicBarrier){
        this.string = str;
        this.cyclicBarrier = cyclicBarrier;

        th = new Thread(this, "StringChanger");
        th.start();
    }

    @Override
    public void run() {
        while(!th.isInterrupted()){
            ChangeString();
            System.out.println(string);
            try {
                cyclicBarrier.await();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            } catch (BrokenBarrierException e) {
                throw new RuntimeException(e);
            }
        }
    }

    private void ChangeString(){
        int index = (int) ((Math.random() * (string.length() - 0)) + 0);
        if(string.charAt(index) == 'A'){
            string = string.substring(0,index)+'C'+string.substring(index+1);
        }else if(string.charAt(index) == 'C'){
            string = string.substring(0,index)+'A'+string.substring(index+1);
        }else if(string.charAt(index) == 'B'){
            string = string.substring(0,index)+'D'+string.substring(index+1);
        }else if(string.charAt(index) == 'D'){
            string = string.substring(0,index)+'B'+string.substring(index+1);
        }
    }

    public void interrupt(){
        th.interrupt();
    }

    public int GetCountOfChar(Character character){
        int cnt = 0;

        for(int i = 0; i < string.length(); i++){
            if(string.charAt(i) == character){
                cnt++;
            }
        }

        return cnt;
    }
}

class CheckerForStop implements Runnable{
    private StringChanger[] stringChangers;

    public CheckerForStop(StringChanger[] stringChangers){
        this.stringChangers = stringChangers;
    }

    @Override
    public void run() {
        if(CheckForStop()){
            StopStringChangers();
            System.out.println("STOP THREADS");
        }else{
            System.out.println("Continue working with strings\n\n");
        }
    }

    private boolean CheckForStop(){
        int sumA = 0;
        int sumB = 0;

        for(int i = 0; i < stringChangers.length; i++){
            sumA += stringChangers[i].GetCountOfChar('A');
            sumB += stringChangers[i].GetCountOfChar('B');
        }

        if(sumA == sumB){
            return true;
        }

        for(int i = 0; i < stringChangers.length; i++){
            if(sumA - stringChangers[i].GetCountOfChar('A') == sumB - stringChangers[i].GetCountOfChar('B')) {
                return true;
            }
        }

        return false;
    }

    private void StopStringChangers(){
        for (int i = 0; i < stringChangers.length; i++){
            stringChangers[i].interrupt();
        }
    }
}