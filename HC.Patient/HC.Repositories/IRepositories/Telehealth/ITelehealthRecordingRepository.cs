using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface ITelehealthRecordingRepository : IRepositoryBase<TelehealthRecordingDetail>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="telehealthRecordingDetail"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthRecordingDetail SaveVideoArchived(TelehealthRecordingDetail telehealthRecordingDetail, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthRecordingDetail GetVideoArchivedDetail(string archiveId, TokenModel tokenModel);
    }
}
