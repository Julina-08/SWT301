package homework;

import java.util.logging.Logger;

// Interface chuẩn tên hóa
interface LoginHandler {
    boolean login(String username, String password);
}

// Class implement interface
class SimpleLogin implements LoginHandler {


    @Override
    public boolean login(String username, String password) {
        return "admin".equals(username) && "123".equals(password);
    }
}

// Class chính sử dụng LoginHandler
public class InterfaceNamingInconsistencyExample {
    private static final Logger logger = Logger.getLogger(InterfaceNamingInconsistencyExample.class.getName());

    public static void main(String[] args) {
        LoginHandler handler = new SimpleLogin();

        boolean success = handler.login("admin", "123");

        if (success) {
            logger.info("Login successful");
        } else {
            logger.info("Login failed");
        }
    }
}
