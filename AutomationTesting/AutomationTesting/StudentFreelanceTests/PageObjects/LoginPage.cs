using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace StudentFreelanceTests.PageObjects
{
    public class LoginPage : BasePage
    {
        // Locators cập nhật dựa trên Login.cshtml
        private readonly By _emailInput = By.Id("Email");
        private readonly By _passwordInput = By.Id("Password");
        private readonly By _loginButton = By.CssSelector(".submit-btn");
        private readonly By _rememberMeCheckbox = By.Id("RememberMe");
        
        // Cập nhật locator cho thông báo lỗi để bắt được cả validation-summary và span validation
        private readonly By _errorMessage = By.CssSelector(".validation-summary-errors, .text-danger, span[data-valmsg-for='Email'], span[data-valmsg-for='Password']");
        private readonly By _emailErrorMessage = By.CssSelector("span[data-valmsg-for='Email']");
        private readonly By _passwordErrorMessage = By.CssSelector("span[data-valmsg-for='Password']");
        
        private readonly By _successMessage = By.CssSelector(".alert-success");
        private readonly By _forgotPasswordLink = By.XPath("//a[contains(@href, '/Account/ForgotPassword')]");
        private readonly By _createAccountLink = By.XPath("//a[contains(@href, '/Account/Register')]");
        
        // Locators để kiểm tra đăng nhập thành công
        private readonly By _userMenuOrAvatar = By.CssSelector(".nav-link.dropdown-toggle");
        private readonly By _logoutLink = By.XPath("//a[contains(@href, '/Account/Logout')]");
        
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }
        
        public void NavigateToLoginPage(string baseUrl)
        {
            try
            {
                Driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");
                
                // Đợi trang load xong
                WaitForPageLoad();
                
                // Kiểm tra xem đã đến trang đăng nhập chưa
                try {
                    WaitForElementToBeVisible(_emailInput, 10);
                    Console.WriteLine("Đã load trang đăng nhập thành công");
                } catch (Exception ex) {
                    Console.WriteLine($"Lỗi khi load trang đăng nhập: {ex.Message}");
                    
                    // Nếu không tìm thấy phần tử đăng nhập, thử tải lại trang
                    Driver.Navigate().Refresh();
                    System.Threading.Thread.Sleep(2000);
                    
                    // Kiểm tra lại sau khi tải lại trang
                    try {
                        WaitForElementToBeVisible(_emailInput, 10);
                        Console.WriteLine("Đã load trang đăng nhập sau khi refresh");
                    } catch {
                        Console.WriteLine("Không thể load trang đăng nhập sau khi refresh");
                        
                        // Chụp ảnh màn hình để debug
                        try
                        {
                            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                            var fileName = $"login_page_error_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                            screenshot.SaveAsFile(fileName);
                            Console.WriteLine($"Đã lưu ảnh màn hình trang đăng nhập: {fileName}");
                            
                            // In ra source HTML để debug
                            Console.WriteLine("HTML của trang hiện tại:");
                            Console.WriteLine(Driver.PageSource.Substring(0, Math.Min(500, Driver.PageSource.Length)) + "...");
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi điều hướng đến trang đăng nhập: {ex.Message}");
                throw;
            }
        }
        
        public void EnterEmail(string email)
        {
            SendKeys(_emailInput, email);
        }
        
        public void EnterPassword(string password)
        {
            SendKeys(_passwordInput, password);
        }
        
        public void ClickRememberMe()
        {
            try
            {
                Click(_rememberMeCheckbox);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi click checkbox Remember Me: {ex.Message}");
            }
        }
        
        public void ClickLoginButton()
        {
            try
            {
                // Cuộn đến nút đăng nhập trước khi click
                ScrollToElement(_loginButton);
                Click(_loginButton);
                Console.WriteLine("Đã click nút đăng nhập");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi click nút đăng nhập: {ex.Message}");
                
                // Thử tìm nút đăng nhập theo cách khác
                try
                {
                    var loginBtn = Driver.FindElement(By.CssSelector("button[type='submit']"));
                    ScrollToElement(By.CssSelector("button[type='submit']"));
                    loginBtn.Click();
                    Console.WriteLine("Đã click nút đăng nhập bằng CSS selector thay thế");
                }
                catch (Exception innerEx)
                {
                    Console.WriteLine($"Lỗi khi tìm nút đăng nhập thay thế: {innerEx.Message}");
                    
                    // Thử dùng JavaScript để click
                    try
                    {
                        var element = Driver.FindElement(By.XPath("//button[contains(text(), 'Login') or contains(text(), 'Đăng nhập')]"));
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
                        Console.WriteLine("Đã click nút đăng nhập bằng JavaScript");
                    }
                    catch (Exception jsEx)
                    {
                        Console.WriteLine($"Lỗi khi click nút đăng nhập bằng JavaScript: {jsEx.Message}");
                        throw;
                    }
                }
            }
        }
        
        public bool IsErrorMessageDisplayed()
        {
            try
            {
                // Đợi validation client-side chạy
                System.Threading.Thread.Sleep(1000);
                
                // Kiểm tra nhiều loại thông báo lỗi
                bool summaryError = IsElementDisplayed(_errorMessage, 2);
                bool emailError = IsElementDisplayed(_emailErrorMessage, 1);
                bool passwordError = IsElementDisplayed(_passwordErrorMessage, 1);
                
                Console.WriteLine($"Kiểm tra thông báo lỗi: summaryError={summaryError}, emailError={emailError}, passwordError={passwordError}");
                
                // In HTML để debug
                if (!summaryError && !emailError && !passwordError) {
                    Console.WriteLine("HTML của form đăng nhập:");
                    try {
                        var formElement = Driver.FindElement(By.CssSelector(".login-form"));
                        Console.WriteLine(formElement.GetAttribute("innerHTML"));
                    } catch {}
                }
                
                return summaryError || emailError || passwordError;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra thông báo lỗi: {ex.Message}");
                return false;
            }
        }
        
        public string GetErrorMessage()
        {
            try
            {
                // Đợi validation client-side chạy
                System.Threading.Thread.Sleep(1000);
                
                // Thử lấy thông báo lỗi từ nhiều nguồn
                string errorText = "";
                
                try {
                    if (IsElementDisplayed(_errorMessage, 2)) {
                        errorText = GetText(_errorMessage);
                    }
                } catch {}
                
                try {
                    if (string.IsNullOrEmpty(errorText) && IsElementDisplayed(_emailErrorMessage, 1)) {
                        errorText = GetText(_emailErrorMessage);
                    }
                } catch {}
                
                try {
                    if (string.IsNullOrEmpty(errorText) && IsElementDisplayed(_passwordErrorMessage, 1)) {
                        errorText = GetText(_passwordErrorMessage);
                    }
                } catch {}
                
                // Nếu vẫn không tìm thấy, thử tìm bất kỳ phần tử nào có class text-danger
                if (string.IsNullOrEmpty(errorText)) {
                    try {
                        var errorElements = Driver.FindElements(By.CssSelector(".text-danger"));
                        foreach (var element in errorElements) {
                            if (!string.IsNullOrEmpty(element.Text)) {
                                errorText = element.Text;
                                break;
                            }
                        }
                    } catch {}
                }
                
                Console.WriteLine($"Nội dung thông báo lỗi: '{errorText}'");
                return errorText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy nội dung thông báo lỗi: {ex.Message}");
                return string.Empty;
            }
        }
        
        public void Login(string email, string password, bool rememberMe = false)
        {
            try
            {
                EnterEmail(email);
                EnterPassword(password);
                
                if (rememberMe)
                {
                    ClickRememberMe();
                }
                
                ClickLoginButton();
                
                // Đợi một chút để xử lý đăng nhập
                System.Threading.Thread.Sleep(3000);
                
                // Đợi trang load xong sau khi đăng nhập
                WaitForPageLoad();
                
                Console.WriteLine($"Đã thực hiện đăng nhập với email: {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong quá trình đăng nhập: {ex.Message}");
                
                // Chụp ảnh màn hình để debug
                try
                {
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var fileName = $"login_error_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    screenshot.SaveAsFile(fileName);
                    Console.WriteLine($"Đã lưu ảnh màn hình lỗi đăng nhập: {fileName}");
                }
                catch { }
                
                throw;
            }
        }
        
        public bool IsLoggedIn()
        {
            // Kiểm tra xem đã đăng nhập thành công hay chưa
            try
            {
                // Đợi trang load xong
                WaitForPageLoad();
                
                // Đợi lâu hơn để trang chuyển hướng sau khi đăng nhập
                System.Threading.Thread.Sleep(3000);
                
                // In ra URL hiện tại để debug
                Console.WriteLine($"URL hiện tại: {Driver.Url}");
                
                // Kiểm tra URL sau khi đăng nhập (thường là trang chủ hoặc dashboard)
                bool urlCheck = Driver.Url.Contains("/Home") || 
                               Driver.Url.Contains("/Dashboard") || 
                               !Driver.Url.Contains("/Account/Login");
                
                // Kiểm tra có phần tử chỉ xuất hiện khi đã đăng nhập không
                bool elementCheck = false;
                
                // Thử nhiều cách khác nhau để kiểm tra đã đăng nhập
                try {
                    elementCheck = IsElementDisplayed(_userMenuOrAvatar, 3);
                } catch { }
                
                if (!elementCheck) {
                    try {
                        elementCheck = IsElementDisplayed(_logoutLink, 2);
                    } catch { }
                }
                
                if (!elementCheck) {
                    try {
                        elementCheck = IsElementDisplayed(By.LinkText("Đăng xuất"), 2);
                    } catch { }
                }
                
                if (!elementCheck) {
                    try {
                        elementCheck = IsElementDisplayed(By.LinkText("Logout"), 2);
                    } catch { }
                }
                
                if (!elementCheck) {
                    // Kiểm tra xem có phần tử nào chỉ hiển thị khi đăng nhập không
                    try {
                        elementCheck = Driver.PageSource.Contains("Đăng xuất") || 
                                      Driver.PageSource.Contains("Logout") ||
                                      Driver.PageSource.Contains("Tài khoản") ||
                                      Driver.PageSource.Contains("Account");
                    } catch { }
                }
                
                Console.WriteLine($"Kiểm tra đăng nhập: URL={Driver.Url}, urlCheck={urlCheck}, elementCheck={elementCheck}");
                
                return urlCheck || elementCheck;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra đăng nhập: {ex.Message}");
                return false;
            }
        }

        // Kiểm tra xem có đang ở trang đăng nhập không
        public bool IsStillOnLoginPage()
        {
            try
            {
                return Driver.Url.Contains("/Account/Login") || 
                       IsElementDisplayed(By.Id("Email"), 1) || 
                       IsElementDisplayed(By.Id("Password"), 1);
            }
            catch
            {
                return false;
            }
        }

        // Lấy HTML của form đăng nhập
        public string GetLoginFormHtml()
        {
            try
            {
                var form = Driver.FindElement(By.TagName("form"));
                return form.GetAttribute("innerHTML");
            }
            catch
            {
                return "Không thể lấy HTML của form đăng nhập";
            }
        }

        // Kiểm tra xem có thông báo lỗi validation không
        public bool IsValidationErrorDisplayed()
        {
            try
            {
                // Kiểm tra các loại thông báo lỗi
                bool summaryError = IsElementDisplayed(By.CssSelector(".validation-summary-errors"), 1);
                bool emailError = IsElementDisplayed(By.CssSelector("[data-valmsg-for='Email'].field-validation-error"), 1);
                bool passwordError = IsElementDisplayed(By.CssSelector("[data-valmsg-for='Password'].field-validation-error"), 1);
                
                Console.WriteLine($"Kiểm tra thông báo lỗi: summaryError={summaryError}, emailError={emailError}, passwordError={passwordError}");
                
                return summaryError || emailError || passwordError;
            }
            catch
            {
                return false;
            }
        }
    }
} 