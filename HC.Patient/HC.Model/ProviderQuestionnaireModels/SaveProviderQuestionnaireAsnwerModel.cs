using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ProviderQuestionnaireModels
{
    public class SaveProviderQuestionnaireAsnwerRequestModel
    {
        public int PatientAppointmentId { get; set; }
        public List<QuestionnaireAnswerModel> Answers { get; set; }
    }

    public class QuestionnaireAnswerModel
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }


}
