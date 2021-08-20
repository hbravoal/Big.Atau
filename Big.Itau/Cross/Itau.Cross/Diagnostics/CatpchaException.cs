using System;

namespace Itau.Common.Diagnostics
{
    public class CatpchaException : System.Exception
    {
        public CatpchaException() : base()
        {
        }

        public CatpchaException(string message)
           : base(String.Format("Error in Trying  Captcha {0}", message))
        {
        }
    }
}