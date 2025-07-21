using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StudentFreelanceTests.PageObjects
{
    public class ReportPage : BasePage
    {
        // Locators cho trang Create Report
        private readonly By _reportedUserDropdown = By.Id("ReportedUserID");
        private readonly By _reportTypeDropdown = By.Id("TypeID");
        private readonly By _projectDropdown = By.Id("ProjectID");
        private readonly By _descriptionTextarea = By.Id("Description");
        private readonly By _submitButton = By.CssSelector("button[type='submit']");
        private readonly By _successMessage = By.CssSelector(".alert-success");
        private readonly By _errorMessage = By.CssSelector(".alert-danger");
        private readonly By _validationSummary = By.CssSelector(".validation-summary-errors");
        private readonly By _validationErrors = By.CssSelector(".text-danger");

        // Locators cho trang List Report
        private readonly By _reportTable = By.CssSelector("table.table");
        private readonly By _reportRows = By.CssSelector("table.table tbody tr");
        private readonly By _statusFilterDropdown = By.Id("SelectedStatusId");
        private readonly By _typeFilterDropdown = By.Id("SelectedTypeId");
        private readonly By _searchInput = By.Id("SearchQuery");
        private readonly By _searchButton = By.CssSelector("button.search-btn");

        // Locators cho trang Detail Report
        private readonly By _reportDetails = By.CssSelector(".report-details");
        private readonly By _statusDropdown = By.Id("StatusID");
        private readonly By _adminResponseTextarea = By.Id("AdminResponse");
        private readonly By _updateStatusButton = By.Id("updateStatusBtn");
        
        // Menu header locators
        private readonly By _reportMenuDropdown = By.CssSelector("a.nav-link[href*='Report']");
        private readonly By _createReportMenuItem = By.CssSelector("a[href*='Report/Create']");
        private readonly By _listReportMenuItem = By.CssSelector("a[href*='Report/ListReport']");
        private readonly By _headerMenu = By.CssSelector("header .navbar");

        public ReportPage(IWebDriver driver) : base(driver)
        {
        }

        // Điều hướng đến trang tạo báo cáo qua menu
        public void NavigateToCreateReport(string baseUrl)
        {
            try
            {
                // Trước tiên, đảm bảo đã ở trang chủ
                Driver.Navigate().GoToUrl(baseUrl);
                WaitForPageLoad();
                
                // Kiểm tra xem có menu Report không
                if (IsElementDisplayed(_reportMenuDropdown, 3))
                {
                    // Click vào menu Report
                    Click(_reportMenuDropdown);
                    WaitForElementVisible(_createReportMenuItem, 3);
                    
                    // Click vào mục Create Report
                    Click(_createReportMenuItem);
                }
                else
                {
                    // Nếu không tìm thấy menu, dùng URL trực tiếp
                    Driver.Navigate().GoToUrl($"{baseUrl}/Report/Create");
                }
                
                WaitForPageLoad();
                Console.WriteLine("Đã chuyển đến trang tạo báo cáo");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi điều hướng đến trang tạo báo cáo: {ex.Message}");
                throw;
            }
        }

        // Điều hướng đến trang danh sách báo cáo qua menu
        public void NavigateToListReport(string baseUrl)
        {
            try
            {
                // Trước tiên, đảm bảo đã ở trang chủ
                Driver.Navigate().GoToUrl(baseUrl);
                WaitForPageLoad();
                
                // Kiểm tra xem có menu Report không
                if (IsElementDisplayed(_reportMenuDropdown, 3))
                {
                    // Click vào menu Report
                    Click(_reportMenuDropdown);
                    WaitForElementVisible(_listReportMenuItem, 3);
                    
                    // Click vào mục List Report
                    Click(_listReportMenuItem);
                }
                else
                {
                    // Nếu không tìm thấy menu, dùng URL trực tiếp
                    Driver.Navigate().GoToUrl($"{baseUrl}/Report/ListReport");
                }
                
                WaitForPageLoad();
                Console.WriteLine("Đã chuyển đến trang danh sách báo cáo");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi điều hướng đến trang danh sách báo cáo: {ex.Message}");
                throw;
            }
        }

        // Điều hướng đến trang chi tiết báo cáo
        public void NavigateToReportDetails(string baseUrl, int reportId)
        {
            try
            {
                Driver.Navigate().GoToUrl($"{baseUrl}/Report/Details/{reportId}");
                WaitForPageLoad();
                Console.WriteLine($"Đã chuyển đến trang chi tiết báo cáo ID: {reportId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi điều hướng đến trang chi tiết báo cáo: {ex.Message}");
                throw;
            }
        }

        // Tạo báo cáo mới
        public void CreateReport(string reportedUser, string reportType, string description, string project = "")
        {
            try
            {
                // Chọn người dùng cần báo cáo
                WaitForElementVisible(_reportedUserDropdown, 5);
                SelectDropdownOptionByText(_reportedUserDropdown, reportedUser);
                Console.WriteLine($"Đã chọn người dùng cần báo cáo: {reportedUser}");

                // Chọn loại báo cáo
                WaitForElementVisible(_reportTypeDropdown, 5);
                SelectDropdownOptionByText(_reportTypeDropdown, reportType);
                Console.WriteLine($"Đã chọn loại báo cáo: {reportType}");

                // Chọn dự án nếu có
                if (!string.IsNullOrEmpty(project) && IsElementDisplayed(_projectDropdown, 2))
                {
                    SelectDropdownOptionByText(_projectDropdown, project);
                    Console.WriteLine($"Đã chọn dự án: {project}");
                }

                // Nhập mô tả
                WaitForElementVisible(_descriptionTextarea, 5);
                SendKeys(_descriptionTextarea, description);
                Console.WriteLine($"Đã nhập mô tả: {description}");

                // Chụp ảnh form trước khi submit
                base.TakeScreenshot("before_submit_report");

                // Click nút gửi báo cáo
                ScrollToElement(_submitButton);
                Click(_submitButton);
                Console.WriteLine("Đã click nút gửi báo cáo");

                // Đợi trang load xong sau khi submit
                WaitForPageLoad();
                
                // Chụp ảnh kết quả sau khi submit
                base.TakeScreenshot("after_submit_report");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo báo cáo: {ex.Message}");
                base.TakeScreenshot("create_report_error");
                throw;
            }
        }

        // Kiểm tra xem báo cáo đã được tạo thành công chưa
        public bool IsReportCreatedSuccessfully()
        {
            try
            {
                // Kiểm tra thông báo thành công
                bool hasSuccessMessage = IsElementDisplayed(_successMessage, 5);
                
                // Kiểm tra URL sau khi tạo thành công (thường sẽ chuyển đến trang danh sách hoặc chi tiết)
                bool isRedirected = !Driver.Url.Contains("Create");
                
                Console.WriteLine($"Thông báo thành công: {hasSuccessMessage}, Đã chuyển hướng: {isRedirected}");
                
                // Chụp ảnh kết quả
                base.TakeScreenshot("report_creation_result");
                
                return hasSuccessMessage || isRedirected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra thông báo thành công: {ex.Message}");
                return false;
            }
        }

        // Kiểm tra thông báo lỗi
        public bool HasValidationErrors()
        {
            try
            {
                Thread.Sleep(1000); // Đợi 1 giây để đảm bảo validation errors đã hiển thị
                
                // Kiểm tra các thông báo lỗi
                bool hasValidationSummary = IsElementDisplayed(_validationSummary, 2);
                
                // Kiểm tra các thông báo lỗi client-side
                var validationElements = Driver.FindElements(_validationErrors);
                bool hasValidationErrors = validationElements.Any(e => !string.IsNullOrEmpty(e.Text));
                
                // Kiểm tra thông báo lỗi chung
                bool hasErrorMessage = IsElementDisplayed(_errorMessage, 2);
                
                // Kiểm tra các thông báo lỗi trực tiếp trên form (màu đỏ)
                bool hasRedValidationText = false;
                try {
                    var redTexts = Driver.FindElements(By.CssSelector(".text-danger:not(.field-validation-valid)"));
                    hasRedValidationText = redTexts.Any(e => !string.IsNullOrEmpty(e.Text));
                } catch {}
                
                // Chụp ảnh form với lỗi
                base.TakeScreenshot("validation_errors");
                
                // In HTML của form để debug
                try {
                    var formHtml = Driver.FindElement(By.TagName("form")).GetAttribute("innerHTML");
                    Console.WriteLine("HTML của form:");
                    Console.WriteLine(formHtml.Substring(0, Math.Min(500, formHtml.Length)) + "...");
                } catch {}

                Console.WriteLine($"Có thông báo lỗi: {hasValidationSummary || hasValidationErrors || hasErrorMessage || hasRedValidationText}");
                return hasValidationSummary || hasValidationErrors || hasErrorMessage || hasRedValidationText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra thông báo lỗi: {ex.Message}");
                return false;
            }
        }

        // Lấy danh sách thông báo lỗi
        public List<string> GetValidationErrors()
        {
            try
            {
                var errors = new List<string>();

                // Kiểm tra validation summary
                if (IsElementDisplayed(_validationSummary, 2))
                {
                    var summaryErrors = Driver.FindElements(By.CssSelector(".validation-summary-errors ul li"));
                    foreach (var error in summaryErrors)
                    {
                        errors.Add(error.Text);
                    }
                }

                // Kiểm tra các thông báo lỗi riêng lẻ
                var fieldErrors = Driver.FindElements(_validationErrors);
                foreach (var error in fieldErrors)
                {
                    if (!string.IsNullOrEmpty(error.Text))
                    {
                        errors.Add(error.Text);
                    }
                }

                // Kiểm tra thông báo lỗi chung
                if (IsElementDisplayed(_errorMessage, 2))
                {
                    errors.Add(Driver.FindElement(_errorMessage).Text);
                }
                
                // Kiểm tra các thông báo lỗi màu đỏ
                try {
                    var redTexts = Driver.FindElements(By.CssSelector(".text-danger:not(.field-validation-valid)"));
                    foreach (var error in redTexts)
                    {
                        if (!string.IsNullOrEmpty(error.Text) && !errors.Contains(error.Text))
                        {
                            errors.Add(error.Text);
                        }
                    }
                } catch {}

                Console.WriteLine($"Các thông báo lỗi: {string.Join(", ", errors)}");
                return errors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy thông báo lỗi: {ex.Message}");
                return new List<string>();
            }
        }

        // Kiểm tra xem bảng danh sách báo cáo có hiển thị không
        public bool IsReportTableDisplayed()
        {
            try
            {
                // Đợi một chút để đảm bảo trang đã load xong
                Thread.Sleep(1000);
                
                // Chụp ảnh trang danh sách
                base.TakeScreenshot("report_list_page");
                
                bool isDisplayed = IsElementDisplayed(_reportTable, 5);
                Console.WriteLine($"Bảng danh sách báo cáo hiển thị: {isDisplayed}");
                
                // Nếu không tìm thấy bảng, kiểm tra xem có thông báo quyền truy cập không
                if (!isDisplayed) {
                    try {
                        var pageContent = Driver.FindElement(By.TagName("body")).Text;
                        if (pageContent.Contains("Admin") || pageContent.Contains("Moderator")) {
                            Console.WriteLine("Trang yêu cầu quyền Admin hoặc Moderator");
                        }
                    } catch {}
                }
                
                return isDisplayed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra bảng danh sách báo cáo: {ex.Message}");
                return false;
            }
        }

        // Đếm số lượng báo cáo trong bảng
        public int CountReportsInTable()
        {
            try
            {
                if (!IsReportTableDisplayed())
                {
                    return 0;
                }

                var rows = Driver.FindElements(_reportRows);
                Console.WriteLine($"Số lượng báo cáo trong bảng: {rows.Count}");
                return rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đếm số lượng báo cáo: {ex.Message}");
                return 0;
            }
        }

        // Lọc báo cáo theo trạng thái
        public void FilterByStatus(string status)
        {
            try
            {
                SelectDropdownOptionByText(_statusFilterDropdown, status);
                
                // Click nút lọc nếu có
                var filterButton = Driver.FindElements(By.CssSelector("button.search-btn, button.filter-btn, button[type='submit']"));
                if (filterButton.Count > 0)
                {
                    filterButton[0].Click();
                }
                
                WaitForPageLoad();
                Console.WriteLine($"Đã lọc theo trạng thái: {status}");
                
                // Chụp ảnh sau khi lọc
                base.TakeScreenshot("after_filter_status");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lọc theo trạng thái: {ex.Message}");
                throw;
            }
        }

        // Lọc báo cáo theo loại
        public void FilterByType(string type)
        {
            try
            {
                SelectDropdownOptionByText(_typeFilterDropdown, type);
                
                // Click nút lọc nếu có
                var filterButton = Driver.FindElements(By.CssSelector("button.search-btn, button.filter-btn, button[type='submit']"));
                if (filterButton.Count > 0)
                {
                    filterButton[0].Click();
                }
                
                WaitForPageLoad();
                Console.WriteLine($"Đã lọc theo loại: {type}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lọc theo loại: {ex.Message}");
                throw;
            }
        }

        // Tìm kiếm báo cáo
        public void SearchReport(string keyword)
        {
            try
            {
                SendKeys(_searchInput, keyword);
                Click(_searchButton);
                WaitForPageLoad();
                Console.WriteLine($"Đã tìm kiếm với từ khóa: {keyword}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm kiếm báo cáo: {ex.Message}");
                throw;
            }
        }

        // Kiểm tra xem trang chi tiết báo cáo có hiển thị không
        public bool IsReportDetailsDisplayed()
        {
            try
            {
                bool isDisplayed = IsElementDisplayed(_reportDetails, 5);
                Console.WriteLine($"Trang chi tiết báo cáo hiển thị: {isDisplayed}");
                return isDisplayed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra trang chi tiết báo cáo: {ex.Message}");
                return false;
            }
        }

        // Cập nhật trạng thái báo cáo
        public void UpdateReportStatus(string status, string adminResponse = "")
        {
            try
            {
                // Chọn trạng thái
                SelectDropdownOptionByText(_statusDropdown, status);
                Console.WriteLine($"Đã chọn trạng thái: {status}");

                // Nhập phản hồi của admin nếu có
                if (!string.IsNullOrEmpty(adminResponse))
                {
                    SendKeys(_adminResponseTextarea, adminResponse);
                    Console.WriteLine($"Đã nhập phản hồi: {adminResponse}");
                }

                // Click nút cập nhật
                Click(_updateStatusButton);
                WaitForPageLoad();
                Console.WriteLine("Đã cập nhật trạng thái báo cáo");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật trạng thái báo cáo: {ex.Message}");
                throw;
            }
        }

        private void SelectDropdownOptionByText(By locator, string optionText)
        {
            try
            {
                var dropdown = Driver.FindElement(locator);
                var select = new SelectElement(dropdown);
                select.SelectByText(optionText);
                Console.WriteLine($"Đã chọn '{optionText}' từ dropdown");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi chọn option '{optionText}' từ dropdown: {ex.Message}");
                
                // Thử cách khác: tìm option bằng text chứa
                try
                {
                    var dropdown = Driver.FindElement(locator);
                    var options = dropdown.FindElements(By.TagName("option"));
                    
                    foreach (var option in options)
                    {
                        if (option.Text.Contains(optionText))
                        {
                            option.Click();
                            Console.WriteLine($"Đã chọn '{option.Text}' từ dropdown (phương pháp thay thế)");
                            return;
                        }
                    }
                    
                    Console.WriteLine($"Không tìm thấy option nào chứa text '{optionText}'");
                }
                catch (Exception innerEx)
                {
                    Console.WriteLine($"Lỗi khi thử phương pháp thay thế: {innerEx.Message}");
                    throw;
                }
            }
        }
    }
} 