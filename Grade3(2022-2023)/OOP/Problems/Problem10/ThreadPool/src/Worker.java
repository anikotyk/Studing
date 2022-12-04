import java.util.concurrent.LinkedBlockingQueue;

public class Worker extends Thread {
    private LinkedBlockingQueue queue;

    private boolean isStoped = false;

    public Worker(LinkedBlockingQueue linkedBlockingQueue){
        this.queue = linkedBlockingQueue;
    }

    @Override
    public void run() {
        Runnable task;

        while (true) {
            synchronized (queue) {
                while (queue.isEmpty()) {
                    if(isStoped){
                        return;
                    }
                    try {
                        queue.wait();
                    } catch (InterruptedException e) {
                        System.out.println("Error waiting queue: " + e.getMessage());
                        return;
                    }
                }
                task = (Runnable) queue.poll();
            }


            try {
                task.run();
            } catch (RuntimeException e) {
                System.out.println("Error run task: " + e.getMessage());
            }
        }
    }

    public void StopWaitingTasks(){
        isStoped = true;
    }
}
