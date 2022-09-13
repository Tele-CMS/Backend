using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories.GlobalCodes;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories.GlobalCodes
{
    public class GlobalCodeRepository : RepositoryBase<GlobalCode>, IGlobalCodeRepository
    {
        private HCOrganizationContext _context;
        public GlobalCodeRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public GlobalCode CheckGlobalCodeExistance(string name, TokenModel tokenModel)
        {
            return _context.GlobalCode
                .Where(s => s.GlobalCodeName.ToLower() == name.ToLower()
                && s.OrganizationID == tokenModel.OrganizationID)
                .FirstOrDefault();
        }

        public GlobalCode CheckGlobalCodeExistanceWithPhoto(GlobalCodeModel gbModel, TokenModel tokenModel)
        {
            return _context.GlobalCode
                .Where(s => s.GlobalCodeName.ToLower() == gbModel.GlobalCodeName.ToLower()
                && s.OrganizationID == tokenModel.OrganizationID
                //&& s.PhotoPath == gbModel.PhotoPath && s.PhotoThumbnailPath == gbModel.PhotoThumbnailPath && s.IsActive == gbModel.IsActive
                 && s.PhotoPath == gbModel.SpecialityIcon && s.PhotoThumbnailPath == gbModel.SpecialityIcon && s.IsActive == gbModel.IsActive
                )
                .FirstOrDefault();
        }

        public GlobalCode getGlobalCodeById(TokenModel tokenModel, int id)
        {
            return _context.GlobalCode
                .Where(s => s.Id == id && s.OrganizationID == tokenModel.OrganizationID && s.IsDeleted == false).FirstOrDefault();
        }


        public GlobalCode saveUpdateGlobalCode(GlobalCode globalCode, TokenModel tokenModel)
        {

            if (globalCode.Id == 0)
                _context.GlobalCode.Add(globalCode);
            else
                _context.GlobalCode.Update(globalCode);

            if (_context.SaveChanges() > 0)
                return globalCode;
            else
                return null;
        }


        public IQueryable<GlobalCode> getGlobalCodeByOrganizationId(TokenModel tokenModel, GlobalCodeFilterModel globalCodeFilterModel)
        {
            if (string.IsNullOrEmpty(globalCodeFilterModel.sortColumn))
                globalCodeFilterModel.sortColumn = "GlobalCodeName";

            if (string.IsNullOrEmpty(globalCodeFilterModel.sortOrder) || globalCodeFilterModel.sortOrder == "asc")
                globalCodeFilterModel.sortOrder = "true";
            else
                globalCodeFilterModel.sortOrder = "false";

            return CommonMethods.OrderByField(_context.GlobalCode
                .Where(s => s.OrganizationID == tokenModel.OrganizationID && s.IsDeleted == false  && s.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "speciality" && (globalCodeFilterModel.SearchText==null || s.GlobalCodeName.Contains(globalCodeFilterModel.SearchText)) )
                .Distinct(), globalCodeFilterModel.sortColumn, Convert.ToBoolean(globalCodeFilterModel.sortOrder));
        }

        public IQueryable<GlobalCode> getGlobalServiceIconName(TokenModel tokenModel, GlobalCodeFilterModel globalCodeFilterModel)
        {
            //if (string.IsNullOrEmpty(globalCodeFilterModel.sortColumn))
            //    globalCodeFilterModel.sortColumn = "GlobalCodeName";
            
            //if (string.IsNullOrEmpty(globalCodeFilterModel.sortOrder) || globalCodeFilterModel.sortOrder == "asc")
            //    globalCodeFilterModel.sortOrder = "true";
            //else
            //    globalCodeFilterModel.sortOrder = "false";

            return _context.GlobalCode
                 .Where(s =>s.IsDeleted == false && s.IsActive == true && s.CreatedBy == 6);
        }


    }
}
