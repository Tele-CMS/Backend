using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Organizations;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HC.Patient.Repositories.Repositories.Organizations
{
    public class OrganizationDatabaseRepository :  IOrganizationDatabaseRepository
    {
        private HCMasterContext _context;
        public OrganizationDatabaseRepository(HCMasterContext context) //: base(context)
        {
            this._context = context;
        }

        public List<OrganizationDatabaseDetailsModel> GetOrganizationDatabaseDetails(string databaseName, string organizationName, int organizationID, string sortColumn, string sortOrder, int pageNumber, int pageSize)
        {
            SqlParameter[] parameters = { new SqlParameter("@DatabaseName", databaseName),
                                          new SqlParameter("@OrganizationName",organizationName),
                                          new SqlParameter("@OrganizationID",organizationID),
                                          new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
                                          new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize),
            };
            return _context.ExecStoredProcedureListWithOutput<OrganizationDatabaseDetailsModel>("ORG_GetOrganizationDatabaseDetail", parameters.Length, parameters).ToList();
        }

        public OrganizationDatabaseDetail SaveOrganizationDatabaseDetail(OrganizationDatabaseDetail organizationDatabaseDetail)
        {
            try
            {
                _context.Add(organizationDatabaseDetail);
                var result= _context.SaveChanges();
                if(result>0)
                {
                    return organizationDatabaseDetail;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception )
            {
                return null;
            }
        }

        public OrganizationDatabaseDetail UpdateOrganizationDatabaseDetail(int id, OrganizationDatabaseDetail organizationDatabaseDetail)
        {
            try
            {
                var _organizationDatabaseDetail = _context.OrganizationDatabaseDetail.Where(h => h.Id == id).FirstOrDefault();
                _organizationDatabaseDetail.DatabaseName = organizationDatabaseDetail.DatabaseName;
                //_organizationDatabaseDetail.IsActive = organizationDatabaseDetail.IsActive;
                _organizationDatabaseDetail.IsCentralised = organizationDatabaseDetail.IsCentralised;
                _organizationDatabaseDetail.Password = organizationDatabaseDetail.Password;
                _organizationDatabaseDetail.ServerName = organizationDatabaseDetail.ServerName;
                _organizationDatabaseDetail.UpdatedBy = organizationDatabaseDetail.UpdatedBy;
                _organizationDatabaseDetail.UpdatedDate = DateTime.UtcNow;
                _organizationDatabaseDetail.UserName = organizationDatabaseDetail.UserName;
                //_context.Add(_organizationDatabaseDetail);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return _organizationDatabaseDetail;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public OrganizationDatabaseDetail DeleteOrganizationDatabaseDetail(int id, int userID)
        {
            try
            {
                var _organizationDatabaseDetail = _context.OrganizationDatabaseDetail.Where(h => h.Id == id).FirstOrDefault();
                _organizationDatabaseDetail.DeletedBy = userID;
                _organizationDatabaseDetail.DeletedDate = DateTime.UtcNow;
                _organizationDatabaseDetail.IsDeleted = true;
                //_context.Add(_organizationDatabaseDetail);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return _organizationDatabaseDetail;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }



    }
}
