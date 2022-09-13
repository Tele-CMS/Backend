using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientPhoneAddressService : BaseService, IPatientPhoneAddressService
    {
        private readonly IPatientAddressRepository _patientAddressRepository;
        private readonly IPatientRepository _patientRepository;
        JsonModel response = new JsonModel();
        public PatientPhoneAddressService(IPatientAddressRepository patientAddressRepository, IPatientRepository patientRepository)
        {
            _patientAddressRepository = patientAddressRepository;
            _patientRepository = patientRepository;
        }

        public JsonModel GetPatientPhoneAddress(int patientId, TokenModel tokenModel)
        {
            Dictionary<string, object> patientAddressModel = _patientAddressRepository.GetPatientPhoneAddress(patientId, tokenModel);
            if (patientAddressModel != null && patientAddressModel.Count > 0)
            { response = new JsonModel(patientAddressModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK); }
            else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }
            return response;
        }

        public JsonModel SavePhoneAddress(int patientId, PhoneAddressModel phoneAddressModel, TokenModel tokenModel)
        {
            XElement addressNodes = new XElement("Parent");
            XElement phoneNumberNodes = new XElement("Parent");
            phoneAddressModel.PatientAddress.ForEach(x =>
            {
                addressNodes.Add(new XElement("Child",
                    new XElement("Id", x.Id),
                    new XElement("Address1", x.Address1),
                    new XElement("Address2", x.Address2),
                    new XElement("ApartmentNumber", x.ApartmentNumber),
                    new XElement("AddressTypeId", x.AddressTypeId),
                    new XElement("CountryId", x.CountryId),
                    new XElement("City", x.City),
                    new XElement("StateId", x.StateId),
                    new XElement("Zip", x.Zip),
                    new XElement("PatientLocationId", x.PatientLocationId),
                    new XElement("IsPrimary", x.IsPrimary),
                    new XElement("IsMailingSame", x.IsMailingSame),
                    new XElement("Latitude", x.Latitude),
                    new XElement("Longitude", x.Longitude),
                    new XElement("IsDeleted", x.IsDeleted),
                    new XElement("Others", x.Others)
                    ));
            });

            phoneAddressModel.PhoneNumbers.ForEach(x =>
            {
                phoneNumberNodes.Add(new XElement("Child",
                    new XElement("Id", x.Id),
                    new XElement("PhoneNumberTypeId", x.PhoneNumberTypeId),
                    new XElement("PhoneNumber", x.PhoneNumber),
                    new XElement("PreferenceID", x.PreferenceID),
                    new XElement("OtherPhoneNumberType", x.OtherPhoneNumberType),
                    new XElement("IsDeleted", x.IsDeleted)
                    ));
            });
            SQLResponseModel sqlResponse= _patientAddressRepository.SavePatientAddressAndPhoneNumbers<SQLResponseModel>(patientId, addressNodes.ToString(), phoneNumberNodes.ToString(), tokenModel).FirstOrDefault();
            return new JsonModel(null,sqlResponse.Message,sqlResponse.StatusCode,string.Empty);
        }
    }
}
