﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomURIParser
{
    /// <summary> 
    ///  This class contains all the utility methods used for URI parsing. It is important to note that a URI has
    ///  4 main parts: 'scheme', 'authority', 'path', 'query', 'fragment'. These are separated by '//', '/', '?' and '#'
    ///  respectively.
    /// </summary> 
    public class URIParser : GenericUriParser
    {
        static string[] componentsArray = new string[5] { "scheme", "authority", "path", "query", "fragment" };
        protected StringBuilder stringError = new StringBuilder("");
        protected bool isAbsolute = true;

        public StringBuilder error
        {
            get { return stringError; }
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
                    parseComponentsAbsolute(tempUri, uriDict);
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
                    else
                    {
                        error.Append(e.Message);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
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
    }
}
