using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Patient.Service.IServices.SecurityQuestion;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Service.Services.SecurityQuestion
{
    public class SecurityQuestionService : BaseService, ISecurityQuestionService
    {
        private HCOrganizationContext _organizationContext;
        private IMasterSecurityQuestionsRepository _masterSecurityQuestionsRepository;
        private ISecurityQuestionsRepository _securityQuestionsRepository;
        private IUserSecurityQuestionAnswerRepository _userSecurityQuestionAnswerRepository;
        public SecurityQuestionService(IMasterSecurityQuestionsRepository masterSecurityQuestionsRepository, ISecurityQuestionsRepository securityQuestionsRepository, IUserSecurityQuestionAnswerRepository userSecurityQuestionAnswerRepository, HCOrganizationContext organizationContext)
        {
            _organizationContext = organizationContext;
            _masterSecurityQuestionsRepository = masterSecurityQuestionsRepository;
            _securityQuestionsRepository = securityQuestionsRepository;
            _userSecurityQuestionAnswerRepository = userSecurityQuestionAnswerRepository;            
        }
        /// <summary>
        /// this will return questions for user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<UserSecurityQuestionAnswer> GetUserSecurityQuestionsById(int UserId, TokenModel token)
        {
            return _userSecurityQuestionAnswerRepository.GetAll(a => a.UserID == UserId && a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).ToList();
        }
        public bool CheckUserQuestionAnswer(SecurityQuestionModel securityQuestionModel, TokenModel token)
        {
            UserSecurityQuestionAnswer userSecurityQuestionAnswerDB = _userSecurityQuestionAnswerRepository.Get(a => a.QuestionID == securityQuestionModel.QuestionID && a.UserID == token.UserID && a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID);
            if (userSecurityQuestionAnswerDB != null && userSecurityQuestionAnswerDB.Answer.Equals(CommonMethods.Encrypt(securityQuestionModel.Answer.ToLower())))
            {
                return true; //if answer match
            }
            return false;
        }

        public bool CheckUserQuestionAnswer(int QuestionID, int UserID, int OrganizationID, string Answer)
        {
            return _securityQuestionsRepository.CheckUserQuestionAnswer(QuestionID, UserID, OrganizationID, CommonMethods.Encrypt(Answer));
        }

        public List<UserSecurityQuestionAnswer> GetUserSecurityQuestionsById(int UserId, int OrganizationID)
        {
            return _userSecurityQuestionAnswerRepository.GetAll(a => a.UserID == UserId
                && a.IsActive == true
                && a.IsDeleted == false
                && a.OrganizationID == OrganizationID
            ).ToList();
        }

        public List<SecurityQuestions> GetSecurityQuestion(TokenModel token)
        {
            return _securityQuestionsRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).ToList();
        }

        //public bool SaveUserSecurityQuestion(SecurityQuestionListModel securityQuestionListModel, TokenModel token)
        //{

        //    try
        //    {
        //        var SecurityQuestions = _userSecurityQuestionAnswerRepository.GetAll(x => x.IsActive == true
        //                                    && x.IsDeleted == false && x.UserID == token.UserID
        //                                    && x.OrganizationID == token.OrganizationID
        //                                ).ToList();

        //        foreach (var item in securityQuestionListModel.SecurityQuestionList.Where(x => x.Id > 0))
        //        {
        //            var Question = SecurityQuestions.Where(x => x.Id == item.Id).FirstOrDefault();
        //            Question.Answer = CommonMethods.Encrypt(item.Answer.ToLower());
        //            _userSecurityQuestionAnswerRepository.Update(Question);
        //        }
        //        foreach (var item in securityQuestionListModel.SecurityQuestionList.Where(x => x.Id == 0))
        //        {
        //            _userSecurityQuestionAnswerRepository.Create(new UserSecurityQuestionAnswer()
        //            {
        //                Answer = CommonMethods.Encrypt(item.Answer.ToLower()),
        //                QuestionID = item.QuestionID,
        //                UserID = token.UserID,
        //                CreatedBy = token.UserID,
        //                CreatedDate = DateTime.UtcNow,
        //                IsActive = true,
        //                IsDeleted = false,
        //                OrganizationID = token.OrganizationID
        //            });
        //        }
        //        _securityQuestionsRepository.SaveChanges();

        //        return true; //On Sucess return true

        //    }
        //    catch (Exception ex)
        //    {
        //        return false; //somthing went worng return false
        //    }

        //}

        public bool SaveUserSecurityQuestion(SecurityQuestionListModel securityQuestionListModel, TokenModel token)
        {
            using (var transaction = _organizationContext.Database.BeginTransaction()) //TO DO do this with SP
            {
                try
                {

                    List<UserSecurityQuestionAnswer> userSecurityQuestionList = new List<UserSecurityQuestionAnswer>();
                    List<UserSecurityQuestionAnswer> userSecurityQuestionUpdateList = new List<UserSecurityQuestionAnswer>();
                    foreach (var item in securityQuestionListModel.SecurityQuestionList)
                    {
                        if (item.Id > 0) //Update
                        {
                            UserSecurityQuestionAnswer userSecurityQuestionAnswerUpdate = _userSecurityQuestionAnswerRepository.GetByID(item.Id);
                            userSecurityQuestionAnswerUpdate.Answer = CommonMethods.Encrypt(item.Answer.ToLower());
                            userSecurityQuestionUpdateList.Add(userSecurityQuestionAnswerUpdate);
                        }
                        else //Insert
                        {
                            UserSecurityQuestionAnswer userSecurityQuestionAnswer = new UserSecurityQuestionAnswer();
                            userSecurityQuestionAnswer.Answer = CommonMethods.Encrypt(item.Answer.ToLower());
                            userSecurityQuestionAnswer.QuestionID = item.QuestionID;
                            userSecurityQuestionAnswer.UserID = token.UserID;
                            userSecurityQuestionAnswer.CreatedBy = token.UserID;
                            userSecurityQuestionAnswer.CreatedDate = DateTime.UtcNow;
                            userSecurityQuestionAnswer.IsActive = true;
                            userSecurityQuestionAnswer.IsDeleted = false;
                            userSecurityQuestionAnswer.OrganizationID = token.OrganizationID;
                            userSecurityQuestionList.Add(userSecurityQuestionAnswer);
                        }
                    }
                    _userSecurityQuestionAnswerRepository.Update(userSecurityQuestionUpdateList.ToArray()); // Insert List
                    _userSecurityQuestionAnswerRepository.Create(userSecurityQuestionList.ToArray()); //Update List 
                    _userSecurityQuestionAnswerRepository.SaveChanges();

                    //transaction commit
                    transaction.Commit();

                    return true; //On Sucess return true

                }
                catch (Exception)
                {
                    //on error transaction rollback
                    transaction.Rollback();
                    return false; //somthing went worng return false
                }
            }
        }
    }
}
