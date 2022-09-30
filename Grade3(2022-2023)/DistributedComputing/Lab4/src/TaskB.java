import java.io.FileWriter;
import java.io.IOException;
import java.util.concurrent.ThreadLocalRandom;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class TaskB {
    public static void main(String[] args) {
        MainB mainB = new MainB();
        new Nature(mainB);
        new Gardener(mainB);
        new Monitor1(mainB);
        new Monitor2(mainB);
    }
}

class MainB {
    public int gardenSize = 5;
    Plant[][] garden;
    private ReadWriteLock readWriteLock;

    public MainB(){
        readWriteLock = new ReentrantReadWriteLock();

        garden = new Plant[gardenSize][gardenSize];
        for(int i = 0; i < gardenSize; i++){
            for(int j = 0; j < gardenSize; j++){
                garden[i][j] = new Plant();
            }
        }
    }

    public void MakePlantWithered(int indexX, int indexY){
        readWriteLock.writeLock().lock();
        garden[indexX][indexY].state = State.Withered;
        readWriteLock.writeLock().unlock();
    }

    public void MakeFirstPlantNormal(){
        readWriteLock.writeLock().lock();

        boolean flag = false;
        for(int i = 0; i < gardenSize; i++){
            for(int j = 0; j < gardenSize; j++){
                if(garden[i][j].state.equals(State.Withered)){
                    garden[i][j].state = State.Normal;
                    flag = true;
                    break;
                }
            }
            if(flag){
                break;
            }
        }
        readWriteLock.writeLock().unlock();
    }

    public void PrintGardenState(){
        readWriteLock.readLock().lock();
        for(int i = 0; i < gardenSize; i++){
            for(int j = 0; j < gardenSize; j++){
                System.out.print(garden[i][j].state.toString()+" ");
            }
            System.out.println("");
        }
        System.out.println("");
        readWriteLock.readLock().unlock();
    }

    public void PrintGardenStateToFile(){
        readWriteLock.readLock().lock();
        String data = "";
        for(int i = 0; i < gardenSize; i++){
            for(int j = 0; j < gardenSize; j++){
                data += garden[i][j].state.toString()+" ";
            }
            data += "\r\n";
        }
        data += "\r\n";
        try(FileWriter fileWriter = new FileWriter("data.txt", true);) {
            fileWriter.write(data);
            fileWriter.flush();
        } catch (IOException e) {
            readWriteLock.readLock().unlock();
            throw new RuntimeException(e);
        }
        readWriteLock.readLock().unlock();
    }
}

class Plant {
    public State state;

    public Plant(){
        this.state = State.Normal;
    }
}

enum State{
    Normal,
    Withered
}

class Nature implements Runnable{
    MainB mainB;

    public Nature(MainB mainB){
        this.mainB = mainB;

        new Thread(this, "Nature").start();
    }

    @Override
    public void run() {
        while(true){
            int indexX = ThreadLocalRandom.current().nextInt(0, mainB.gardenSize);
            int indexY = ThreadLocalRandom.current().nextInt(0, mainB.gardenSize);
            mainB.MakePlantWithered(indexX, indexY);
        }
    }
}

class Gardener implements Runnable{
    MainB mainB;

    public Gardener(MainB mainB){
        this.mainB = mainB;

        new Thread(this, "Gardener").start();
    }

    @Override
    public void run() {
        while(true){
            mainB.MakeFirstPlantNormal();
        }
    }
}

class Monitor1 implements Runnable{
    MainB mainB;

    public Monitor1(MainB mainB){
        this.mainB = mainB;

        new Thread(this, "Monitor1").start();
    }

    @Override
    public void run() {
        while(true){
            mainB.PrintGardenStateToFile();
        }
    }
}

class Monitor2 implements Runnable{
    MainB mainB;

    public Monitor2(MainB mainB){
        this.mainB = mainB;

        new Thread(this, "Monitor2").start();
    }

    @Override
    public void run() {
        while(true){
            mainB.PrintGardenState();
        }
    }
}