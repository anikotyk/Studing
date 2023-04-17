public class Task implements Runnable {
    int id;
    int timeMs;

    public Task(int id, int timeMs){
        this.id = id;
        this.timeMs = timeMs;
    }

    @Override
    public void run() {
        try {
            Thread.sleep(timeMs);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }

        System.out.println("Task "+id+" is finished");
    }
}
