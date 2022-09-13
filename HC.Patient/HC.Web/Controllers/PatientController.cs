//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.PatientCommon.Interfaces;
using HC.Patient.Web.Filters;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    //[Authorize(Roles = "Admin")]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class PatientController : CustomJsonApiController<Patients, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IPatientCommonService _patientCommonService;
        private readonly IPatientService _patientService;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        public JsonModel response;
        private static HttpClient Client { get; } = new HttpClient();
        CommonMethods commonMethods = new CommonMethods();

        public long streamLength = 0;


        #region Construtor of the class
        public PatientController(
        IJsonApiContext jsonApiContext,
        IResourceService<Patients, int> resourceService,
        ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IPatientService patientService, IUserRepository userRepository, IUserCommonRepository userCommonRepository, IImageService imageService)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _imageService = imageService;
                _userRepository = userRepository;
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                _patientCommonService = patientCommonService;
                _patientService = patientService;

                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

                }
            }
            catch
            {

            }
        }

        #endregion

        #region Class Methods



        /// <summary>
        /// this method is used for get request for patient
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public override async Task<IActionResult> GetAsync()
        {
            return await CustomFilters();
        }

        [HttpGet("GetAsync")]
        public List<PatientModel> GetAsyncFiltered()
        {
            TokenModel tokenModel = commonMethods.GetTokenDataModel(HttpContext);
            string searchKey = null;
            string startWith = null;
            string operation = null;
            string tags = "";
            string locationIDs = "";
            string isActive = null;
            _jsonApiContext.QuerySet.Filters.ForEach(p =>
            {
                if (p.Key.ToUpper() == PatientSearch.SEARCHKEY.ToString())
                {
                    searchKey = p.Value.ToString().Trim();
                }
                if (p.Key.ToUpper() == PatientSearch.STARTWITH.ToString())
                {
                    startWith = p.Value.ToString().Trim();
                }
                if (p.Key.ToUpper() == PatientSearch.OPERATION.ToString())
                {
                    operation = p.Value.ToString().Trim();
                }
                if (p.Key.ToUpper() == PatientSearch.TAGS.ToString())
                {
                    tags = tags + "," + p.Value.ToString();
                }
                if (p.Key.ToUpper() == PatientSearch.LOCATIONID.ToString())
                {
                    locationIDs = locationIDs + "," + p.Value.ToString();
                }
                if (p.Key.ToUpper() == PatientSearch.ISACTIVE.ToString())
                {
                    isActive = p.Value.ToString();
                }
            });
            int pageSize = _jsonApiContext.QuerySet.PageQuery.PageSize;
            List<PatientModel> model = _patientCommonService.GetFilteredPatients(searchKey, operation, startWith, tags.TrimStart(',').TrimEnd(','), locationIDs.TrimStart(',').TrimEnd(','), isActive, pageSize, tokenModel);
            model.ForEach(a =>
            {
                if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                    a.PhotoThumbnailPath = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientThumbPhotos, a.PhotoThumbnailPath);
                else
                    a.PhotoThumbnailPath = "";
            });
            return model;
        }

        [HttpGet("{type}/{id}/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipAsync(string type, int id, string relationshipName)
        {
            return await base.GetRelationshipAsync(id, relationshipName);
        }


        [HttpGet("{type}/{id}/relationships/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipsAsync(string type, int id, string relationshipName)
        {
            return await base.GetRelationshipsAsync(id, relationshipName);
        }

        /// <summary>
        /// this method is used for save patient
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]Patients entity)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            var Password = "password1234"; //change it to dynamic for patients
            try
            {
                Patients newPatient = new Patients();
                if (ModelState.IsValid)
                {
                    newPatient = _dbContextResolver.GetDbSet<Patients>().Where(m => m.Email == entity.Email).FirstOrDefault();

                    if (!string.IsNullOrEmpty(Password)) { Password = commonMethods.Encrypt(Password); }
                    if (newPatient != null)
                    {
                        Response.StatusCode = 422;//(Unprocessable Entity)
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.PatientAlreadyExist,
                            StatusCode = 422
                        });
                    }
                    _imageService.ConvertBase64ToImage(entity);
                }
                else
                {
                    Response.StatusCode = 422;//(Unprocessable Entity)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidData,
                        StatusCode = 422
                    });
                }
            }
            catch (Exception)
            {
            }
            //TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            entity.OrganizationID = token.OrganizationID;
            entity.LocationID = token.LocationID;
            ///////Save User
            User requestUser = SaveUser(entity, token, Password);
            ///////////Save Patient/////////////
            entity.UserID = requestUser.Id;
            var patient = await base.PostAsync(entity);
            Patients savedPatient = (Patients)((ObjectResult)patient).Value;
            ////////saved patient location///////////////
            return patient;

        }

        private User SaveUser(Patients entity, TokenModel token, string password)
        {
            // patient Role by organization base
            int patientRoleID = _dbContextResolver.GetDbSet<UserRoles>().Where(m => m.RoleName.ToLower() == "client" && m.OrganizationID == token.OrganizationID && m.IsActive == true && m.IsDeleted == false).FirstOrDefault().Id;

            User requestUser = new User();
            requestUser.UserName = entity.Email;
            requestUser.Password = password; //change it to dynamic later on and send email to patient
            requestUser.RoleID = patientRoleID;//4; 
            requestUser.CreatedBy = token.UserID;
            requestUser.CreatedDate = DateTime.UtcNow;
            requestUser.IsActive = true;
            requestUser.IsDeleted = false;
            requestUser.OrganizationID = entity.OrganizationID;
            _userRepository.Create(requestUser);
            _userRepository.SaveChanges();
            return requestUser;
        }

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]Patients patientInfo)
        {

            CommonMethods commonMethods = new CommonMethods();
            AttrAttribute photoPath = new AttrAttribute(AttrToUpdate.PhotoPath.ToString(), AttrToUpdate.PhotoPath.ToString());
            AttrAttribute photoThumbnailPath = new AttrAttribute(AttrToUpdate.PhotoThumbnailPath.ToString(), AttrToUpdate.PhotoThumbnailPath.ToString());

            if (!string.IsNullOrEmpty(patientInfo.PhotoBase64))
            {
                _jsonApiContext.AttributesToUpdate.Remove(photoPath);
                _jsonApiContext.AttributesToUpdate.Remove(photoThumbnailPath);

                patientInfo = _imageService.ConvertBase64ToImage(patientInfo);

                if (!string.IsNullOrEmpty(patientInfo.PhotoPath) && !string.IsNullOrEmpty(patientInfo.PhotoThumbnailPath))
                {
                    _jsonApiContext.AttributesToUpdate.Add(photoPath, patientInfo.PhotoPath);
                    _jsonApiContext.AttributesToUpdate.Add(photoThumbnailPath, patientInfo.PhotoThumbnailPath);
                }
            }
            return await base.PatchAsync(id, patientInfo);
        }

        [HttpPost("GetPatientByTag")]
        public List<HC.Patient.Model.Patient.PatientModel> GetPatientByTag([FromBody]JObject tag)
        {
            string tags = Convert.ToString(tag["tag"]);
            string startWith = Convert.ToString(tag["startwith"]);
            bool? isActive = !string.IsNullOrEmpty(Convert.ToString(tag["isactive"])) ? (bool?)tag["isactive"] : null;
            int? locationID = !string.IsNullOrEmpty(Convert.ToString(tag["locationid"])) ? (int?)tag["locationid"] : null;

            List<PatientModel> model = _patientService.GetPatientsByTags(tags, startWith, locationID, isActive);

            model.ForEach(a =>
            {
                if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                    a.PhotoThumbnailPath = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientThumbPhotos, a.PhotoThumbnailPath);
                else
                    a.PhotoThumbnailPath = "";
            });

            return model;
        }
        #endregion

        //[HttpGet("GetPatientsDetails/{id}")]
        //public PatientInfoDetails GetPatientsDetails(int id)
        //{
        //    return _patientService.GetPatientsDetails(id);

        //}


        [HttpGet]
        [Route("GetPatientsDetails/{id}")]
        public JsonResult GetPatientsDetails(int id)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            response = _patientService.GetPatientsDetails(id, token);
            return Json(response);
        }


        /// <summary>
        /// Get Patient Guarantor detail by PatientId
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientGuarantor/{patientId}")]
        public JsonResult GetPatientGuarantor(int patientId)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            response = _patientService.GetPatientGuarantor(patientId, token);
            return Json(response);
        }

        [HttpGet]
        [Route("GetPatientCCDA/{id}")]
        public IActionResult GetPatientCCDA(int id)
        {
            MemoryStream tempStream = null;
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext); //{ UserID = 1, OrganizationID = 1 }; //
            tempStream = _patientService.GetPatientCCDA(id, token);
            if (ReferenceEquals(tempStream, null))
                tempStream = new MemoryStream();

            string fileName = "CDA.zip";

            var fileStreams = new Dictionary<string, string>
            {
                { "CDA.xml", "http://" + Request.Host.Value + "/CDA/CDA.xml" },
                { "CDA.xsl",  "http://"+ Request.Host.Value + "/CDA/CDA.xsl" },
            };
            return new FileCallbackResult(new MediaTypeHeaderValue("application/octet-stream"), async (outputStream, _) =>
            {
                using (var zipArchive = new ZipArchive(new WriteOnlyStreamWrapper(outputStream), ZipArchiveMode.Create))
                {
                    foreach (var kvp in fileStreams)
                    {
                        var zipEntry = zipArchive.CreateEntry(kvp.Key);
                        using (var zipStream = zipEntry.Open())
                        using (var stream = await Client.GetStreamAsync(kvp.Value))
                            await stream.CopyToAsync(zipStream);
                    }
                }

                string path = Directory.GetCurrentDirectory() + "\\wwwroot\\CDA\\CDA.xml";
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
            })
            {
                FileDownloadName = fileName
            };

        }

        [HttpPost]
        [Route("ImportPatientCCDA")]
        public JsonResult ImportPatientCCDA([FromBody]JObject file)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            return Json(_patientService.ImportPatientCCDA(file, token));
        }

        #region Helping Methods
        /// <summary>
        /// this method is used for get the custom filter
        /// </summary>
        /// <returns></returns>
        private async Task<IActionResult> CustomFilters()
        {
            bool IsSearchKey = false;
            bool IsStartWith = false;
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            FilterQuery filterQuery = new FilterQuery("", "", "");
            var locationFilter = _jsonApiContext.QuerySet.Filters.Where(p => p.Key.ToUpper() == PatientSearch.LOCATIONID.ToString()).ToList();
            List<int> locationIds = new List<int>();
            if (locationFilter.Count > 0) { locationFilter.ForEach(k => { locationIds.Add(Convert.ToInt32(k.Value)); }); }
            if (_jsonApiContext.QuerySet != null && _jsonApiContext.QuerySet.Filters != null)
            {
                _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.SEARCHKEY.ToString()) { IsSearchKey = true; filterQuery = p; } });
                _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.STARTWITH.ToString()) { IsStartWith = true; filterQuery = p; } });
                if (IsSearchKey)
                {

                    List<Patients> patients = new List<Patients>();
                    CommonMethods commonMethods = new CommonMethods();
                    var type = commonMethods.ParseString(filterQuery.Value);
                    if (type == DataType.System_String || type == DataType.System_Int32 || type == DataType.System_Int64)
                    {
                        patients = _jsonApiContext.GetDbContextResolver().GetDbSet<Patients>().Where(l => (l.FirstName.ToUpper().Contains(filterQuery.Value.ToUpper()) ||
                        l.LastName.ToUpper().Contains(filterQuery.Value.ToUpper()) || l.MiddleName.ToUpper().Contains(filterQuery.Value.ToUpper()) ||
                        l.SSN.ToUpper().Contains(filterQuery.Value.ToUpper()) || l.MRN.ToUpper().Contains(filterQuery.Value.ToUpper())) && l.IsDeleted == false && l.IsActive == true && (locationIds.Count == 0 || locationIds.Contains(l.LocationID))).ToList();
                        _jsonApiContext.QuerySet.Filters.Remove(filterQuery);
                    }
                    else if (type == DataType.System_DateTime)
                    {
                        patients = DateFilter(filterQuery, patients);

                    }
                    patients = CustomSorting(patients);
                    if (_jsonApiContext.QuerySet.PageQuery.PageSize != 0)
                    {
                        patients = patients.Take(_jsonApiContext.QuerySet.PageQuery.PageSize).ToList();
                    }
                    var asyncPatients = await base.GetAsync();

                    //patients.ForEach(p => p.GenderValue = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterGender>().Where(o => o.Id == p.Gender).FirstOrDefault().Gender);
                    patients.ForEach(p => p.PatientAddress = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientAddress>().Where(o => o.PatientID == p.Id).ToList());
                    ((ObjectResult)asyncPatients).Value = patients;
                    _jsonApiContext.PageManager.TotalRecords = patients.Count();

                    return asyncPatients;
                }
                if (IsStartWith)
                {

                    List<Patients> patients = new List<Patients>();
                    CommonMethods commonMethods = new CommonMethods();
                    var type = commonMethods.ParseString(filterQuery.Value);
                    if (type == DataType.System_String || type == DataType.System_Int32 || type == DataType.System_Int64)
                    {
                        patients = _jsonApiContext.GetDbContextResolver().GetDbSet<Patients>().Where(l => l.FirstName.ToUpper().StartsWith(filterQuery.Value.ToUpper()) && l.IsDeleted == false && l.IsActive == true && (locationIds.Count == 0 || locationIds.Contains(l.LocationID))).ToList();
                        _jsonApiContext.QuerySet.Filters.Remove(filterQuery);
                    }
                    else if (type == DataType.System_DateTime)
                    {
                        patients = DateFilter(filterQuery, patients);

                    }
                    patients = CustomSortingForStartWith(patients);
                    if (_jsonApiContext.QuerySet.PageQuery.PageSize != 0)
                    {
                        patients = patients.Take(_jsonApiContext.QuerySet.PageQuery.PageSize).OrderByDescending(a => a.CreatedDate).ToList();
                    }
                    var asyncPatients = await base.GetAsync();

                    //patients.ForEach(p => p.GenderValue = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterGender>().Where(o => o.Id == p.Gender).FirstOrDefault().Gender);
                    ((ObjectResult)asyncPatients).Value = patients;
                    _jsonApiContext.PageManager.TotalRecords = patients.Count();

                    return asyncPatients;
                }
                else
                {

                    locationFilter.ForEach(k => { _jsonApiContext.QuerySet.Filters.Remove(k); });
                    if (locationFilter.Count > 0)
                    {
                        var asyncPatients = await base.GetAsync();
                        var patients = (IQueryable<Patients>)(((ObjectResult)asyncPatients).Value);

                        List<Patients> patientsResult = new List<Patients>();
                        locationFilter.ForEach(j =>
                        {
                            patientsResult.AddRange(patients.Where(l => l.LocationID == Int32.Parse(j.Value)).AsQueryable());
                        });
                        ((ObjectResult)asyncPatients).Value = patientsResult;
                        _jsonApiContext.PageManager.TotalRecords = patientsResult.Count();
                        return asyncPatients;
                    }
                    else
                    {
                        return await base.GetAsync();
                    }
                }

            }
            else
            {
                locationFilter.ForEach(k => { _jsonApiContext.QuerySet.Filters.Remove(k); });
                if (locationFilter.Count > 0)
                {
                    var asyncPatients = await base.GetAsync();
                    var patients = (IQueryable<Patients>)(((ObjectResult)asyncPatients).Value);

                    List<Patients> patientsResult = new List<Patients>();
                    locationFilter.ForEach(j =>
                    {
                        patientsResult.AddRange(patients.Where(l => l.LocationID == Int32.Parse(j.Value)).AsQueryable());
                    });
                    ((ObjectResult)asyncPatients).Value = patientsResult;
                    _jsonApiContext.PageManager.TotalRecords = patientsResult.Count();
                    return asyncPatients;
                }
                else
                {
                    return await base.GetAsync();
                }
            }
        }
        private List<Patients> CustomSortingForStartWith(List<Patients> patients)
        {
            if (patients.Count > 0)
            {
                patients = patients.OrderByDescending(a => a.CreatedDate).ToList();
            }
            return patients;
        }
        private List<Patients> CustomSorting(List<Patients> patients)
        {
            if (_jsonApiContext.QuerySet.SortParameters != null)
            {
                _jsonApiContext.QuerySet.SortParameters.ForEach(i =>
                {
                    if (i.Direction == SortDirection.Ascending)
                        switch ((PatientSearch)Enum.Parse(typeof(PatientSearch), i.SortedAttribute.InternalAttributeName.ToString().ToUpper()))
                        {
                            case PatientSearch.ID:
                                patients = patients.OrderBy(m => m.Id).ToList();
                                break;
                            case PatientSearch.STAFFNAME:
                                patients = patients.OrderBy(m => m.StaffName).ToList();
                                break;
                            case PatientSearch.FIRSTNAME:
                                patients = patients.OrderBy(m => m.FirstName).ToList();
                                break;
                            case PatientSearch.LASTNAME:
                                patients = patients.OrderBy(m => m.LastName).ToList();
                                break;
                            case PatientSearch.GENDER:
                                patients = patients.OrderBy(m => m.Gender).ToList();
                                break;
                            case PatientSearch.SSN:
                                patients = patients.OrderBy(m => m.SSN).ToList();
                                break;
                            case PatientSearch.DOB:
                                patients = patients.OrderBy(m => m.DOB).ToList();
                                break;
                            case PatientSearch.PATIENTINSURANCE:
                                patients = patients.OrderBy(m => m.PatientInsurance).ToList();
                                break;


                        }
                    else if (i.Direction == SortDirection.Descending)
                        switch ((PatientSearch)Enum.Parse(typeof(PatientSearch), i.SortedAttribute.InternalAttributeName.ToString().ToUpper()))
                        {
                            case PatientSearch.ID:
                                patients = patients.OrderByDescending(m => m.Id).ToList();
                                break;
                            case PatientSearch.STAFFNAME:
                                patients = patients.OrderByDescending(m => m.StaffName).ToList();
                                break;
                            case PatientSearch.FIRSTNAME:
                                patients = patients.OrderByDescending(m => m.FirstName).ToList();
                                break;
                            case PatientSearch.LASTNAME:
                                patients = patients.OrderByDescending(m => m.LastName).ToList();
                                break;
                            case PatientSearch.GENDER:
                                patients = patients.OrderByDescending(m => m.Gender).ToList();
                                break;
                            case PatientSearch.SSN:
                                patients = patients.OrderByDescending(m => m.SSN).ToList();
                                break;
                            case PatientSearch.DOB:
                                patients = patients.OrderByDescending(m => m.DOB).ToList();
                                break;
                            case PatientSearch.PATIENTINSURANCE:
                                patients = patients.OrderByDescending(m => m.PatientInsurance).ToList();
                                break;
                        }
                });
            }

            return patients;
        }

        private List<Patients> DateFilter(FilterQuery filterQuery, List<Patients> patients)
        {
            DateTime searchDate = new DateTime();
            DateTime.TryParse(filterQuery.Value, out searchDate);

            switch ((PatientSearch)Enum.Parse(typeof(PatientSearch), filterQuery.Operation.ToUpper()))
            {
                case PatientSearch.FROMDATE:
                    patients = _jsonApiContext.GetDbContextResolver().GetDbSet<Patients>().Where(o => o.CreatedDate >= searchDate && o.IsDeleted == false && o.IsActive == true).ToList();
                    _jsonApiContext.QuerySet.Filters.Remove(filterQuery);
                    break;
                case PatientSearch.TODATE:
                    patients = _jsonApiContext.GetDbContextResolver().GetDbSet<Patients>().Where(o => o.CreatedDate <= searchDate && o.IsDeleted == false && o.IsActive == true).ToList();
                    _jsonApiContext.QuerySet.Filters.Remove(filterQuery);
                    break;
                case PatientSearch.FROMDOB:
                    patients = _jsonApiContext.GetDbContextResolver().GetDbSet<Patients>().Where(o => o.DOB >= searchDate && o.IsDeleted == false && o.IsActive == true).ToList();
                    _jsonApiContext.QuerySet.Filters.Remove(filterQuery);
                    break;
                case PatientSearch.TODOB:
                    patients = _jsonApiContext.GetDbContextResolver().GetDbSet<Patients>().Where(o => o.DOB <= searchDate && o.IsDeleted == false && o.IsActive == true).ToList();
                    _jsonApiContext.QuerySet.Filters.Remove(filterQuery);
                    break;
            }

            return patients;
        }

        // some custom validation logic


        //        private Patients ConvertBase64ToImage(Patients entity, CommonMethods commonMethods)
        //        {
        //            try
        //            {
        //                if (!string.IsNullOrEmpty(entity.PhotoBase64))
        //                {
        //                    string webRootPath = "";
        //                    //todo: Need to fix static Directory path "webRootPath" in release mode
        //#if DEBUG
        //                    webRootPath = Directory.GetCurrentDirectory();
        //#else
        //            //webRootPath = Directory.GetCurrentDirectory().Replace("HC_Patient", "HC_Photos");
        //            //webRootPath = Directory.GetCurrentDirectory().Replace("hcpatient_test", "HC_Photos");           
        //            ////  Static Root
        //            webRootPath = "C:\\inetpub\\wwwroot\\HC_Photos";
        //#endif
        //                    //webRootPath = Directory.GetCurrentDirectory().Replace("HC_Patient", "HC_Photos");
        //                    //webRootPath = Directory.GetCurrentDirectory().Replace("hcpatient_test", "HC_Photos");

        //                    //getting data from base64 url
        //                    var base64Data = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        //                    //getting extension of the image
        //                    string extension = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

        //                    extension = "." + extension;
        //                    if (!Directory.Exists(webRootPath + "\\Images\\PatientPhotos\\"))
        //                    {
        //                        Directory.CreateDirectory(webRootPath + "\\Images\\PatientPhotos\\");
        //                    }
        //                    if (!Directory.Exists(webRootPath + "\\Images\\PatientPhotos\\thumb\\"))
        //                    {
        //                        Directory.CreateDirectory(webRootPath + "\\Images\\PatientPhotos\\thumb\\");
        //                    }
        //                    string photoConc = Guid.NewGuid().ToString();

        //                    List<ImageModel> obj = new List<ImageModel>();

        //                    ImageModel img = new ImageModel();

        //                    img.Base64 = base64Data;
        //                    img.ImageUrl = webRootPath + "\\Images\\PatientPhotos\\Patientphoto_" + photoConc + extension;
        //                    img.ThumbImageUrl = webRootPath + "\\Images\\PatientPhotos\\thumb\\pic_thumb_" + photoConc + extension;
        //                    obj.Add(img);

        //                    commonMethods.SaveImageAndThumb(obj);
        //#if DEBUG
        //                    entity.PhotoPath = webRootPath + "\\Images\\PatientPhotos\\Patientphoto_" + photoConc + extension;
        //                    entity.PhotoThumbnailPath = webRootPath + "\\Images\\PatientPhotos\\thumb\\pic_thumb_" + photoConc + extension;
        //#else
        //                    entity.PhotoPath = "http://108.168.203.227/HC_Photos/Images/PatientPhotos/Patientphoto_" + photoConc + extension;
        //                    entity.PhotoThumbnailPath = "http://108.168.203.227/HC_Photos/Images/PatientPhotos/thumb/pic_thumb_" + photoConc + extension;
        //#endif              
        //                }
        //                else if (string.IsNullOrEmpty(entity.PhotoPath) && string.IsNullOrEmpty(entity.PhotoThumbnailPath))
        //                {
        //                    entity.PhotoPath = string.Empty;
        //                    entity.PhotoThumbnailPath = string.Empty;
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                throw;
        //            }

        //            return entity;

        //        }

        [HttpPatch]
        [Route("UpdatePatientPortalVisibility/{patientID}/{isPortalActive}")]
        public JsonResult UpdatePatientPortalVisibility(int patientID, int userID, bool isPortalActive)
        {
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                if (!ReferenceEquals(token, null))
                {
                    return Json(_patientService.UpdatePatientPortalVisibility(patientID, userID, isPortalActive, token));
                }
                else
                    response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidToken,
                        StatusCode = (int)HttpStatusCodes.Unauthorized
                    };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                });
            }
        }


        [HttpPatch]
        [Route("UpdatePatientActiveStatus/{patientID}/{isActive}")]
        public JsonResult UpdatePatientActiveStatus(int patientID, bool isActive)
        {
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                if (!ReferenceEquals(token, null))
                {
                    return Json(_patientService.UpdatePatientActiveStatus(patientID, isActive, token));
                }
                else
                    response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidToken,
                        StatusCode = (int)HttpStatusCodes.Unauthorized
                    };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                });
            }
        }

        /// <summary>
        /// Created By - Sunny Bhardwaj
        /// Created Date - 22nd Jan 2018
        /// Description - This action will get all the service codes assigned to a patient payer with their authorization if they are attcahed to any authorization
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="payerPreference"></param>
        /// <returns></returns>
        [HttpGet("GetPatientPayerServiceCodes/{patientId}/{payerPreference}/{date}")]
        public JsonResult GetPatientPayerServiceCodes(int patientId, string payerPreference, Nullable<DateTime> date)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            return Json(_patientService.GetPatientPayerServiceCodes(patientId, payerPreference, Convert.ToDateTime((date == null ? DateTime.UtcNow : date)), token));
        }


        /// <summary>
        /// Created By - Sunny Bhardwaj
        /// Created Date - 25nd Jan 2018
        /// Description - Get All authorizations for patients
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllAuthorizationsForPatient/{patientId}/{pageNumber}/{pageSize}/{authType}")]
        public JsonResult GetAllAuthorizationsForPatient(int patientId, int pageNumber, int pageSize, string authType)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            return Json(_patientService.GetAllAuthorizationsForPatient(patientId, pageNumber, pageSize, authType, token));
        }




        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

        #endregion

    }
}