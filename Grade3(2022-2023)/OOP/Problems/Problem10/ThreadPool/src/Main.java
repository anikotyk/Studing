public class Main {
    public static void main(String[] args) throws InterruptedException {
        int tasksCount = 10;
        int threadsCount = 3;

        ThreadPool threadPool = new ThreadPool(threadsCount);

        for (int i = 0; i < tasksCount; i++) {
            Task task = new Task(i, 1000);
            threadPool.ExecuteTask(task);
        }

        threadPool.CloseTaskQueue();
    }
}