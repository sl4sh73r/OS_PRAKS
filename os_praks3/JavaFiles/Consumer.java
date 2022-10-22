import java.util.Queue;
import java.util.Random;
public class Consumer implements Runnable {

    private Queue<?> queue;
    //  private SomeUtil someUtil;

    public Consumer(Queue<?> queue) {
        this.queue = queue;
        //      someUtil = new SomeUtil();
    }

    @Override
    public void run() {
        try {
            Random rand = new Random();
            while (queue.size() > 0 || !Main.key.equals("q")) {

                if (queue.size() <= 0) {
                    Thread.sleep(rand.nextInt(1000));
                } else {
                    System.out.println(Thread.currentThread().getName() + " consumed: " + queue.poll());
                }
                System.out.println("Queue elements size is: " + queue.size());
                Thread.sleep(rand.nextInt(1000));
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        System.out.println("--------------------------Ending " + Thread.currentThread().getName());
    }
}
