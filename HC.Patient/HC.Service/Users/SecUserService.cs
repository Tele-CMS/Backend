using HC.Patient.Entity;
using HC.Patient.Service.Users.Interfaces;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Data;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Service;

namespace HC.Patient.Service.Users
{
    public class SecUserService : BaseService, ISecUserService
    {
        private readonly ISecUserRepository _secUserRepository;
        public SecUserService(ISecUserRepository secUserRepository)
        {
            this._secUserRepository = secUserRepository;
        }
        public void SaveSecUser()
        {
            SecUser sec = new SecUser();
            sec.ID = Guid.NewGuid();
            sec.SecUserID = Guid.NewGuid();
            sec.Email = "kkr@smartdatainc.net";

            //_secUserRepository.get

        }
    }
}
