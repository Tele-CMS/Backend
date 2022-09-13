using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ThemeSettings
{
    public class ThemeSettingsModel
    {
        public int UserId { get; set; }

        public string Theme { get; set; }

        public int ThemeScheme { get; set; }

        public int ThemeLayout { get; set; }
    }
}
