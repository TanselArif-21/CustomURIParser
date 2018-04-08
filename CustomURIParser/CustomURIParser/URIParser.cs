using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomURIParser
{
    /// <summary> 
    ///  This is a class for URI parsing and determining if a URI is valid or not. It is important to note that a URI has
    ///  4 main parts: 'scheme', 'authority', 'path', 'query', 'fragment'. These are usually separated by '//', '/', '?' and '#'
    ///  respectively. <see cref="https://tools.ietf.org/html/rfc3986"/> for a full definition
    /// </summary> 
    public class URIParser : GenericUriParser
    {
        static string[] componentsArray = new string[9] { "scheme", "authority", "path", "query", "fragment", "username", "password", "host", "port" };
        protected StringBuilder error = new StringBuilder("");
        protected bool isAbsolute = true;
        protected bool isSilent = true;
        protected string absoluteURI = "";
        public enum validationType { Absolute, Relative, Either };

        public StringBuilder Error
        {
            get { return error; }
        }

        public string AbsoluteURI
        {
            get { return absoluteURI; }
            set { absoluteURI = value; }
        }

        public bool Absolute
        {
            get { return isAbsolute; }
            set { isAbsolute = value; }
        }

        public bool IsSilent
        {
            get { return isSilent; }
            set { isSilent = value; }
        }

        /// <summary>
        /// A default constructor
        /// </summary>
        public URIParser() : base(GenericUriParserOptions.Default)
        {
        }

        /// <summary>
        /// A constructor which sets the isAbsolute switch. This switch determines whether
        /// this parser is set up as an absolute parser or not
        /// </summary>
        /// <param name="isAbsolute">This parameter determines whether the class is set up as an absolute URI
        /// parser or a relative URI parser</param>
        public URIParser(bool isAbsolute) : base(GenericUriParserOptions.Default)
        {
            Absolute = isAbsolute;
        }

        /// <summary>
        /// A constructor which set the isAbsolute switch as well as enabling the user to
        /// provide an absolute URI as a reference
        /// </summary>
        /// <param name="isAbsolute">This parameter determines whether the class is set up as an absolute URI
        /// parser or a relative URI parser</param>
        /// <param name="absoluteURI">This sets the aboslute URI. This constructor is overloaded so that this
        /// parameter may not be specified even if the class is to be set up as a relative parser
        /// </param>
        public URIParser(bool isAbsolute, string absoluteURI) : base(GenericUriParserOptions.Default)
        {
            Absolute = isAbsolute;
            AbsoluteURI = absoluteURI;
        }

        /// <summary>
        /// A method to set up a generic dicitonary for use by the URIParser
        /// </summary>
        /// <param name="uriDict">The dictionary to set up according to the components array</param>
        public void setUpDictionary(Dictionary<string, string> uriDict)
        {
            // Create the key-value pairs
            foreach (string key in componentsArray)
            {
                uriDict.Add(key, "");
            }
        }

        /// <summary>
        /// Main method to parse URIs
        /// </summary>
        /// <param name="uri">The string URI to be parsed</param>
        /// <returns></returns>
        public Dictionary<string, string> parseUri(string uri)
        {
            Dictionary<string, string> uriDict = new Dictionary<string, string>();
            Error.Clear();

            // Set up the dictionary
            setUpDictionary(uriDict);

            // Create a temporary uri string
            string tempUri = uri;

            // Parse tempUri
            if (isAbsolute)
            {
                parseAbsolute(tempUri, uriDict);
            }
            else
            {
                parseRelative(tempUri, uriDict);
            }

            if(!IsSilent)
                logComponents(uriDict);

            return uriDict;
        }

        /// <summary>
        /// This method uses the Uri class to parse absolute URIs and populate the dictionary
        /// to be returned to the user
        /// </summary>
        /// <param name="uri">The string URI to be parsed</param>
        /// <param name="uriDict">A dictionary to return the component of the given URI</param>
        public void parseComponentsAbsolute(string uri, Dictionary<string, string> uriDict)
        {
            Uri myUri = new Uri(uri,UriKind.Absolute);
            if(myUri.Scheme == "file" && !uri.Contains("file:"))
            {
                throw new Exception("Invalid URI: The scheme cannot be empty of an absolute URI.");
            }
            else
            {
                uriDict["scheme"] = myUri.Scheme;
            }

            if (string.IsNullOrEmpty(myUri.UserInfo))
            {
                uriDict["authority"] = myUri.Authority;
            }
            else
            {
                uriDict["authority"] = myUri.UserInfo + "@" + myUri.Authority;
            }
            
            uriDict["path"] = myUri.AbsolutePath;
            uriDict["query"] = myUri.Query;
            uriDict["fragment"] = myUri.Fragment;
            uriDict["host"] = myUri.Host;

            // Do not use a default port
            if (myUri.IsDefaultPort)
            {
                uriDict["port"] = "";
            }
            else
            {
                uriDict["port"] = myUri.Port.ToString();
            }

            // Split up the user info field
            if (myUri.UserInfo.Contains(':'))
            {
                uriDict["username"] = myUri.UserInfo.Split(new char[] { ':' }, 2)[0];
                uriDict["password"] = myUri.UserInfo.Split(new char[] { ':' }, 2)[1];
            }
            else
            {
                uriDict["username"] = myUri.UserInfo.Split(new char[] { ':' }, 2)[0];
                uriDict["password"] = "";
            }
        }

        /// <summary>
        /// This method uses the Uri class to parse relative URIs if an absolute URI is given
        /// and populates the dictionary to be returned to the user
        /// </summary>
        /// <param name="uri">The string URI to be parsed</param>
        /// <param name="uriDict">A dictionary to return the component of the given URI</param>
        /// <param name="absoluteURI">A string URI to be used as the absolute reference for the parsing</param>
        public void parseComponentsRelative(string uri, Dictionary<string, string> uriDict, string absoluteURI)
        {
            Uri absUri = new Uri(absoluteURI, UriKind.Absolute);
            Uri myUri = new Uri(absUri, uri);
            uriDict["scheme"] = myUri.Scheme;

            if (string.IsNullOrEmpty(myUri.UserInfo))
            {
                uriDict["authority"] = myUri.Authority;
            }
            else
            {
                uriDict["authority"] = myUri.UserInfo + "@" + myUri.Authority;
            }

            uriDict["path"] = myUri.AbsolutePath;
            uriDict["query"] = myUri.Query;
            uriDict["fragment"] = myUri.Fragment;
            uriDict["host"] = myUri.Host;
            uriDict["port"] = myUri.Port.ToString();

            // Do not use a default port
            if (myUri.IsDefaultPort)
            {
                uriDict["port"] = "";
            }
            else
            {
                uriDict["port"] = myUri.Port.ToString();
            }

            // Split up the user info field
            if (myUri.UserInfo.Contains(':'))
            {
                uriDict["username"] = myUri.UserInfo.Split(new char[] { ':' }, 2)[0];
                uriDict["password"] = myUri.UserInfo.Split(new char[] { ':' }, 2)[1];
            }
            else
            {
                uriDict["username"] = myUri.UserInfo.Split(new char[] { ':' }, 2)[0];
                uriDict["password"] = "";
            }
        }

        /// <summary>
        /// This method deals with the URI parsing for absolute URIs
        /// </summary>
        /// <param name="tempUri">The string URI to be parsed</param>
        /// <param name="uriDict">A dictionary to return the component of the given URI</param>
        public void parseAbsolute(string tempUri, Dictionary<string, string> uriDict)
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
                        uriDict["host"] = "";
                    }
                    else
                    {
                        throw new Exception("Invalid URI: This is not an absolute URI.");
                    }
                }
                else
                {
                    parseComponentsAbsolute(tempUri, uriDict);
                }
            }
            catch (Exception e)
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

        /// <summary>
        /// This method deals with the URI parsing for relative URIs. In the case of a relative URI
        /// an absolute URI may have been provided for which this URI is using as a reference. If no 
        /// absolute URI is provided as reference, the caller is assuming that a basic absolute URI
        /// is the reference 
        /// </summary>
        /// <param name="tempUri">The string URI to parse</param>
        /// <param name="uriDict">A dictionary to return the component of the given URI</param>
        public void parseRelative(string tempUri, Dictionary<string, string> uriDict)
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
                            uriDict["host"] = "";
                        }
                    }
                    else
                    {
                        parseComponentsRelative(tempUri, uriDict, AbsoluteURI);
                    }
                }
                else
                {
                    // An absolute URI was not given so we create one
                    string tempAbsoluteURI = "http://authority";

                    parseComponentsRelative(tempUri, uriDict, tempAbsoluteURI);

                    // An absolute URI was not given so it doesn't make sense to report any reference to an absolute URI
                    uriDict["scheme"] = "";
                    uriDict["authority"] = "";
                    uriDict["host"] = "";
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Invalid URI: The format of the URI could not be determined.")
                {
                    if (AbsoluteURI.IndexOf(':') == 0)
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
                    if (AbsoluteURI.Contains(":///"))
                    {
                        error.Append("Invalid URI: If the authority component is empty, remove the double slash.");
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

        /// <summary>
        /// This method validates a given URI. It returns true if the URI is valid and false otherwise.
        /// In the latter case, the Error string will contain the reasons why it is not valid. The result
        /// depends on the desiredValidationType argument which determines if this URI should be judged
        /// according to Absolute URI definition, Relative URI definition or either.
        /// </summary>
        /// <param name="uri">The URI string to validate</param>
        /// <param name="desiredValidationType">A switch for the validation type. Values are Absolute, Relative and Either. 
        /// This parameter determines whether to check if a URI is valid as an absolute uri, relative uri or either of the two.</param>
        /// <returns>bool</returns>
        public bool validateUri(string uri, validationType desiredValidationType)
        {
            Dictionary<string, string> uriDict = new Dictionary<string, string>();
            string tempError = "";
            // Set up the dictionary
            setUpDictionary(uriDict);

            if (desiredValidationType == validationType.Absolute || desiredValidationType == validationType.Either)
            {
                // Check if this is a valid URI assuming it is absolute
                parseAbsolute(uri, uriDict);

                if (string.IsNullOrEmpty(error.ToString()))
                {
                    return true;
                }
            }

            if (desiredValidationType == validationType.Relative || desiredValidationType == validationType.Either)
            {
                tempError = Error.ToString();
                Error.Clear();
                // Check if this is a valid URI assuming it is absolute
                parseRelative(uri, uriDict);

                if (string.IsNullOrEmpty(error.ToString()))
                {
                    return true;
                }
                else
                {
                    Error.AppendLine("; " + tempError);
                }
            }

            return false;
        }

        /// <summary>
        /// This method writes the component information to the output log. The method can be overrided
        /// as part of extensibility work
        /// </summary>
        public virtual void logComponents(Dictionary<string, string> uriDict)
        {
            for (int i = 0; i < componentsArray.Length; i++)
            {
                Console.WriteLine(uriDict[componentsArray[i]]);
            }
        }
    }
}
