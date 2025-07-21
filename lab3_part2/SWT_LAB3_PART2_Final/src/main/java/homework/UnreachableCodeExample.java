package homework;

import java.util.logging.Logger;

public class UnreachableCodeExample {
    private static final Logger logger = Logger.getLogger(UnreachableCodeExample.class.getName());

    public static int getNumber() {
        // Nếu cần log trước khi return
        logger.info("Preparing to return number");
        return 42;
    }

    public static void main(String[] args) {
        logger.info("Returned number: " + getNumber());
    }
}
