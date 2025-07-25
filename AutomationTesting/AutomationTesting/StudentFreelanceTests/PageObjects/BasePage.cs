using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace StudentFreelanceTests.PageObjects
{
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; }
        
        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }
        
        protected void WaitForPageLoad(int timeoutSeconds = 20)
        {
            try
            {
                var jsExecutor = (IJavaScriptExecutor)Driver;
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                
                // Đợi cho đến khi document.readyState là "complete"
                wait.Until(wd => {
                    try {
                        var readyState = jsExecutor.ExecuteScript("return document.readyState").ToString();
                        return readyState == "complete";
                    }
                    catch (Exception) {
                        // Nếu có lỗi khi thực thi JavaScript, thử kiểm tra theo cách khác
                        return false;
                    }
                });
                
                Console.WriteLine("Trang đã load xong");
            }
            catch (WebDriverTimeoutException)
            {
                // Nếu timeout, thử kiểm tra các phần tử quan trọng trên trang thay vì readyState
                Console.WriteLine("Timeout khi đợi trang load hoàn toàn, kiểm tra các phần tử quan trọng...");
                
                try
                {
                    // Đợi một chút để trang có thêm thời gian load
                    System.Threading.Thread.Sleep(2000);
                    
                    // Kiểm tra xem có phần tử body không
                    bool hasBody = Driver.FindElements(By.TagName("body")).Count > 0;
                    
                    if (hasBody)
                    {
                        Console.WriteLine("Trang đã load một phần (phát hiện thẻ body)");
                    }
                    else
                    {
                        Console.WriteLine("CẢNH BÁO: Không thể xác nhận trang đã load");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi kiểm tra phần tử sau timeout: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đợi trang load: {ex.Message}");
                
                // Đợi một chút để trang có thêm thời gian load
                try
                {
                    System.Threading.Thread.Sleep(2000);
                }
                catch { }
            }
        }
        
        // Giữ phương thức cũ để tương thích với code hiện tại
        protected void WaitForElementToBeVisible(By locator, int timeoutSeconds = 10)
        {
            WaitForElementVisible(locator, timeoutSeconds);
        }
        
        // Thêm phương thức mới với tên mới
        protected void WaitForElementVisible(By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                Console.WriteLine($"Phần tử {locator} đã hiển thị");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TIMEOUT: Không tìm thấy phần tử {locator} sau {timeoutSeconds} giây. Chi tiết: {ex.Message}");
                
                // Chụp ảnh màn hình để debug
                try
                {
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var fileName = $"error_screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    screenshot.SaveAsFile(fileName);
                    Console.WriteLine($"Đã lưu ảnh màn hình lỗi: {fileName}");
                }
                catch { }
                
                throw;
            }
        }
        
        protected void WaitForElementToBeClickable(By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                Console.WriteLine($"Phần tử {locator} đã có thể click");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TIMEOUT: Phần tử {locator} không thể click sau {timeoutSeconds} giây. Chi tiết: {ex.Message}");
                
                // Chụp ảnh màn hình để debug
                try
                {
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var fileName = $"error_screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    screenshot.SaveAsFile(fileName);
                    Console.WriteLine($"Đã lưu ảnh màn hình lỗi: {fileName}");
                }
                catch { }
                
                throw;
            }
        }
        
        protected void WaitForElementToBePresent(By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                Console.WriteLine($"Phần tử {locator} đã có trong DOM");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TIMEOUT: Không tìm thấy phần tử {locator} trong DOM sau {timeoutSeconds} giây. Chi tiết: {ex.Message}");
                
                // Chụp ảnh màn hình để debug
                try
                {
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var fileName = $"error_screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    screenshot.SaveAsFile(fileName);
                    Console.WriteLine($"Đã lưu ảnh màn hình lỗi: {fileName}");
                }
                catch { }
                
                throw;
            }
        }
        
        protected IWebElement FindElement(By locator)
        {
            try
            {
            return Driver.FindElement(locator);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm phần tử {locator}: {ex.Message}");
                throw;
            }
        }
        
        protected void Click(By locator)
        {
            try
            {
                WaitForElementVisible(locator);
                var element = FindElement(locator);
                element.Click();
                Console.WriteLine($"Đã click vào phần tử {locator}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi click phần tử {locator}: {ex.Message}");
                
                // Thử dùng JavaScript để click
                try
                {
                    var element = FindElement(locator);
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
                    Console.WriteLine($"Đã click vào phần tử {locator} bằng JavaScript");
                }
                catch (Exception jsEx)
                {
                    Console.WriteLine($"Lỗi khi click phần tử {locator} bằng JavaScript: {jsEx.Message}");
                    throw;
                }
            }
        }
        
        protected void SendKeys(By locator, string text)
        {
            try
        {
            WaitForElementVisible(locator);
            var element = FindElement(locator);
            element.Clear();
            element.SendKeys(text);
                Console.WriteLine($"Đã nhập '{text}' vào phần tử {locator}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi nhập text vào phần tử {locator}: {ex.Message}");
                
                // Thử dùng JavaScript để nhập text
                try
                {
                    var element = FindElement(locator);
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].value = arguments[1];", element, text);
                    Console.WriteLine($"Đã nhập '{text}' vào phần tử {locator} bằng JavaScript");
                }
                catch (Exception jsEx)
                {
                    Console.WriteLine($"Lỗi khi nhập text vào phần tử {locator} bằng JavaScript: {jsEx.Message}");
                    throw;
                }
            }
        }
        
        protected string GetText(By locator)
        {
            try
        {
            WaitForElementVisible(locator);
            return FindElement(locator).Text;
        }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy text từ phần tử {locator}: {ex.Message}");
                
                // Thử dùng JavaScript để lấy text
                try
                {
                    var element = FindElement(locator);
                    var text = (string)((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].textContent || arguments[0].innerText;", element);
                    return text?.Trim() ?? string.Empty;
                }
                catch (Exception jsEx)
                {
                    Console.WriteLine($"Lỗi khi lấy text từ phần tử {locator} bằng JavaScript: {jsEx.Message}");
                    return string.Empty;
                }
            }
        }
        
        public bool IsElementDisplayed(By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(driver => {
                    try {
                        var element = driver.FindElement(locator);
                        return element.Displayed;
                    } catch (StaleElementReferenceException) {
                        return false;
                    } catch (NoSuchElementException) {
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra phần tử {locator} có hiển thị không: {ex.Message}");
                return false;
            }
        }
        
        protected void ScrollToElement(By locator)
        {
            try
            {
                var element = FindElement(locator);
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
                Console.WriteLine($"Đã cuộn đến phần tử {locator}");
                
                // Đợi một chút sau khi cuộn
                System.Threading.Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cuộn đến phần tử {locator}: {ex.Message}");
            }
        }
        
        protected void RefreshPage()
        {
            try
            {
                Driver.Navigate().Refresh();
                WaitForPageLoad();
                Console.WriteLine("Đã tải lại trang");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải lại trang: {ex.Message}");
            }
        }
        
        protected void NavigateBack()
        {
            try
            {
                Driver.Navigate().Back();
                WaitForPageLoad();
                Console.WriteLine("Đã quay lại trang trước");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi quay lại trang trước: {ex.Message}");
            }
        }
        
        protected bool RetryClick(By locator, int maxRetries = 3)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    Click(locator);
                    return true;
                }
                catch (Exception ex)
                {
                    retries++;
                    Console.WriteLine($"Lần thử {retries}/{maxRetries} click vào phần tử {locator} thất bại: {ex.Message}");
                    System.Threading.Thread.Sleep(1000);
                }
            }
            
            Console.WriteLine($"Đã thử click vào phần tử {locator} {maxRetries} lần nhưng không thành công");
            return false;
        }
        
        protected bool WaitForUrlContains(string urlPart, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(ExpectedConditions.UrlContains(urlPart));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"URL không chứa '{urlPart}' sau {timeoutSeconds} giây. URL hiện tại: {Driver.Url}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đợi URL chứa '{urlPart}': {ex.Message}");
                return false;
            }
        }
        
        protected void TakeScreenshot(string fileNamePrefix)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var fileName = $"{fileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(fileName);
                Console.WriteLine($"Đã lưu ảnh màn hình: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi chụp ảnh màn hình: {ex.Message}");
            }
        }
    }
}
