package com.zela.pages;

import org.openqa.selenium.*;
import org.openqa.selenium.support.ui.ExpectedConditions;

public class GoogleLoginPage extends BasePage {
    private By emailInput = By.id("identifierId");
    private By emailNextBtn = By.id("identifierNext");
    private By passwordInput = By.name("password");
    private By passwordNextBtn = By.id("passwordNext");

    public GoogleLoginPage(WebDriver driver) {
        super(driver);
    }

    public void loginWithGoogle(String email, String password) {
        // Nhập email
        wait.until(ExpectedConditions.visibilityOfElementLocated(emailInput)).sendKeys(email);
        wait.until(ExpectedConditions.elementToBeClickable(emailNextBtn)).click();

        // Nhập password
        wait.until(ExpectedConditions.visibilityOfElementLocated(passwordInput)).sendKeys(password);
        wait.until(ExpectedConditions.elementToBeClickable(passwordNextBtn)).click();

        // Nếu có captcha, test sẽ dừng ở đây (bạn có thể mock hoặc skip nếu cần)
    }
}