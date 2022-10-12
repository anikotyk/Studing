import java.util.concurrent.BrokenBarrierException;
import java.util.concurrent.CyclicBarrier;
import java.util.stream.IntStream;

public class TaskA {
    public static void main(String[] args) {
        MainA mainA = new MainA();
    }
}

class MainA {
    private int recruitsCount = 100;
    public int[] recruits;

    public MainA(){
        recruits = new int[recruitsCount];

        int recruitArraysCount = 10;

        int direction = (int)Math.floor(Math.random()*(1-0+1)+0);
        for (int i = 0; i < recruitsCount; i++){
            recruits[i] = (int)Math.floor(Math.random()*(2));
        }
        recruits[0] = direction;
        recruits[recruitsCount-1] = direction;

        RecruitsArray[] recruitArrays = new RecruitsArray[recruitArraysCount];

        CyclicBarrier cyclicBarrier = new CyclicBarrier(recruitArraysCount, new RecruitsDirectionChecker(this, direction, recruitArrays));

        for(int i = 0; i < recruitArraysCount; i++){
            int finalI = i;
            int[] currentRecruitsIndexes = IntStream.range(i, recruitsCount-1).filter(index -> (index - finalI) % recruitArraysCount == 0).map(index -> index).toArray();
            recruitArrays[i] = new RecruitsArray(this, cyclicBarrier, currentRecruitsIndexes);
        }
    }

    public void SetRecruitDirection(int index, int direction){
        recruits[index] = direction;
    }
}

class RecruitsArray implements Runnable{

    private Thread th;
    private CyclicBarrier cyclicBarrier;

    private int[] currentRecruitsIndexes;

    private MainA mainA;

    public RecruitsArray(MainA mainA, CyclicBarrier cyclicBarrier, int[] currentRecruitsIndexes){
        this.mainA = mainA;
        this.cyclicBarrier = cyclicBarrier;
        this.currentRecruitsIndexes = currentRecruitsIndexes;

        th = new Thread(this, "RecruitsArray");
        th.start();
    }
    private void RotateRecruits(){
        int[] recruits = mainA.recruits;
        for(int i = 0; i < currentRecruitsIndexes.length; i++){
            int index = currentRecruitsIndexes[i];
            int direction = (int)Math.floor(Math.random()*(2));

            if(direction==0 && index>0){
                if(recruits[index-1]==1){
                    direction = 1;
                }
            }else if(direction==1 && index<recruits.length-1){
                if(recruits[index+1]==0){
                    direction = 0;
                }
            }

            mainA.SetRecruitDirection(index, direction);
        }
    }

    public void interrupt(){
        th.interrupt();
    }

    @Override
    public void run() {
        while(!th.isInterrupted()){
            RotateRecruits();
            try {
                cyclicBarrier.await();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            } catch (BrokenBarrierException e) {
                throw new RuntimeException(e);
            }
        }
    }
}

class RecruitsDirectionChecker implements Runnable{
    private int direction;
    private RecruitsArray[] recruitArrays;
    private MainA mainA;

    public RecruitsDirectionChecker(MainA mainA, int direction, RecruitsArray[] recruitArrays){
        this.mainA = mainA;
        this.direction = direction;
        this.recruitArrays = recruitArrays;
    }

    private boolean CheckDirection(){
        for (int i = 0; i < mainA.recruits.length; i++){
            if(mainA.recruits[i]!=direction){
                return false;
            }
        }
        return true;
    }

    private void StopRecruitArrays(){
        for (int i = 0; i < recruitArrays.length; i++){
            recruitArrays[i].interrupt();
        }
    }

    private void PrintRecruits(){
        for (int i = 0; i < mainA.recruits.length; i++){
            System.out.print(mainA.recruits[i]+ " ");
        }
        System.out.println("");
    }


    @Override
    public void run() {
        PrintRecruits();
        if(CheckDirection()){
            StopRecruitArrays();
            System.out.println("STOP RECRUITS. Direction is correct");
        }else{
            System.out.println("Continue rotating\n\n");
        }
    }
}