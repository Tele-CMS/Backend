using EDIParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDIParser.IServices
{
    public interface IEDI999Service
    {
        EDI999SchemaModel ParseEDI999(string fileText);
    }
}
