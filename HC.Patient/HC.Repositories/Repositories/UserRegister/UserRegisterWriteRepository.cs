using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories
{
    public class UserRegisterWriteRepository : IUserRegisterWriteRepository
    {
        private HCOrganizationContext _context;
        public UserRegisterWriteRepository(HCOrganizationContext context)
        {
            this._context = context;
        }
        public int SaveNewUser(Entity.User user, Entity.Staffs staffs, Entity.StaffLocation staffLocation, Entity.UserInvitation userInvitation)
        {
            int result = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Add New user entry
                    _context.User.Add(user);
                    result = _context.SaveChanges();
                    if (result <= 0)
                        return result;

                    //add new staff entry
                    result = 0;
                    staffs.UserID = user.Id;
                    if (staffs.UserID <= 0)
                        return result;
                    _context.Staffs.Add(staffs);
                    result = _context.SaveChanges();
                    if (result <= 0)
                    {
                        transaction.Rollback();
                        return result;
                    }


                    //#region Need to remove once tested
                    //staffs.IsActive = false;
                    //_context.Staffs.Update(staffs);
                    //result = _context.SaveChanges();
                    //if (result <= 0)
                    //    return result;
                    //#endregion
                    //add new staff location entry
                    result = 0;
                    staffLocation.StaffId = staffs.Id;
                    _context.StaffLocation.Add(staffLocation);
                    result = _context.SaveChanges();
                    if (result <= 0)
                    {
                        transaction.Rollback();
                        return result;
                    }

                    //updating invitaion detail
                    userInvitation.InvitationStatus = (int)Common.Enums.CommonEnum.UserInvitationStatus.Accepted;
                    userInvitation.UpdatedBy = user.Id;
                    userInvitation.UpdatedDate = DateTime.UtcNow;
                    userInvitation.InvitedUserId = user.Id;
                    _context.UserInvitation.Update(userInvitation);
                    result = _context.SaveChanges();
                    if (result <= 0)
                    {
                        transaction.Rollback();
                        return result;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    transaction.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public int SaveNewUser(Entity.User user, Entity.Staffs staffs, Entity.StaffLocation staffLocation)
        {
            int result = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Add New user entry
                    _context.User.Add(user);
                    result = _context.SaveChanges();
                    if (result <= 0)
                        return result;

                    //add new staff entry
                    result = 0;
                    staffs.UserID = user.Id;
                    if (staffs.UserID <= 0)
                        return result;
                    _context.Staffs.Add(staffs);
                    result = _context.SaveChanges();
                    if (result <= 0)
                    {
                        transaction.Rollback();
                        return result;
                    }


                    //#region Need to remove once tested
                    //staffs.IsActive = false;
                    //_context.Staffs.Update(staffs);
                    //result = _context.SaveChanges();
                    //if (result <= 0)
                    //    return result;
                    //#endregion
                    //add new staff location entry
                    result = 0;
                    staffLocation.StaffId = staffs.Id;
                    _context.StaffLocation.Add(staffLocation);
                    result = _context.SaveChanges();
                    if (result <= 0)
                    {
                        transaction.Rollback();
                        return result;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    transaction.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public InvitationRejectLog RejectInvitation(InvitationRejectLog invitationRejectLog)
        {
            int result = 0;
            _context.InvitationRejectLogs.Add(invitationRejectLog);
            result = _context.SaveChanges();
            if (result <= 0)
                return null;
            return invitationRejectLog;
        }
    }
}
