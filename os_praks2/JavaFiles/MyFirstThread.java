import java.security.NoSuchAlgorithmException;
import java.time.Instant;
import java.time.LocalDateTime;
import java.util.concurrent.Callable;

public class MyFirstThread implements Callable<String> {

        String name;
        MyFirstThread(String name)
        {
            this.name = name;
        }
    @Override
    public String call() {
            long startTime = System.currentTimeMillis();

            int count = 5;

            String word = "";

            String hash = "";

       while (!PasswordSha256.indent(hash)){
           word = new WordsGenerator().getAlphaNumericString(count);

           try {
               hash = PasswordSha256.sha256(word);
           } catch (NoSuchAlgorithmException e) {
               throw new RuntimeException(e);
           }

       }
            long timeSpent = System.currentTimeMillis() - startTime;
            return "Выполнен: " + this.name
                +" | hash слова "+ word +
                " из 5 букв: "+ hash+"программа выполнялась " + timeSpent + " миллисекунд";

    }
}
