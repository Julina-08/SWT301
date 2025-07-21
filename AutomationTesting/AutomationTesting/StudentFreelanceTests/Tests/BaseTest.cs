using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace StudentFreelanceTests.Tests
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver { get; private set; }
        protected string BaseUrl { get; private set; } = "https://localhost:7100";
        protected bool IsApplicationRunning { get; private set; } = false;

        public BaseTest()
        {
            Console.WriteLine("Lưu ý: Đảm bảo ứng dụng ASP.NET đã được chạy ở URL: " + BaseUrl);
            
            // Kiểm tra xem ứng dụng ASP.NET có đang chạy không
            IsApplicationRunning = CheckApplicationRunning().GetAwaiter().GetResult();
            
            if (!IsApplicationRunning)
            {
                Console.WriteLine("CẢNH BÁO: Không thể kết nối đến ứng dụng ASP.NET ở " + BaseUrl);
                Console.WriteLine("Vui lòng đảm bảo ứng dụng đang chạy trước khi thực hiện test.");
            }
            
            // Khởi tạo ChromeDriver với các tùy chọn
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            // Thêm cấu hình để tránh lỗi timeout
            options.AddArgument("--disable-browser-side-navigation");
            options.AddArgument("--disable-features=NetworkService");
            options.AddArgument("--disable-background-networking");
            options.AddArgument("--disable-default-apps");
            
            // Thiết lập các tham số timeout
            var service = ChromeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = false;
            
            // Tăng timeout để tránh lỗi khi trang load chậm
            Driver = new ChromeDriver(service, options);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
        }

        public void Dispose()
        {
            try
            {
                Driver?.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đóng trình duyệt: {ex.Message}");
            }
        }
        
        // Kiểm tra xem ứng dụng ASP.NET có đang chạy không
        private async Task<bool> CheckApplicationRunning()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = await client.GetAsync(BaseUrl);
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
        
        // Phương thức tiện ích để lấy ảnh chụp màn hình
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