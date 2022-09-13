using HC.Model;
using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;
namespace HC.Patient.Repositories.IRepositories
{
    public interface IStaffQualificationRepository
    {
        /// <summary>
        /// To Save and Update Staff Qualifications
        /// </summary>
        /// <param name="staffQualification"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        StaffQualification SaveUpdateStaffQualification(StaffQualification staffQualification, TokenModel tokenModel);

        /// <summary>
        /// To Get Staff Qualifications based on staffid
        /// </summary>
        /// <param name="staffId">staff id</param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        List<StaffQualification> GetStaffQualification(int staffId, TokenModel tokenModel);

        /// <summary>
        /// to get list of staff qualifications based on staff id
        /// </summary>
        /// <param name="id">staff id</param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        StaffQualification GetStaffQualificationById(int id, TokenModel tokenModel);
    }
}
