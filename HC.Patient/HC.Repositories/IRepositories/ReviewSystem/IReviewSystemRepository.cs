using HC.Patient.Model.ReviewSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.ReviewSystem
{
    public interface IReviewSystemRepository
    {
        public FullReviewSystemDto GetReviewSystemData<T>(GetROSModel model);
        public bool SaveReviewSystem(ROSModel request);
    }
}
