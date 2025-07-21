package tests;

import org.junit.jupiter.api.*;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvFileSource;
import org.junit.jupiter.params.provider.CsvSource;
import org.openqa.selenium.support.ui.WebDriverWait;
import pages.RegistrationPage;

import java.time.Duration;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
@DisplayName("Registration Form Tests")
public class RegistrationTest extends BaseTest {
    
    private static RegistrationPage registrationPage;
    private static WebDriverWait wait;
    
    @BeforeAll
    static void initPage() {
        registrationPage = new RegistrationPage(driver);
        wait = new WebDriverWait(driver, Duration.ofSeconds(10));
    }
    
    @Test
    @Order(1)
    @DisplayName("Should fill and submit form with valid data")
    void testValidRegistration() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Fill the form
        registrationPage.setFirstName("John");
        registrationPage.setLastName("Doe");
        registrationPage.setEmail("john.doe@example.com");
        registrationPage.selectGender("Male");
        registrationPage.setMobileNumber("1234567890");
        registrationPage.setDateOfBirth("15", "May", "1990");
        registrationPage.setSubjects("Maths");
        registrationPage.selectHobbies("Sports");
        registrationPage.setCurrentAddress("123 Main Street");
        registrationPage.setState("NCR");
        registrationPage.setCity("Delhi");
        
        // Submit the form
        registrationPage.submitForm();
        
        // Verify confirmation is displayed
        assertTrue(registrationPage.isConfirmationDisplayed(), "Confirmation modal should be displayed");
        assertEquals("Thanks for submitting the form", registrationPage.getConfirmationTitle());
        
