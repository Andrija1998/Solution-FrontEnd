using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Linq;
namespace AutomatedSeleniumUITests
{
    public class Tests
    {
        private const string Url = "https://contactbook.softuniqa.repl.co/";
        private WebDriver driver;

        [OneTimeSetUp]
        public void Start()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        [Test]
        public void NameCheck_SteveJobs_Albert()
        {
            //Goigng to the URL
            driver.Navigate().GoToUrl(Url);
            var contscts = driver.FindElement(By.XPath("//a[@href='/contacts'][contains(.,'Contacts')]"));
            contscts.Click();

            //Checking if the first contact is Steve Jobs
            var firstName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td"));
            Assert.That(firstName.Text, Is.EqualTo("Steve"));
            var lastName = driver.FindElement(By.XPath("(//td[contains(.,'Jobs')])[1]"));
            Assert.That(lastName.Text, Is.EqualTo("Jobs"));

            //Goign back to Home page and clicking on search again
            var homeButton = driver.FindElement(By.XPath("//a[@href='/'][contains(.,'Home')]"));
            homeButton.Click();
            var searchContacts = driver.FindElement(By.XPath("//span[@class='icon'][contains(.,'⌕')]"));
            searchContacts.Click();
            var searchField = driver.FindElement(By.XPath("//input[contains(@id,'keyword')]"));
            searchField.SendKeys("albert");

            //Checking that afther clicking on search the contack dosplayed is Albert Einstain
            var clickSearch = driver.FindElement(By.XPath("//button[contains(@id,'search')]"));
            clickSearch.Click();

            //Switching the way of finding Elements using CSS selector modified in the Console using $$("CSS selector path")
            var nameAlbert = driver.FindElement(By.CssSelector("tbody > tr.fname > td"));
            Assert.That(nameAlbert.Text, Is.EqualTo("Albert"));
            var lastNameAlbert = driver.FindElement(By.CssSelector("tbody > tr.lname > td"));
            Assert.That(lastNameAlbert.Text, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Error_Display()
        {
            driver.Navigate().GoToUrl(Url);
            var searchContacts = driver.FindElement(By.XPath("//span[@class='icon'][contains(.,'⌕')]"));
            searchContacts.Click();

            //Searching for invalid name and checking if Error Massage appears
            var searchField = driver.FindElement(By.XPath("//input[contains(@id,'keyword')]"));
            searchField.SendKeys("missing{randnum}");
            var clickSearch = driver.FindElement(By.XPath("//button[contains(@id,'search')]"));
            clickSearch.Click();
            var errorMessage = driver.FindElement(By.Id("searchResult"));
            Assert.That(errorMessage.Text, Is.EqualTo("No contacts found."));
        }

        [Test]
        public void Adding_Invalid_Contact()
        {
            driver.Navigate().GoToUrl(Url);
            var createButton = driver.FindElement(By.XPath("(//a[@href='/contacts/create'][contains(.,'Create')])[1]"));
            createButton.Click();

            //List of elements to use
            var FirstName = driver.FindElement(By.Id("firstName"));
            var LastName = driver.FindElement(By.Id("lastName"));
            var Email = driver.FindElement(By.Id("email"));
            var Phone = driver.FindElement(By.Id("phone"));
            var Comments = driver.FindElement(By.Id("comments"));

            //This can also be done using a [TestCase] if we needed to add more Contacts with just adding more cases to the test and writeing different names
            //FirstName is going to be enpty in order to get Error message displayed
            LastName.SendKeys("Bozilovic");
            Email.SendKeys("andrija.bozilovic@gmail.com");
            Phone.SendKeys("*0000#");
            Comments.SendKeys("Hard Working Individual");
            //Click button Create
            var createButtonClick = driver.FindElement(By.Id("create"));
            createButtonClick.Click();
            //Checking if we have the Error Message
            var errorMessage = driver.FindElement(By.CssSelector("body > main > div"));
            Assert.That(errorMessage.Text, Is.EqualTo("Error: First name cannot be empty!"));
        }

        [Test]
        public void Adding_Valid_Contact()
        {
            driver.Navigate().GoToUrl(Url);
            var createButton = driver.FindElement(By.XPath("(//a[@href='/contacts/create'][contains(.,'Create')])[1]"));
            createButton.Click();

            //Elements to use
            var FirstName = driver.FindElement(By.Id("firstName"));
            var LastName = driver.FindElement(By.Id("lastName"));
            var Email = driver.FindElement(By.Id("email"));
            var Phone = driver.FindElement(By.Id("phone"));
            var Comments = driver.FindElement(By.Id("comments"));

            FirstName.SendKeys("Andrija");
            LastName.SendKeys("Bozilovic");
            Email.SendKeys("andrija.bozilovic@gmail.com");
            Phone.SendKeys("*0000#");
            Comments.SendKeys("Hard Working Individual");
            var createkontakt = driver.FindElement(By.Id("create"));
            createkontakt.Click();

            //Checking if the last contsct added maches our Contact
            var FirstNameNew = driver.FindElements(By.CssSelector("tbody > tr.fname > td")).Last();
            Assert.That(FirstNameNew.Text, Is.EqualTo("Andrija"));
        }
    }
}

//TEST ENDS HERE IN ORDER FOR IT TO WORK EVERYTHING BELOW NEEDS TO BE COMMENTED 

//Now i wanted to display some additional inforamtion if we plan on testing the same things unsing "Windows Application" and "Moblile Application"
//In order for the Moblile Aplication to work we will need to use an Application called Appium Server GUI
//So that we can connect our test with the Moblile or Windows Aplication.
//The main difference will be the Setup.
//To provide enough information for the Moblile application to run we need to use something like this:
// ***IMPORTANT*** i wont be putting comments on the setup so that it is easier to see what needs to be changed and install any NuGet Pacage required
//I will list the important Imports which are mandatory for the test to work.
//Also to be able to simulate a Phone we will use Android Studio im my case that was a "Nexus6"
//To Create a Simulation you will need to go to ***File>New>New Project*** select for example "Empty Activity" and setup you devices prefered settings



**APPIUM MOBILE TESTING SETUP**
//NuGet Packages for Mobile Testing: AppiumWebDriver

using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
public class AppiumMobileTests
{
    private AndroidDriver<AndroidElement> driver;
    private AppiumOptions options;

