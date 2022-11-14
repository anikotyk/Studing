public class ReentrantLock {
    int lockCount;
    long currentThreadId;

    public ReentrantLock(){
        lockCount = 0;
    }

    public synchronized void lock() {
        if(lockCount == 0){
            lockCount++;
            currentThreadId = Thread.currentThread().getId();
        }
        else if(lockCount > 0 && currentThreadId == Thread.currentThread().getId()){
            lockCount++;
        }
        else{
            try {
                wait();
                lock();
            } catch (InterruptedException e) {
                System.out.println("Error: " + e);
            }
        }
    }

    public synchronized void unlock() {
        if(lockCount == 0){
            System.out.println("Error, can't unlock, locked count is 0");
        }else{
            lockCount--;

            if(lockCount == 0){
                notify();
            }
        }
    }
}
