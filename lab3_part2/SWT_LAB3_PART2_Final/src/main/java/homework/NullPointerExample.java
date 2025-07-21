package homework;

import java.util.logging.Logger;

public class NullPointerExample {
    private static final Logger logger = Logger.getLogger(NullPointerExample.class.getName());

    public static void main(String[] args) {
        String text = getNullableText(); // mô phỏng đầu vào có thể null

        if (text != null && !text.isEmpty()) {
            logger.info("Text is not empty");
        } else if (text == null) {
            logger.warning("Text is null");
        } else {
            logger.info("Text is empty");
        }
    }

    private static String getNullableText() {
        // Mô phỏng nguồn dữ liệu: đôi khi null, đôi khi có chuỗi
        return Math.random() > 0.5 ? "hello" : null;
    }
}
