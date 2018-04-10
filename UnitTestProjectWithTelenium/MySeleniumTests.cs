using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Events;
using System.Diagnostics;

using System.Threading;

namespace UnitTestProjectWithTelenium
{
    /// <summary>
    /// MySeleniumTests 的摘要说明
    /// </summary>
    [TestClass]
    public class MySeleniumTests
    {
        public MySeleniumTests()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private IWebDriver driver;
        private string appURL;

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            appURL = "https://cn.bing.com/?ensearch=1&FORM=BEHPTB";

            string browser = "Chrome";
            switch (browser)
            {
                case "Chrome":
                    driver = new ChromeDriver();
                    break;
                case "Firefox":
                    driver = new FirefoxDriver();
                    break;
                case "IE":
                    driver = new InternetExplorerDriver();
                    break;
                default:
                    driver = new ChromeDriver();
                    break;
            }
        }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
            driver.Quit();
        }
        //
        #endregion

        [TestMethod]
        [TestCategory("Chrome")]
        public void TheBingSearchTest()
        {
            driver.Navigate().GoToUrl(appURL + "/");
            driver.FindElement(By.Id("sb_form_q")).SendKeys("VSTS");
            driver.FindElement(By.Id("sb_form_go")).Click();
            driver.FindElement(
                //By.XPath("(//ol[@id='b_results']/li/div/h2/a/strong)[3]")
                By.CssSelector("ol#b_results>li:nth-child(3)>div>h2>a>strong")
            ).Click();
            Assert.IsTrue(driver.Title.Contains("VSTS"), "Verified title of the page");
        }

        [TestMethod]
        [TestCategory("Chrome")]
        public void TheBaiduSelectTest()
        {
            driver.Navigate().GoToUrl("http://tieba.baidu.com/f/search/adv");
            IList<IWebElement> listOption =
                //driver.FindElement(By.Name("sm")).FindElements(By.TagName("option"));
                driver.FindElements(By.CssSelector("[name=sm]>option"));

            string targetStr = "按相关性排序";

            foreach (var item in listOption)
            {
                if (item.Text == targetStr) item.Click();
            }

            Thread.Sleep(10000);
        }

        [TestMethod]
        [TestCategory("Chrome")]
        public void TheBaiduTextTest()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://www.baidu.com");

            IWebElement web = driver.FindElement(By.Id("setf"));
            Debug.WriteLine(web.Text);
        }

        [TestMethod]
        [TestCategory("Chrome")]
        public void TheBaiduSwitchWindowTest()
        {
            //IWebDriver driver = new ChromeDriver();//谷歌浏览器
            IWebDriver driver = new EventFiringWebDriver(new ChromeDriver());



            driver.Navigate().GoToUrl("http://tieba.baidu.com/f/search/adv");
            
            //找到注册元素
            IWebElement register = driver.FindElement(By.CssSelector("#com_userbar>ul>li:nth-child(5)>div>a"));
            register.Click();

            //显示所有标识
            IList<string> listHand = driver.WindowHandles;//拿到所有标识
            //切换到注册窗口再输入12345
            driver.SwitchTo().Window(listHand[1]);
            driver.FindElement(By.Name("userName")).SendKeys("12345");
        }

        #region 事件区

        private IList<string> listMeassage = new List<string>();

        /// <summary>
        /// 导航前发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"即将要跳转到的URL为：{e.Driver.Url}");
        }

        /// <summary>
        /// 导航后发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"跳转到的URL为：{e.Driver.Url}");
        }

        /// <summary>
        /// 查找元素前发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_FindingElement(object sender, FindElementEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");

            Debug.WriteLine($"即将查找的元素为：{e.FindMethod.ToString()}");

        }

        /// <summary>
        /// 查找元素后发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"找到元素，条件为：{e.FindMethod.ToString()}");

        }


        /// <summary>
        /// 单击元素前发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"要单击的元素的value属性为：{e.Element.GetAttribute("value")}");


        }

        /// <summary>
        /// 单击元素后发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_ElementClicked(object sender, WebElementEventArgs e)
        {
            System.Threading.Thread.Sleep(3 * 1000);//暂停3秒
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"单击元素后，现在的URL为：{e.Driver.Url}");

        }

        /// <summary>
        /// 单击元素前发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_ElementValueChanging(object sender, WebElementEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"元素更改前的值为：{e.Element.GetAttribute("value")}");

        }

        /// <summary>
        /// 单击元素后发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDriver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine($"元素更改后的值为：{e.Element.GetAttribute("value")}");

        }
        #endregion
    }
}