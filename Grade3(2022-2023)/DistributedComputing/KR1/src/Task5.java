import java.util.concurrent.Semaphore;

public class Task5 {
    public static void main(String[] args) {
        int countBusStations = 3;
        int maxCountBusesOnStation = 2;
        int busesCount = 10;

        BusStation[] busStations = new BusStation[countBusStations];
        for(int i = 0; i < countBusStations; i++){
            busStations[i] = new BusStation(maxCountBusesOnStation);
        }

        for(int i = 0; i < busesCount; i++){
            new Bus(busStations, i);
        }
    }
}

class BusStation{
    int maxCountBuses;
    Semaphore semaphore;

    public BusStation(int maxCountBuses){
        this.maxCountBuses = maxCountBuses;
        semaphore = new Semaphore(maxCountBuses);
    }

    public void BusEnter() throws InterruptedException {
        semaphore.acquire();
    }

    public void BusExit() {
        semaphore.release();
    }
}

class Bus implements Runnable{
    BusStation[] busStations;
    int indexCurrentBusStation;
    Thread th;
    public int number;

    public Bus(BusStation[] busStations, int number){
        this.busStations = busStations;
        this.number = number;
        this.indexCurrentBusStation = -1;

        th = new Thread(this, "Bus"+number);
        th.start();
    }

    @Override
    public void run() {
        while (true){
            indexCurrentBusStation++;

            if(indexCurrentBusStation >= busStations.length){
                indexCurrentBusStation = 0;
            }

            try {
                busStations[indexCurrentBusStation].BusEnter();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }

            System.out.println("Bus "+number+" is on station "+indexCurrentBusStation);

            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }

            System.out.println("Bus "+number+" exits station "+indexCurrentBusStation);

            busStations[indexCurrentBusStation].BusExit();
        }
    }
}