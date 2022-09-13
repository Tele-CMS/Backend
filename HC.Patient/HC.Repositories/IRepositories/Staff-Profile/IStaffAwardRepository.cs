using HC.Model;
using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IStaffAwardRepository
    {
        /// <summary>
        /// To save and update staff award
        /// </summary>
        /// <param name="staffAward"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        StaffAward SaveUpdateStaffAward(StaffAward staffAward, TokenModel tokenModel);

        /// <summary>
        /// To get list of staff awards by staff id
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        List<StaffAward> GetStaffAward(int staffId, TokenModel tokenModel);

        /// <summary>
        /// To get staff award by award id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        StaffAward GetStaffAwardById(int id, TokenModel tokenModel);
    }
}
