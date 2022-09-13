using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientAddressRepository : IRepositoryBase<PatientAddress>
    {
        Dictionary<string, object> GetPatientPhoneAddress(int patientId, TokenModel tokenModel);

        JsonModel SavePhoneAddress(Patients patients, bool updated);
        Patients GetPatientWithPhoneAddress(int patientId);
        IQueryable<T> SavePatientAddressAndPhoneNumbers<T>(int patientId, string patientAddresses, string phoneNumbers,TokenModel token) where T : class,new();
    }
}
