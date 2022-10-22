import java.util.ArrayList;
import java.util.Scanner;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Main {
    public static String Gethash(){
        Scanner in = new Scanner(System.in);
        System.out.print("Введите хэш: ");
        String hash= in.next();
        return (hash);
    }
    public static String hash1=Main.Gethash();
    static ExecutorService threadsPool = Executors.newCachedThreadPool();

    static ArrayList<Callable<String>> tasks = new ArrayList<>();

    public static void main(String[] args) throws ExecutionException, InterruptedException {
        Scanner in = new Scanner(System.in);

        System.out.print("Введите кол-во потоков:");

        int n = in.nextInt();

        System.out.printf("кол-во потоков: %d \n", n);

        in.close();

        for (int i=0;i<n;i++){
            tasks.add(new MyFirstThread("Поток №"+i));
            //threadsPool.execute(new MyFirstThread("Поток №"+i));
        }
        String result = threadsPool.invokeAny(tasks);
        System.out.println("проверка завершения потока:"+threadsPool.isShutdown());
        threadsPool.shutdown();

        System.out.println(result);

        System.out.println("проверка завершения потока:"+threadsPool.isShutdown());
        System.exit(1);
    }

}
