public class Main {
    public static void main(String[] args) throws InterruptedException {
        boolean isSocket = true;

        if(isSocket){
            ServerSocketTask5 server = new ServerSocketTask5();
            Thread.sleep(1000);
            ClientSocketTask5 client = new ClientSocketTask5();
            Thread.sleep(1000);
            ClientSocketTask5 client2 = new ClientSocketTask5();
        }else{
            ServerRmiTask5 server = new ServerRmiTask5();
            Thread.sleep(1000);
            ClientRmiTask5 client = new ClientRmiTask5();
            Thread.sleep(1000);
            ClientRmiTask5 client2 = new ClientRmiTask5();
        }
    }
}