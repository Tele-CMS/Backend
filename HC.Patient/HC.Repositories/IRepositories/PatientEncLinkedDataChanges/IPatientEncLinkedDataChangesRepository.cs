using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges
{
    public interface IPatientEncLinkedDataChangesRepository:IRepositoryBase<PatientEncounterLinkedDataChanges>
    {
        void savePatientEncounterChanges(List<ChangesLog> changesLogs, int? recordId, int encounterId, TokenModel tokenModel);
    }
}
