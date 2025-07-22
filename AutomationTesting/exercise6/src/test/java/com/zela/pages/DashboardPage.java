package com.zela.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;

public class DashboardPage extends BasePage {
    private By userNameLabel = By.cssSelector(".navbar .user-name, .profile-name"); // chỉnh lại selector nếu cần

    public DashboardPage(WebDriver driver) {
        super(driver);
    }

    public boolean isLoaded() {
        // Kiểm tra tiêu đề hoặc tên user xuất hiện
        return wait.until(ExpectedConditions.or(
                ExpectedConditions.titleContains("Zela"),
                ExpectedConditions.visibilityOfElementLocated(userNameLabel)
        )) != null;
    }

    public String getUserName() {
        return driver.findElement(userNameLabel).getText();
    }
}