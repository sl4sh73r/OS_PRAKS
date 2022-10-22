public class WordsGenerator {
    //public int count=5;

    String getAlphaNumericString(int count)
    {
        String AlphaNumericString = "abcdefghijklmnopqrstuvxyz";

        StringBuilder sb = new StringBuilder(count);

            for (int i = 0; i < count; i++) {
                int index = (int) (AlphaNumericString.length() * Math.random());

                sb.append(AlphaNumericString.charAt(index));
            }

        return sb.toString();
    }
}
