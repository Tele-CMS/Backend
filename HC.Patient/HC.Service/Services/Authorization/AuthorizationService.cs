using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Authorization;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Authorization;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Authorization
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private HCOrganizationContext _context;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly IPatientAuthorizationRepository _patientAuthorizationRepository;
        private readonly IPatientAuthorizationProceduresRepository _patientAuthorizationProceduresRepository;
        private readonly IPatientAuthorizationProcedureCPTLinkRepository _patientAuthorizationProcedureCPTLinkRepository;
        private readonly IAuthorizationRepository _authorizationRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);

        public AuthorizationService(HCOrganizationContext context, IUserCommonRepository userCommonRepository, IPatientAuthorizationRepository patientAuthorizationRepository, IPatientAuthorizationProceduresRepository patientAuthorizationProceduresRepository, IPatientAuthorizationProcedureCPTLinkRepository patientAuthorizationProcedureCPTLinkRepository, IAuthorizationRepository authorizationRepository)
        {
            _context = context;
            _userCommonRepository = userCommonRepository;
            _patientAuthorizationRepository = patientAuthorizationRepository;
            _patientAuthorizationProceduresRepository = patientAuthorizationProceduresRepository;
            _patientAuthorizationProcedureCPTLinkRepository = patientAuthorizationProcedureCPTLinkRepository;
            _authorizationRepository = authorizationRepository;
        }

        public JsonModel GetAllAuthorizationsForPatient(int patientId, int pageNumber, int pageSize, string authType, TokenModel token)
        {
            try
            {
                Dictionary<string, object> dict = _patientAuthorizationRepository.GetAllAuthorizationsForPatient(patientId, pageNumber, pageSize, authType);
                return new JsonModel()
                {
                    data = dict,
                    meta = new Meta()
                    {
                        TotalRecords = (List<AuthorizationModel>)dict["Authorization"] != null && ((List<AuthorizationModel>)dict["Authorization"]).Count > 0 ? ((List<AuthorizationModel>)dict["Authorization"]).First().TotalCount : 0
                        ,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(((List<AuthorizationModel>)dict["Authorization"] != null && ((List<AuthorizationModel>)dict["Authorization"]).Count > 0) ? ((List<AuthorizationModel>)dict["Authorization"]).First().TotalCount : 0) / pageSize)
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }

        public JsonModel SaveAuthorization(AuthModel authModel, TokenModel tokenModel)
        {
            Entity.Authorization authorization = null;
            List<AuthorizationProcedures> authorizationProceduresList = null;
            List<AuthProcedureCPT> authProcedureCPTList = null;
            List<AuthProcedureCPTModifiers> authProcedureCPTModifiersList = null;

            AuthorizationProcedures authProc = null;
            AuthProcedureCPT authProcCPT = null;
            AuthProcedureCPTModifiers authProcCPTModifiers = null;

            DateTime Currentdate = DateTime.UtcNow;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (authModel.Id == 0) //new insert
                    {
                        //new initialization
                        authorization = new Entity.Authorization();
                        authorizationProceduresList = new List<AuthorizationProcedures>();

                        //map autorization
                        AutoMapper.Mapper.Map(authModel, authorization);
                        authorization.CreatedBy = tokenModel.UserID;
                        authorization.CreatedDate = Currentdate;
                        authorization.IsActive = true;
                        authorization.IsDeleted = false;
                        authorization.OrganizationID = tokenModel.OrganizationID;

                        //foreach on authorization procedures 
                        foreach (AuthProceduresModel item in authModel.AuthorizationProcedures)
                        {
                            authProcedureCPTList = new List<AuthProcedureCPT>();
                            authProc = new AuthorizationProcedures();
                            authProcedureCPTModifiersList = new List<AuthProcedureCPTModifiers>();

                            //map authorization procedures one by one
                            AutoMapper.Mapper.Map(item, authProc);
                            authProc.CreatedBy = tokenModel.UserID;
                            authProc.CreatedDate = Currentdate;
                            authProc.IsActive = true;
                            authProc.IsDeleted = false;
                            foreach (AuthProcedureCPTModel cpt in item.AuthProcedureCPT)
                            {
                                authProcCPT = new AuthProcedureCPT();

                                //map authorization procedures cpt one by one
                                AutoMapper.Mapper.Map(cpt, authProcCPT);
                                authProcCPT.CreatedBy = tokenModel.UserID;
                                authProcCPT.CreatedDate = Currentdate;
                                authProcCPT.IsActive = true;
                                authProcCPT.IsDeleted = false;

                                foreach (AuthProcedureCPTModifierModel cptModifier in cpt.AuthProcedureCPTModifiers)
                                {
                                    authProcCPTModifiers = new AuthProcedureCPTModifiers();

                                    //map authorization procedures cpt one by one
                                    AutoMapper.Mapper.Map(cptModifier, authProcCPTModifiers);
                                    authProcCPTModifiers.CreatedBy = tokenModel.UserID;
                                    authProcCPTModifiers.CreatedDate = Currentdate;
                                    authProcCPTModifiers.IsActive = true;
                                    authProcCPTModifiers.IsDeleted = false;
                                    authProcedureCPTModifiersList.Add(authProcCPTModifiers);
                                }

                                authProcCPT.AuthProcedureCPTModifiers = authProcedureCPTModifiersList;
                                authProcedureCPTList.Add(authProcCPT);
                            }
                            authProc.AuthProcedureCPTLink = authProcedureCPTList;
                            authorizationProceduresList.Add(authProc);
                        }
                        //authProcedures
                        authorization.AuthorizationProcedures = authorizationProceduresList;
                        _context.Authorization.Add(authorization);
                        _context.SaveChanges();

                        //return respose
                        response = new JsonModel(authorization, StatusMessage.AuthorizationSave, (int)HttpStatusCodes.OK);
                    }
                    else
                    {
                        authorization = _patientAuthorizationRepository.Get(a => a.Id == authModel.Id && a.IsDeleted == false && a.IsActive == true);
                        if (authorization != null)
                        {
                            //map autorization
                            authorization.AuthorizationNumber = authModel.AuthorizationNumber;
                            authorization.AuthorizationTitle = authModel.AuthorizationTitle;
                            authorization.EndDate = authModel.EndDate;
                            authorization.PatientInsuranceId = authModel.PatientInsuranceId;
                            authorization.IsVerified = authModel.IsVerified;
                            authorization.Notes = authModel.Notes;
                            authorization.StartDate = authModel.StartDate;
                            authorization.UpdatedDate = Currentdate;
                            authorization.UpdatedBy = tokenModel.UserID;

                            //foreach on authorization procedures 
                            foreach (AuthProceduresModel item in authModel.AuthorizationProcedures)
                            {
                                if (item.Id == 0) // when there is new authorization procedure in update case
                                {
                                    authProcedureCPTList = new List<AuthProcedureCPT>();
                                    authProc = new AuthorizationProcedures();
                                    authProcedureCPTModifiersList = new List<AuthProcedureCPTModifiers>();

                                    //map authorization procedures one by one
                                    AutoMapper.Mapper.Map(item, authProc);
                                    authProc.CreatedBy = tokenModel.UserID;
                                    authProc.CreatedDate = Currentdate;
                                    authProc.IsActive = true;
                                    authProc.IsDeleted = false;
                                    foreach (AuthProcedureCPTModel cpt in item.AuthProcedureCPT)
                                    {
                                        authProcCPT = new AuthProcedureCPT();

                                        //map authorization procedures cpt one by one
                                        AutoMapper.Mapper.Map(cpt, authProcCPT);
                                        authProcCPT.CreatedBy = tokenModel.UserID;
                                        authProcCPT.CreatedDate = Currentdate;
                                        authProcCPT.IsActive = true;
                                        authProcCPT.IsDeleted = false;

                                        foreach (AuthProcedureCPTModifierModel cptModifier in cpt.AuthProcedureCPTModifiers)
                                        {
                                            authProcCPTModifiers = new AuthProcedureCPTModifiers();
                                            //map authorization procedures cpt one by one
                                            AutoMapper.Mapper.Map(cptModifier, authProcCPTModifiers);
                                            authProcCPTModifiers.CreatedBy = tokenModel.UserID;
                                            authProcCPTModifiers.CreatedDate = Currentdate;
                                            authProcCPTModifiers.IsActive = true;
                                            authProcCPTModifiers.IsDeleted = false;
                                            authProcedureCPTModifiersList.Add(authProcCPTModifiers);
                                        }

                                        authProcCPT.AuthProcedureCPTModifiers = authProcedureCPTModifiersList;
                                        authProcedureCPTList.Add(authProcCPT);
                                    }
                                    authProc.AuthProcedureCPTLink = authProcedureCPTList;
                                    //authProcedures
                                    authorization.AuthorizationProcedures.Add(authProc);
                                }
                                else
                                {
                                    //get authorization procedures by id for update
                                    authProc = _patientAuthorizationProceduresRepository.Get(a => a.Id == item.Id && a.IsActive == true && a.IsDeleted == false);
                                    if (authProc != null)
                                    {
                                        authProcedureCPTList = new List<AuthProcedureCPT>();
                                        authProcedureCPTModifiersList = new List<AuthProcedureCPTModifiers>();
                                        AutoMapper.Mapper.Map(item, authProc);
                                        authProc.UpdatedBy = tokenModel.UserID;
                                        authProc.UpdatedDate = Currentdate;
                                        if (authProc.IsDeleted) { authProc.DeletedDate = Currentdate; authProc.DeletedBy = tokenModel.UserID; }

                                        foreach (var cpt in item.AuthProcedureCPT)
                                        {
                                            if (cpt.Id == 0)
                                            {
                                                authProcCPT = new AuthProcedureCPT();

                                                AutoMapper.Mapper.Map(cpt, authProcCPT);
                                                authProcCPT.CreatedBy = tokenModel.UserID;
                                                authProcCPT.CreatedDate = Currentdate;

                                                foreach (AuthProcedureCPTModifierModel cptModifier in cpt.AuthProcedureCPTModifiers)
                                                {
                                                    authProcCPTModifiers = new AuthProcedureCPTModifiers();
                                                    //map authorization procedures cpt one by one
                                                    AutoMapper.Mapper.Map(cptModifier, authProcCPTModifiers);
                                                    authProcCPTModifiers.CreatedBy = tokenModel.UserID;
                                                    authProcCPTModifiers.CreatedDate = Currentdate;
                                                    authProcCPTModifiers.IsActive = true;
                                                    authProcCPTModifiers.IsDeleted = false;
                                                    authProcedureCPTModifiersList.Add(authProcCPTModifiers);
                                                }

                                                authProcCPT.AuthProcedureCPTModifiers = authProcedureCPTModifiersList;
                                                authProcedureCPTList.Add(authProcCPT);
                                            }
                                            else
                                            {
                                                authProcCPT = _patientAuthorizationProcedureCPTLinkRepository.Get(a => a.Id == cpt.Id && a.IsActive == true && a.IsDeleted == false);
                                                if (authProcCPT != null)
                                                {
                                                    //authProcedureCPTList = new List<AuthProcedureCPT>();
                                                    //authProcedureCPTModifiersList = new List<AuthProcedureCPTModifiers>();
                                                    AutoMapper.Mapper.Map(cpt, authProcCPT);
                                                    authProcCPT.UpdatedBy = tokenModel.UserID;
                                                    authProcCPT.UpdatedDate = Currentdate;
                                                    if (authProcCPT.IsDeleted) { authProcCPT.DeletedDate = Currentdate; authProcCPT.DeletedBy = tokenModel.UserID; }

                                                    foreach (AuthProcedureCPTModifierModel cptModifier in cpt.AuthProcedureCPTModifiers)
                                                    {
                                                        if (cptModifier.Id == 0)
                                                        {
                                                            authProcCPTModifiers = new AuthProcedureCPTModifiers();

                                                            //map authorization procedures cpt one by one
                                                            AutoMapper.Mapper.Map(cptModifier, authProcCPTModifiers);
                                                            authProcCPTModifiers.CreatedBy = tokenModel.UserID;
                                                            authProcCPTModifiers.CreatedDate = Currentdate;
                                                            authProcCPTModifiers.IsActive = true;
                                                            authProcCPTModifiers.IsDeleted = false;
                                                            authProcedureCPTModifiersList.Add(authProcCPTModifiers);
                                                        }
                                                        else
                                                        {
                                                            authProcCPTModifiers = _context.AuthProcedureCPTModifiers.Where(a => a.Id == cptModifier.Id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();

                                                            //map authorization procedures cpt one by one
                                                            AutoMapper.Mapper.Map(cptModifier, authProcCPTModifiers);
                                                            authProcCPTModifiers.UpdatedBy = tokenModel.UserID;
                                                            authProcCPTModifiers.UpdatedDate = Currentdate;
                                                            if (cptModifier.IsDeleted) { authProcCPTModifiers.DeletedDate = Currentdate; authProcCPTModifiers.DeletedBy = tokenModel.UserID; }
                                                            authProcedureCPTModifiersList.Add(authProcCPTModifiers);
                                                        }
                                                    }

                                                    authProcCPT.AuthProcedureCPTModifiers = authProcedureCPTModifiersList;
                                                    authProcedureCPTList.Add(authProcCPT);

                                                }
                                            }
                                            authProc.AuthProcedureCPTLink.Add(authProcCPT);
                                        }
                                    }
                                }
                            }
                            authorization.AuthorizationProcedures.Add(authProc);
                            _context.Authorization.Update(authorization);
                            _context.SaveChanges();
                            //return response
                            response = new JsonModel(authorization, StatusMessage.AuthorizationUpdated, (int)HttpStatusCodes.OK);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = e.Message,
                        StatusCode = (int)HttpStatusCodes.InternalServerError//Error
                    };
                }
            }
            return response;
        }

        public JsonModel GetAuthorizationById(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {

            GetAuthorizationByIdModel authorization = _authorizationRepository.GetAutorizationById(searchFilterModel, tokenModel);
            AuthModel authModel = new AuthModel();
            if (authorization != null && authorization.Authorization != null)
            {
                authModel = authorization.Authorization;
                if (authorization.AuthorizationProcedures != null && authorization.AuthorizationProcedures.Count > 0)
                {
                    authModel.AuthorizationProcedures = authorization.AuthorizationProcedures;

                    authModel.AuthorizationProcedures.ForEach(a =>
                    {
                        if (authorization.AuthorizationProcedureCPT != null && authorization.AuthorizationProcedureCPT.Count > 0)
                        {
                            a.AuthProcedureCPT = authorization.AuthorizationProcedureCPT.Where(z => z.AuthorizationProceduresId == a.Id).ToList();

                            a.AuthProcedureCPT.ForEach(c =>
                            {
                                if(authorization.AuthorizationProcedureCPTModifiers !=null && authorization.AuthorizationProcedureCPTModifiers.Count > 0)
                                {
                                    c.AuthProcedureCPTModifiers = authorization.AuthorizationProcedureCPTModifiers.Where(b => b.AuthProcedureCPTLinkId == c.Id).ToList();
                                }

                            });
                        }
                    });

                }
                response = new JsonModel(authModel,StatusMessage.FetchMessage,(int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteAutorization(int id, TokenModel token)
        {
            //List<AuthorizationProcedures> authProcedure = _context.AuthorizationProcedures.Where(a => a.AuthorizationId == id && a.IsDeleted == false && a.IsActive == true).ToList();
            //List<AuthProcedureCPT> authProcedureCPT = _context.AuthProcedureCPT.Where(a => authProcedure.Select(z => z.Id).Contains(a.AuthorizationProceduresId) && a.IsActive == true && a.IsDeleted == false).ToList();
            //List<int> list = authProcedureCPT.Select(z => z.Id).ToList();
            //List<AppointmentAuthorization> appointmentAuth = _context.AppointmentAuthorization.Where(a => list.Contains(a.AuthProcedureCPTLinkId) && a.IsActive == true && a.IsDeleted == false).ToList();

            var appointmentAuth = _context.AuthorizationProcedures.Join(_context.AuthProcedureCPT,
                AP => AP.Id,
                AC => AC.AuthorizationProceduresId, (AP, AC) => new { AP, AC }).Join(_context.AppointmentAuthorization,
                AA => AA.AC.Id, AAA => AAA.AuthProcedureCPTLinkId, (AA, AAA) => new { AA, AAA }).Where(M => M.AA.AP.AuthorizationId == id && M.AA.AP.IsDeleted == false && M.AA.AP.IsActive == true && M.AAA.IsDeleted == false && M.AAA.IsActive == true && M.AA.AC.IsDeleted == false && M.AA.AC.IsActive == true).Select(z => z.AAA).ToList();


            if (appointmentAuth.Count > 0)
            {
                return new JsonModel() { data = new object(), Message = StatusMessage.AlreadyExists, StatusCode = (int)HttpStatusCodes.Unauthorized };
            }
            else
            {
                Entity.Authorization authDB = _context.Authorization.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (authDB != null)
                {
                    authDB.IsDeleted = true;
                    authDB.DeletedBy = token.UserID;
                    authDB.DeletedDate = DateTime.UtcNow;
                    _context.Authorization.Update(authDB);


                    List<AuthorizationProcedures> authProcedure = _context.AuthorizationProcedures.Where(a => a.AuthorizationId == id && a.IsDeleted == false && a.IsActive == true).ToList();
                    authProcedure.ForEach(a => { a.IsDeleted = true; a.DeletedDate = DateTime.UtcNow; a.DeletedBy = token.UserID; });
                    _context.AuthorizationProcedures.UpdateRange(authProcedure.ToArray());

                    List<AuthProcedureCPT> authProcedureCPT = _context.AuthProcedureCPT.Where(a => authProcedure.Select(z => z.Id).Contains(a.AuthorizationProceduresId) && a.IsActive == true && a.IsDeleted == false).ToList();
                    authProcedureCPT.ForEach(a => { a.IsDeleted = true; a.DeletedDate = DateTime.UtcNow; a.DeletedBy = token.UserID; });
                    _context.AuthProcedureCPT.UpdateRange(authProcedureCPT.ToArray());

                    _context.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DeletedSuccessfully.Replace("[controller]", "Authorization"),
                        StatusCode = (int)HttpStatusCodes.NoContent//(Invalid credentials) };
                    };

                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }
    }
}