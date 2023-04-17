public class Task implements Runnable{
    int id;
    ReentrantLock reentrantLock;
    public Task(int id, ReentrantLock reentrantLock) {
        this.reentrantLock = reentrantLock;
        this.id = id;
        new Thread(this, "Task "+id).start();
    }

    public void run(){
        System.out.println("Task "+id+" is started");
        reentrantLock.lock();
        System.out.println("Task "+id+" is locked");

        try {
            Thread.sleep(4000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        reentrantLock.unlock();

        System.out.println("Task "+id+" is unlocked");
    }
}