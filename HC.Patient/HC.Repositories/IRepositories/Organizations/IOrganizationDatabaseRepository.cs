using HC.Patient.Entity;
using HC.Patient.Model.Organizations;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Organizations
{
    public interface IOrganizationDatabaseRepository 
    {
        List<OrganizationDatabaseDetailsModel> GetOrganizationDatabaseDetails(string databaseName, string organizationName, int organizationID, string sortColumn, string sortOrder, int pageNumber, int pageSize);
        OrganizationDatabaseDetail SaveOrganizationDatabaseDetail(OrganizationDatabaseDetail organizationDatabaseDetail);
        OrganizationDatabaseDetail UpdateOrganizationDatabaseDetail(int id, OrganizationDatabaseDetail organizationDatabaseDetail);
        OrganizationDatabaseDetail DeleteOrganizationDatabaseDetail(int id, int userID);
    }
}
