public class Main {
    public static void main(String[] args) {
        int tasksCount = 3;
        Phaser phaser = new Phaser();

        for (int i = 0; i < tasksCount; i++){
            Task task = new Task(i, phaser);
        }
    }
}