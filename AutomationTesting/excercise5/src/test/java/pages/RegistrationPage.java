package pages;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class RegistrationPage extends BasePage {
    // Locators
    private final By firstNameField = By.id("firstName");
    private final By lastNameField = By.id("lastName");
    private final By emailField = By.id("userEmail");
    private final By maleGenderRadio = By.xpath("//label[text()='Male']");
    private final By femaleGenderRadio = By.xpath("//label[text()='Female']");
    private final By otherGenderRadio = By.xpath("//label[text()='Other']");
    private final By mobileNumberField = By.id("userNumber");
    private final By dateOfBirthField = By.id("dateOfBirthInput");
    private final By subjectsInput = By.id("subjectsInput");
    private final By sportsHobby = By.xpath("//label[text()='Sports']");
    private final By readingHobby = By.xpath("//label[text()='Reading']");
    private final By musicHobby = By.xpath("//label[text()='Music']");
    private final By uploadPictureBtn = By.id("uploadPicture");
    private final By currentAddressField = By.id("currentAddress");
    private final By stateDropdown = By.id("state");
    private final By stateInput = By.id("react-select-3-input");
    private final By cityDropdown = By.id("city");
    private final By cityInput = By.id("react-select-4-input");
    private final By submitButton = By.id("submit");
    
    // Form validation elements
    private final By firstNameValidation = By.xpath("//input[@id='firstName']/following-sibling::div[contains(@class, 'error')]");
    private final By lastNameValidation = By.xpath("//input[@id='lastName']/following-sibling::div[contains(@class, 'error')]");
    private final By emailValidation = By.xpath("//input[@id='userEmail']/following-sibling::div[contains(@class, 'error')]");
    private final By mobileValidation = By.xpath("//input[@id='userNumber']/following-sibling::div[contains(@class, 'error')]");
    
    // Confirmation elements
    private final By confirmationModal = By.className("modal-content");
    private final By confirmationModalTitle = By.id("example-modal-sizes-title-lg");
    private final By confirmationTable = By.className("table-responsive");

    public RegistrationPage(WebDriver driver) {
        super(driver);
    }

    public void navigate() {
        navigateTo("https://demoqa.com/automation-practice-form");
    }

    public void setFirstName(String firstName) {
        type(firstNameField, firstName);
    }

    public void setLastName(String lastName) {
        type(lastNameField, lastName);
    }

    public void setEmail(String email) {
        type(emailField, email);
    }

    public void selectGender(String gender) {
        switch (gender.toLowerCase()) {
            case "male":
                clickWithJS(maleGenderRadio);
                break;
            case "female":
                clickWithJS(femaleGenderRadio);
                break;
            case "other":
                clickWithJS(otherGenderRadio);
                break;
            default:
                throw new IllegalArgumentException("Invalid gender: " + gender);
        }
    }

    public void setMobileNumber(String mobileNumber) {
        type(mobileNumberField, mobileNumber);
    }

    public void setDateOfBirth(String day, String month, String year) {
        // Sử dụng JavaScript để mở date picker thay vì click trực tiếp
        WebElement dateElement = waitForVisibility(dateOfBirthField);
        JavascriptExecutor js = (JavascriptExecutor) driver;
        js.executeScript("arguments[0].click();", dateElement);
        
        try {
            // Chờ một chút để date picker hiển thị
            Thread.sleep(500);
            
            // Select year
            By yearDropdown = By.className("react-datepicker__year-select");
            selectByVisibleText(yearDropdown, year);
            
            // Select month
            By monthDropdown = By.className("react-datepicker__month-select");
            selectByVisibleText(monthDropdown, month);
            
            // Select day bằng JavaScript
            By daySelector = By.xpath("//div[contains(@class, 'react-datepicker__day') and text()='" + day + "']");
            WebElement dayElement = waitForVisibility(daySelector);
            js.executeScript("arguments[0].click();", dayElement);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    public void setSubjects(String... subjects) {
        for (String subject : subjects) {
            WebElement subjectField = waitForVisibility(subjectsInput);
            subjectField.sendKeys(subject);
            try {
                Thread.sleep(500); // Chờ để suggestions hiển thị
                By option = By.xpath("//div[contains(@class, 'subjects-auto-complete__option') and contains(text(), '" + subject + "')]");
                clickWithJS(option);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            } catch (Exception e) {
                System.out.println("Could not select subject: " + subject + " - " + e.getMessage());
            }
        }
    }

    public void selectHobbies(String... hobbies) {
        for (String hobby : hobbies) {
            switch (hobby.toLowerCase()) {
                case "sports":
                    clickWithJS(sportsHobby);
                    break;
                case "reading":
                    clickWithJS(readingHobby);
                    break;
                case "music":
                    clickWithJS(musicHobby);
                    break;
                default:
                    throw new IllegalArgumentException("Invalid hobby: " + hobby);
            }
        }
    }

    public void uploadPicture(String filePath) {
        uploadFile(uploadPictureBtn, filePath);
    }

    public void setCurrentAddress(String address) {
        type(currentAddressField, address);
    }

    public void setState(String state) {
        scrollToElement(stateDropdown);
        clickWithJS(stateDropdown);
        try {
            Thread.sleep(500); // Chờ dropdown hiển thị
            WebElement input = waitForVisibility(stateInput);
            input.sendKeys(state);
            input.sendKeys(org.openqa.selenium.Keys.ENTER);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    public void setCity(String city) {
        scrollToElement(cityDropdown);
        clickWithJS(cityDropdown);
        try {
            Thread.sleep(500); // Chờ dropdown hiển thị
            WebElement input = waitForVisibility(cityInput);
            input.sendKeys(city);
            input.sendKeys(org.openqa.selenium.Keys.ENTER);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    public void submitForm() {
        scrollToElement(submitButton);
        clickWithJS(submitButton);
    }

    public boolean isConfirmationDisplayed() {
        try {
            return isElementVisible(confirmationModal);
        } catch (Exception e) {
            return false;
        }
    }

    public String getConfirmationTitle() {
        return getText(confirmationModalTitle);
    }

    public boolean verifySubmittedData(String label, String expectedValue) {
        try {
            By rowLocator = By.xpath("//div[@class='table-responsive']//tr[td[text()='" + label + "']]/td[2]");
            String actualValue = getText(rowLocator);
            return actualValue.equals(expectedValue);
        } catch (Exception e) {
            return false;
        }
    }
    
    // New methods for validation checking
    
    public boolean isFirstNameValidationDisplayed() {
        return isElementVisible(firstNameValidation);
    }
    
    public boolean isLastNameValidationDisplayed() {
        return isElementVisible(lastNameValidation);
    }
    
    public boolean isEmailValidationDisplayed() {
        return isElementVisible(emailValidation);
    }
    
    public boolean isMobileValidationDisplayed() {
        return isElementVisible(mobileValidation);
    }
    
    public String getFirstNameValidationMessage() {
        if (isFirstNameValidationDisplayed()) {
            return getText(firstNameValidation);
        }
        return "";
    }
    
    public String getLastNameValidationMessage() {
        if (isLastNameValidationDisplayed()) {
            return getText(lastNameValidation);
        }
        return "";
    }
    
    public String getEmailValidationMessage() {
        if (isEmailValidationDisplayed()) {
            return getText(emailValidation);
        }
        return "";
    }
    
    public String getMobileValidationMessage() {
        if (isMobileValidationDisplayed()) {
            return getText(mobileValidation);
        }
        return "";
    }
    
    // Helper method to get all selected hobbies
    public List<String> getSelectedHobbies() {
        List<String> selectedHobbies = Arrays.asList("Sports", "Reading", "Music").stream()
                .filter(hobby -> {
                    By hobbyLocator = null;
                    switch (hobby) {
                        case "Sports":
                            hobbyLocator = sportsHobby;
                            break;
                        case "Reading":
                            hobbyLocator = readingHobby;
                            break;
                        case "Music":
                            hobbyLocator = musicHobby;
                            break;
                    }
                    try {
                        WebElement element = driver.findElement(hobbyLocator);
                        return element.isSelected();
                    } catch (Exception e) {
                        return false;
                    }
                })
                .collect(Collectors.toList());
        return selectedHobbies;
    }
    
    // Helper method to click using JavaScript (useful for elements that might be covered by other elements)
    private void clickWithJS(By locator) {
        WebElement element = waitForVisibility(locator);
        JavascriptExecutor js = (JavascriptExecutor) driver;
        js.executeScript("arguments[0].click();", element);
    }
} 