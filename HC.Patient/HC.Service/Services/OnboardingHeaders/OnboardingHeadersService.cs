using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Image;
using HC.Patient.Model.OnboardingDetails;
using HC.Patient.Model.OnboardingHeader;
using HC.Patient.Repositories.IRepositories.OnboardingDetails;
using HC.Patient.Repositories.IRepositories.OnboardingHeaders;
using HC.Patient.Service.IServices.OnboardingHeaders;
using HC.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HC.Patient.Service.Services.OnboardingHeaders
{
    public class OnboardingHeadersService:BaseService, IOnboardingHeadersService
    {
        private readonly IOnboardingHeaderRepository _onboardingHeaderRepository;
       // private readonly IRepository<User, long> _lookup_userRepository;
      //  private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly IOnboardingDetailsRepository _onboardingDetailRepository;
        private HCOrganizationContext _context;
      //  JsonModel response;

        public OnboardingHeadersService(IOnboardingHeaderRepository onboardingHeaderRepository,
           
           IOnboardingDetailsRepository onboardingDetailRepository, HCOrganizationContext context)
        //   IRepository<User, long> lookup_userRepository, ICommonLookupAppService commonLookupAppService,
        {
            _onboardingHeaderRepository = onboardingHeaderRepository;
            //_lookup_userRepository = lookup_userRepository;
            //_commonLookupAppService = commonLookupAppService;
            _onboardingDetailRepository = onboardingDetailRepository;
            this._context = context;
        }

        //public async Task<PagedResultDto<GetOnboardingHeaderForViewDto>> GetAll(GetAllOnboardingHeadersInput input)
        //{

        //    var filteredOnboardingHeaders = _onboardingHeaderRepository.GetAll()
        //                .Include(e => e.UserFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Header.Contains(input.Filter) || e.HeaderDescription.Contains(input.Filter) || e.HeaderImage.Contains(input.Filter) || e.HeaderVideo.Contains(input.Filter) || e.Category.Contains(input.Filter))
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.HeaderFilter), e => e.Header == input.HeaderFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.HeaderDescriptionFilter), e => e.HeaderDescription == input.HeaderDescriptionFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.HeaderImageFilter), e => e.HeaderImage == input.HeaderImageFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.HeaderVideoFilter), e => e.HeaderVideo == input.HeaderVideoFilter)
        //                .WhereIf(input.ActiveStatusFilter.HasValue && input.ActiveStatusFilter > -1, e => (input.ActiveStatusFilter == 1 && e.ActiveStatus) || (input.ActiveStatusFilter == 0 && !e.ActiveStatus))
        //                .WhereIf(input.MinOrganizationIdFilter != null, e => e.OrganizationId >= input.MinOrganizationIdFilter)
        //                .WhereIf(input.MaxOrganizationIdFilter != null, e => e.OrganizationId <= input.MaxOrganizationIdFilter)
        //                .WhereIf(input.MinTenantIdFilter != null, e => e.TenantId >= input.MinTenantIdFilter)
        //                .WhereIf(input.MaxTenantIdFilter != null, e => e.TenantId <= input.MaxTenantIdFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryFilter), e => e.Category == input.CategoryFilter)
        //                .WhereIf(input.MinTotalStepsFilter != null, e => e.TotalSteps >= input.MinTotalStepsFilter)
        //                .WhereIf(input.MaxTotalStepsFilter != null, e => e.TotalSteps <= input.MaxTotalStepsFilter)
        //                .WhereIf(input.MinIsImageFilter != null, e => e.IsImage >= input.MinIsImageFilter)
        //                .WhereIf(input.MaxIsImageFilter != null, e => e.IsImage <= input.MaxIsImageFilter)
        //                .WhereIf(input.MinDurationFilter != null, e => e.Duration >= input.MinDurationFilter)
        //                .WhereIf(input.MaxDurationFilter != null, e => e.Duration <= input.MaxDurationFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

        //    var pagedAndFilteredOnboardingHeaders = filteredOnboardingHeaders
        //        .OrderBy(input.Sorting ?? "id asc")
        //        .PageBy(input);

        //    var onboardingHeaders = from o in pagedAndFilteredOnboardingHeaders
        //                            join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
        //                            from s1 in j1.DefaultIfEmpty()

        //                            select new
        //                            {

        //                                o.Header,
        //                                o.HeaderDescription,
        //                                o.HeaderImage,
        //                                o.HeaderVideo,
        //                                o.ActiveStatus,
        //                                o.OrganizationId,
        //                                o.TenantId,
        //                                o.Category,
        //                                o.TotalSteps,
        //                                o.IsImage,
        //                                o.Duration,
        //                                Id = o.Id,
        //                                UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
        //                            };

        //    var totalCount = await filteredOnboardingHeaders.CountAsync();

        //    var dbList = await onboardingHeaders.ToListAsync();
        //    var results = new List<GetOnboardingHeaderForViewDto>();

        //    foreach (var o in dbList)
        //    {
        //        var res = new GetOnboardingHeaderForViewDto()
        //        {
        //            OnboardingHeader = new OnboardingHeaderDto
        //            {

        //                Header = o.Header,
        //                HeaderDescription = o.HeaderDescription,
        //                HeaderImage = o.HeaderImage,
        //                HeaderVideo = o.HeaderVideo,
        //                ActiveStatus = o.ActiveStatus,
        //                OrganizationId = o.OrganizationId,
        //                TenantId = o.TenantId,
        //                Category = o.Category,
        //                TotalSteps = o.TotalSteps,
        //                IsImage = o.IsImage,
        //                Duration = o.Duration,
        //                Id = o.Id,
        //            },
        //            UserName = o.UserName
        //        };

        //        results.Add(res);
        //    }

        //    return new PagedResultDto<GetOnboardingHeaderForViewDto>(
        //        totalCount,
        //        results
        //    );

        //}

        public async Task<OnboardingHeaderDto> GetOnboardingHeaderForView(int id, TokenModel token)
        {
           // var httpContent = _commonLookupAppService.GetHttpContext();

            var onboardingHeader =  _onboardingHeaderRepository.GetByID(id);
            OnboardingHeaderDto onboardingHeaderDto = new OnboardingHeaderDto();

            var output = new GetOnboardingHeaderForViewDto { OnboardingHeader = AutoMapper.Mapper.Map(onboardingHeader, onboardingHeaderDto) };

            //if (output.OnboardingHeader.UserId != null)
            //{
            //    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OnboardingHeader.UserId);
            //    output.UserName = _lookupUser?.Name?.ToString();
            //}
            var totalCounts = _onboardingDetailRepository.GetAll().Where(x => x.OnboardingHeaderId == output.OnboardingHeader.Id && x.IsDeleted == false);
            output.OnboardingHeader.HeaderImage = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OnboardingModule, output.OnboardingHeader.HeaderImage);
            output.OnboardingHeader.TotalSteps = totalCounts.Count();


            return output.OnboardingHeader;
        }

     //   [AbpAuthorize(AppPermissions.Pages_OnboardingHeaders_Edit)]
        public async Task<GetOnboardingHeaderForEditOutput> GetOnboardingHeaderForEdit(EntityDto input, TokenModel token)
        {
           // var httpContent = _commonLookupAppService.GetHttpContext();
            var onboardingHeader =  _onboardingHeaderRepository.GetByID(input.Id);

            //CreateOrEditOnboardingHeaderDto createOrEditOnboarding = new CreateOrEditOnboardingHeaderDto();
            var output = new GetOnboardingHeaderForEditOutput { OnboardingHeader = AutoMapper.Mapper.Map<CreateOrEditOnboardingHeaderDto>(onboardingHeader) };

            //if (output.OnboardingHeader.UserId != null)
            //{
            //    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OnboardingHeader.UserId);
            //    output.UserName = _lookupUser?.Name?.ToString();
            //}
            output.OnboardingHeader.HeaderImage = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OnboardingModule, output.OnboardingHeader.HeaderImage);

            return output;
        }

        public int CreateOrEdit(CreateOrEditOnboardingHeaderDto input, TokenModel token)
        {
            var result = new OnboardingHeader();
            if (input.Id == null)
            {
                result =  Create(input, token);
            }
            else
            {
                result =  Update(input, token);
            }
            return result.Id;


        }

     //   [AbpAuthorize(AppPermissions.Pages_OnboardingHeaders_Create)]
        protected virtual OnboardingHeader Create(CreateOrEditOnboardingHeaderDto input, TokenModel token)
        {
            //image upload
            if (!string.IsNullOrEmpty(input.HeaderImage))
            {
                string webRootPath = "";

                //get root path
                webRootPath = Directory.GetCurrentDirectory();

                //getting data from base64 url
                var base64Data = Regex.Match(input.HeaderImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                //getting extension of the image
                string extension = Regex.Match(input.HeaderImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                extension = "." + extension;

                if (!Directory.Exists(webRootPath + ImagesPath.OnboardingModule))
                {
                    Directory.CreateDirectory(webRootPath + ImagesPath.OnboardingModule);
                }


                string picName = Guid.NewGuid().ToString();

                List<ImageModel> obj = new List<ImageModel>();

                ImageModel img = new ImageModel();

                img.Base64 = base64Data;
                img.ImageUrl = webRootPath + ImagesPath.OnboardingModule + picName + extension;
                img.ThumbImageUrl = webRootPath + ImagesPath.OnboardingModule + picName + extension;
                obj.Add(img);
                CommonMethods.SaveImageAndThumb(obj);

                input.HeaderImage = picName + extension;
                // input.FilePath = img.ImageUrl;
            }
            var onboardingHeader = AutoMapper.Mapper.Map<OnboardingHeader>(input);
            onboardingHeader.OrganizationId = token.OrganizationID;
            onboardingHeader.CreatedBy = token.UserID;
            onboardingHeader.CreatedDate = DateTime.UtcNow;

            //var resultID = await _onboardingHeaderRepository.InsertAndGetIdAsync(onboardingHeader);
            //onboardingHeader.Id = resultID;
            _context.OnboardingHeader.Add(onboardingHeader);
            _context.SaveChanges();
            return onboardingHeader;

        }

      //  [AbpAuthorize(AppPermissions.Pages_OnboardingHeaders_Edit)]
        protected virtual OnboardingHeader Update(CreateOrEditOnboardingHeaderDto input, TokenModel token)
        {
            var onboardingHeader =  _onboardingHeaderRepository.GetByID((int)input.Id);
            if (!string.IsNullOrEmpty(input.HeaderImage))
            {
                int startIndex = input.HeaderImage.IndexOf("/Images/Onboarding/");

                if (startIndex > -1)
                    input.HeaderImage = input.HeaderImage.Substring(startIndex);
                //var path =input.HeaderImage.Replace("/","\\");

                string webRootPath = "";
                //get root path
                webRootPath = Directory.GetCurrentDirectory();

                string filepath = webRootPath + input.HeaderImage.Replace("/", "\\");
                if (File.Exists(filepath))
                {
                    var data =  _onboardingHeaderRepository.GetByID((int)input.Id);
                    input.HeaderImage = data.HeaderImage;
                    /*byte[] bytes = File.ReadAllBytes(filepath);
                    input.HeaderImage = "image/jpeg;base64," + Convert.ToBase64String(bytes);*/
                }

                else
                {
                    //getting data from base64 url
                    var base64Data = Regex.Match(input.HeaderImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(input.HeaderImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.OnboardingModule))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.OnboardingModule);
                    }


                    string picName = Guid.NewGuid().ToString();

                    List<ImageModel> obj = new List<ImageModel>();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.OnboardingModule + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.OnboardingModule + picName + extension;
                    obj.Add(img);
                    CommonMethods.SaveImageAndThumb(obj);

                    input.HeaderImage = picName + extension;
                    // input.FilePath = img.ImageUrl;
                }
            }
            AutoMapper.Mapper.Map(input, onboardingHeader);
            onboardingHeader.UpdatedBy = token.UserID;
            onboardingHeader.UpdatedDate = DateTime.UtcNow;
            onboardingHeader.OrganizationId = token.OrganizationID;
            _context.OnboardingHeader.Update(onboardingHeader);
            _context.SaveChanges();
            return onboardingHeader;

        }

      //  [AbpAuthorize(AppPermissions.Pages_OnboardingHeaders_Delete)]
        public async Task Delete(EntityDto input,TokenModel token)
        {
            var onboardingHeader = _onboardingHeaderRepository.GetByID((int)input.Id);
            onboardingHeader.IsDeleted = true;
            onboardingHeader.DeletedDate = DateTime.UtcNow;
            onboardingHeader.DeletedBy = token.UserID;
            // _onboardingHeaderRepository.Delete(input.Id);
            _context.OnboardingHeader.Update(onboardingHeader);
            _context.SaveChanges();
        }

      //  [AbpAuthorize(AppPermissions.Pages_OnboardingHeaders)]
        //public async Task<PagedResultDto<OnboardingHeaderUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        //{
        //    var query = _lookup_userRepository.GetAll().WhereIf(
        //           !string.IsNullOrWhiteSpace(input.Filter),
        //          e => e.Name != null && e.Name.Contains(input.Filter)
        //       );

        //    var totalCount = await query.CountAsync();

        //    var userList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<OnboardingHeaderUserLookupTableDto>();
        //    foreach (var user in userList)
        //    {
        //        lookupTableDtoList.Add(new OnboardingHeaderUserLookupTableDto
        //        {
        //            Id = user.Id,
        //            DisplayName = user.Name?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<OnboardingHeaderUserLookupTableDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}

        [AllowAnonymous]
        public async Task<List<OnboardingHeaderDto>> GetAllWithoutPagination(TokenModel token)
        {
          //  var httpContent = _commonLookupAppService.GetHttpContext();

            var filteredOnboardingHeaders = _onboardingHeaderRepository.GetAll().Where(x => x.IsDeleted == false).ToList();

            // var pagedAndFilteredOnboardingHeaders = filteredOnboardingHeaders
            //   .OrderBy(input.Sorting ?? "id asc")
            //   .PageBy(input);

            var onboardingHeaders = (from o in filteredOnboardingHeaders

                                    select new
                                    {

                                        o.Header,
                                        o.HeaderDescription,
                                        o.HeaderImage,
                                        o.HeaderVideo,
                                        o.ActiveStatus,
                                        o.OrganizationId,
                                        o.Category,
                                        o.TotalSteps,
                                        o.IsImage,
                                        o.Duration,
                                        Id = o.Id
                                    }).ToList();

            //  var totalCount = await filteredOnboardingHeaders.CountAsync();

            var dbList =  onboardingHeaders.ToList();
            var results = new List<OnboardingHeaderDto>();
            foreach (var o in dbList)
            {
                var totalCounts = _onboardingDetailRepository.GetAll().Where(x => x.OnboardingHeaderId == o.Id);

                var res = new GetOnboardingHeaderForViewDto()
                {
                    OnboardingHeader = new OnboardingHeaderDto
                    {

                        Header = o.Header,
                        HeaderDescription = o.HeaderDescription,
                        HeaderImage = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OnboardingModule, o.HeaderImage),
                        HeaderVideo = o.HeaderVideo,
                        ActiveStatus = o.ActiveStatus,
                        OrganizationId = o.OrganizationId,
                        Category = o.Category,
                        TotalSteps = totalCounts.Count(),
                        IsImage = o.IsImage,
                        Duration = o.Duration,
                        Id = o.Id,
                    },
                };

                results.Add(res.OnboardingHeader);
            }

            //return new PagedResultDto<GetOnboardingHeaderForViewDto>(
            //    totalCount,
            //    results
            //);
            return results;

        }

        [AllowAnonymous]
        public async Task<List<OnboardingHeaderDto>> GetAllByCategory(string category, TokenModel token)
        {
            //var httpContent = _commonLookupAppService.GetHttpContext();

            var filteredOnboardingHeaders = _onboardingHeaderRepository.GetAll()
                .Where(e => category =="" || category == null || e.Category.Contains(category));

            // var pagedAndFilteredOnboardingHeaders = filteredOnboardingHeaders
            //   .OrderBy(input.Sorting ?? "id asc")
            //   .PageBy(input);

            var onboardingHeaders = from o in filteredOnboardingHeaders

                                    select new
                                    {

                                        o.Header,
                                        o.HeaderDescription,
                                        o.HeaderImage,
                                        o.HeaderVideo,
                                        o.ActiveStatus,
                                        o.OrganizationId,
                                        o.Category,
                                        o.TotalSteps,
                                        o.IsImage,
                                        o.Duration,
                                        Id = o.Id
                                    };

            //  var totalCount = await filteredOnboardingHeaders.CountAsync();

            var dbList = await onboardingHeaders.ToListAsync();
            var results = new List<OnboardingHeaderDto>();

            foreach (var o in dbList)
            {
                var totalCounts = _onboardingDetailRepository.GetAll().Where(x => x.OnboardingHeaderId == o.Id);
                var res = new GetOnboardingHeaderForViewDto()
                {
                    OnboardingHeader = new OnboardingHeaderDto
                    {

                        Header = o.Header,
                        HeaderDescription = o.HeaderDescription,
                        HeaderImage = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OnboardingModule, o.HeaderImage),
                        HeaderVideo = o.HeaderVideo,
                        ActiveStatus = o.ActiveStatus,
                        OrganizationId = o.OrganizationId,
                        Category = o.Category,
                        TotalSteps = totalCounts.Count(),
                        IsImage = o.IsImage,
                        Duration = o.Duration,
                        Id = o.Id,
                    }
                };

                results.Add(res.OnboardingHeader);
            }

            //return new PagedResultDto<GetOnboardingHeaderForViewDto>(
            //    totalCount,
            //    results
            //);
            return results;

        }
    }
}
