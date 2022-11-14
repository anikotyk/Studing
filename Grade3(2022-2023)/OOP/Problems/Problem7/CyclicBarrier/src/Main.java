public class Main {
    public static void main(String[] args) {
        int threadsCount = 3;
        FinalTask finalTask = new FinalTask();
        CyclicBarrier cyclicBarrier = new CyclicBarrier(threadsCount, finalTask);

        for(int i = 0; i < threadsCount; i++) {
            Task task = new Task(i, cyclicBarrier);
        }
    }
}