package com.zela.tests;

import com.zela.pages.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class ProfileUpdateTest extends BaseTest {
    private final String baseUrl = "http://localhost:5160";
    private final String googleEmail = "4clone201@gmail.com";
    private final String googlePassword = "4clonene@201";

    @Test
    public void testUpdateFullName() {
        // Ghi chú: Cần login trước khi vào trang profile
        LoginPage loginPage = new LoginPage(driver);
        loginPage.open(baseUrl);
        loginPage.clickGoogleLogin();

        GoogleLoginPage googleLoginPage = new GoogleLoginPage(driver);
        googleLoginPage.loginWithGoogle(googleEmail, googlePassword);

        DashboardPage dashboardPage = new DashboardPage(driver);
        assertTrue(dashboardPage.isLoaded());

        // Sau khi login, vào trang profile
        ProfilePage profilePage = new ProfilePage(driver);
        profilePage.open(baseUrl);

        String newName = "Test User " + System.currentTimeMillis();
        profilePage.updateFullName(newName);

        assertTrue(profilePage.isUpdateSuccess(), "Profile update should show success message");
    }
}