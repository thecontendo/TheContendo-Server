using System;
using System.Collections.Generic;
using System.Text;

namespace Contendo.Models
{
    public class ServerResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public ServerResponseMessage Message { get; set; }

        public ServerResponse()
        {
            Message = new ServerResponseMessage();
        }

        public ServerResponse(T data)
        {
            Message = new ServerResponseMessage();
            this.Data = data;
        }

        public void AddSuccess(string text = "")
        {
            this.Message.Code = "0";
            this.Message.Text = text == string.Empty ? "Success" : text;
            this.Success = true;
        }

        public void AddError(string text = "", string code = "")
        {
            this.Message.Code = code;
            this.Message.Text = text == string.Empty ?  "Error" : text;
            this.Success = false;
        }

        public void AddModelError()
        {
            this.Message.Code = "-1";
            this.Message.Text = "Model Validation Error: Please check the required fields...";
            this.Success = false;
        }

        public void AddNotFound()
        {
            this.Message.Code = "-2";
            this.Message.Text = "Cannot find the corresponding entity...";
            this.Success = false;
        }
    }
}