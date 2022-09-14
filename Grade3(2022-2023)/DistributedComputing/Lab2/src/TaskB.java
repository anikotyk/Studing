class TaskB {
    public static void main(String args[]) {
        MainB mainB = new MainB();
        new Ivanov_Producer(mainB);
        new Petrov_Consumer(mainB);
        new Nechiporchuk_Consumer(mainB);
    }
}
class MainB {
    int putedCount = 0;
    int loadedCount = 0;
    int calculatedCount = 0;

    int priceOneItem = 10;

    synchronized void loadOne() {
        while(this.putedCount <= 0)
            try {
                wait();

            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }

        System.out.println("Load");

        this.loadedCount++;
        this.putedCount--;

        notify();
    }

    synchronized void calculatePrice() {
        while(this.calculatedCount == this.loadedCount)
            try {
                wait();

            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }

        System.out.println("Price amount: " + (this.loadedCount*priceOneItem));
        this.calculatedCount = this.loadedCount;

        notify();
    }

    synchronized void putOne() {
        while(this.putedCount > 0)
            try {
                wait();
            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }

        this.putedCount++;

        System.out.println("Put");
        notify();
    }
}
class Ivanov_Producer implements Runnable {
    MainB mainB;

    Ivanov_Producer(MainB mainB) {
        this.mainB = mainB;
        new Thread(this, "Ivanov_Producer").start();
    }

    public void run() {
        while(true) {
            mainB.putOne();
        }
    }
}

class Petrov_Consumer implements Runnable {
    MainB mainB;

    Petrov_Consumer(MainB mainB) {
        this.mainB = mainB;
        new Thread(this, "Petrov_Consumer").start();
    }

    public void run() {
        while(true) {
            mainB.loadOne();
        }
    }
}

class Nechiporchuk_Consumer implements Runnable {
    MainB mainB;

    Nechiporchuk_Consumer(MainB mainB) {
        this.mainB = mainB;
        new Thread(this, "Nechiporchuk_Consumer").start();
    }

    public void run() {
        while(true) {
            mainB.calculatePrice();
        }
    }
}
