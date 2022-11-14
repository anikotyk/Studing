public class Main {
    public static void main(String[] args) {
        ReentrantLock reentrantLock = new ReentrantLock();
        Task task1 = new Task(1, reentrantLock);
        Task task2 = new Task(2, reentrantLock);
        Task task3 = new Task(3, reentrantLock);
    }
}