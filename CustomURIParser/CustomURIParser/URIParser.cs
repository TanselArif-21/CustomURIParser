using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomURIParser
{
    /// <summary> 
    ///  This class contains all the utility methods used for URI parsing. It is important to note that a URI has
    ///  4 main parts: 'scheme', 'authority', 'path', 'query', 'fragment'. These are separated by '//', '/', '?' and '#'
    ///  respectively. <see cref="https://tools.ietf.org/html/rfc3986"/>
    /// </summary> 
    public class URIParser : GenericUriParser
    {
        static string[] componentsArray = new string[5] { "scheme", "authority", "path", "query", "fragment" };
        protected StringBuilder stringError = new StringBuilder("");
        protected bool isAbsolute = true;
        protected string absoluteURI = "";

        public StringBuilder error
        {
            get { return stringError; }
        }

        public string AbsoluteURI
        {
            get { return absoluteURI; }
            set { absoluteURI = value; }
        }

        public bool absolute
        {
            get { return isAbsolute; }
            set { isAbsolute = value; }
        }
 
        public URIParser() : base(GenericUriParserOptions.Default)
        {
        }

        public URIParser(bool isAbsolute) : base(GenericUriParserOptions.Default)
        {
            absolute = isAbsolute;
        }

        public URIParser(bool isAbsolute, string absoluteURI) : base(GenericUriParserOptions.Default)
        {
            absolute = isAbsolute;
            AbsoluteURI = absoluteURI;
        }

        public void setUpDictionary(Dictionary<string, string> uriDict)
        {
            // Create the key-value pairs
            foreach (string key in componentsArray)
            {
                uriDict.Add(key, "");
            }
        }

        public Dictionary<string, string> parseUri(string uri)
        {
            Dictionary<string, string> uriDict = new Dictionary<string, string>();

            // Set up the dictionary
            setUpDictionary(uriDict);

            // Create a temporary uri string
            string tempUri = uri;

            // Parse tempUri
            if (isAbsolute)
            {
                try
                {
                    // If authority component is absent this may still be a valid uri
                    if (!tempUri.Contains("//"))
                    {
                        if (tempUri.Contains(":"))
                        {
                            string[] splitUri = tempUri.Split(new char[] { ':' }, 2);
                            tempUri = splitUri[0] + "://a/" + splitUri[1];
                            parseComponentsAbsolute(tempUri, uriDict);
                            uriDict["authority"] = "";
                        }
                    }
                    else
                    {
                        parseComponentsAbsolute(tempUri, uriDict);
                    }
                }
                catch(Exception e)
                {
                    if(e.Message == "Invalid URI: The format of the URI could not be determined.")
                    {
                        if(tempUri.IndexOf(':') == 0)
                        {
                            error.Append("Invalid URI: The scheme cannot be empty of an absolute URI.");
                        }
                        else
                        {
                            error.Append(e.Message);
                        }
                    }
                    else if (e.Message == "Invalid URI: The hostname could not be parsed.")
                    {
                        if (tempUri.Contains(":///"))
                        {
                            error.Append("Invalid URI: If the authority component is empty, remove the double slash.");
                        }
                        else
                        {
                            error.Append(e.ToString());
                        }
                    }
                    else
                    {
                        error.Append(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    // If absolute URI is provided
                    if (!string.IsNullOrEmpty(AbsoluteURI))
                    {
                        // If authority component is absent this may still be a valid uri. 
                        // So we add an authority component to the absolute URI and remove it again.
                        if (!AbsoluteURI.Contains("//"))
                        {
                            if (AbsoluteURI.Contains(":"))
                            {
                                string[] splitUri = AbsoluteURI.Split(new char[] { ':' }, 2);
                                string tempAbsoluteURI = splitUri[0] + "://a/" + splitUri[1];
                                parseComponentsRelative(tempUri, uriDict, tempAbsoluteURI);
                                uriDict["authority"] = "";
                            }
                        }
                        else
                        {
                            parseComponentsRelative(tempUri, uriDict, AbsoluteURI);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                catch(Exception e)
                {
                    if (e.Message == "Invalid URI: The format of the URI could not be determined.")
                    {
                        if (tempUri.IndexOf(':') == 0)
                        {
                            error.Append("Invalid URI: The scheme cannot be empty of an absolute URI.");
                        }
                        else
                        {
                            error.Append(e.Message);
                        }
                    }
                    else if (e.Message == "Invalid URI: The hostname could not be parsed.")
                    {
                        if (tempUri.Contains(":///"))
                        {
                            error.Append("Invalid URI: If the authority component is empty, remove the double slash.");
                        }
                        else
                        {
                            error.Append(e.ToString());
                        }
                    }
                    else
                    {
                        error.Append(e.Message);
                    }
                }
            }

            return uriDict;
        }

        /// <summary>
        /// This function uses the Uri class to parse absolute URIs
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="uriDict"></param>
        /// <param name="error"></param>
        public void parseComponentsAbsolute(string uri, Dictionary<string, string> uriDict)
        {
            Uri myUri = new Uri(uri,UriKind.Absolute);
            uriDict["scheme"] = myUri.Scheme;
            uriDict["authority"] = myUri.Authority;
            uriDict["path"] = myUri.AbsolutePath;
            uriDict["query"] = myUri.Query;
            uriDict["fragment"] = myUri.Fragment;
        }

        /// <summary>
        /// This function uses the Uri class to parse relative URIs if an absolute URI is given
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="uriDict"></param>
        /// <param name="error"></param>
        public void parseComponentsRelative(string uri, Dictionary<string, string> uriDict, string absoluteURI)
        {
            Uri absUri = new Uri(absoluteURI, UriKind.Absolute);
            Uri myUri = new Uri(absUri, uri);
            uriDict["scheme"] = myUri.Scheme;
            uriDict["authority"] = myUri.Authority;
            uriDict["path"] = myUri.AbsolutePath;
            uriDict["query"] = myUri.Query;
            uriDict["fragment"] = myUri.Fragment;
        }
    }
}
