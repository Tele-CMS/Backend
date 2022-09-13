using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.EDI;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.EDI
{
    public interface IEDI999Repository : IRepositoryBase<EDI999AcknowledgementMaster>
    {
        IQueryable<T> SaveEDI999Acknowledgement<T>(EDI999FileModel edi999FileModel, TokenModel token) where T : class, new();

    }
}
