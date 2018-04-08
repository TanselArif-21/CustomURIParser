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
        string[] parts = new string[] { "scheme", "authority", "path", "query", "fragment", "username", "password", "host", "port" };

        /// <summary>
        /// Simple absolute URI unit tests. These tests are meant to check whether CustomURIParser
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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "authority");
            expectedComponents.Add("port", "");

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
            expectedComponents["username"] = "";
            expectedComponents["password"] = "";
            expectedComponents["host"] = "authority";
            expectedComponents["port"] = "";

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
            expectedComponents["username"] = "";
            expectedComponents["password"] = "";
            expectedComponents["host"] = "auth";
            expectedComponents["port"] = "";

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
            expectedComponents["username"] = "";
            expectedComponents["password"] = "";
            expectedComponents["host"] = "auth";
            expectedComponents["port"] = "";

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
            expectedComponents["username"] = "";
            expectedComponents["password"] = "";
            expectedComponents["host"] = "auth";
            expectedComponents["port"] = "";

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
            expectedComponents["username"] = "";
            expectedComponents["password"] = "";
            expectedComponents["host"] = "tools.ietf.org";
            expectedComponents["port"] = "";

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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");
            
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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// Simple relative URI unit tests. These tests are meant to check whether CustomURIParse
        /// correctly parses simple RELATIVE URIs
        /// </summary>
        [TestMethod]
        public void parseTestRelativeMain1()
        {
            string schemeTest = "http";
            string authorityTest = "authority";
            string pathTest = "path2";
            string queryTest = "query";
            string fragmentTest = "fragment";

            URIParser MyParser = new URIParser(false, schemeTest + "://" + authorityTest);

            string uri = null;
            Dictionary<string, string> actualComponents = null;
            Dictionary<string, string> expectedComponents = null;

            // Test 1: relative URI <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 2: relative URI, query + fragment <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 3: relative URI, fragment <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4: relative URI, upper hierarchy path <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "./" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 5: relative URI, query <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 6: relative URI, path and query when there is authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "/" + pathTest + "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 7: relative URI, fragment only when there is an authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 8: relative URI, path and fragment with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 9: relative URI, path, query and fragment with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 10: relative URI, path (semi-colon delimited) without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = ";" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/;" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 11: relative URI, multiple paths (semi-colon delimited) with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + ";" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest + ";" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 12: relative URI, multiple paths (semi-colon delimited), query and fragment without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + ";" + pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest + ";" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 13: relative URI, empty path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 14: relative URI, current path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = ".";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 15: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "./";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 16: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 17: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 18: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 19: relative URI, previous level and path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 20: relative URI, up two paths with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 21: relative URI, up two paths and then path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../../" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// Simple relative URI unit tests. These tests are meant to check whether CustomURIParse
        /// correctly parses simple RELATIVE URIs
        /// </summary>
        [TestMethod]
        public void parseTestRelativeMain2()
        {
            string schemeTest = "http";
            string pathTest = "path2";
            string queryTest = "query";
            string fragmentTest = "fragment";

            URIParser MyParser = new URIParser(false, schemeTest + ":");

            string uri = null;
            Dictionary<string, string> actualComponents = null;
            Dictionary<string, string> expectedComponents = null;

            // Test 1: relative URI <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");


            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 2: relative URI, query + fragment <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 3: relative URI, fragment <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4: relative URI, path <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 5: relative URI, upper hierarchy path when there is no higher path <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "./" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 6: relative URI, query <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 7: relative URI, path and query when no authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 8: relative URI, fragment only <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 9: relative URI, path and fragment without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 10: relative URI, path, query and fragment without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 11: relative URI, path (semi-colon delimited) without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = ";" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/;" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 12: relative URI, multiple paths (semi-colon delimited) without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + ";" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest + ";" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 13: relative URI, multiple paths (semi-colon delimited), query and fragment without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + ";" + pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest + ";" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 14: relative URI, empty path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 15: relative URI, current path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = ".";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 16: relative URI, previous path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "./";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 16: relative URI, previous path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 17: relative URI, previous path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 18: relative URI, previous path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 19: relative URI, previous path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 20: relative URI, up two paths without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 21: relative URI, up two paths and then path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../../" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// Simple relative URI unit tests. These tests are meant to check whether CustomURIParse
        /// correctly parses simple RELATIVE URIs. Using a multi level absolute URI.
        /// </summary>
        [TestMethod]
        public void parseTestRelativeMain3()
        {
            string schemeTest = "http";
            string authorityTest = "authority";
            string pathTest = "path2";
            string queryTest = "query";
            string fragmentTest = "fragment";
            
            URIParser MyParser = new URIParser(false, schemeTest + "://" + authorityTest + "/a/b/c/d;p?q");

            string uri = null;
            Dictionary<string, string> actualComponents = null;
            Dictionary<string, string> expectedComponents = null;

            // Test 1: relative URI <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 2: relative URI, query + fragment <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/d;p");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 3: relative URI, fragment <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/d;p");
            expectedComponents.Add("query", "?q");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4: relative URI, upper hierarchy path <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "./" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 5: relative URI, query <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/d;p");
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 6: relative URI, path and query when there is authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "/" + pathTest + "?" + queryTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 7: relative URI, fragment only when there is an authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/d;p");
            expectedComponents.Add("query", "?q");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 8: relative URI, path and fragment with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 9: relative URI, path, query and fragment with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 10: relative URI, path (semi-colon delimited) without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = ";" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/;" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 11: relative URI, multiple paths (semi-colon delimited) with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + ";" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest + ";" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 12: relative URI, multiple paths (semi-colon delimited), query and fragment without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + ";" + pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest + ";" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 13: relative URI, empty path without authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/d;p");
            expectedComponents.Add("query", "?q");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 14: relative URI, current path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = ".";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 15: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "./";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 16: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 17: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 18: relative URI, previous path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 19: relative URI, previous level and path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 20: relative URI, up two paths with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 21: relative URI, up two paths and then path with authority <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "../../" + pathTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/" + pathTest);
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", authorityTest);
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// Simple relative URI unit tests. These tests are meant to check whether CustomURIParse
        /// correctly parses simple RELATIVE URIs. Using a multi level absolute URI. 
        /// <see cref="https://www.w3.org/2004/04/uri-rel-test.html"/> 
        /// </summary>
        [TestMethod]
        public void parseTestRelativeMain4()
        {
            URIParser MyParser = new URIParser(false, "http://a/b/c/d;p?q");

            string uri = null;
            Dictionary<string, string> actualComponents = null;
            Dictionary<string, string> expectedComponents = null;

            // Test 1
            uri = "g";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 2
            uri = "./g";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 3
            uri = "g/";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 4
            uri = "//g";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "g");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "g");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 5
            uri = "?y";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/d;p");
            expectedComponents.Add("query", "?y");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 6
            uri = "g?y";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g");
            expectedComponents.Add("query", "?y");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 7
            uri = "#s";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/d;p");
            expectedComponents.Add("query", "?q");
            expectedComponents.Add("fragment", "#s");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 8
            uri = "g#s";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "#s");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 9
            uri = "g?y#s";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g");
            expectedComponents.Add("query", "?y");
            expectedComponents.Add("fragment", "#s");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 10
            uri = ";x";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/;x");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 11
            uri = "g;x";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g;x");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 12
            uri = "g;x?y#s";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/g;x");
            expectedComponents.Add("query", "?y");
            expectedComponents.Add("fragment", "#s");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 13
            uri = "";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/d;p");
            expectedComponents.Add("query", "?q");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 14
            uri = ".";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 15
            uri = "./";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/c/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 16
            uri = "..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 17
            uri = "../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 18
            uri = "../g";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/b/g");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 19
            uri = "../..";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 20
            uri = "../../";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }

            // Test 21
            uri = "../../g";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "a");
            expectedComponents.Add("path", "/g");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "a");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// Simple relative URI unit tests. These tests are meant to check whether CustomURIParse
        /// correctly parses simple RELATIVE URIs. Using a multi level absolute URI. 
        /// <see cref="https://www.w3.org/2004/04/uri-rel-test.html"/> 
        /// </summary>
        [TestMethod]
        public void parseTestRelativeMain5()
        {
            URIParser MyParser = new URIParser(false, "http://a/b/c/d;p?q");

            string uri = null;
            Dictionary<string, string> actualComponents = null;
            Dictionary<string, string> expectedComponents = null;

            // Test 1
            uri = "gggg:h";

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "gggg");
            expectedComponents.Add("authority", "");
            expectedComponents.Add("path", "h");
            expectedComponents.Add("query", "");
            expectedComponents.Add("fragment", "");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// This test tests the validation capabilities of CustomURIParser when the application is
        /// absolute
        /// </summary>
        [TestMethod]
        public void parseTestValidateForAbsolute()
        {
            //VALIDATE SCHEME

            // Test 1
            string uri = "1http://authority/path?query#fragment";
            URIParser MyParser = new URIParser();
            string expectedResult = "Invalid URI: The URI scheme is not valid.";
            Dictionary<string, string> actualComponents = MyParser.parseUri(uri);
            Assert.AreEqual(expectedResult, MyParser.Error.ToString());

            // Test 2
            MyParser = new URIParser();
            uri = "h?ttp://authority/path1/path2?query#fragment";
            expectedResult = "Invalid URI: The URI scheme is not valid.";
            actualComponents = MyParser.parseUri(uri);
            Assert.AreEqual(expectedResult, MyParser.Error.ToString());
            
            Uri myUri = new Uri(uri, UriKind.Relative);

            // Test 3
            MyParser = new URIParser();
            uri = "://authority/path1/path2?query#fragment";
            expectedResult = "Invalid URI: The scheme cannot be empty of an absolute URI.";
            actualComponents = MyParser.parseUri(uri);
            Assert.AreEqual(expectedResult, MyParser.Error.ToString());

            // Test 4
            MyParser = new URIParser();
            uri = "http:///path1/path2?query#fragment";
            expectedResult = "Invalid URI: If the authority component is empty, remove the double slash.";
            actualComponents = MyParser.parseUri(uri);
            Assert.AreEqual(expectedResult, MyParser.Error.ToString());
        }

        /// <summary>
        /// This test tests the validation capabilities of CustomURIParser when the application is
        /// relative and the absolute URI is provided
        /// </summary>
        [TestMethod]
        public void parseTestValidateSchemeForRelativeWithAbsolute()
        {
            string schemeTest = "http";

            URIParser MyParser = new URIParser(false, schemeTest + ":");

            // Test 1
            //uri = "..";
            //expectedResult = "Invalid URI: The path is not valid.";
            //actualComponents = MyParser.parseUri(uri);
            //Assert.AreEqual(expectedResult, MyParser.Error.ToString());
        }

        /// <summary>
        /// This test tests the validation capabilities of CustomURIParser when the application is
        /// relative and the absolute URI is provided
        /// </summary>
        [TestMethod]
        public void parseTestFullAuthorityTest()
        {
            string schemeTest = "http";
            string authorityTest = "username:1234@host.com:123";
            string pathTest = "path2";
            string queryTest = "query";
            string fragmentTest = "fragment";

            URIParser MyParser = new URIParser(false, schemeTest + "://" + authorityTest + "/a/b/c/d;p?q");

            string uri = null;
            Dictionary<string, string> actualComponents = null;
            Dictionary<string, string> expectedComponents = null;

            // Test 1: relative URI <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = pathTest + "?" + queryTest + "#" + fragmentTest;

            expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", schemeTest);
            expectedComponents.Add("authority", authorityTest);
            expectedComponents.Add("path", "/a/b/c/" + pathTest);
            expectedComponents.Add("query", "?" + queryTest);
            expectedComponents.Add("fragment", "#" + fragmentTest);
            expectedComponents.Add("username", "username");
            expectedComponents.Add("password", "1234");
            expectedComponents.Add("host", "host.com");
            expectedComponents.Add("port", "123");

            actualComponents = MyParser.parseUri(uri);

            for (int i = 0; i < parts.Length; i++)
            {
                Assert.AreEqual(expectedComponents[parts[i]], actualComponents[parts[i]]);
            }
        }

        /// <summary>
        /// This test tests the validation capabilities of CustomURIParser when the application is
        /// relative and the absolute URI is provided
        /// </summary>
        [TestMethod]
        public void parseTestValidateURI()
        {
            string schemeTest = "http";
            string authorityTest = "username:1234@host.com:123";

            URIParser MyParser = new URIParser(false, schemeTest + "://" + authorityTest + "/a/b/c/d;p?q");
            
            string uri = null;
            bool expectedResult = false;
            bool actualResult = true;
            string expectedError = "Invalid URI: The hostname could not be parsed.; Invalid URI: The scheme cannot be empty of an absolute URI.\r\n";

            // Test 1: relative URI <see cref="https://tools.ietf.org/html/rfc3986#section-3.3"/> 
            uri = "///a/path?query#fragment";

            actualResult = MyParser.validateUri(uri, URIParser.validationType.Either);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedError.ToString(), MyParser.Error.ToString());
        }


        [TestMethod]
        public void justATest()
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
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "authority");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri(uri);
        }

        [TestMethod]
        public void justATest2()
        {
            URIParser MyParser = new URIParser(false, "http://username:1234@host.com:123/path2?Query#fragment");

            // Test 1: full absolute URI
            string uri = "/path1?query#fragment";
            Dictionary<string, string> actualComponents = null;

            Dictionary<string, string> expectedComponents = new Dictionary<string, string>();
            expectedComponents.Add("scheme", "http");
            expectedComponents.Add("authority", "authority");
            expectedComponents.Add("path", "/path1");
            expectedComponents.Add("query", "?query");
            expectedComponents.Add("fragment", "#fragment");
            expectedComponents.Add("username", "");
            expectedComponents.Add("password", "");
            expectedComponents.Add("host", "authority");
            expectedComponents.Add("port", "");

            actualComponents = MyParser.parseUri("http:a:b");
        }
    }
}
