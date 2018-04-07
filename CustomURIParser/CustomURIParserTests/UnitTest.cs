using System;
using System.Collections.Generic;
using System.Text;
using CustomURIParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomURIParserTests
{
    /// <summary> 
    ///  This class stores all the tests that should be run against CustomURIParser and all its functions.
    /// </summary> 
    [TestClass]
    public class UnitTest
    {
        string[] parts = new string[] { "scheme", "authority", "path", "query", "fragment" };

        //[TestMethod]
        //public void justATest()
        //{
        //    // Test 1
        //    string uri = "http:/a/b/c";
        //    Uri myUri = new Uri(uri);
        //    Uri myBaseUri = new Uri("http:/a/b/c",UriKind.Absolute);
        //    myUri = new Uri(myBaseUri,uri);
        //}

        /// <summary>
        /// Simple absolute URI unit tests. These tests are meant to check whether CustomURIParse
        /// correctly parses simple ABSOLUTE URIs
        /// </summary>
        [TestMethod]
        public void parseTestAbsoluteMain()
        {
            URIParser MyParser = new URIParser();

            // Test 1: full absolute URI
            string uri = "http://authority/path1?query#fragment";
            Dictionary<string, string> actualComponents = null;

            Dictionary<string, string> expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "authority");
            expectedComponents.Add("path", "/path1");
            expectedComponents.Add("query", "?query");
            expectedComponents.Add("fragment", "#fragment");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 2: full absolute URI with 2 paths
            uri = "http://authority/path1/path2?query#fragment";

            expectedComponents["scheme"] = "http";
            expectedComponents["authority"] = "authority";
            expectedComponents["path"] = "/path1/path2";
            expectedComponents["query"] = "?query";
            expectedComponents["fragment"] = "#fragment";

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 3: full absolute URI with delimiter : in path
            uri = "http://auth/path1:path2?query#fragment";

            expectedComponents["scheme"] = "http";
            expectedComponents["authority"] = "auth";
            expectedComponents["path"] = "/path1:path2";
            expectedComponents["query"] = "?query";
            expectedComponents["fragment"] = "#fragment";

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4: absolute URI with no fragment
            uri = "http://auth/path1:path2?query";

            expectedComponents["scheme"] = "http";
            expectedComponents["authority"] = "auth";
            expectedComponents["path"] = "/path1:path2";
            expectedComponents["query"] = "?query";
            expectedComponents["fragment"] = "";

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 5: absolute URI with no path and no fragment
            uri = "http://auth?query";

            expectedComponents["scheme"] = "http";
            expectedComponents["authority"] = "auth";
            expectedComponents["path"] = "/";
            expectedComponents["query"] = "?query";
            expectedComponents["fragment"] = "";

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 6: absolute URI with no path and no fragment
            uri = "https://tools.ietf.org/html/rfc3986#section-3.3";

            expectedComponents["scheme"] = "https";
            expectedComponents["authority"] = "tools.ietf.org";
            expectedComponents["path"] = "/html/rfc3986";
            expectedComponents["query"] = "";
            expectedComponents["fragment"] = "#section-3.3";

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// special absolute URI unit tests. These tests are meant to check whether CustomURIParser
        /// correctly parses special ABSOLUTE URIs
        /// </summary>
        [TestMethod]
        public void parseTestAbsoluteCornerCases()
        {
            URIParser MyParser = new URIParser();
            string uri = null;
            Dictionary<string, string> actualComponents = null;
            string schemeTest = "http";
            string authorityTest = "authority";
            string pathTest = "path";
            string queryTest = "query";
            string fragmentTest = "fragment";

            // Test 1: absolute uri with no authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = schemeTest + ":" + pathTest + "?" + queryTest + "#" + fragmentTest;

            Dictionary<string, string> expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 2: absolute uri with no authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = schemeTest + ":";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 3: absolute uri with authority but empty path <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = schemeTest + ":" + "//" + authorityTest + "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4: absolute uri with no authority but with query <see cref="https://tools.ietf.org/html/rfc3986#section-3.4"/>
            // There is no mention of whether a query component technically needs a non-empty path component
            uri = schemeTest + ":" + "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4: absolute uri with no authority but with query <see cref="https://tools.ietf.org/html/rfc3986#section-3.5"/>
            // There is no mention of whether a fragment component technically needs a non-empty path component
            uri = schemeTest + ":" + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }
    }
}
