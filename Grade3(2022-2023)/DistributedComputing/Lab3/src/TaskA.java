public class TaskA {
    static int beesCount = 5;

    public static void main(String args[]) {
        MainA mainA = new MainA();

        new Bear(mainA);
        for(int i = 0; i < beesCount; i++){
            new Bee(mainA);
        }
    }
}
class MainA{
    int maxN = 10;
    int currentN = 0;
    synchronized void EatHoney() {
        if(this.currentN < this.maxN){
            try {
                wait();
            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }
        }
        this.currentN = 0;
        System.out.println("Bear eat honey");
        notifyAll();
    }

    synchronized void AddHoney(){
        while(currentN>=maxN){
            try {
                wait();
            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }
        }

        this.currentN++;
        System.out.println("Honey "+ this.currentN);
        if(currentN==maxN){
            notify();
        }
    }
}

class Bee implements Runnable {
    MainA mainA;

    Bee(MainA mainA) {
        this.mainA = mainA;
        new Thread(this, "Bee").start();
    }

    public void run() {
        while(true) {
            this.mainA.AddHoney();
        }
    }
}

class Bear implements Runnable {
    MainA mainA;

    Bear(MainA mainA) {
        this.mainA = mainA;
        new Thread(this, "Bear").start();
    }

    public void run() {
        while(true) {
            this.mainA.EatHoney();
        }
    }
}