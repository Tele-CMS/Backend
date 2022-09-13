using System;
using NLog.Layouts;
using NLog.Config;

namespace HC.NLog.Targets.ElasticSearch
{
    [NLogConfigurationItem]
    public class Field
    {
        public Field()
        {
            LayoutType = typeof (string);
        }

        [RequiredParameter]
        public string Name { get; set; }

        [RequiredParameter]
        public Layout Layout { get; set; }

        public Type LayoutType { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, LayoutType: {LayoutType}, Layout: {Layout}";
        }
    }
}