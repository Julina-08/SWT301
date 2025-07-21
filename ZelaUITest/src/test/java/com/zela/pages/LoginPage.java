package com.zela.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;

public class LoginPage extends BasePage {
    private By googleLoginBtn = By.cssSelector(".btn-google");

    public LoginPage(WebDriver driver) {
        super(driver);
    }

    public void open(String baseUrl) {
        driver.get(baseUrl + "/Account/Login");
    }

    public void clickGoogleLogin() {
        wait.until(ExpectedConditions.elementToBeClickable(googleLoginBtn)).click();
    }
}