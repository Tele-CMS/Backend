using HC.Model;
using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
   public interface IOpenTokSettingsRepository
    {
        /// <summary>
        /// Get Open Tok Settings By OrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        OpenTokSettings GetOpenTokKeysByOrganizationId(TokenModel tokenModel);
    }
}
