public class CyclicBarrier {
    int threadsCount;
    Runnable finalThread;

    int currentCount;

    public CyclicBarrier(int threadsCount){
        this.threadsCount = threadsCount;
        this.finalThread = null;
        this.currentCount = 0;
    }

    public CyclicBarrier(int threadsCount, Runnable finalThread){
        this.threadsCount = threadsCount;
        this.finalThread = finalThread;
        this.currentCount = 0;
    }

    public synchronized void await(){
        currentCount++;
        boolean res = CheckIsReady();
        if(!res){
            try {
                wait();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }
    }

    private synchronized boolean CheckIsReady(){
        if(currentCount >= threadsCount){
            currentCount = 0;
            notifyAll();
            if(finalThread!=null){
                finalThread.run();
            }
            return true;
        }else{
            return false;
        }
    }
}
