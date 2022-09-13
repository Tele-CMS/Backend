using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.ReviewSystem;
using HC.Patient.Repositories.IRepositories.ReviewSystem;
using HC.Patient.Service.IServices.ReviewSystem;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HC.Patient.Service.Services.ReviewSystem
{
    public class ReviewSystemService : BaseService, IReviewSystemService
    {
        private JsonModel response = null;
        private IReviewSystemRepository _reviewSystemRepository;
        public ReviewSystemService(IReviewSystemRepository reviewSystemRepository)
        {
            _reviewSystemRepository = reviewSystemRepository;
        }

        public JsonModel SaveReviewSystem(ROSModel model)
        {
            var result = _reviewSystemRepository.SaveReviewSystem(model);
            if(result)
            {
                response = new JsonModel(model, StatusMessage.UpdatedSuccessfully, (int)HttpStatusCode.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.ErrorOccured, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }

        public JsonModel GetReviewSystem(GetROSModel model)

        {
            FullReviewSystemDto fullReview = new FullReviewSystemDto();
            fullReview = _reviewSystemRepository.GetReviewSystemData<FullReviewSystemDto>(model);
            //if (categoryModels != null && categoryModels.Count > 0)
            //{
            //    response = new JsonModel(categoryModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            //    response.meta = new Meta(categoryModels, categoryFilterModel);
            //}
           
            //ReviewSystemDto reviewSystem = new ReviewSystemDto();
            //ReviewSystemDto savedReviewSystem = new ReviewSystemDto();
            //reviewSystem.system = data.system;
            //reviewSystem.history = data.history;
            //fullReview.reviewSystemDto = reviewSystem;
            //savedReviewSystem.system = data.system;
            //savedReviewSystem.history = data.history2;
            //fullReview.savedReviewSystemDto = savedReviewSystem;
            // return fullReview;
            response = new JsonModel(fullReview, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            return response;
        }
    }

}

