using HC.Model;
using HC.Patient.Model.ReviewSystem;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.ReviewSystem
{
    public interface IReviewSystemService : IBaseService
    {
        JsonModel GetReviewSystem(GetROSModel model);
        JsonModel SaveReviewSystem(ROSModel model);
    }
}
