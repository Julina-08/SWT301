package homework;

import java.util.logging.Logger;

public class OvercatchingExceptionExample {
    private static final Logger logger = Logger.getLogger(OvercatchingExceptionExample.class.getName());

    public static void main(String[] args) {
        try {
            int[] arr = new int[5];
            // Gán giá trị để tránh cảnh báo "never written to"
            for (int i = 0; i < arr.length; i++) {
                arr[i] = i * 10;
            }

            logger.info("Accessing element at index 10...");
            logger.info("Value: " + arr[2]); // ví dụ với chỉ số hợp lệ
        } catch (ArrayIndexOutOfBoundsException e) {
            logger.severe("Array index is out of bounds: " + e.getMessage());
        }
    }
}