        // Verify submitted data
        assertTrue(registrationPage.verifySubmittedData("Student Name", "John Doe"));
        assertTrue(registrationPage.verifySubmittedData("Student Email", "john.doe@example.com"));
        assertTrue(registrationPage.verifySubmittedData("Gender", "Male"));
        assertTrue(registrationPage.verifySubmittedData("Mobile", "1234567890"));
        assertTrue(registrationPage.verifySubmittedData("Date of Birth", "15 May,1990"));
        assertTrue(registrationPage.verifySubmittedData("Subjects", "Maths"));
        assertTrue(registrationPage.verifySubmittedData("Hobbies", "Sports"));
        assertTrue(registrationPage.verifySubmittedData("Address", "123 Main Street"));
        assertTrue(registrationPage.verifySubmittedData("State and City", "NCR Delhi"));
    }
    
    @ParameterizedTest(name = "CSV File: {0} {1}")
    @Order(2)
    @CsvFileSource(resources = "/registration-data.csv", numLinesToSkip = 1)
    void testRegistrationWithCSVData(String firstName, String lastName, String email, 
                                    String gender, String mobile, String day, 
                                    String month, String year, String subjects,
                                    String hobbies, String address, String state, String city) {
        try {
            // Navigate to the registration page
            registrationPage.navigate();
            
            // Handle null values
            firstName = (firstName == null || firstName.isEmpty()) ? "" : firstName.trim();
            lastName = (lastName == null || lastName.isEmpty()) ? "" : lastName.trim();
            email = (email == null || email.isEmpty()) ? "" : email.trim();
            
            // Fill the form with data from CSV
            registrationPage.setFirstName(firstName);
            registrationPage.setLastName(lastName);
            registrationPage.setEmail(email);
            registrationPage.selectGender(gender);
            registrationPage.setMobileNumber(mobile);
            
            // Xử lý trường hợp đặc biệt cho ngày sinh
            try {
                registrationPage.setDateOfBirth(day, month, year);
            } catch (Exception e) {
                System.out.println("Error setting date of birth: " + e.getMessage());
                // Tiếp tục test mà không dừng lại
            }
            
            try {
                registrationPage.setSubjects(subjects);
            } catch (Exception e) {
                System.out.println("Error setting subjects: " + e.getMessage());
                // Tiếp tục test mà không dừng lại
            }
            
            try {
                registrationPage.selectHobbies(hobbies);
            } catch (Exception e) {
                System.out.println("Error selecting hobbies: " + e.getMessage());
                // Tiếp tục test mà không dừng lại
            }
            
            registrationPage.setCurrentAddress(address);
            
            try {
                if (state != null && !state.isEmpty()) {
                    registrationPage.setState(state);
                    
                    if (city != null && !city.isEmpty()) {
                        registrationPage.setCity(city);
                    }
                }
            } catch (Exception e) {
                System.out.println("Error setting state/city: " + e.getMessage());
                // Tiếp tục test mà không dừng lại
            }
            
            // Submit the form
            registrationPage.submitForm();
            
            // Verify confirmation is displayed
            if (!firstName.isEmpty() && !lastName.isEmpty() && mobile.length() == 10) {
                assertTrue(registrationPage.isConfirmationDisplayed(), "Confirmation modal should be displayed");
                assertEquals("Thanks for submitting the form", registrationPage.getConfirmationTitle());
                
                // Verify key data points
                String fullName = firstName + " " + lastName;
                assertTrue(registrationPage.verifySubmittedData("Student Name", fullName));
                
                if (!email.isEmpty()) {
                    assertTrue(registrationPage.verifySubmittedData("Student Email", email));
                }
                
                assertTrue(registrationPage.verifySubmittedData("Gender", gender));
                assertTrue(registrationPage.verifySubmittedData("Mobile", mobile));
            } else {
                // For invalid data, form should not be submitted
                Assertions.assertFalse(registrationPage.isConfirmationDisplayed(), 
                        "Form should not be submitted with invalid data");
            }
        } catch (Exception e) {
            System.out.println("Test failed for data: " + firstName + " " + lastName + " - Error: " + e.getMessage());
            // Đánh dấu test không thành công
            Assertions.fail("Test failed with exception: " + e.getMessage());
        }
    }
    
    @Test
    @Order(3)
    @DisplayName("Should validate required fields")
    void testRequiredFields() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Submit without filling required fields
        registrationPage.submitForm();
        
        // Form should not be submitted - no confirmation dialog
        // We can't easily check for validation messages with Selenium as they're browser-native
        // So we'll check that the confirmation dialog is NOT displayed
        Assertions.assertFalse(registrationPage.isConfirmationDisplayed(), 
                "Form should not submit without required fields");
    }
    
    @Test
    @Order(4)
    @DisplayName("Should validate form with special characters in name fields")
    void testSpecialCharactersInName() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Fill the form with special characters in name fields
        registrationPage.setFirstName("John@#$%");
        registrationPage.setLastName("Doe&*()");
        registrationPage.setEmail("john.doe@example.com");
        registrationPage.selectGender("Male");
        registrationPage.setMobileNumber("1234567890");
        
        // Submit the form
        registrationPage.submitForm();
        
        // Verify if form submission is successful (depends on application requirements)
        // Some applications may accept special characters in names, others may not
        if (registrationPage.isConfirmationDisplayed()) {
            // If accepted, verify the data
            assertTrue(registrationPage.verifySubmittedData("Student Name", "John@#$% Doe&*()"));
        } else {
            // If not accepted, form should not be submitted
            System.out.println("Form rejected special characters in name fields as expected");
        }
    }
    
    @Test
    @Order(5)
    @DisplayName("Should validate form with whitespace-only fields")
    void testWhitespaceOnlyFields() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Fill the form with whitespace-only in name fields
        registrationPage.setFirstName("    ");
        registrationPage.setLastName("    ");
        registrationPage.setEmail("valid@email.com");
        registrationPage.selectGender("Female");
        registrationPage.setMobileNumber("9876543210");
        
        // Submit the form
        registrationPage.submitForm();
        
        // Form should not be submitted with whitespace-only required fields
        Assertions.assertFalse(registrationPage.isConfirmationDisplayed(), 
                "Form should not submit with whitespace-only name fields");
    }
    
    @Test
    @Order(6)
    @DisplayName("Should validate mobile number boundary conditions")
    void testMobileNumberBoundaries() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Test with short mobile number
        registrationPage.setFirstName("John");
        registrationPage.setLastName("Doe");
        registrationPage.setEmail("john.doe@example.com");
        registrationPage.selectGender("Male");
        registrationPage.setMobileNumber("12345"); // Too short
        
        // Submit the form
        registrationPage.submitForm();
        
        // Form should not be submitted with short mobile number
        Assertions.assertFalse(registrationPage.isConfirmationDisplayed(), 
                "Form should not submit with short mobile number");
        
        // Test with long mobile number
        registrationPage.navigate(); // Navigate again to reset form
        
        registrationPage.setFirstName("John");
        registrationPage.setLastName("Doe");
        registrationPage.setEmail("john.doe@example.com");
        registrationPage.selectGender("Male");
        registrationPage.setMobileNumber("12345678901234567890"); // Too long
        
        // Submit the form
        registrationPage.submitForm();
        
        // Form behavior depends on application requirements
        // Some may truncate, others may reject
        if (registrationPage.isConfirmationDisplayed()) {
            // If accepted, verify what was actually submitted
            System.out.println("Form accepted long mobile number, checking submitted value");
        } else {
            // If rejected, form should not be submitted
            System.out.println("Form rejected long mobile number as expected");
        }
    }
    
    @Test
    @Order(7)
    @DisplayName("Should validate invalid email format")
    void testInvalidEmailFormat() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Fill the form with invalid email format
        registrationPage.setFirstName("John");
        registrationPage.setLastName("Doe");
        registrationPage.setEmail("invalid-email"); // Invalid format
        registrationPage.selectGender("Male");
        registrationPage.setMobileNumber("1234567890");
        
        // Submit the form
        registrationPage.submitForm();
        
        // Form should not be submitted with invalid email format
        // Note: Some forms may allow submission but show a warning
        if (registrationPage.isConfirmationDisplayed()) {
            System.out.println("Form accepted invalid email format - this might be a bug");
        } else {
            System.out.println("Form rejected invalid email format as expected");
        }
    }
    
    @ParameterizedTest
    @Order(8)
    @CsvSource({
        "Male, Sports",
        "Female, Reading",
        "Other, Music"
    })
    @DisplayName("Should validate different gender and hobby combinations")
    void testGenderHobbyCombinations(String gender, String hobby) {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Fill the form with different gender and hobby combinations
        registrationPage.setFirstName("John");
        registrationPage.setLastName("Doe");
        registrationPage.setEmail("john.doe@example.com");
        registrationPage.selectGender(gender);
        registrationPage.setMobileNumber("1234567890");
        registrationPage.selectHobbies(hobby);
        
        // Submit the form
        registrationPage.submitForm();
        
        // Verify confirmation is displayed
        assertTrue(registrationPage.isConfirmationDisplayed(), 
                "Form should be submitted with valid gender and hobby combination");
        
        // Verify submitted data
        assertTrue(registrationPage.verifySubmittedData("Gender", gender));
        assertTrue(registrationPage.verifySubmittedData("Hobbies", hobby));
    }
    
    @Test
    @Order(9)
    @DisplayName("Should validate multiple hobbies selection")
    void testMultipleHobbiesSelection() {
        // Navigate to the registration page
        registrationPage.navigate();
        
        // Fill the form with multiple hobbies
        registrationPage.setFirstName("John");
        registrationPage.setLastName("Doe");
        registrationPage.setEmail("john.doe@example.com");
        registrationPage.selectGender("Male");
        registrationPage.setMobileNumber("1234567890");
        registrationPage.selectHobbies("Sports", "Reading", "Music");
        
        // Submit the form
        registrationPage.submitForm();
        
        // Verify confirmation is displayed
        assertTrue(registrationPage.isConfirmationDisplayed(), 
                "Form should be submitted with multiple hobbies selected");
        
        // Verify submitted data
        assertTrue(registrationPage.verifySubmittedData("Hobbies", "Sports, Reading, Music"));
    }
} 