using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.Reports;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Appointment
{
    public class AppointmentRepository : RepositoryBase<PatientAppointment>, IAppointmentRepository
    {
        private HCOrganizationContext _context;

        public AppointmentRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<T> DeleteAppointment<T>(int appointmentId, bool isAdmin, bool deleteSeries, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@AppointmentId", appointmentId),
                                          new SqlParameter("@IsAdmin", isAdmin),
                                          new SqlParameter("@DeleteSeries", deleteSeries),
                                          new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_DeleteAppointment.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> CheckIsValidAppointment<T>(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@StartDate",startDate),
                                          new SqlParameter("@EndDate",endDate),
                                          new SqlParameter("@CurrentDate",currentDate),
                                          new SqlParameter("@OrganizationId",token.OrganizationID),
                                          new SqlParameter("@LocationId",token.LocationID),
                                          new SqlParameter("@PatientId",patientId),
                                          new SqlParameter("@appointmentTypeID",appointmentTypeID==0?null:appointmentTypeID),
                                          new SqlParameter("@PatientAppointmentId",patientAppointmentId==0?null:patientAppointmentId),
                                          new SqlParameter("@Offset",CommonMethods.GetTimezoneOffset(currentDate.Value,token)),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_CheckIsValidAppointment.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> DeleteAppointmentfrompaymentpage<T>(int appointmentId, bool isAdmin, bool deleteSeries, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@AppointmentId", appointmentId),
                                          new SqlParameter("@IsAdmin", isAdmin),
                                          new SqlParameter("@DeleteSeries", deleteSeries)

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_DeleteAppointment_v1.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> UpdateApptStatusFromPaymentpage<T>(int appointmentId) where T : class, new()
        {
            SqlParameter[] parameters = { 
                new SqlParameter("@AppointmentId", appointmentId)
                                        
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_UpdateApptStatus.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> CheckIsValidAppointmentWithLocation<T>(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, decimal currentOffset, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@StartDate",startDate),
                                          new SqlParameter("@EndDate",endDate),
                                          new SqlParameter("@CurrentDate",currentDate),
                                          new SqlParameter("@OrganizationId",token.OrganizationID),
                                          new SqlParameter("@LocationId",token.LocationID),
                                          new SqlParameter("@PatientId",patientId),
                                          new SqlParameter("@appointmentTypeID",appointmentTypeID==0?null:appointmentTypeID),
                                          new SqlParameter("@PatientAppointmentId",patientAppointmentId==0?null:patientAppointmentId),
                                          new SqlParameter("@Offset",currentOffset),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_CheckIsValidAppointment.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAppointmentDetails<T>(int appointmentId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@AppointmentId", appointmentId) };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetPatientAppointmentDetails", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAppointmentList<T>(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags, int organizationId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@LocationIds", locationIds),
                                          new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@PatientIds", patientIds),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PatientTags", patientTags),
                                          new SqlParameter("@StaffTags", staffTags),
                                          new SqlParameter("@OrganizationId", organizationId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetPatientAppointmentList", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAppointmentListForProviderDashboard<T>(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags, int organizationId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@LocationIds", locationIds),
                                          new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@PatientIds", patientIds),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PatientTags", patientTags),
                                          new SqlParameter("@StaffTags", staffTags),
                                          new SqlParameter("@OrganizationId", organizationId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetPatientAppointmentList_testsp", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetFilteredAppointmentList<T>(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags, string appttype, string apptmode, int organizationId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@LocationIds", locationIds),
                                          new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@PatientIds", patientIds),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PatientTags", patientTags),
                                          new SqlParameter("@StaffTags", staffTags),
                                          new SqlParameter("@OrganizationId", organizationId),
                                          new SqlParameter("@bookingtype", appttype),
                                          new SqlParameter("@bookingmode", apptmode)
            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetPatientFilteredAppointmentList", parameters.Length, parameters).AsQueryable();
        }



        public IQueryable<T> GetPendingAppointmentList<T>(PatientAppointmentFilterModel appointmentFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", token.OrganizationID),
                new SqlParameter("@UserID", token.UserID),
                new SqlParameter("@PageNumber", appointmentFilterModel.pageNumber),
                new SqlParameter("@PageSize", appointmentFilterModel.pageSize),
                new SqlParameter("@FromDate", appointmentFilterModel.FromDate),
                new SqlParameter("@ToDate", appointmentFilterModel.ToDate),
                new SqlParameter("@Status", appointmentFilterModel.Status)
            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetPendingAppointmentList", parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetPastAndUpcomingAppointmentsList<T>(int patientId, DateTime dateTime, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", token.OrganizationID),
                new SqlParameter("@UserID", token.UserID),
                new SqlParameter("@DateTime", dateTime),
                new SqlParameter("@PatientId", patientId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_PastAndUpcomingAppointmentsList, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetAppointmentListForClientDashboard<T>(FilterModel filterModel, DateTime fromDate, DateTime toDate, string locationIds, string staffIds, string patientIds, string patientTags, string statusName, TokenModel token) where T : class, new()
        {
            //System.DateTime today = System.DateTime.UtcNow;

            //if (fromDate != null && toDate != null && fromDate == toDate)
            //{
            //    var diff = today - fromDate;
            //    fromDate = fromDate.AddMinutes(diff.TotalMinutes);
            //}
            SqlParameter[] parameters = {
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@LocationIds", null),
                                          new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@PatientIds", patientIds),
                                          new SqlParameter("@PatientTags", patientTags),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@StatusName", statusName),
                                          new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                          new SqlParameter("@StaffTags",null)

            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetPatientAppointmentListForDashboard", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetUrgentCareAppointmentList<T>(PatientAppointmentFilterModel appointmentFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", token.OrganizationID),
                new SqlParameter("@UserID", token.UserID),
                new SqlParameter("@PageNumber", appointmentFilterModel.pageNumber),
                new SqlParameter("@PageSize", appointmentFilterModel.pageSize),
                new SqlParameter("@FromDate", appointmentFilterModel.FromDate),
                new SqlParameter("@ToDate", appointmentFilterModel.ToDate),
                new SqlParameter("@Status", appointmentFilterModel.Status)
            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetUrgentCareAppointmentList", parameters.Length, parameters).AsQueryable();
        }

        public AppointmentReportingResponseModel GetFilteredAppointmentsForReporting(AppointmentReportingFilterModel appointmentReportingFilter, TokenModel token)
        {
            SqlParameter[] parameters = {
                             new SqlParameter("@StartDateTime", appointmentReportingFilter.StartDateTime),
                             new SqlParameter("@EndDateTime", appointmentReportingFilter.EndDateTime),
                             new SqlParameter("@CurrentDate", appointmentReportingFilter.CurrentDate),
                             new SqlParameter("@CancellationType", appointmentReportingFilter.CancellationType),
                             new SqlParameter("@CareManagerIds", appointmentReportingFilter.CareManagerIds),
                             new SqlParameter("@MemberName", appointmentReportingFilter.MemberName),
                             new SqlParameter("@Status", appointmentReportingFilter.Status),
                             new SqlParameter("@XmlProgramTypes", appointmentReportingFilter.XmlProgramTypes),
                             new SqlParameter("@XmlEnrolledPrograms", appointmentReportingFilter.XmlEnrolledPrograms),
                             new SqlParameter("@NextAppointmentPresent", appointmentReportingFilter.NextAppointmentPresent),
                             new SqlParameter("@IsEligible", appointmentReportingFilter.IsEligible),
                             new SqlParameter("@OrganizationId", token.OrganizationID),
                             new SqlParameter("@UserId", token.UserID),
                             new SqlParameter("@pageNumber", appointmentReportingFilter.pageNumber),
                             new SqlParameter("@pageSize", appointmentReportingFilter.pageSize),
                             new SqlParameter("@sortColumn", appointmentReportingFilter.sortColumn),
                             new SqlParameter("@sortOrder", appointmentReportingFilter.sortOrder),
                             new SqlParameter("@IsFetchTotalCount", true)};
            return _context.ExecStoredProcedureListWithOutputForAppointmentReporting(SQLObjects.RPT_GetFilteredAppointmentsForReporting, parameters.Length, parameters);
        }

    }

    public class PatientAppointmentRepository : IPatientAppointmentRepository
    {
        private HCOrganizationContext _context;
        public PatientAppointmentRepository(HCOrganizationContext context)
         : base()
        {
            this._context = context;
        }
        public List<PatientAppointmentsModel> UpdatePatientAppointment(PatientAppointmentFilter patientAppointmentFilter)
        {
            try
            {
                string Staffs = "";
                if (patientAppointmentFilter.PatientAppointment != null && patientAppointmentFilter.PatientAppointment.Count != 0)
                {
                    if (patientAppointmentFilter.PatientAppointment.FirstOrDefault().StaffIDs != null
                        && patientAppointmentFilter.PatientAppointment.FirstOrDefault().StaffIDs.Count() != 0)
                    {
                        patientAppointmentFilter.PatientAppointment.FirstOrDefault().StaffIDs.ToList().ForEach(l =>
                        {
                            Staffs = Staffs + "," + l;
                        });
                        Staffs = Staffs.Trim(',');
                        patientAppointmentFilter.PatientAppointment.FirstOrDefault().StaffIDs = null;
                    }

                    List<PatientAppointmentsModel> patientAppointmentsModel = new List<PatientAppointmentsModel>();

                    patientAppointmentsModel = patientAppointmentFilter.PatientAppointment.Select(j => new PatientAppointmentsModel
                    {
                        AppointmentTypeID = j.AppointmentTypeID,
                        CreatedBy = j.CreatedBy,
                        CreatedDate = j.CreatedDate,
                        DeletedBy = j.DeletedBy,
                        DeletedDate = j.DeletedDate,
                        EndDateTime = j.EndDateTime,
                        IsActive = j.IsActive,
                        IsClientRequired = j.IsClientRequired,
                        IsDeleted = j.IsDeleted,
                        Notes = j.Notes,
                        ParentAppointmentID = j.ParentAppointmentID,
                        PatientAppointmentId = j.PatientAppointmentId,
                        PatientID = j.PatientID,
                        PatientLocationID = j.PatientLocationID,
                        RecurrenceRule = j.RecurrenceRule,
                        ServiceAddressID = j.ServiceAddressID,
                        ServiceID = j.ServiceID,
                        ServiceLocationID = j.ServiceLocationID,
                        StaffID = j.StaffID,
                        StartDateTime = j.StartDateTime,
                        UpdatedBy = j.UpdatedBy,
                        UpdatedDate = j.UpdatedDate,
                        IsTelehealthAppointment = j.IsTelehealthAppointment
                    }).ToList();

                    return _context.ExecStoredProcedureListWithOutput<PatientAppointmentsModel>("UpdatePatientAppointment",
                    typeof(PatientAppointmentsModel).GetProperties().Count(),
                    new SqlParameter()
                    {
                        ParameterName = "@PatientAppointmentList",
                        SqlDbType = SqlDbType.Structured,
                        Direction = ParameterDirection.Input,
                        Value = GetSqlDataTableFromList(patientAppointmentsModel)

                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IsUpdate",
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input,
                        Value = patientAppointmentFilter.IsUpdate
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IsDelete",
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input,
                        Value = patientAppointmentFilter.IsDelete
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IsInsert",
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input,
                        Value = patientAppointmentFilter.IsInsert
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Staffs",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = Staffs
                    }).ToList();
                }
                else
                {
                    return null;

                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        public List<StaffAvailabilityModel> GetStaffAvailability(string StaffID, DateTime FromDate, DateTime ToDate)
        {
            return _context.ExecStoredProcedureListWithOutput<StaffAvailabilityModel>("APPOINTMENT_GetStaffAvailability",
                typeof(StaffAvailabilityModel).GetProperties().Count(),

                new SqlParameter()
                {
                    ParameterName = "@StaffID",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = StaffID
                },
                new SqlParameter()
                {
                    ParameterName = "@FromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    Value = FromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@ToDate",
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    Value = ToDate
                }
                ).ToList();
        }


        public List<SqlDataRecord> GetSqlDataTableFromList<TEntity>(List<TEntity> entity) where TEntity : class, new()
        {

            List<SqlDataRecord> datatable = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaData = new SqlMetaData[typeof(TEntity).GetProperties().Count()];
            if (entity != null)
            {
                entity.ForEach(k =>
                {
                    //string propertyName = "";
                    foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                    {
                        // {
                        SqlDbType SqlType = SqlHelper.GetDbType(p.PropertyType);
                        if (SqlType.ToString() == "NVarChar")
                        {
                            sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType, 500);
                        }
                        else
                        {
                            sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType);

                        }
                        //  }
                    }
                    SqlDataRecord row = new SqlDataRecord(sqlMetaData);
                    foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                    {
                        if (p.Name.ToLower() != "staffids")
                        {
                            row.SetValue(typeof(TEntity).GetProperties().ToList().IndexOf(p), k.GetType().GetProperty(p.Name).GetValue(k));
                        }
                        else
                        {
                            row.SetValue(typeof(TEntity).GetProperties().ToList().IndexOf(p), k.GetType().GetProperty(p.Name).GetValue(""));
                        }
                    }
                    datatable.Add(row);

                });
            }
            else
            {
                foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                {
                    string propertyName = p.Name;
                    //SqlDbType SqlType = SqlHelper.GetDbType(p.PropertyType);
                    //if (SqlType.ToString() == "NVarChar")
                    //{
                    //    sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType, 500);
                    //}
                    //else
                    //{
                    //    sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType);

                    //}
                }
                //SqlDataRecord row = new SqlDataRecord(sqlMetaData);
                //foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                //{
                //    row.SetValue(typeof(TEntity).GetProperties().ToList().IndexOf(p), k.GetType().GetProperty(p.Name).GetValue(k));
                //}
                // datatable.Add(row);

            }
            return datatable;

        }

        public StaffPatientModel GetStaffAndPatientByLocation(string locationIds, string permissionKey, string isActiveCheckRequired, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@LocationIds", locationIds),
                new SqlParameter("@OrganizationId", token.OrganizationID),
                new SqlParameter("@LoginUserId", token.StaffID), //pass staff id
                new SqlParameter("@PermissionKey", permissionKey),
                new SqlParameter("@IsActiveCheckRequired", isActiveCheckRequired),

            };
            return _context.ExecStoredProcedureForStaffPatientLocation("APT_GetStaffAndPatientByLocation", parameters.Length, parameters);
        }

        public IQueryable<T> GetStaffByLocation<T>(string locationIds, string isActiveCheckRequired, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@LocationIds", locationIds),
                new SqlParameter("@OrganizationId", token.OrganizationID),
                new SqlParameter("@IsActiveCheckRequired", isActiveCheckRequired),
            };
            return _context.ExecStoredProcedureListWithOutput<T>("APT_GetStaffByLocation", parameters.Length, parameters).AsQueryable();
        }


        public List<PatientAppointment> GetLastNewAppintment(int providerId, TokenModel token)
        {
            int patientUserId = token.UserID;
            var patient = _context.Patients.FirstOrDefault(x => x.UserID == patientUserId);
            if (patient == null) return null;
            var patientAppointments = _context.PatientAppointment
                .Include(a => a.AppointmentStaff)
                .Where(x => x.PatientID == patient.Id && x.BookingType == "New" && x.OrganizationID == token.OrganizationID ).ToList();
            return patientAppointments.Where(x => x.StaffIDs.Contains(providerId)).ToList();

           
            
        }





        public List<PatientAppointment> GetPreviousAppintment(int providerId, int patientId,TokenModel token)
        {
           
            if (patientId == 0) return null;
            var patientAppointments = _context.PatientAppointment
                .Include(a => a.AppointmentStaff)
                .Where(x => x.PatientID == patientId && x.BookingType == "New" && x.OrganizationID == token.OrganizationID).ToList();
            return patientAppointments.Where(x => x.StaffIDs.Contains(providerId)).ToList();



        }

        public List<PatientAppointment> GetLastUrgentCareCallStatus(int providerId, TokenModel token)
        {
           
            var patientAppointments = _context.PatientAppointment
                .Include(a => a.AppointmentStaff)
                .Where(x => x.IsUrgentCareAppointment == true && x.IsActive == true && x.IsDeleted == false && x.OrganizationID == token.OrganizationID).ToList();
            return patientAppointments.Where(x => x.StaffIDs.Contains(providerId)).ToList();



        }
      

    }
}