    private const string appLocation = @"Path to your .apk file";
    private const string appiumServer = "Server most commonly used, [127.0.0.1:4723/wd/hub]";

    [SetUp]
    public void AppPreparation()
    {
        this.options = new AppiumOptions() { PlatformName = "Here you will type your Platform for us this will be called "Android"" };
        options.AddAdditionalCapability("app", appLocation);
        driver = new AndroidDriver<AndroidElement>(new Uri(appiumServer), options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }
}

    [TearDown]
    public void CloseApp()
    {
        driver.Quit();
    }
    //Finding Elements is most likely the same but we will be using an APP called Appium Inspector to enable us to find the location of Elements we need.
    //Here we will need to do a fiew setups.
    //Our remote host "127.0.0.1"
    //Remote Port "4723"
    //Remote Path "/wd/hub"
    //On the tab Desired Capabilities we will need 3 Capabilities: "platformName" in our case "Android" + "app" location of the Applications .apk file + "deviceName" in our case "Nexus6"
    //Finding elements is the easiest part by just clicking the element you want to inspect when your device is shown on the Appium Inspector App

    **APPIUM DESKTOP TESTING SETUP**
    //NuGet Packages for Windows Testing: AppiumWebDriver

//Now for the Windows Application we will need a program called WinAppDriverUiRecorder for us to be able to see the Elements
// Locating the elements is the easier part by pressing the Record Button and clicking on Whatever element in the Windows app you want to locate
//SetUp for Windows looks something like this:
    public class AppiumDesktopTests
    {
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"Location of your .exe file of the Application you want to open";
        private const string appiumServer = "Server most commonly used, [127.0.0.1:4723/wd/hub]";
        private const string appServer = "";

        [SetUp]
        public void AppPreparation()
        {
            this.options = new AppiumOptions();
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }
    }
        //Please note that the test where not written because of development on the Windows APP and Mobile APP because it was lacking Element ID's and the Xpaths and CSS Selectors provided were bugged out
        //Hopefully i have provided you with some usefull information on how to execute SeleniumTests, AppiumMobileTests and AppiumDesktopTests.