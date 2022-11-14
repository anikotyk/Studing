public class Task implements Runnable{
    int id;
    Phaser phaser;
    public Task(int id, Phaser phaser) {
        this.phaser = phaser;
        this.id = id;
        new Thread(this, "Task "+id).start();
    }

    public void run(){
        phaser.register();

        System.out.println("Task "+id+" is arrive");
        phaser.arriveAndAwaitAdvance();

        try {
            Thread.sleep(3000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        System.out.println("Task "+id+" is arrive");
        phaser.arriveAndAwaitAdvance();

        try {
            Thread.sleep(3000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        phaser.arriveAndDeregister();
    }
}