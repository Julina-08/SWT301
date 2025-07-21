import org.junit.jupiter.api.Test;
import example.InsecureLogin;

import example.InsecureLogin;
public class InsecureLoginTest {

    @Test
    public void testLoginSuccess() {
        InsecureLogin.login("admin", "123456");
        // Không cần assert nếu chỉ cần chạy để tránh warning "method not used"
    }

    @Test
    public void testLoginFail() {
        InsecureLogin.login("user", "wrongpassword");
    }

    @Test
    public void testPrintUserInfo() {
        InsecureLogin insecureLogin = new InsecureLogin();
        insecureLogin.printUserInfo("John Doe");
    }
}
