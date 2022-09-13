using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Common
{
    public class PasswordCheckModel
    {
        public PasswordCheckModel() { }
        public PasswordCheckModel(string message,bool status,string code="", int statusCode=0, int leftDays = 0)
        {
            Message = message;
            Status = status;
            ColorCode = code;
            StatusCode = statusCode;
            LeftDays = leftDays;
        }
        public string Message { get; set; }
        public bool Status { get; set; }
        public string ColorCode { get; set; }
        public int StatusCode { get; set; }
        public int LeftDays { get; set; }
    }
}
