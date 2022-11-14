import java.util.ArrayList;

public class Phaser {
    int phase;
    int registeredCount;

    int arrivedCount;

    ArrayList<Long> registeredIds;

    public Phaser(){
        phase = 0;
        registeredCount = 0;
        arrivedCount = 0;
        registeredIds = new ArrayList<Long>();
    }

    public synchronized int getPhase(){
        return phase;
    }

    public synchronized void register(){
        if(!registeredIds.contains(Thread.currentThread().getId())){
            registeredIds.add(Thread.currentThread().getId());
            registeredCount++;
        }
    }

    public synchronized void arriveAndDeregister(){
        if(registeredIds.contains(Thread.currentThread().getId())){
            registeredIds.remove(Thread.currentThread().getId());
            registeredCount--;
            CheckForNextStep();
        }
    }

    public synchronized void arriveAndAwaitAdvance() {
        if(registeredIds.contains(Thread.currentThread().getId())){
            arrivedCount++;
            boolean res = CheckForNextStep();
            if(!res){
                try{
                    wait();
                }catch(Exception e){
                    System.out.println("Error: "+ e);
                }
            }
        }
    }

    private synchronized boolean CheckForNextStep(){
        if(arrivedCount >= registeredCount){
            phase++;
            System.out.println("START PHASE " + phase);
            arrivedCount = 0;
            notifyAll();
            return true;
        }else{
            return false;
        }
    }
}
