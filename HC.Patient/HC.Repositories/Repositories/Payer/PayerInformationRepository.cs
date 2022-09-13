using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.MasterServiceCodes;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class PayerInformationRepository : RepositoryBase<PayerServiceCodes>, IPayerInformationRepository
    {
        private HCOrganizationContext _context;
        private IConfiguration configuration;
        public PayerInformationRepository(HCOrganizationContext context, IConfiguration config)
         : base(context)
        {
            configuration = config;
            this._context = context;
        }

        public List<PayerInformationModel> UpdatePayerInformation(PayerInfoUpdateModel payerInforUpdateModel)
        {
            if (payerInforUpdateModel.PayerAppointmentTypeList != null && payerInforUpdateModel.PayerAppointmentTypeList.Count != 0)
            {
                return ParsePayerActivityData<PayerInformationModel>(payerInforUpdateModel);
            }
            else
            {
                return ParsePayerInfoData<PayerInformationModel>(payerInforUpdateModel);
            }
        }

        private List<TEntity> ParsePayerInfoData<TEntity>(PayerInfoUpdateModel payerInforUpdateModel) where TEntity : class, new()
        {
            var entity = ((dynamic)payerInforUpdateModel.PayerAppointmentTypeList != null ? (dynamic)payerInforUpdateModel.PayerAppointmentTypeList : ((dynamic)payerInforUpdateModel.PayerInformationList != null ? (dynamic)payerInforUpdateModel.PayerInformationList : ((dynamic)payerInforUpdateModel.PayerServiceCodesList != null ? (dynamic)payerInforUpdateModel.PayerServiceCodesList : null)));
            string entityParameterName = ((dynamic)payerInforUpdateModel.PayerAppointmentTypeList != null ? "@PayerAppointmentTypeList" : ((dynamic)payerInforUpdateModel.PayerInformationList != null ? "@PayerInformationList" : ((dynamic)payerInforUpdateModel.PayerServiceCodesList != null ? "@PayerServiceCodesList" : null)));
            return _context.ExecStoredProcedureListWithOutput<TEntity>("UpdatePayerInformation",
            typeof(TEntity).GetProperties().Count(),

            new SqlParameter()
            {
                ParameterName = entityParameterName,
                SqlDbType = SqlDbType.Structured,
                Direction = ParameterDirection.Input,
                Value = ToDataTable(entity),//GetSqlDataTableFromList(entity)

            },
            new SqlParameter()
            {
                ParameterName = "@IsUpdate",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = payerInforUpdateModel.IsUpdate
            },
            new SqlParameter()
            {
                ParameterName = "@IsDelete",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = payerInforUpdateModel.IsDelete
            },
            new SqlParameter()
            {
                ParameterName = "@IsInsert",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = payerInforUpdateModel.IsInsert
            }
            ).ToList();
        }


        private List<TEntity> ParsePayerActivityData<TEntity>(PayerInfoUpdateModel payerInforUpdateModel) where TEntity : class, new()
        {
            var entity = ((dynamic)payerInforUpdateModel.PayerAppointmentTypeList != null ? (dynamic)payerInforUpdateModel.PayerAppointmentTypeList : ((dynamic)payerInforUpdateModel.PayerInformationList != null ? (dynamic)payerInforUpdateModel.PayerInformationList : ((dynamic)payerInforUpdateModel.PayerServiceCodesList != null ? (dynamic)payerInforUpdateModel.PayerServiceCodesList : null)));
            string entityParameterName = ((dynamic)payerInforUpdateModel.PayerAppointmentTypeList != null ? "@PayerAppointmentTypeList" : ((dynamic)payerInforUpdateModel.PayerInformationList != null ? "@PayerInformationList" : ((dynamic)payerInforUpdateModel.PayerServiceCodesList != null ? "@PayerServiceCodesList" : null)));
            return _context.ExecStoredProcedureListWithOutput<TEntity>("UpdatePayerInformation_Backup",
            typeof(TEntity).GetProperties().Count(),

            new SqlParameter()
            {
                ParameterName = entityParameterName,
                SqlDbType = SqlDbType.Structured,
                Direction = ParameterDirection.Input,
                Value = ToDataTable(entity),//GetSqlDataTableFromList(entity)

            },
            new SqlParameter()
            {
                ParameterName = "@IsUpdate",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = payerInforUpdateModel.IsUpdate
            },
            new SqlParameter()
            {
                ParameterName = "@IsDelete",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = payerInforUpdateModel.IsDelete
            },
            new SqlParameter()
            {
                ParameterName = "@IsInsert",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = payerInforUpdateModel.IsInsert
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
                    foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                    {
                        string propertyName = p.Name;
                        SqlDbType SqlType = SqlHelper.GetDbType(p.PropertyType);
                        if (SqlType.ToString() == "NVarChar")
                        //if(1==1)
                        {
                            sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType, 500);
                        }
                        else
                        {
                            sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType);

                        }
                    }
                    SqlDataRecord row = new SqlDataRecord(sqlMetaData);
                    foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                    {
                        row.SetValue(typeof(TEntity).GetProperties().ToList().IndexOf(p), k.GetType().GetProperty(p.Name).GetValue(k));
                    }
                    datatable.Add(row);

                });
            }
            else
            {
                foreach (PropertyInfo p in typeof(TEntity).GetProperties())
                {
                    string propertyName = p.Name;
                    SqlDbType SqlType = SqlHelper.GetDbType(p.PropertyType);
                    if (SqlType.ToString() == "NVarChar")
                    //if(1==1)
                    {
                        sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType, 500);
                    }
                    else
                    {
                        sqlMetaData[typeof(TEntity).GetProperties().ToList().IndexOf(p)] = new SqlMetaData(p.Name, SqlType);

                    }
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


        //private List<PayerInformationModel> ParsePayerInfoData<TEntity>(List<TEntity> entity,string entityParameterName, bool IsInsert = false, bool IsUpdate = false, bool IsDelete = false) where TEntity : class, new()
        //{
        //    return _context.ExecStoredProcedureListWithOutput<PayerInformationModel>("UpdatePayerInformation",
        //    typeof(TEntity).GetProperties().Count(),

        //    new SqlParameter()
        //    {
        //        ParameterName = entityParameterName,
        //        SqlDbType = SqlDbType.Structured,
        //        Direction = ParameterDirection.Input,
        //        Value = new ObjectDataReader<TEntity>(entity.AsEnumerable<TEntity>())
        //    },
        //    new SqlParameter()
        //    {
        //        ParameterName = "@IsInsert",
        //        SqlDbType = SqlDbType.Bit,
        //        Direction = ParameterDirection.Input,
        //        Value = IsInsert
        //    },
        //    new SqlParameter()
        //    {
        //        ParameterName = "@IsUpdate",
        //        SqlDbType = SqlDbType.Bit,
        //        Direction = ParameterDirection.Input,
        //        Value = IsUpdate
        //    },
        //    new SqlParameter()
        //    {
        //        ParameterName = "@IsDelete",
        //        SqlDbType = SqlDbType.Bit,
        //        Direction = ParameterDirection.Input,
        //        Value = IsDelete
        //    }
        //    ).ToList();
        //}

        public List<PayerServiceCodes> UpdatePayerServiceCodesToDB(List<PayerServiceCodes> payerServiceCodes, bool IsInsert = false, bool IsUpdate = false, bool IsDelete = false)
        {
            try
            {
                //List<SqlDataRecord> datatable = new List<SqlDataRecord>();
                //SqlMetaData[] sqlMetaData = new SqlMetaData[15];
                //PayerServiceCodes.ForEach(k =>
                //{

                //    //if (k.IsSelected == true)
                //    {

                //        sqlMetaData[0] = new SqlMetaData("ServiceCodeId", SqlDbType.Int);
                //        sqlMetaData[1] = new SqlMetaData("Description", SqlDbType.VarChar, 500);
                //        sqlMetaData[2] = new SqlMetaData("UnitDuration", SqlDbType.Int);
                //        sqlMetaData[3] = new SqlMetaData("UnitType", SqlDbType.Int);
                //        sqlMetaData[4] = new SqlMetaData("RatePerUnit", SqlDbType.Decimal);
                //        sqlMetaData[5] = new SqlMetaData("IsBillable", SqlDbType.Bit);
                //        sqlMetaData[6] = new SqlMetaData("PayerId", SqlDbType.Int);
                //        sqlMetaData[7] = new SqlMetaData("IsActive", SqlDbType.Bit);
                //        sqlMetaData[8] = new SqlMetaData("IsDeleted", SqlDbType.Bit);
                //        sqlMetaData[9] = new SqlMetaData("DeletedBy", SqlDbType.Int);
                //        sqlMetaData[10] = new SqlMetaData("DeletedDate", SqlDbType.DateTime);
                //        sqlMetaData[11] = new SqlMetaData("CreatedBy", SqlDbType.Int);
                //        sqlMetaData[12] = new SqlMetaData("CreatedDate", SqlDbType.DateTime);
                //        sqlMetaData[13] = new SqlMetaData("UpdatedDate", SqlDbType.DateTime);
                //        sqlMetaData[14] = new SqlMetaData("UpdatedBy", SqlDbType.Int);
                //        SqlDataRecord row = new SqlDataRecord(sqlMetaData);

                //        row.SetValue(0, k.ServiceCodeId);
                //        row.SetValue(1, k.Description);
                //        row.SetValue(2, k.UnitDuration);
                //        row.SetValue(3, k.UnitType);
                //        row.SetValue(4, k.RatePerUnit);
                //        row.SetValue(5, k.IsBillable);
                //        row.SetValue(6, k.PayerId);

                //        row.SetValue(7, k.IsActive);
                //        row.SetValue(8, k.IsDeleted);
                //        row.SetValue(9, k.DeletedBy);
                //        row.SetValue(10, k.DeletedDate);
                //        row.SetValue(11, k.CreatedBy);
                //        row.SetValue(12, k.CreatedDate);

                //        row.SetValue(13, k.UpdatedDate);
                //        row.SetValue(14, k.UpdatedBy);


                //        //row.SetValues(new object[] { 0,k.PayerId.ToString(), k.ServiceCodeId.ToString(),
                //        //    k.Description.ToString(), k.UnitDuration.ToString(),
                //        //k.UnitType.ToString(), k.RatePerUnit.ToString(), k.IsBillable.ToString(),
                //        //    k.IsActive.ToString(), k.IsDeleted.ToString(), k.DeletedBy.ToString(), k.CreatedBy.ToString(),
                //        //k.CreatedDate.ToString(), k.DeletedDate.ToString(), k.UpdatedDate.ToString() });
                //        datatable.Add(row);
                //    }
                //});


                return _context.ExecStoredProcedureListWithOutput<PayerServiceCodes>("UpdatePayerInformation",
                typeof(PayerServiceCodes).GetProperties().Count(),

                new SqlParameter()
                {
                    ParameterName = "@PayerServiceCodesList",
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    Value = new ObjectDataReader<PayerServiceCodes>(payerServiceCodes)
                },
                new SqlParameter()
                {
                    ParameterName = "@IsInsert",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    Value = IsInsert
                }
                ).ToList();
                //ExecProcedureDataTableWithParamsAsync("sp_UpdatePayerServiceCodes", sqlMetaData)



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
            }
            catch (Exception)
            {
                throw;
            }

        }



        public List<PayerInformationModel> GetPayerInformationByFilter(PayerSearchFilter payerSearchFilter)
        {
            try
            {
                IList<PayerInformationModel> payerInformationModelList = PasrsePayerData<PayerInformationModel>(payerSearchFilter, "@IsPayerInformation");

                if (payerInformationModelList.Count > 0)
                {
                    return payerInformationModelList.ToList();
                }
                else
                {
                    payerSearchFilter.PageNumber--;
                    payerInformationModelList = PasrsePayerData<PayerInformationModel>(payerSearchFilter, "@IsPayerInformation");
                    return payerInformationModelList.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<PayerInformationModel> GetMasterServiceCodesByFilter(PayerSearchFilter payerSearchFilter)
        {

            try
            {
                IList<PayerInformationModel> masterServiceCodeModelList = PasrsePayerData<PayerInformationModel>(payerSearchFilter, "@IsMasterServiceCode");
                if (masterServiceCodeModelList.Count > 0)
                {
                    List<int> serviceCodeIds = masterServiceCodeModelList.Select(z => z.ServiceCodeID).ToList();
                    List<MasterServiceCodeModifiers> masterserviceCodeModifierList = _context.MasterServiceCodeModifiers.Where(a => serviceCodeIds.Contains(a.ServiceCodeID) && a.IsDeleted == false && a.IsActive == true).ToList();

                    foreach (var item in masterServiceCodeModelList)
                    {
                        List<ModifierModel> modifiers = masterserviceCodeModifierList.Where(z => z.ServiceCodeID == item.ServiceCodeID).Select(x => new ModifierModel { ModifierID = x.Id, Modifier = x.Modifier, Rate = x.Rate, ServiceCodeId = x.ServiceCodeID }).Where(x => x.ServiceCodeId == item.ServiceCodeID).ToList();
                        item.ModifierModel = modifiers;
                    }
                }

                return masterServiceCodeModelList.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MasterDropDown> GetMasterServiceCodesEx(int payerID, int organizationID)
        {
            try
            {
                return _context.ExecStoredProcedureListWithOutput<MasterDropDown>("GetPayerInformation",
                    typeof(MasterDropDown).GetProperties().Count(),
              new SqlParameter()
              {
                  ParameterName = "@ID",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerID,
              },
               new SqlParameter()
               {
                   ParameterName = "@PageNumber",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = 1,
               },
               new SqlParameter()
               {
                   ParameterName = "@PageSize",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = 10,
               },
              new SqlParameter()
              {
                  ParameterName = "@OrganizationID",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = organizationID,
              },
              new SqlParameter()
              {
                  ParameterName = "@IsMasterServiceCodesEx",
                  SqlDbType = SqlDbType.Bit,
                  Direction = ParameterDirection.Input,
                  Value = true,
              }
               ).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<MasterDropDown> GetPayerServiceCodesByPayerId(int payerID, int organizationID)
        {
            try
            {
                return _context.ExecStoredProcedureListWithOutput<MasterDropDown>("GetPayerServiceCodesByPayerId",
                    typeof(MasterDropDown).GetProperties().Count(),
              new SqlParameter()
              {
                  ParameterName = "@payerID",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerID,
              },
              new SqlParameter()
              {
                  ParameterName = "@organizationID",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = organizationID,
              }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MasterDropDown> GetPayerServiceCodesEx(int payerID, int activityID, int organizationID)
        {
            try
            {
                return _context.ExecStoredProcedureListWithOutput<MasterDropDown>("GetPayerInformation",
                    typeof(MasterDropDown).GetProperties().Count(),
              new SqlParameter()
              {
                  ParameterName = "@ID",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerID,
              },
              new SqlParameter()
              {
                  ParameterName = "@ID2",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = activityID,
              },
              new SqlParameter()
              {
                  ParameterName = "@PageNumber",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = 1,
              },
               new SqlParameter()
               {
                   ParameterName = "@PageSize",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = 10,
               },
              new SqlParameter()
              {
                  ParameterName = "@OrganizationID",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = organizationID,
              },
              new SqlParameter()
              {
                  ParameterName = "@IsPayerServiceCodesEx",
                  SqlDbType = SqlDbType.Bit,
                  Direction = ParameterDirection.Input,
                  Value = true,
              }
               ).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PayerInformationModel> GetPayerActivityByFilter(PayerSearchFilter payerSearchFilter)
        {

            try
            {
                IList<PayerInformationModel> payerAppointmentTypeModelList = PasrsePayerActivityData<PayerInformationModel>(payerSearchFilter, "@IsPayerActivity");

                return payerAppointmentTypeModelList.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<PayerAppTypeModel> GetPayerServiceCodesByFilter(PayerSearchFilter payerSearchFilter)
        {

            try
            {
                IList<PayerAppTypeModel> payerServiceCodeModelList = PasrsePayerActivityData<PayerAppTypeModel>(payerSearchFilter, "@IsPayerServiceCode");

                return payerServiceCodeModelList.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TEntity> PasrsePayerActivityData<TEntity>(PayerSearchFilter payerSearchFilter, string AdditionalParameter) where TEntity : class, new()
        {
            string proceduureName = "GetPayerInformation";

            if (payerSearchFilter.IsPayerActivityCode == true)
            {
                proceduureName = "GetPayerInformation_Backup";
                AdditionalParameter = "@IsPayerActivityCode";
            }
            else if (AdditionalParameter == "@IsPayerServiceCode" || AdditionalParameter == "@IsPayerActivity")
            {
                proceduureName = "GetPayerInformation_Backup";
                //AdditionalParameter = "@IsPayerServiceCode";
            }
            else
            {
                proceduureName = "GetPayerInformation";

            }
            //if (payerSearchFilter.IsPayerActivityCode == true)
            //{
            //    return _context.ExecStoredProcedureListWithOutput<TEntity>("GetPayerInformation_Backup",
            //        typeof(TEntity).GetProperties().Count(),
            //  new SqlParameter()
            //  {
            //      ParameterName = "@ID",
            //      SqlDbType = SqlDbType.VarChar,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.ID,
            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@ID2",
            //      SqlDbType = SqlDbType.Int,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.ID2,
            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@Name",
            //      SqlDbType = SqlDbType.VarChar,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.Name,
            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@PageNumber",
            //      SqlDbType = SqlDbType.Int,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.PageNumber,
            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@PageSize",
            //      SqlDbType = SqlDbType.Int,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.PageSize,
            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@SortColumn",
            //      SqlDbType = SqlDbType.VarChar,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.SortColumn,
            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@SortOrder",
            //      SqlDbType = SqlDbType.VarChar,
            //      Direction = ParameterDirection.Input,
            //      Value = payerSearchFilter.SortOrder,

            //  },
            //  new SqlParameter()
            //  {
            //      ParameterName = "@IsPayerActivityCode",
            //      SqlDbType = SqlDbType.Bit,
            //      Direction = ParameterDirection.Input,
            //      Value = true,
            //  }
            // );
            //}
            //else
            //{
            return _context.ExecStoredProcedureListWithOutput<TEntity>(proceduureName,
                 typeof(TEntity).GetProperties().Count(),
           new SqlParameter()
           {
               ParameterName = "@ID",
               SqlDbType = SqlDbType.VarChar,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.ID,
           },
           new SqlParameter()
           {
               ParameterName = "@ID2",
               SqlDbType = SqlDbType.Int,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.ID2,
           },
           new SqlParameter()
           {
               ParameterName = "@Name",
               SqlDbType = SqlDbType.VarChar,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.Name,
           },
           new SqlParameter()
           {
               ParameterName = "@OrganizationID",
               SqlDbType = SqlDbType.Int,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.OrganizationID,
           },
           new SqlParameter()
           {
               ParameterName = "@PageNumber",
               SqlDbType = SqlDbType.Int,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.PageNumber,
           },
           new SqlParameter()
           {
               ParameterName = "@PageSize",
               SqlDbType = SqlDbType.Int,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.PageSize,
           },
           new SqlParameter()
           {
               ParameterName = "@SortColumn",
               SqlDbType = SqlDbType.VarChar,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.SortColumn,
           },
           new SqlParameter()
           {
               ParameterName = "@SortOrder",
               SqlDbType = SqlDbType.VarChar,
               Direction = ParameterDirection.Input,
               Value = payerSearchFilter.SortOrder,

           },
           new SqlParameter()
           {
               ParameterName = AdditionalParameter,
               SqlDbType = SqlDbType.Bit,
               Direction = ParameterDirection.Input,
               Value = true,
           }
            );
            //  }
        }

        public IList<TEntity> PasrsePayerData<TEntity>(PayerSearchFilter payerSearchFilter, string AdditionalParameter) where TEntity : class, new()
        {
            // return null;
            if (payerSearchFilter.IsPayerActivityCode == true)
            {
                return _context.ExecStoredProcedureListWithOutput<TEntity>("GetPayerInformation",
                    typeof(TEntity).GetProperties().Count(),
              new SqlParameter()
              {
                  ParameterName = "@ID",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.ID,
              },
              new SqlParameter()
              {
                  ParameterName = "@ID2",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.ID2,
              },
              new SqlParameter()
              {
                  ParameterName = "@Name",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.Name,
              },
              new SqlParameter()
              {
                  ParameterName = "@OrganizationID",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.OrganizationID,
              },
              new SqlParameter()
              {
                  ParameterName = "@PageNumber",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.PageNumber,
              },
              new SqlParameter()
              {
                  ParameterName = "@PageSize",
                  SqlDbType = SqlDbType.Int,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.PageSize,
              },
              new SqlParameter()
              {
                  ParameterName = "@SortColumn",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.SortColumn,
              },
              new SqlParameter()
              {
                  ParameterName = "@SortOrder",
                  SqlDbType = SqlDbType.VarChar,
                  Direction = ParameterDirection.Input,
                  Value = payerSearchFilter.SortOrder,

              },
              new SqlParameter()
              {
                  ParameterName = "@IsPayerActivityCode",
                  SqlDbType = SqlDbType.Bit,
                  Direction = ParameterDirection.Input,
                  Value = true,
              }
             );
            }
            else
            {
                return _context.ExecStoredProcedureListWithOutput<TEntity>("GetPayerInformation",
                     typeof(TEntity).GetProperties().Count(),
               new SqlParameter()
               {
                   ParameterName = "@ID",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.ID,
               },
               new SqlParameter()
               {
                   ParameterName = "@ID2",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.ID2,
               },
               new SqlParameter()
               {
                   ParameterName = "@Name",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.Name,
               },
               new SqlParameter()
               {
                   ParameterName = "@OrganizationID",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.OrganizationID,
               },
               new SqlParameter()
               {
                   ParameterName = "@PageNumber",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.PageNumber,
               },
               new SqlParameter()
               {
                   ParameterName = "@PageSize",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.PageSize,
               },
               new SqlParameter()
               {
                   ParameterName = "@SortColumn",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.SortColumn,
               },
               new SqlParameter()
               {
                   ParameterName = "@SortOrder",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = payerSearchFilter.SortOrder,

               },
               new SqlParameter()
               {
                   ParameterName = AdditionalParameter,
                   SqlDbType = SqlDbType.Bit,
                   Direction = ParameterDirection.Input,
                   Value = true,
               }
                );
            }
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
                //    dataSet = new DataSet(reader);
                //}
                var result = await cmd.ExecuteNonQueryAsync();
                return dataSet;
            }
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



        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public IQueryable<T> GetPayerServiceCodeDetailsById<T>(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerAppointmentTypeId", payerAppointmentTypeId),
            new SqlParameter("@PayerServiceCodeId",payerServiceCodeId)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetPayerServiceCodeDetail.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetMasterActivitiesForPayer<T>(int payerId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerId", payerId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetMasterAppointmentTypeForPayer.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
