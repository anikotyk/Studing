import java.util.LinkedList;
import java.util.Queue;
import java.util.Random;
import java.util.concurrent.TimeUnit;

public class TaskB {
    static int countIterations = 3;
    static int maxCountClients = 3;

    public static void main(String args[]){
        MainB mainB = new MainB();

        new HairCuter(mainB);
        Random rand = new Random();

        for(int i = 0; i < countIterations; i++){
            int countClients = rand.nextInt((maxCountClients - 1) + 1) + 1;;
            for(int j = 0; j < countClients; j++){
                new Client(mainB);
            }

            try{
                TimeUnit.SECONDS.sleep(2);
            } catch (Exception e){

            }
        }

    }
}

class MainB{

    Queue<Client> queue = new LinkedList<>();
    Client currentClient = null;
    synchronized void MakeHaircut(){
        if(this.queue.isEmpty() && this.currentClient==null){
            System.out.println("Haircuter go sleep");
            try {
                wait();
            } catch(InterruptedException e) {
                System.out.println("InterruptedException caught");
            }
        }
        else if(this.currentClient==null){
            notify();
        }
        else if(this.currentClient!=null){
            System.out.println("Made haircut");
            this.currentClient.interrupt();
            this.currentClient = null;
        }
    }

    synchronized void ComeForHaircut(Client client){
        this.queue.add(client);
        System.out.println("New client come, count "+queue.size());
        if(this.queue.size()>1 || this.currentClient!=null){
            try {
                wait();
            } catch(Exception e) {

            }
        }
    }

    synchronized void SitInChair(Client client){
        if(this.currentClient!=null || !queue.peek().equals(client)){
            try {
                wait();
            } catch(Exception e) {

            }
        }else{
            System.out.println("Client sit");
            queue.remove(client);
            this.currentClient = client;
            notify();

            try {
                wait();
            } catch(Exception e) {

            }
        }

    }
}

class HairCuter implements Runnable{
    MainB mainB;

    HairCuter(MainB mainB) {
        this.mainB = mainB;
        new Thread(this, "HairCuter").start();
    }

    public void run() {
        while(true) {
            this.mainB.MakeHaircut();
        }
    }
}

class Client implements Runnable{
    MainB mainB;
    Thread th;
    Client(MainB mainB) {
        this.mainB = mainB;
        th = new Thread(this, "Client");
        th.start();
    }

    public void run() {
        if(!th.isInterrupted()){
            this.mainB.ComeForHaircut(this);
            this.mainB.SitInChair(this);
        }
    }

    public void interrupt(){
        System.out.println("Client exit");
        th.interrupt();
    }

}