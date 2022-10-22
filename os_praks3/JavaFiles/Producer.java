import java.util.Queue;
import java.util.Random;

public class Producer implements Runnable{

    private Queue<Integer> queue;

    public Producer(Queue<Integer> queue) {
        this.queue = queue;
    }

    @Override
    public void run() {
        Random rand = new Random();
        while(!Main.key.equals("q")) {
            System.out.println("Current key is "+Main.key);
            try {
                Thread.sleep(rand.nextInt(1000));
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            if(queue.size() <=80){
                System.out.println("Producer thread "+Thread.currentThread().getName()+" produced number");
                queue.add(rand.nextInt(100));
            }
            System.out.println("Queue elements size is: " + queue.size());
        }
        System.out.println("-------------------------Ending " + Thread.currentThread().getName());
    }
}