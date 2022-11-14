import java.util.concurrent.LinkedBlockingQueue;

public class ThreadPool {
    private int threadsCount;
    private Worker[] workers;
    private LinkedBlockingQueue linkedBlockingQueue;

    public ThreadPool(int threadsCount) {
        this.threadsCount = threadsCount;
        linkedBlockingQueue = new LinkedBlockingQueue();
        workers = new Worker[threadsCount];

        for (int i = 0; i < threadsCount; i++) {
            workers[i] = new Worker(linkedBlockingQueue);
            workers[i].start();
        }
    }

    public void ExecuteTask(Runnable task) {
        synchronized (linkedBlockingQueue) {
            linkedBlockingQueue.add(task);
            linkedBlockingQueue.notify();
        }
    }
}