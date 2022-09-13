using EDIParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDIParser.IServices
{
    public interface IEDI271Service
    {
        EDI271SchemaModel ParseEDI271(string fileText);
    }
}
