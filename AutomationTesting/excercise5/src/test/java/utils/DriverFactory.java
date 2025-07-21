package utils;

import io.github.bonigarcia.wdm.WebDriverManager;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;

import java.util.HashMap;
import java.util.Map;

public class DriverFactory {
    public static WebDriver createDriver() {
        WebDriverManager.chromedriver().setup();

        ChromeOptions options = new ChromeOptions();
        Map<String, Object> prefs = new HashMap<>();
        prefs.put("profile.managed_default_content_settings.javascript", 1); // Allow JavaScript
        prefs.put("profile.default_content_setting_values.ads", 2); // Block ads
        options.setExperimentalOption("prefs", prefs);
        
        // Add additional options
        options.addArguments("--incognito"); // Incognito mode
        options.addArguments("--disable-notifications"); // Disable notifications
        options.addArguments("--disable-popup-blocking"); // Disable popup blocking
        options.addArguments("--disable-extensions"); // Disable extensions
        options.addArguments("--disable-infobars"); // Disable infobars
        options.addArguments("--disable-gpu"); // Disable GPU acceleration
        options.addArguments("--disable-dev-shm-usage"); // Overcome limited resource problems
        options.addArguments("--no-sandbox"); // Bypass OS security model

        return new ChromeDriver(options);
    }
} 