using HC.Model;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.CareManager
{
    public interface ICareManagerService: IBaseService
    {
        JsonModel GetCareManagerTeamList(int patientId, CommonFilterModel filterModel, TokenModel token);
        JsonModel GetCareManagerList(TokenModel token);
        JsonModel AssignAndRemoveCareManagerToAllPatient(int careTeamMemberId, bool isAttach, TokenModel token);
    }
}
