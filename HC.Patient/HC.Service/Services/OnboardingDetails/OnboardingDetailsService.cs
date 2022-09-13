using HC.Patient.Entity;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Repositories.IRepositories.OnboardingDetails;
using System.Threading.Tasks;
using HC.Patient.Model.OnboardingDetails;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using HC.Patient.Repositories.IRepositories.OnboardingHeaders;
using HC.Patient.Service.IServices.OnboardingDetails;
using HC.Model;

namespace HC.Patient.Service.Services
{
    public class OnboardingDetailsService : BaseService,IOnboardingDetailsService
    {
        private readonly IOnboardingDetailsRepository _onboardingDetailRepository;
        private readonly IOnboardingHeaderRepository _lookup_onboardingHeaderRepository;

        public OnboardingDetailsService(IOnboardingDetailsRepository onboardingDetailRepository
            ,IOnboardingHeaderRepository lookup_onboardingHeaderRepository)
        {
            _onboardingDetailRepository = onboardingDetailRepository;
            _lookup_onboardingHeaderRepository = lookup_onboardingHeaderRepository;

        }
       
        //public async Task<PagedResultDto<GetOnboardingDetailForViewDto>> GetAll(GetAllOnboardingDetailsInput input)
        //{

        //    var filteredOnboardingDetails = _onboardingDetailRepository.GetAll()
        //                .Include(e => e.OnboardingHeaderFk)
        //                .Where( e => false || e.Title.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.Description.Contains(input.Filter))
        //                .Where( e => e.Title == input.TitleFilter)
        //                .Where( e => e.ShortDescription == input.ShortDescriptionFilter)
        //                .Where( e => e.Description == input.DescriptionFilter)
        //                .Where(e => e.OrganizationId >= input.MinOrganizationIdFilter)
        //                .Where( e => e.OrganizationId <= input.MaxOrganizationIdFilter)
        //                .Where( e => e.Order >= input.MinOrderFilter)
        //                .Where(e => e.Order <= input.MaxOrderFilter)
        //                .Where(e => e.OnboardingHeaderFk != null && e.OnboardingHeaderFk.Header == input.OnboardingHeaderHeaderFilter);

        //    var pagedAndFilteredOnboardingDetails = filteredOnboardingDetails
        //        .OrderBy(input.Sorting ?? "id asc").Page(input.page)

        //        //.PageBy(input);

        //    var onboardingDetails = from o in pagedAndFilteredOnboardingDetails
        //                            join o1 in _lookup_onboardingHeaderRepository.GetAll() on o.OnboardingHeaderId equals o1.Id into j1
        //                            from s1 in j1.DefaultIfEmpty()

        //                            select new
        //                            {

        //                                o.Title,
        //                                o.ShortDescription,
        //                                o.Description,
        //                                o.OrganizationId,
        //                                o.TenantId,
        //                                o.Order,
        //                                Id = o.Id,
        //                                OnboardingHeaderHeader = s1 == null || s1.Header == null ? "" : s1.Header.ToString()
        //                            };

        //    var totalCount = await filteredOnboardingDetails.CountAsync();

        //    var dbList = await onboardingDetails.ToListAsync();
        //    var results = new List<GetOnboardingDetailForViewDto>();

        //    foreach (var o in dbList)
        //    {
        //        var res = new GetOnboardingDetailForViewDto()
        //        {
        //            OnboardingDetail = new OnboardingDetailDto
        //            {

        //                Title = o.Title,
        //                ShortDescription = o.ShortDescription,
        //                Description = o.Description,
        //                OrganizationId = o.OrganizationId,
        //                TenantId = o.TenantId,
        //                Order = o.Order,
        //                Id = o.Id,
        //            },
        //            OnboardingHeaderHeader = o.OnboardingHeaderHeader
        //        };

        //        results.Add(res);
        //    }

        //    return new PagedResultDto<GetOnboardingDetailForViewDto>(
        //        totalCount,
        //        results
        //    );

        //}

        public async Task<GetOnboardingDetailForViewDto> GetOnboardingDetailForView(int id)
        {
            var onboardingDetail =  _onboardingDetailRepository.GetByID(id);

            OnboardingDetailDto onboardingDetailDto = new OnboardingDetailDto();

            var output = new GetOnboardingDetailForViewDto { OnboardingDetail = AutoMapper.Mapper.Map(onboardingDetail, onboardingDetailDto) };

            if (output.OnboardingDetail.OnboardingHeaderId != null)
            {
                var _lookupOnboardingHeader =  _lookup_onboardingHeaderRepository.GetByID((int)output.OnboardingDetail.OnboardingHeaderId);
                output.OnboardingHeaderHeader = _lookupOnboardingHeader?.Header?.ToString();
            }

            return output;
        }

