using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HC.Service.Interfaces
{
    public interface IBaseService
    {
        T ExecuteFunctions<T>(Func<T> method);
    }
}
