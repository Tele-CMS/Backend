using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.ReviewSystem;
using HC.Patient.Repositories.IRepositories.ReviewSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HC.Patient.Repositories.Repositories.ReviewSystem
{
    public class ReviewSystemRepository : IReviewSystemRepository
    {
        private readonly HCOrganizationContext _context;
        public ReviewSystemRepository(HCOrganizationContext organizationContext)
        {
            this._context = organizationContext;
        }
        public bool SaveReviewSystem(ROSModel request)
        {
            try
            {
                ClinicalDatagridSystemHistory categories = new ClinicalDatagridSystemHistory();
                foreach (var item in request.system_review)
                {
                    ClinicalDatagridSystemCategory datagridSystem = new ClinicalDatagridSystemCategory();
                    //if (item.saved_category_id > 0)
                    //{
                    datagridSystem = _context.ClinicalDatagridSystemCategory.Where(x => x.id == item.saved_category_id && x.patient_empi == request.patient_id && x.encounter_id == request.encounter_id).FirstOrDefault();
                    if (datagridSystem != null)
                    {
                        datagridSystem.comments = item.comments;
                        datagridSystem.UpdatedDate = DateTime.UtcNow;
                        datagridSystem.UpdatedBy = request.user_id;
                    }

                    //}
                    else
                    {
                        datagridSystem = new ClinicalDatagridSystemCategory();
                        datagridSystem.patient_empi = request.patient_id;
                        datagridSystem.encounter_id = request.encounter_id;
                        datagridSystem.system_id = item.system_id;
                        datagridSystem.comments = item.comments;
                        datagridSystem.CreatedDate = DateTime.UtcNow;
                        datagridSystem.CreatedBy = request.user_id;
                        datagridSystem.IsActive = true;
                        datagridSystem.active = true;

                        _context.ClinicalDatagridSystemCategory.Add(datagridSystem);
                    }
                    _context.SaveChanges();
                    //   await this._context.SaveChangesAsync(cancellationToken);
                    if (item.selectedhistoptions != null)
                    {
                        foreach (var history in item.selectedhistoptions)
                        {
                            ClinicalDatagridSystemHistory clinicalDatagridHistory = new ClinicalDatagridSystemHistory();
                            clinicalDatagridHistory = this._context.ClinicalDatagridSystemHistory.Where(x => x.history_id == history.history_id && x.clinical_system_id == datagridSystem.id).FirstOrDefault();
                            if (clinicalDatagridHistory != null)
                            {
                                clinicalDatagridHistory.active = history.active;
                                clinicalDatagridHistory.UpdatedDate = DateTime.UtcNow;
                                clinicalDatagridHistory.UpdatedBy = request.user_id;
                            }
                            else
                            {
                                ClinicalDatagridSystemHistory datagridHistory = new ClinicalDatagridSystemHistory();
                                datagridHistory.clinical_system_id = datagridSystem.id;
                                datagridHistory.history_id = history.history_id;
                                datagridHistory.active = history.active;
                                datagridHistory.CreatedDate = DateTime.UtcNow;
                                datagridHistory.CreatedBy = request.user_id;
                                _context.ClinicalDatagridSystemHistory.Add(datagridHistory);
                            }
                        }
                        _context.SaveChanges();
                        //    await this._context.SaveChangesAsync(cancellationToken);

                    }
                    _context.SaveChanges();
                    //await this._context.SaveChangesAsync(cancellationToken);

                }
            //    _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        //  public FullReviewSystemDto GetReviewSystemData(ReviewSystemQuery request)
        public FullReviewSystemDto GetReviewSystemData<T>(GetROSModel model)
        {
            try
            {
                //List<FullReviewSystemDto> modelList = new List<FullReviewSystemDto>();
                SqlParameter[] parameters = { new SqlParameter ("@emp_id", Convert.ToInt32(model.emp_id) ),
                                          new SqlParameter ("@encounter_id", Convert.ToInt32(model.encounter_id) )};
                return _context.ExecStoredProcedureListWithOutputForROS("usr_getmastersystemandhistory", parameters.Length, parameters);

               // modelList = _context.Set<FullReviewSystemDto>().FromSql("usr_getmastersystemandhistory @emp_id = {0},@encounter_id = {1}", model.emp_id, model.encounter_id).ToList();

               //// using (var con = _context.GetConnection())
               // {
               //     FullReviewSystemDto fullReview = new FullReviewSystemDto();
               //     //DynamicParameters ObjParm = new DynamicParameters();
               //     //ObjParm.Add("@emp_id", request.emp_id);
               //     //ObjParm.Add("@encounter_id", request.encounter_id);
               //     var result = (await con.QueryMultipleAsync(DapperConstants.usr_getmastersystemandhistory, ObjParm, commandType: CommandType.StoredProcedure));
               //     ReviewSystemDto reviewSystem = new ReviewSystemDto();
               //     ReviewSystemDto savedReviewSystem = new ReviewSystemDto();
               //     //reviewSystem.system = result<ReviewSystemCategoryDto>().Result.ToList();
               //     //reviewSystem.history = result.ReadAsync<ReviewSystemHistoryDto>().Result.ToList();
               //     //fullReview.reviewSystemDto = reviewSystem;
               //     //savedReviewSystem.system = result.ReadAsync<ReviewSystemCategoryDto>().Result.ToList();
               //     //savedReviewSystem.history = result.ReadAsync<ReviewSystemHistoryDto>().Result.ToList();
               //     //fullReview.savedReviewSystemDto = savedReviewSystem;
               //     //return fullReview;
               // }
                //return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}
