import javax.swing.*;
import java.io.IOException;
import java.util.ArrayDeque;
import java.util.Queue;
import java.util.Scanner;

public class Main {

    static Queue<Integer> integerQueue = new ArrayDeque<>();

    static String key = "";

    public static Queue<Integer> getIntegerQueue() {
        return integerQueue;
    }

    public static void main(String[] args) throws InterruptedException {

        Producer producer = new Producer(integerQueue);
        Consumer consumer = new Consumer(integerQueue);

        Thread t1 = new Thread(producer);
        Thread t2 = new Thread(producer);
        Thread t3 = new Thread(producer);
        Thread t4 = new Thread(consumer);
        Thread t5 = new Thread(consumer);

        t1.start();
        t2.start();
        t3.start();
        t4.start();
        t5.start();

        //Thread.sleep(5000);

        Scanner scanner = new Scanner(System.in);

        while (!key.equals("q")) {
            key = scanner.nextLine();

            System.out.println(key);
        }

        System.out.println("Finish!");

    }
}