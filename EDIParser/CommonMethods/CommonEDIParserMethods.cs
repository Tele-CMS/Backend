using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EDIParser.CommonMethods
{
    public static class CommonEDIParserMethods
    {
        public static T GetSegmentInfo<T>(T instance, string[] segment)
        {
            string value = null;
            Type type = instance.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Count(); i++)
            {
                try { value = segment[i] != null ? segment[i].TrimStart() : null; }
                catch { value = null; }
                properties[i].SetValue(instance, Convert.ChangeType(value, properties[i].PropertyType), null);
            }
            return instance;
        }
    }
}
