import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvFileSource;
import Lab_2_SWT301.AccountService;
import static org.junit.jupiter.api.Assertions.*;

public class AccountServiceTest {
    AccountService accountService = new AccountService();

    @ParameterizedTest
    @CsvFileSource(resources = "/test-data.csv", numLinesToSkip = 1)
    void testRegisterAccount(String username, String password, String email, boolean expected) {
        boolean result = accountService.registerAccount(username, password, email);
        assertEquals(expected, result);
    }

    @Test
    void testValidPassword() {
        assertEquals(true, accountService.isValidPassword("password"));
    }
}
