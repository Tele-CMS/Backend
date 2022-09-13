using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SecurityQuestion
{
    public class SecurityQuestionModel
    {
        public int Id { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }

    }

    public class SecurityQuestionListModel
    {           
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public List<SecurityQuestionModel> SecurityQuestionList { get; set; }
    }

}
