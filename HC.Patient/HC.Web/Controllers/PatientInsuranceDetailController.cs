//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Model.Image;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.IServices.Images;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class PatientInsuranceDetailController : CustomJsonApiController<PatientInsuranceDetails, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IImageService _imageService;
        CommonMethods commonMethods = new CommonMethods();
        #region Construtor of the class
        public PatientInsuranceDetailController(
        IJsonApiContext jsonApiContext,
        IResourceService<PatientInsuranceDetails, int> resourceService,
        ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository, IImageService imageService)
        : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                _imageService = imageService;
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
        /// this method is used for get patient insurance detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {   

            
            var patientResponse = await base.GetAsync();
            
            //image
            IQueryable<PatientInsuranceDetails> QueryObj = ((IQueryable<PatientInsuranceDetails>)((ObjectResult)patientResponse).Value);
            List<PatientInsuranceDetails> listObj = QueryObj.ToList();
            listObj.ForEach(a =>
            {
                if (!string.IsNullOrEmpty(a.InsurancePhotoPathFront)) { a.InsurancePhotoPathFront = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceFront, a.InsurancePhotoPathFront); } else { a.InsurancePhotoPathFront = ""; }
                if (!string.IsNullOrEmpty(a.InsurancePhotoPathThumbFront)) { a.InsurancePhotoPathThumbFront = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceFrontThumb, a.InsurancePhotoPathThumbFront); } else { a.InsurancePhotoPathThumbFront = ""; }
                if (!string.IsNullOrEmpty(a.InsurancePhotoPathBack)) { a.InsurancePhotoPathBack = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceBack, a.InsurancePhotoPathBack   ); } else { a.InsurancePhotoPathBack = ""; }
                if (!string.IsNullOrEmpty(a.InsurancePhotoPathThumbBack)) { a.InsurancePhotoPathThumbBack = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceBackThumb, a.InsurancePhotoPathThumbBack); } else { a.InsurancePhotoPathThumbBack = ""; }
            });

            ((ObjectResult)patientResponse).Value = listObj;
            ////////////////////

            return patientResponse;
        }


        /// <summary>
        /// this method is used for save Patient Insurance
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]PatientInsuranceDetails entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PatientInsuranceDetails alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientInsuranceDetails>().Where(l => l.InsuranceCompanyID == entity.InsuranceCompanyID && l.PatientID == entity.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                    if (alreadyExist != null)// if PatientImmunization already exist
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.ClientInsuranceAlreadyLink,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                        });
                    }
                    else
                    {   
                        _imageService.ConvertBase64ToImageForInsurance(entity);
                        return await base.PostAsync(entity);
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.UnprocessedEntity;//(Not Found)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ModelState,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = Response.StatusCode;//(Error Code)
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode//(Error Code)
                });
            }
        }

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientInsuranceDetails patientInsuranceDetail)
        {
            try
            {
                PatientInsuranceDetails alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientInsuranceDetails>().Where(l => l.InsuranceCompanyID == patientInsuranceDetail.InsuranceCompanyID && l.Id != patientInsuranceDetail.Id && l.PatientID == patientInsuranceDetail.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if PatientImmunization already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ClientInsuranceAlreadyLink,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    CommonMethods commonMethods = new CommonMethods();
                    //Convert Base64 to Image
                    patientInsuranceDetail = _imageService.ConvertBase64ToImageForInsurance(patientInsuranceDetail);
                    //For Fornt Image
                    if (!string.IsNullOrEmpty(patientInsuranceDetail.InsurancePhotoPathFront) && !string.IsNullOrEmpty(patientInsuranceDetail.InsurancePhotoPathThumbFront))
                    {
                        AttrAttribute InsurancePhotoPathFront = new AttrAttribute(AttrToUpdate.InsurancePhotoPathFront.ToString(), AttrToUpdate.InsurancePhotoPathFront.ToString());
                        AttrAttribute InsurancePhotoPathThumbFront = new AttrAttribute(AttrToUpdate.InsurancePhotoPathThumbFront.ToString(), AttrToUpdate.InsurancePhotoPathThumbFront.ToString());

                        _jsonApiContext.AttributesToUpdate.Remove(InsurancePhotoPathFront);
                        _jsonApiContext.AttributesToUpdate.Remove(InsurancePhotoPathThumbFront);

                        _jsonApiContext.AttributesToUpdate.Add(InsurancePhotoPathFront, patientInsuranceDetail.InsurancePhotoPathFront);
                        _jsonApiContext.AttributesToUpdate.Add(InsurancePhotoPathThumbFront, patientInsuranceDetail.InsurancePhotoPathThumbFront);
                    }

                    //For Back Image
                    if (!string.IsNullOrEmpty(patientInsuranceDetail.InsurancePhotoPathBack) && !string.IsNullOrEmpty(patientInsuranceDetail.InsurancePhotoPathThumbBack))
                    {
                        AttrAttribute InsurancePhotoPathBack = new AttrAttribute(AttrToUpdate.InsurancePhotoPathBack.ToString(), AttrToUpdate.InsurancePhotoPathBack.ToString());
                        AttrAttribute InsurancePhotoPathThumbBack = new AttrAttribute(AttrToUpdate.InsurancePhotoPathThumbBack.ToString(), AttrToUpdate.InsurancePhotoPathThumbBack.ToString());

                        _jsonApiContext.AttributesToUpdate.Remove(InsurancePhotoPathBack);
                        _jsonApiContext.AttributesToUpdate.Remove(InsurancePhotoPathThumbBack);

                        _jsonApiContext.AttributesToUpdate.Add(InsurancePhotoPathBack, patientInsuranceDetail.InsurancePhotoPathBack);
                        _jsonApiContext.AttributesToUpdate.Add(InsurancePhotoPathThumbBack, patientInsuranceDetail.InsurancePhotoPathThumbBack);
                    }

                    var patientInsuranceResponse = await base.PatchAsync(id, patientInsuranceDetail);

                    //image
                    if (!string.IsNullOrEmpty(((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathFront))
                        ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathFront = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceFront, ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathFront);
                    if (!string.IsNullOrEmpty(((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathThumbFront))
                        ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathThumbFront = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceFrontThumb, ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathThumbFront);

                    if (!string.IsNullOrEmpty(((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathBack))
                        ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathBack = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceBack, ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathBack);
                    if (!string.IsNullOrEmpty(((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathThumbBack))
                        ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathThumbBack = commonMethods.CreateImageUrl(HttpContext, ImagesPath.PatientInsuranceBackThumb, ((PatientInsuranceDetails)((ObjectResult)patientInsuranceResponse).Value).InsurancePhotoPathThumbBack);
                    ////////////////////

                    return patientInsuranceResponse;
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode//(Error Code)
                });
            }
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
        #endregion

        #region Helping Methods
        //        /// <summary>
        //        /// this method is used to convert base64 to Images 
        //        /// </summary>
        //        /// <param name="entity"></param>
        //        /// <param name="commonMethods"></param>
        //        private PatientInsuranceDetails ConvertBase64ToImage(PatientInsuranceDetails entity, CommonMethods commonMethods)
        //        {
        //            try
        //            {
        //                string webRootPath = "";
        //#if DEBUG
        //                webRootPath = Directory.GetCurrentDirectory();
        //#else
        //            //webRootPath = Directory.GetCurrentDirectory().Replace("HC_Patient", "HC_Photos");
        //            //webRootPath = Directory.GetCurrentDirectory().Replace("hcpatient_test", "HC_Photos");           
        //            ////  Static Root
        //            webRootPath = "C:\\inetpub\\wwwroot\\HC_Photos";
        //#endif
        //                if (!Directory.Exists(webRootPath + "\\Images\\PatientInsurancePhotos\\"))
        //                {
        //                    Directory.CreateDirectory(webRootPath + "\\Images\\PatientInsurancePhotos\\");
        //                }
        //                if (!Directory.Exists(webRootPath + "\\Images\\PatientInsurancePhotos\\thumb\\"))
        //                {
        //                    Directory.CreateDirectory(webRootPath + "\\Images\\PatientInsurancePhotos\\thumb\\");
        //                }

        //                List<ImageModel> obj = new List<ImageModel>();
        //                if (!string.IsNullOrEmpty(entity.Base64Front))
        //                {
        //                    //getting data from base64 url
        //                    var base64Datafront = Regex.Match(entity.Base64Front, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        //                    //getting extension of the image
        //                    string extensionfront = Regex.Match(entity.Base64Front, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];
        //                    extensionfront = "." + extensionfront;

        //                    string photoConcF = Guid.NewGuid().ToString();

        //                    //Front
        //                    ImageModel imgF = new ImageModel();
        //                    imgF.Base64 = base64Datafront;
        //                    imgF.ImageUrl = webRootPath + "\\Images\\PatientInsurancePhotos\\pic_" + photoConcF + extensionfront;
        //                    imgF.ThumbImageUrl = webRootPath + "\\Images\\PatientInsurancePhotos\\thumb\\pic_thumb_" + photoConcF + extensionfront;
        //                    obj.Add(imgF);

        //#if DEBUG
        //                    entity.InsurancePhotoPathFront = imgF.ImageUrl;
        //                    entity.InsurancePhotoPathThumbFront = imgF.ThumbImageUrl;
        //#else
        //                    entity.InsurancePhotoPathFront = "http://108.168.203.227/HC_Photos/Images/PatientInsurancePhotos/pic_" + photoConcF + extensionfront;
        //                    entity.InsurancePhotoPathThumbFront = "http://108.168.203.227/HC_Photos/Images/PatientInsurancePhotos/thumb/pic_thumb_"+ photoConcF + extensionfront;
        //#endif
        //                }
        //                else if (string.IsNullOrEmpty(entity.InsurancePhotoPathFront) && string.IsNullOrEmpty(entity.InsurancePhotoPathThumbFront))
        //                {
        //                    //Delete Images
        //                    entity.InsurancePhotoPathFront = string.Empty;
        //                    entity.InsurancePhotoPathThumbFront = string.Empty;
        //                }

        //                if (!string.IsNullOrEmpty(entity.Base64Back))
        //                {
        //                    //getting data from base64 url
        //                    var base64Databack = Regex.Match(entity.Base64Back, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        //                    //getting extension of the image
        //                    string extensionback = Regex.Match(entity.Base64Back, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];
        //                    extensionback = "." + extensionback;

        //                    string photoConcB = Guid.NewGuid().ToString();
        //                    ImageModel imgB = new ImageModel();
        //                    imgB.Base64 = base64Databack;
        //                    imgB.ImageUrl = webRootPath + "\\Images\\PatientInsurancePhotos\\pic_" + photoConcB + extensionback;
        //                    imgB.ThumbImageUrl = webRootPath + "\\Images\\PatientInsurancePhotos\\thumb\\pic_thumb_" + photoConcB + extensionback;
        //                    obj.Add(imgB);

        //#if DEBUG
        //                    entity.InsurancePhotoPathBack = imgB.ImageUrl;
        //                    entity.InsurancePhotoPathThumbBack = imgB.ThumbImageUrl;
        //#else

        //                    entity.InsurancePhotoPathBack = "http://108.168.203.227/HC_Photos/Images/PatientInsurancePhotos/pic_" + photoConcB + extensionback;
        //                    entity.InsurancePhotoPathThumbBack = "http://108.168.203.227/HC_Photos/Images/PatientInsurancePhotos/thumb/pic_thumb_"+ photoConcB + extensionback;
        //#endif
        //                }
        //                else if (string.IsNullOrEmpty(entity.InsurancePhotoPathBack) && string.IsNullOrEmpty(entity.InsurancePhotoPathThumbBack))
        //                {
        //                    //Delete Images
        //                    entity.InsurancePhotoPathBack = string.Empty;
        //                    entity.InsurancePhotoPathThumbBack = string.Empty;
        //                }

        //                commonMethods.SaveImageAndThumb(obj);
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //            return entity;
        //        }

        // some custom validation logic
        private bool RequestIsValid() => true;
        #endregion
    }
}