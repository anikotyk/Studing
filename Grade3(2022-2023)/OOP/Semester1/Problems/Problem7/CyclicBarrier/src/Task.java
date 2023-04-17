public class Task implements  Runnable{
    int id;
    CyclicBarrier cyclicBarrier;

    public Task(int id, CyclicBarrier cyclicBarrier){
        this.id = id;
        this.cyclicBarrier = cyclicBarrier;
        new Thread(this, "Task "+id).start();
    }

    @Override
    public void run() {
        System.out.println("Started task "+id);

        try {
            Thread.sleep(2000);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }

        System.out.println("Ended task "+id+" wait for other threads");
        cyclicBarrier.await();
        System.out.println("Finished task "+id);
    }
}
