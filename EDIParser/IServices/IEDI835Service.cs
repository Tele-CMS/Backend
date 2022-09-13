using EDIParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDIParser.IServices
{
    public interface IEDI835Service
    {
        EDI835SchemaModel ParseEDI835(string path);
    }
}
