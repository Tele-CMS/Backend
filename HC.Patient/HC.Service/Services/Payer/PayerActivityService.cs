using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payer
{
    public class PayerActivityService : BaseService, IPayerActivityService
    {
        private readonly IPayerActivityRepository _payerActivityRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = null;

        public PayerActivityService(IPayerActivityRepository payerActivityRepository, IUserCommonRepository userCommonRepository)
        {

            _payerActivityRepository = payerActivityRepository;
            response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetActivities(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {

            List<PayerActivityModel> payerActivityModels = _payerActivityRepository.GetActivities<PayerActivityModel>(searchFilterModel, tokenModel).ToList();
            if (payerActivityModels != null && payerActivityModels.Count > 0)
            {
                response = new JsonModel(payerActivityModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(payerActivityModels, searchFilterModel);
            }
            return response;
        }

        public JsonModel GetPayerActivityServiceCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<PayerActivityServiceCodeModel> result = _payerActivityRepository.GetPayerActivityServiceCodes<PayerActivityServiceCodeModel>(searchFilterModel, tokenModel).ToList();
            if (result != null && result.Count > 0)
            {
                response = new JsonModel(result, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                response.meta = new Meta(result, searchFilterModel);
            }
            return response;
        }

        public JsonModel GetMasterActivitiesForPayer(SearchFilterModel searchFilterModel, TokenModel token)
        {

            List<MasterActivitiesModel> result = _payerActivityRepository.GetMasterActivitiesForPayer<MasterActivitiesModel>(searchFilterModel, token).ToList();
            if (result != null && result.Count > 0)
            {
                response = new JsonModel(result, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                response.meta = new Meta(result, searchFilterModel);
            }
            return response;
        }

        public JsonModel UpdatePayerActivityModifiers(PayerAppointmentTypesModel payerAppointmentTypesModel, TokenModel token)
        {
            try
            {
                PayerAppointmentTypes payerAppointmentType = _payerActivityRepository.GetByID(payerAppointmentTypesModel.Id);

                payerAppointmentType.RatePerUnit = payerAppointmentTypesModel.RatePerUnit;
                payerAppointmentType.Modifier1 = payerAppointmentTypesModel.Modifier1;
                payerAppointmentType.Modifier2 = payerAppointmentTypesModel.Modifier2;
                payerAppointmentType.Modifier3 = payerAppointmentTypesModel.Modifier3;
                payerAppointmentType.Modifier4 = payerAppointmentTypesModel.Modifier4;

                _payerActivityRepository.Update(payerAppointmentType);
                _payerActivityRepository.SaveChanges();


                return new JsonModel()
                {
                    data = payerAppointmentType,
                    Message = StatusMessage.PayerActivityUpdated,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel DeletePayerActivityCode(int id, TokenModel tokenModel)
        {
            PayerAppointmentTypes payerAppointmentType = _payerActivityRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (payerAppointmentType != null)
            {
                payerAppointmentType.IsDeleted = true;
                payerAppointmentType.DeletedBy = tokenModel.UserID;
                payerAppointmentType.DeletedDate = DateTime.UtcNow;
                _payerActivityRepository.Update(payerAppointmentType);
                _payerActivityRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.PayerActivityDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel SavePayerActivityCode(PayerActivityCodeModel payerActivityCodeModel, TokenModel tokenModel)
        {
            PayerAppointmentTypes payerAppointmentTypes = null;
            if (payerActivityCodeModel.Id == 0)
            {
                payerAppointmentTypes = new PayerAppointmentTypes();
                AutoMapper.Mapper.Map(payerActivityCodeModel, payerAppointmentTypes);
                payerAppointmentTypes.CreatedBy = tokenModel.UserID;
                payerAppointmentTypes.CreatedDate = DateTime.UtcNow;
                payerAppointmentTypes.IsActive = true;
                payerAppointmentTypes.IsDeleted = false;
                _payerActivityRepository.Create(payerAppointmentTypes);
                _payerActivityRepository.SaveChanges();
                response = new JsonModel(payerAppointmentTypes, StatusMessage.PayerActivitySave, (int)HttpStatusCode.OK);
            }
            return response;
        }

    }
}
