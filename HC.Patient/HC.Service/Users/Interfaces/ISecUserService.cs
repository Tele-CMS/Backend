using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.Users.Interfaces
{
    public interface ISecUserService: IBaseService
    {

        void SaveSecUser();
    }
}
