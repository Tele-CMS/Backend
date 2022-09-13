using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Payroll;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Patient.Service.IServices.Payroll;
using HC.Service;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payroll
{
    public class StaffPayrollRateForActivityService : BaseService, IStaffPayrollRateForActivityService
    {
        private readonly IStaffPayrollRateForActivityRepository _staffPayrollRateForActivityRepository;
        private JsonModel response = null;
        public StaffPayrollRateForActivityService(IStaffPayrollRateForActivityRepository staffPayrollRateForActivityRepository)
        {
            _staffPayrollRateForActivityRepository = staffPayrollRateForActivityRepository;
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        }

        public JsonModel SaveUpdateStaffPayrollRateForActivity(List<StaffPayrollRateForActivityModel> staffPayrollRateForActivityModels, TokenModel tokenModel)
        {
            XElement headerElement = null;
            if (staffPayrollRateForActivityModels != null && staffPayrollRateForActivityModels.Count > 0)
            {
                headerElement = new XElement("Parent");
                staffPayrollRateForActivityModels.ForEach(x =>
                {
                    headerElement.Add(new XElement("Child",
                        new XElement("Id", x.Id),
                        new XElement("UserId", tokenModel.UserID),
                        new XElement("AppointmentTypeId", x.AppointmentTypeId),
                        new XElement("PayRate", x.PayRate),
                        new XElement("StaffId", x.StaffId)
                        ));
                });
                SQLResponseModel sqlResponse = _staffPayrollRateForActivityRepository.SaveUpdateStaffPayrollRateForActivity<SQLResponseModel>(headerElement).FirstOrDefault();
                return new JsonModel(null, sqlResponse.Message, sqlResponse.StatusCode);
            }
            return response;
        }
        public JsonModel GetStaffPayRateOfActivity(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<StaffPayrollRateForActivityModel> staffPayrollRateForActivityModels = _staffPayrollRateForActivityRepository.GetStaffPayRateOfActivity<StaffPayrollRateForActivityModel>(searchFilterModel, tokenModel).ToList();
            if(staffPayrollRateForActivityModels !=null && staffPayrollRateForActivityModels.Count > 0)
            {
                response = new JsonModel(staffPayrollRateForActivityModels, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                response.meta = new Meta(staffPayrollRateForActivityModels,searchFilterModel);
            }
            return response;
        }
    }
}
