using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientAddressRepository : RepositoryBase<PatientAddress>, IPatientAddressRepository
    {
        private HCOrganizationContext _context;
        JsonModel response = new JsonModel();
        public PatientAddressRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public Dictionary<string, object> GetPatientPhoneAddress(int patientId, TokenModel tokenModel)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId)
            };
            return _context.ExecStoredProcedureForPatientPhoneAddress(SQLObjects.PAT_GetPatientPhoneAddress.ToString(), parameters.Length, parameters);
        }

        public JsonModel SavePhoneAddress(Patients patients,bool updated)
        {
            try
            {
                _context.Update(patients);
                _context.SaveChanges();
                if (updated){response = new JsonModel(new object(), StatusMessage.ClientAddressUpdated, (int)HttpStatusCodes.OK);}
                else{response = new JsonModel(new object(), StatusMessage.ClientAddressCreated, (int)HttpStatusCodes.OK);}
            }
            catch (Exception e) { response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message); }
            return response;
        }

        public Patients GetPatientWithPhoneAddress(int patientId)
        {
            return _context.Patients.Where(a => a.Id == patientId && a.IsDeleted == false && a.IsActive == true).Include(z => z.PatientAddress).Include(v => v.PhoneNumbers).FirstOrDefault();
        }

        public IQueryable<T> SavePatientAddressAndPhoneNumbers<T>(int patientId, string patientAddresses, string phoneNumbers,TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PatientId", patientId),
                new SqlParameter("@PatientAddresses", patientAddresses),
                new SqlParameter("@PhoneNumbers", phoneNumbers),
                new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_SavePatientAddressAndPhoneNumber, parameters.Length, parameters).AsQueryable();
        }
    }
}
