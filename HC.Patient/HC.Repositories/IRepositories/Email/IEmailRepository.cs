using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IEmailRepository
    {
        /// <summary>
        /// To Save Email Of All type or maintain email log in the site
        /// </summary>
        /// <param name="emailLog">EmailLog Entity</param>
        /// <returns></returns>
        EmailLog SaveEmailLog(EmailLog emailLog);
    }
}
