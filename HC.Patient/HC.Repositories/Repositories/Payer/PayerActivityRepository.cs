using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class PayerActivityRepository : RepositoryBase<PayerAppointmentTypes>, IPayerActivityRepository
    {
        private HCOrganizationContext _context;
        private IConfiguration configuration;
        public PayerActivityRepository(HCOrganizationContext context, IConfiguration config)
         : base(context)
        {
                configuration = config;
                this._context = context;
        }

        public List<PayerAppointmentTypes> UpdatePayerActivityToDB(List<PayerAppointmentTypes> payerAppointmentTypes)
        {
            try
            {
                List<SqlDataRecord> datatable = new List<SqlDataRecord>();
                SqlMetaData[] sqlMetaData = new SqlMetaData[15];
                payerAppointmentTypes.ForEach(k =>
                {

                    //if (k.IsSelected == true)
                    {
                    //    foreach (PropertyInfo p in typeof(PayerAppointmentTypes).GetProperties())
                    //    {
                    //        string propertyName = p.Name;
                    //        sqlMetaData[0] = new SqlMetaData(p.Name, p.PropertyType);
                            
                    //    }
              


                    //    sqlMetaData[1] = new SqlMetaData("Description", SqlDbType.VarChar, 500);
                    //    sqlMetaData[2] = new SqlMetaData("UnitDuration", SqlDbType.Int);
                    //    sqlMetaData[3] = new SqlMetaData("UnitType", SqlDbType.Int);
                    //    sqlMetaData[4] = new SqlMetaData("RatePerUnit", SqlDbType.Decimal);
                    //    sqlMetaData[5] = new SqlMetaData("IsBillable", SqlDbType.Bit);
                    //    sqlMetaData[6] = new SqlMetaData("PayerId", SqlDbType.Int);
                    //    sqlMetaData[7] = new SqlMetaData("IsActive", SqlDbType.Bit);
                    //    sqlMetaData[8] = new SqlMetaData("IsDeleted", SqlDbType.Bit);
                    //    sqlMetaData[9] = new SqlMetaData("DeletedBy", SqlDbType.Int);
                    //    sqlMetaData[10] = new SqlMetaData("DeletedDate", SqlDbType.DateTime);
                    //    sqlMetaData[11] = new SqlMetaData("CreatedBy", SqlDbType.Int);
                    //    sqlMetaData[12] = new SqlMetaData("CreatedDate", SqlDbType.DateTime);
                    //    sqlMetaData[13] = new SqlMetaData("UpdatedDate", SqlDbType.DateTime);
                    //    sqlMetaData[14] = new SqlMetaData("UpdatedBy", SqlDbType.Int);
                    //    SqlDataRecord row = new SqlDataRecord(sqlMetaData);

                    //    row.SetValue(0, k.ServiceCodeId);
                    //    row.SetValue(1, k.Description);
                    //    row.SetValue(2, k.UnitDuration);
                    //    row.SetValue(3, k.UnitType);
                    //    row.SetValue(4, k.RatePerUnit);
                    //    row.SetValue(5, k.IsBillable);
                    //    row.SetValue(6, k.PayerId);

                    //    row.SetValue(7, k.IsActive);
                    //    row.SetValue(8, k.IsDeleted);
                    //    row.SetValue(9, k.DeletedBy);
                    //    row.SetValue(10, k.DeletedDate);
                    //    row.SetValue(11, k.CreatedBy);
                    //    row.SetValue(12, k.CreatedDate);

                    //    row.SetValue(13, k.UpdatedDate);
                    //    row.SetValue(14, k.UpdatedBy);


                        //row.SetValues(new object[] { 0,k.PayerId.ToString(), k.ServiceCodeId.ToString(),
                        //    k.Description.ToString(), k.UnitDuration.ToString(),
                        //k.UnitType.ToString(), k.RatePerUnit.ToString(), k.IsBillable.ToString(),
                        //    k.IsActive.ToString(), k.IsDeleted.ToString(), k.DeletedBy.ToString(), k.CreatedBy.ToString(),
                        //k.CreatedDate.ToString(), k.DeletedDate.ToString(), k.UpdatedDate.ToString() });
                        datatable.Add(null);
                    }
                });


                var task = ExecProcedureDataTableWithParamsAsync<object>("sp_UpdatePayerActivity", new List<SqlParameter>()
            {
                new SqlParameter()
                {
                     ParameterName = "@payerActivityList",
                     SqlDbType = SqlDbType.Structured,
                     Direction = ParameterDirection.Input,
                     Value = datatable
                }
                
            });
                //ExecProcedureDataTableWithParamsAsync("sp_UpdatePayerActivity", sqlMetaData)



                //var task = _context.ExecProcedureDataTableWithParamsAsync<object>("VIEWTABLE", new List<SqlParameter>()
                //{
                //    new SqlParameter()
                //    {
                //         ParameterName = "@paramtable",
                //         SqlDbType = SqlDbType.Structured,
                //         Direction = ParameterDirection.Input,
                //         Value = datatable
                //    }
                //});

                return payerAppointmentTypes;
            }
            catch (Exception)
            {
                throw;
            }

        }        

        public List<PayerServiceCodeModifiers> GetPayerServiceCodeModifiers(int payerServiceCodeId)
        {
            return _context.PayerServiceCodeModifiers.Where(a => a.PayerServiceCodeId == payerServiceCodeId && a.IsActive == true && a.IsDeleted == false).ToList();
        }

        public Task<DataSet> ExecProcedureDataTableWithParamsAsync<T>(string ProcedureName, List<SqlParameter> listParams)
        {
            string conString = ConfigurationExtensions.GetConnectionString(configuration, "HCOrganization");
            //return configuration["ElasticsearchUrl"];

            return ExecuteDataTableAsync<T>(conString, ProcedureName, listParams, CommandType.StoredProcedure);
        }
        #region "internal functions"

        internal async Task<int> ExecuteNonQueryAsync(string connectionString, string query, List<SqlParameter> list, CommandType cmdType)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                PrepareCommand(cmd, conn, null, cmdType, query, list.ToArray());
                int val = await cmd.ExecuteNonQueryAsync();
                return val;
            }
        }

        internal async Task<DataSet> ExecuteDataTableAsync<T>(string connectionString, string query, List<SqlParameter> list, CommandType cmdType)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                PrepareCommand(cmd, conn, null, cmdType, query, list.ToArray());
                DataSet dataSet = new DataSet();
                var cols = new List<string>();
                //using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                //{
                //    dataSet = new DataSet<T>(reader);
                //}
                var result = await cmd.ExecuteNonQueryAsync();
                return dataSet;
            }
        }


        public IQueryable<T> GetActivities<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationID",tokenModel.OrganizationID)
                                         ,new SqlParameter("@PayerId",searchFilterModel.PayerId)
                                         ,new SqlParameter("@SearchText",searchFilterModel.SearchText)
                                         ,new SqlParameter("@PageNumber",searchFilterModel.pageNumber)
                                         ,new SqlParameter("@PageSize",searchFilterModel.pageSize)
                                         ,new SqlParameter("@SortColumn",searchFilterModel.sortColumn)
                                         ,new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetActivities, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPayerActivityServiceCodes<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationID",tokenModel.OrganizationID)
                                         ,new SqlParameter("@PayerId",searchFilterModel.PayerId)
                                         ,new SqlParameter("@ActivityId",searchFilterModel.ActivityId)
                                         ,new SqlParameter("@SearchText",searchFilterModel.SearchText)
                                         ,new SqlParameter("@PageNumber",searchFilterModel.pageNumber)
                                         ,new SqlParameter("@PageSize",searchFilterModel.pageSize)
                                         ,new SqlParameter("@SortColumn",searchFilterModel.sortColumn)
                                         ,new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_ActivityServiceCodes, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetMasterActivitiesForPayer<T>(SearchFilterModel searchFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerId", searchFilterModel.PayerId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetMasterAppointmentTypeForPayer.ToString(), parameters.Length, parameters).AsQueryable();
        }


        #endregion

        #region "Auxiliares"
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }
            }
        }
        #endregion
    }

}
