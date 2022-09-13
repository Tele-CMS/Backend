using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ProviderQuestionnaireModels
{
    public class QuestionnaireControlModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasOptions { get; set; }
        public string Key { get; set; }
    }
}
