namespace dform.NET_Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;

    using dform.NET;
    using dform.NET_Test.StubClasses;

    /// <summary>
    /// Tests the serialization and execution of each of the dform field types.
    /// 
    /// </summary>
    [TestClass]
    public class SerializerAndExecutionTests
    {
        private delegate void TestFunction(IJavaScriptExecutor js);

        [TestMethod]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\IEDriverServer.exe")]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\chromedriver.exe")]
        public void TextSerializationTest()
        {
            TestInAllBrowsers(_TextSerializationTest);

        }

        private void _TextSerializationTest(IJavaScriptExecutor js)
        {
            var test = new TextInput();
            string json = DFormSerializer.serialize(test);
            Assert.IsFalse(String.IsNullOrEmpty(json));
            Assert.IsTrue(js.CreateDForm(json));
            IWebDriver driver = js as IWebDriver;

            IWebElement testTextInput = driver.FindElement(By.Id("test_id"));
            Assert.AreEqual(testTextInput.GetAttribute("value"), test.Text, "value was not the same as test objects!");
            Assert.AreEqual(testTextInput.TagName.ToLower(), "input", "text input field tag was not input it was: " + testTextInput.TagName);
            Assert.IsTrue(testTextInput.GetAttribute("class").Contains("test_class"), "text input class was not correct it was " + testTextInput.GetAttribute("class"));
            Assert.AreEqual(testTextInput.GetAttribute("name"), "test_name", "text input name was not correct it was " + testTextInput.GetAttribute("name"));
  
            
        }

        [TestMethod]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\IEDriverServer.exe")]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\chromedriver.exe")]
        public void FormTest()
        {
            TestInAllBrowsers(_FormTest);

        }

        private void _FormTest(IJavaScriptExecutor js)
        {
            var form = new Form();
            string json = DFormSerializer.serialize(form);
            Assert.IsFalse(String.IsNullOrEmpty(json));
            Assert.IsTrue(js.CreateDForm(json));
            IWebDriver driver = js as IWebDriver;

            IWebElement testTextInput = driver.FindElement(By.Id("test_form"));
            Assert.AreEqual(testTextInput.TagName.ToLower(),"form","form tag was not form! it was:" + testTextInput.TagName);
        }

        [TestMethod]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\IEDriverServer.exe")]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\chromedriver.exe")]
        public void RadioTest()
        {
            TestInAllBrowsers(_RadioTest);

        }

        private void _RadioTest(IJavaScriptExecutor js)
        {
            var test = new RadioInput();
            string json = DFormSerializer.serialize(test);
            Assert.IsFalse(String.IsNullOrEmpty(json));
            Assert.IsTrue(js.CreateDForm(json));
            IWebDriver driver = js as IWebDriver;

            IWebElement testTextInput = driver.FindElement(By.Id("radio_test"));
            Assert.AreEqual(testTextInput.TagName.ToLower(), "input", "1st radio tag name was" + testTextInput.TagName + " but is meant to be input");
            Assert.IsTrue(testTextInput.Selected,"1st radio was not selected");
            testTextInput = driver.FindElement(By.Id("radio_test_1"));
            Assert.AreEqual(testTextInput.TagName.ToLower(), "input", "2nd radio tag name was" + testTextInput.TagName + " but is meant to be input");
            Assert.IsFalse(testTextInput.Selected,"2nd radio was selected");
        }

        [TestMethod]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\IEDriverServer.exe")]
        [DeploymentItem(@"D:\Programming\dform.net\dform.NET_Test\chromedriver.exe")]
        public void CheckboxTest()
        {
            TestInAllBrowsers(_CheckboxTest);

        }

        private void _CheckboxTest(IJavaScriptExecutor js)
        {
            var test = new CheckboxInput();
            string json = DFormSerializer.serialize(test);
            Assert.IsFalse(String.IsNullOrEmpty(json));
            Assert.IsTrue(js.CreateDForm(json));
            IWebDriver driver = js as IWebDriver;

            IWebElement testTextInput = driver.FindElement(By.Id("checkbox1"));
            Assert.AreEqual(testTextInput.TagName.ToLower(), "input", "1st checkbox tag name was" + testTextInput.TagName + " but is meant to be input");
            Assert.IsTrue(testTextInput.Selected, "1st checkbox was not selected");
            Assert.AreEqual(testTextInput.GetAttribute("type"), "checkbox");
            testTextInput = driver.FindElement(By.Id("checkbox2"));
            Assert.AreEqual(testTextInput.TagName.ToLower(), "input", "2nd checkbox tag name was" + testTextInput.TagName + " but is meant to be input");
            Assert.IsFalse(testTextInput.Selected, "2nd checkbox was selected");
            Assert.AreEqual(testTextInput.GetAttribute("type"), "checkbox");
        }

        #region "Worker Methods"

        private void TestInAllBrowsers(TestFunction function)
        {
            List<string> errors = new List<string>();
            IWebDriver driver = null;
            IJavaScriptExecutor js;
            try
            {
                using (driver = SetupDriver(new FirefoxDriver()))
                {
                    js = driver as IJavaScriptExecutor;
                    function(js);
                    driver.Close();
                    driver = null;
                }
            }
            catch (Exception ex)
            {
                errors.Add("Firefox Error:" + Environment.NewLine + ex.Message);
            }
            try
              {
                
                  using (driver = SetupDriver(new InternetExplorerDriver()))
                  {
                      js = driver as IJavaScriptExecutor;
                      function(js);
                      driver.Close();
                  }
              }
              catch (Exception ex)
              {
                  errors.Add("IE error:" + Environment.NewLine + ex.Message);
              }

            /*try
            {
                using (driver = SetupDriver(new ChromeDriver()))
                {
                    js = driver as IJavaScriptExecutor;
                    function(js);
                    driver.Close();
                }
            }
            catch (Exception ex)
            {
                errors.Add("Chrome Errors:" + Environment.NewLine + ex.Message);
            }
         

            try
            {  
       
                using (driver = SetupDriver(new SafariDriver()))
                {
                    js = driver as IJavaScriptExecutor;
                    function(js);
                    driver.Close();
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }*/

            if (errors.Any())
                Assert.Fail(errors.Aggregate((s, x) => s + Environment.NewLine + x));

        }

        
        private static IWebDriver SetupDriver(IWebDriver driver)
        {
            Contract.Ensures(driver != null);
            var exe = driver as IJavaScriptExecutor;
            if (exe == null)
                throw new AssertInconclusiveException("Could not setup driver (driver is null)");
            using (var reader = new StreamReader(File.OpenRead(@"D:\Programming\dform.net\dform.NET_Test\lib\jquery-1.7.2.js")))
            {
                exe.ExecuteScript(reader.ReadToEnd());
            }
            using (var reader = new StreamReader(File.OpenRead(@"D:\Programming\dform.net\dform.NET_Test\lib\jquery.dform-1.0.0.nolib.min.js")))
            {
                exe.ExecuteScript(reader.ReadToEnd());
            }
            return driver;
        }

        #endregion
    }
    public static class JavascriptExts
    {
        public static void AddValue(this IJavaScriptExecutor js, string id, string value)
        {
            AddValue(js, id, value, "hidden");
        }

        public static void AddValue(this IJavaScriptExecutor js, string id, string value, string simType)
        {
            js.ExecuteScript("if (typeof testcontext == 'undefined'){ testcontext = {}; testcontext.fieldContext = {}; testcontext.fieldContext.elements = []; };" +
                             "testcontext.fieldContext.elements[0] = {id:'" + id + "',tagName:'input',type:'" + simType + "',value:'" + value + "'};" +
                             "if ($('#" + id + "').length == 0)" +
                                "$('body').append('<hidden id=\"" + id + "\" value=\"" + value + "\" />');" +
                             "else " +
                                "$('#" + id + "').val('" + value + "');");

        }

        public static bool CreateDForm(this IJavaScriptExecutor js, string json)
        {
            var toRet = js.ExecuteScript(" try {var parsedForm = $.parseJSON('" + json + "'); } catch(e) { return 'Could not parse json'; }"
                           + " $('body').dform(parsedForm); ");
            return String.IsNullOrEmpty(toRet as string);
        }

    }

}

