using HC.Patient.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HC.Patient.Model.ProviderQuestionnaireModels
{
    public class ProviderQuestionnaireControlModel
    {
        public int QuestionId { get; set; }
        public int StaffID { get; set; }
        public int QuestionnaireTypeControlId { get; set; }
        public  DynamicControlModel Control { get; set; }
        public string OptionsString { get => (Control == null || Control.Options == null || Control.Options.Count == 0) ? "" : string.Join(", ",Control.Options.Select(s => s.Key)); }
        public string ControlName { get => Control == null ? "" : Control.ControlName; }
        public string QuestionText { get => Control == null ? "" : Control.QuestionText; }
        public int Order { get => Control == null ? 0 : Control.Order; }
        public bool IsActive { get; set; }
        public bool IsRequired { get => Control == null ? false : Control.IsRequired; }
        public string Type { get; set; }
    }

    public class ProviderQuestionnaireControlRequstModel
    {
        public int? Id { get; set; }
        public int StaffID { get; set; }
        public int QuestionnaireTypeControlId { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }

    }

    public class DynamicControlModel
    {
        public List<ControlOptionItem> Options { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public string Label { get; set; }
        public string FormControlName { get; set; }
        public string QuestionText { get; set; }
        public string Value { get; set; }
        public string ControlType { get; set; }
        public string ControlName { get; set; }
        public string Type { get; set; }

    }

    public class ControlOptionItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
