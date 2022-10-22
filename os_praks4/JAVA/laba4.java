public class Main {

    public static void main(String[] args) {


        int numIterrations = 100000000;
        long m;
        for (int j = 0; j < 5; j++) {
            int a = 0, b = 3, c = 3;
            m = System.currentTimeMillis();
            for (int i = 0; i < numIterrations; i++) {
                a += b * 2 + c - i;
            }
            System.out.println((double) (System.currentTimeMillis() - m) * 1.000 + " ms");
        }
    }
}
