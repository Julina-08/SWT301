package homework;

import java.util.logging.Logger;

// Class chứa hằng số
final class AppConstants {
    public static final int MAX_USERS = 100;

    private AppConstants() {
        // ngăn tạo đối tượng
    }
}

// Class chính sử dụng Logger
public class InterfaceFieldModificationExample {
    private static final Logger logger = Logger.getLogger(InterfaceFieldModificationExample.class.getName());

    public static void main(String[] args) {
        int max = AppConstants.MAX_USERS;
        logger.info("Max allowed users: " + max);
    }
}
