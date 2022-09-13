using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientInsuranceRepository : RepositoryBase<PatientInsuranceDetails>, IPatientInsuranceRepository
    {
        private HCOrganizationContext _context;
        JsonModel response = new JsonModel();
        public PatientInsuranceRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public JsonModel SavePatientInsurance(PatientInsuranceDetails patientInsuranceDetails,bool Updated)
        {
            try
            {
                if (Updated)
                {
                    _context.PatientInsuranceDetails.Update(patientInsuranceDetails);
                    _context.SaveChanges();
                    response = new JsonModel(patientInsuranceDetails, StatusMessage.ClientInsuranceUpdated, (int)HttpStatusCodes.OK);
                }
                else
                {
                    _context.PatientInsuranceDetails.Add(patientInsuranceDetails);
                    _context.SaveChanges();
                    response = new JsonModel(patientInsuranceDetails, StatusMessage.ClientInsuranceCreated, (int)HttpStatusCodes.OK);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError,e.Message);
            }
            return response;
        }


 

        public PatientInsuranceDetails GetInsuranceDetail(int id)
        {   
            return _context.PatientInsuranceDetails.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).Include(k => k.InsuredPerson).FirstOrDefault();
        }
        public Dictionary<string, object> GetPatientInsurances(int patientId, TokenModel tokenModel)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId)
            };
            return _context.ExecStoredProcedureForPatientInsuranceInsuredPerson(SQLObjects.PAT_GetPatientInsuranceInsuredPerson.ToString(), parameters.Length, parameters);
        }      
        public Dictionary<string, object> GetPatientInsurancesWithPagination(PatientFilterModel filterModel, TokenModel tokenModel)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", filterModel.PatientId),
                     new SqlParameter("@PageNumber", filterModel.pageNumber),
                                           new SqlParameter("@PageSize", filterModel.pageSize),
                                           new SqlParameter("@SortOrder", filterModel.sortOrder),
                                           new SqlParameter("@SortColumn", filterModel.sortColumn)
            };
            return _context.ExecStoredProcedureForPatientInsuranceInsuredPerson(SQLObjects.PAT_GetPatientInsuranceInsuredPerson.ToString(), parameters.Length, parameters);
        }
    }
}
