package com.zela.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;

public class ProfilePage extends BasePage {
    private By fullNameInput = By.id("FullName"); // chỉnh lại id nếu khác
    private By saveBtn = By.cssSelector("button[type='submit']");
    private By successMsg = By.cssSelector(".alert-success, .profile-update-success"); // chỉnh lại selector nếu cần

    public ProfilePage(WebDriver driver) {
        super(driver);
    }

    public void open(String baseUrl) {
        driver.get(baseUrl + "/Profile/Index");
    }

    public void updateFullName(String newName) {
        wait.until(ExpectedConditions.visibilityOfElementLocated(fullNameInput)).clear();
        driver.findElement(fullNameInput).sendKeys(newName);
        wait.until(ExpectedConditions.elementToBeClickable(saveBtn)).click();
    }

    public boolean isUpdateSuccess() {
        return wait.until(ExpectedConditions.visibilityOfElementLocated(successMsg)) != null;
    }
}