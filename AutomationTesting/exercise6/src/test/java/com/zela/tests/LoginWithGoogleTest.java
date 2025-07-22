package com.zela.tests;

import com.zela.pages.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class LoginWithGoogleTest extends BaseTest {
    private final String baseUrl = "http://localhost:5160"; // chỉnh lại nếu khác
    private final String googleEmail = "4clone201@gmail.com";
    private final String googlePassword = "4clonene@201";

    @Test
    public void testLoginWithGoogle() {
        LoginPage loginPage = new LoginPage(driver);
        loginPage.open(baseUrl);
        loginPage.clickGoogleLogin();

        GoogleLoginPage googleLoginPage = new GoogleLoginPage(driver);
        googleLoginPage.loginWithGoogle(googleEmail, googlePassword);

        DashboardPage dashboardPage = new DashboardPage(driver);
        assertTrue(dashboardPage.isLoaded(), "Dashboard should be loaded after login");
        assertTrue(dashboardPage.getPageTitle().contains("Zela") || dashboardPage.getUserName().length() > 0);
    }
}