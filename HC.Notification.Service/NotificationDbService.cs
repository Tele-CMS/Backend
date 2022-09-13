using HC.Notification.Service.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace HC.Notification.Service
{
    public class NotificationDbService
    {
        public NotificationDbService()
        {

        }
        public Dictionary<string, object> GetPushNotificationAndDeviceTokenDetails(MasterDatabaseModel databaseDetails)
        {
            SqlConnection con = null;
            string connection = ConnectionString(databaseDetails);
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                con = new SqlConnection(connection);
                if (con != null)
                {
                    SqlCommand cmd = new SqlCommand(SQLObjects.NS_NotificationDetail, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@OrganizationId", databaseDetails.OrganizationID));
                    con.Open();
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("PatientDetail", NotificationServiceCommon.DataReaderMapToList<PatientDetail>(reader).ToList());
                        reader.NextResult();
                    }                   
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public void UpdateNotificationStatus(int mappingId, string message, MasterDatabaseModel databaseDetails)
        {
            SqlConnection con = null;
            Dictionary<string, object> result = new Dictionary<string, object>();
            string connection = ConnectionString(databaseDetails);
            try
            {
                con = new SqlConnection(connection);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@NotificationID", mappingId),
                    new SqlParameter("@Message", message)
                };

                using (var cmd = con.CreateCommand())
                {
                    AddParametersToDbCommand(SQLObjects.NS_UpdateNotificationStatus, parameters, cmd);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                con.Close();
            }
        }
        public Dictionary<string, object> GetSMTPDetails(MasterDatabaseModel databaseDetails)
        {

            SqlConnection con = null;
            string connection = ConnectionString(databaseDetails);
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                con = new SqlConnection(connection);

                SqlCommand cmd = new SqlCommand(SQLObjects.NS_GetSmtpDetails, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OrganizationId", databaseDetails.OrganizationID));
                con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (var reader = cmd.ExecuteReader())
                {
                    result.Add("OrganizationSMTPCommonModel", NotificationServiceCommon.DataReaderMapToList<OrganizationSMTPCommonModel>(reader).FirstOrDefault());
                    reader.NextResult();
                }
                return result;
            }
            finally
            {
                con.Close();
            }
        }
        public Dictionary<string, object> GetEmailTemplate(MasterDatabaseModel databaseDetails)
        {
            SqlConnection con = null;
            string connection = ConnectionString(databaseDetails);
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand(SQLObjects.NS_GetEmailTemplate, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@OrganizationId", databaseDetails.OrganizationID));
                con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (var reader = cmd.ExecuteReader())
                {
                    result.Add("TemplateModel", NotificationServiceCommon.DataReaderMapToList<TemplateModel>(reader).ToList());
                    reader.NextResult();
                }
                return result;
            }
            finally
            {
                con.Close();
            }
        }
        public Dictionary<string, object> GetMasterOrganizationData()
        {
            string conString = ConfigurationManager.ConnectionStrings["OHCMaster"].ConnectionString;
            SqlConnection con = null;
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                con = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand(SQLObjects.NS_OrganizationAndDatabaseDetail, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    result.Add("MasterDatabaseDetail", NotificationServiceCommon.DataReaderMapToList<MasterDatabaseModel>(reader).ToList());
                    reader.NextResult();
                }
                return result;
            }
            finally
            {
                con.Close();
            }
        }
        public void InsertErrorLog(ErrorLogModel errorLogModel, MasterDatabaseModel databaseDetails)
        {
            SqlConnection con = null;
            Dictionary<string, object> result = new Dictionary<string, object>();
            string connection = ConnectionString(databaseDetails);
            try
            {
                con = new SqlConnection(connection);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@ErrorLine",errorLogModel.ErrorLine),
                    new SqlParameter("@ErrorMessage",errorLogModel.ErrorMessage),
                    new SqlParameter("@ErrorNumber",errorLogModel.ErrorNumber),
                    new SqlParameter("@ErrorTime",errorLogModel.ErrorTime),
                    new SqlParameter("@ErrorLogTypeId",errorLogModel.ErrorLogTypeId),
                    new SqlParameter("@OrganizationId",errorLogModel.OrganizationId),
                };

                using (var cmd = con.CreateCommand())
                {
                    AddParametersToDbCommand(SQLObjects.Insert_EmailLog, parameters, cmd);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                con.Close();
            }
        }
        private void AddParametersToDbCommand(string commandText, object[] parameters, System.Data.Common.DbCommand cmd)
        {
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p != null)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
            }
        }
        public string ConnectionString(MasterDatabaseModel databaseDetails)
        {
            string conn = @"data source =" + databaseDetails.ServerName + "; database =" + databaseDetails.DatabaseName + "; User ID =" + databaseDetails.UserName + "; Password =" + databaseDetails.Password + "; Integrated Security = false;";
            return conn;
        }

        public void UpdateSendNotificationAttempts(int mappingId, string message, MasterDatabaseModel databaseDetails)
        {
            SqlConnection con = null;
            Dictionary<string, object> result = new Dictionary<string, object>();
            string connection = ConnectionString(databaseDetails);
            try
            {
                con = new SqlConnection(connection);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@NotificationID", mappingId),
                    new SqlParameter("@Message", message)
                };

                using (var cmd = con.CreateCommand())
                {
                    AddParametersToDbCommand(SQLObjects.NS_UpdateNotificationStatus, parameters, cmd);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                con.Close();
            }
        }
    }
}
