using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class Response<T>
    {
        public string? ErrorMsg {get;set;}
        public T? RetVal { get;set;}
        /// <summary>
        /// This constuctor intiates a response object with null values
        /// </summary>
        public Response() { }
        /// <summary>
        /// This constuctor intiates a response object with an error message
        /// </summary>
        /// <param name="msg">The error message</param>
        public Response(string msg)
        {
            ErrorMsg = msg;
        }
        /// <summary>
        /// This constuctor intiates a response object with an error message and\or a generic value
        /// </summary>
        /// <param name="msg">The error message</param>
        /// <param name="retVal">The generic return value</param>
        public Response(string msg, T retVal)
        {
            ErrorMsg = msg;
            RetVal = retVal;
        }
    }
}