        public async Task<GetOnboardingDetailForEditOutput> GetOnboardingDetailForEdit(EntityDto input)
        {
            var onboardingDetail =  _onboardingDetailRepository.GetByID(input.Id);
            CreateOrEditOnboardingDetailDto createOrEditOnboardingDetailDto = new CreateOrEditOnboardingDetailDto();

            var output = new GetOnboardingDetailForEditOutput { OnboardingDetail = AutoMapper.Mapper.Map(onboardingDetail, createOrEditOnboardingDetailDto) };

            if (output.OnboardingDetail.OnboardingHeaderId != null)
            {
                var _lookupOnboardingHeader =  _lookup_onboardingHeaderRepository.GetByID((int)output.OnboardingDetail.OnboardingHeaderId);
                output.OnboardingHeaderHeader = _lookupOnboardingHeader?.Header?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOnboardingDetailDto input, TokenModel token)
        {
            if (input.Id == null)
            {
                 Create(input,token);
            }
            else
            {
                await Update(input, token);
            }
        }

     //   [AbpAuthorize(AppPermissions.Pages_OnboardingDetails_Create)]
        protected void Create(CreateOrEditOnboardingDetailDto input, TokenModel token)
        {
            var count = _onboardingDetailRepository.GetAll().Where(x => x.OnboardingHeaderId == input.OnboardingHeaderId && x.IsDeleted == false);
            input.Order = count.Count();
            OnboardingDetail onboardingDetail = new OnboardingDetail();
            var onboardingDetails = AutoMapper.Mapper.Map(input, onboardingDetail);
            onboardingDetails.OrganizationId = token.OrganizationID;
            onboardingDetails.CreatedBy = token.UserID;
            onboardingDetails.CreatedDate = DateTime.UtcNow;
            _onboardingDetailRepository.Create(onboardingDetails);
            _onboardingDetailRepository.SaveChanges();

        }

        // [AbpAuthorize(AppPermissions.Pages_OnboardingDetails_Edit)]
        protected virtual async Task Update(CreateOrEditOnboardingDetailDto input, TokenModel token)
        {
            var onboardingDetail =  _onboardingDetailRepository.GetByID((int)input.Id);
            //  ObjectMapper.Map(input, onboardingDetail);
           
            AutoMapper.Mapper.Map(input, onboardingDetail);
            onboardingDetail.UpdatedBy = token.UserID;
            onboardingDetail.UpdatedDate = DateTime.UtcNow;
            onboardingDetail.OrganizationId = token.OrganizationID;
            _onboardingDetailRepository.Update(onboardingDetail);
            _onboardingDetailRepository.SaveChanges();

        }

       // [AbpAuthorize(AppPermissions.Pages_OnboardingDetails_Delete)]
        public async Task Delete(EntityDto input, TokenModel token)
        {
            //  _onboardingDetailRepository.Delete(input.Id);
            var onboardingDetail = _onboardingDetailRepository.GetByID((int)input.Id);
            onboardingDetail.IsDeleted = true;
            onboardingDetail.DeletedBy = token.UserID;
            onboardingDetail.DeletedDate = DateTime.UtcNow;

            _onboardingDetailRepository.Update(onboardingDetail);
            _onboardingDetailRepository.SaveChanges();
            changeOrder(onboardingDetail);
        }

       // [AbpAuthorize(AppPermissions.Pages_OnboardingDetails)]
        //public async Task<PagedResultDto<OnboardingDetailOnboardingHeaderLookupTableDto>> GetAllOnboardingHeaderForLookupTable(GetAllForLookupTableInput input)
        //{
        //    var query = _lookup_onboardingHeaderRepository.GetAll().WhereIf(
        //           !string.IsNullOrWhiteSpace(input.Filter),
        //          e => e.Header != null && e.Header.Contains(input.Filter)
        //       );

        //    var totalCount = await query.CountAsync();

        //    var onboardingHeaderList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<OnboardingDetailOnboardingHeaderLookupTableDto>();
        //    foreach (var onboardingHeader in onboardingHeaderList)
        //    {
        //        lookupTableDtoList.Add(new OnboardingDetailOnboardingHeaderLookupTableDto
        //        {
        //            Id = onboardingHeader.Id,
        //            DisplayName = onboardingHeader.Header?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<OnboardingDetailOnboardingHeaderLookupTableDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}
      //  [AllowAnonymous]
        public List<OnboardingDetail> GetAllByHeaderId(int id)
        {

            var filteredOnboardingDetails = _onboardingDetailRepository.GetAll()
                .Where(e => false || e.OnboardingHeaderId == id && e.IsDeleted == false).ToList();
            return filteredOnboardingDetails;

        }

        protected void changeOrder(OnboardingDetail onboardingDetail)
        {
            var filteredOnboardingDetails = _onboardingDetailRepository.GetAll()
                .Where(e => false || e.OnboardingHeaderId == onboardingDetail.OnboardingHeaderId && e.IsDeleted == false && e.Order > onboardingDetail.Order).ToList();
          //  return filteredOnboardingDetails;
          foreach(var item in filteredOnboardingDetails)
            {
                var onboardingDetails = _onboardingDetailRepository.GetByID((int)item.Id);
                onboardingDetails.Order = onboardingDetails.Order - 1;

                _onboardingDetailRepository.Update(onboardingDetails);
                _onboardingDetailRepository.SaveChanges();
            }
        }

    }
}
