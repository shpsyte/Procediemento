using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Functions
{
    public class Retorno
    {
        public Retorno()
        {
        }

        public Retorno(char isError, char isSuccess, string Message, string Controller, string Action, string View)
        {
            this.isError = isError;
            this.isSuccess = isSuccess;
            this.Message = Message;
            this.Controller = Controller;
            this.Action = Action;
            this.View = View;
        }

        public char isError { get; set; }
        public char isSuccess { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string View { get; set; }
    }
}