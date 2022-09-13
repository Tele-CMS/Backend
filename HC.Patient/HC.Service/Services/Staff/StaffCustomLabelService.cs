using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.Staff;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Staff
{
    public class StaffCustomLabelService : BaseService, IStaffCustomLabelService
    {
        private readonly IStaffCustomLabelRepository _staffCustomLabelRepository;
        JsonModel response;        
        public StaffCustomLabelService(IStaffCustomLabelRepository staffCustomLabelRepository)
        {
            _staffCustomLabelRepository = staffCustomLabelRepository;            
        }
        public JsonModel GetStaffCustomLabels(int staffId, TokenModel tokenModel)
        {
            Dictionary<string, object> staffCustomLabels = _staffCustomLabelRepository.GetStaffCustomLabel(staffId, tokenModel);
            if ((List<MasterCustomLabelModel>)staffCustomLabels["MasterCustomLabels"] != null)
            {
                response = new JsonModel(staffCustomLabels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            else
            {
                response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }

        public JsonModel SaveCustomLabels(List<StaffCustomLabelModel> staffCustomLabelModels, TokenModel tokenModel)
        {
            List<StaffCustomLabels> staffCustomLabelList = null;
            StaffCustomLabels staffCustomLabels = null;
            DateTime CurrentDate = DateTime.UtcNow;
            int? staffId = 0;
            if (staffCustomLabelModels != null && staffCustomLabelModels.Exists(a => a.Id > 0))
            {
                staffId = staffCustomLabelModels.FirstOrDefault().StaffID;
                staffCustomLabelList = _staffCustomLabelRepository.GetAll(a => a.StaffID == staffId && a.IsDeleted == false && a.IsActive == true).ToList();
                foreach (var item in staffCustomLabelModels)
                {
                    if (item.Id > 0)
                    {
                        staffCustomLabels = staffCustomLabelList.Where(a => a.Id == item.Id).FirstOrDefault();
                        staffCustomLabels.UpdatedBy = tokenModel.UserID;
                        staffCustomLabels.UpdatedDate = CurrentDate;
                        staffCustomLabels.CustomLabelValue = item.CustomLabelValue;
                        staffCustomLabels.CustomLabelID = item.CustomLabelID;
                        staffCustomLabels.CustomLabelDataType = CommonMethods.ParseString(item.CustomLabelValue).ToString();
                        _staffCustomLabelRepository.Update(staffCustomLabels);
                    }
                    else
                    {
                        staffCustomLabels = new StaffCustomLabels();
                        AutoMapper.Mapper.Map(item, staffCustomLabels);
                        staffCustomLabels.CreatedBy = tokenModel.UserID;
                        staffCustomLabels.CreatedDate = CurrentDate;
                        staffCustomLabels.IsActive = true;
                        staffCustomLabels.IsDeleted = false;
                        staffCustomLabels.CustomLabelDataType = CommonMethods.ParseString(item.CustomLabelValue).ToString();
                        _staffCustomLabelRepository.Create(staffCustomLabels);
                    }
                }
                response = new JsonModel(staffCustomLabelList, StatusMessage.StaffCustomLabelUpdated, (int)HttpStatusCode.OK);
            }
            else
            {
                staffCustomLabelList = new List<StaffCustomLabels>();
                AutoMapper.Mapper.Map(staffCustomLabelModels, staffCustomLabelList);
                staffCustomLabelList.ForEach(a => { a.CreatedBy = tokenModel.UserID; a.CreatedDate = CurrentDate; a.IsActive = true; a.IsDeleted = false; a.CustomLabelDataType = CommonMethods.ParseString(a.CustomLabelValue).ToString(); });
                _staffCustomLabelRepository.Create(staffCustomLabelList.ToArray());
                response = new JsonModel(staffCustomLabelList, StatusMessage.StaffCustomLabelSaved, (int)HttpStatusCode.OK);
            }
            _staffCustomLabelRepository.SaveChanges();  
             return response;
    }
    }
}
