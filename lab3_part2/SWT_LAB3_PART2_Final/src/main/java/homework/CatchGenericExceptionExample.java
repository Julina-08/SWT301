package homework;

import java.util.logging.Logger;

public class CatchGenericExceptionExample {
    private static final Logger logger = Logger.getLogger(CatchGenericExceptionExample.class.getName());

    public static void main(String[] args) {
        String s = getNullableString(); // nhận dữ liệu có thể null

        try {
            if (s != null) {
                logger.info("Length: " + s.length());
            } else {
                logger.warning("String is null, cannot call length()");
            }
        } catch (NullPointerException e) {
            logger.severe("Unexpected NullPointerException: " + e.getMessage());
        }
    }

    private static String getNullableString() {
        // Mô phỏng dữ liệu có thể null, ví dụ từ DB hoặc input
        return Math.random() > 0.5 ? "hello" : null;
    }
}
