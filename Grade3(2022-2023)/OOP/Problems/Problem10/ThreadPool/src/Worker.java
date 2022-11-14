import java.util.concurrent.LinkedBlockingQueue;

public class Worker extends Thread {
    private LinkedBlockingQueue queue;

    public Worker(LinkedBlockingQueue linkedBlockingQueue){
        this.queue = linkedBlockingQueue;
    }

    @Override
    public void run() {
        Task task;

        while (true) {
            synchronized (queue) {
                while (queue.isEmpty()) {
                    try {
                        queue.wait();
                    } catch (InterruptedException e) {
                        System.out.println("Error waiting queue: " + e.getMessage());
                    }
                }
                task = (Task) queue.poll();
            }

            try {
                task.run();
            } catch (RuntimeException e) {
                System.out.println("Error run task: " + e.getMessage());
            }
        }
    }
}
