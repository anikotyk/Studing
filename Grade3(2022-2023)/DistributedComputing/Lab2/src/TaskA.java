class TaskA {
    static int beesCount = 10;

    public static void main(String args[]) {
        MainA mainA = new MainA();
        for(int i = 0; i < beesCount; i++){
            new Bee(mainA);
        }
    }
}
class MainA {
    boolean isBearFound = false;
    int arr[][];
    int size = 100;

    int x = 0;
    MainA() {
        createArray();
    }

    void createArray(){
        this.arr = new int[this.size][this.size];
        int xBear = (int) (Math.random() * this.size);
        int yBear = (int) (Math.random() * this.size);
        this.arr[xBear][yBear] = 1;
    }

    synchronized int getTargetXCoordForBee(){
        if( this.isBearFound ){
            return -1;
        }

        int res = this.x;
        this.x++;
        if(res>=this.size){
            res = -1;
        }
        return res;
    }

    synchronized void bearFound(){
        this.isBearFound = true;
        System.out.println("Found bear");
    }

    void searchBear(int x, int y) {
        if(this.arr[x][y]==1){
            this.bearFound();
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
        int x = 0;
        int y = 0;

        x = mainA.getTargetXCoordForBee();
        while(true) {
            if(y>=mainA.size){
                x = mainA.getTargetXCoordForBee();
                y = 0;
                if(x==-1){
                    break;
                }
            }

            if(x!=-1){
                mainA.searchBear(x, y);
                y++;
            }else{
                break;
            }
        }
    }
}