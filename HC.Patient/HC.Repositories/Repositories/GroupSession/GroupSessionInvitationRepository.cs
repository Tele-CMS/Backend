using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System;
using System.Linq;

namespace HC.Patient.Repositories.Repositories
{
   public class GroupSessionInvitationRepository : RepositoryBase<GroupSessionInvitations>, IGroupSessionInvitationRepository
    {
        private HCOrganizationContext _context;
        public GroupSessionInvitationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public GroupSessionInvitations SaveGroupSessionInvitation(GroupSessionInvitations groupSessionInvitations,TokenModel tokenModel)
        {
            _context.Add(groupSessionInvitations);
            if (_context.SaveChanges() > 0)
                return groupSessionInvitations;
            else return null;
        }
        public GroupSessionInvitations UpdateGroupSessionInvitation(GroupSessionInvitations groupSessionInvitations, TokenModel tokenModel)
        {
            _context.Update(groupSessionInvitations);
            if (_context.SaveChanges() > 0)
                return groupSessionInvitations;
            else return null;
        }

        public GroupSessionInvitations GetGroupSessionByInvitaionId(Guid? invitaionId, TokenModel tokenModel)
        {
            return _context.GroupSessionInvitations.Where(x => x.InvitaionId == invitaionId && x.OrganizationId == tokenModel.OrganizationID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }
        public GroupSessionInvitations GetGroupSessionByInvitaionAppointmentId(int invitedAppointmentId, TokenModel tokenModel)
        {
            return _context.GroupSessionInvitations.Where(x => x.InvitedAppointmentId == invitedAppointmentId && x.OrganizationId == tokenModel.OrganizationID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }

        public GroupSessionInvitations GetGroupSessionByEmailAndName(GroupSessionInvitationModel groupSessionInvitationModel, TokenModel tokenModel)
        {
            return _context.GroupSessionInvitations
                .Where(x => 
                x.Name.ToUpper() == groupSessionInvitationModel.Name.ToUpper() 
                && x.Email == groupSessionInvitationModel.Email
                && x.SessionId == groupSessionInvitationModel.SessionId
                && x.AppointmentId == groupSessionInvitationModel.AppointmentId
                && x.OrganizationId == tokenModel.OrganizationID 
                && x.IsActive == true 
                && x.IsDeleted == false)
                .FirstOrDefault();
        }
        public GroupSessionInvitations GetGroupSessionByEmail(GroupSessionInvitationModel groupSessionInvitationModel, TokenModel tokenModel)
        {
            return _context.GroupSessionInvitations
                .Where(x =>
                //x.Name.ToUpper() == groupSessionInvitationModel.Name.ToUpper()
                //&& 
                x.Email == groupSessionInvitationModel.Email
                && x.SessionId == groupSessionInvitationModel.SessionId
                && x.AppointmentId == groupSessionInvitationModel.AppointmentId
                && x.OrganizationId == tokenModel.OrganizationID
                && x.IsActive == true
                && x.IsDeleted == false)
                .FirstOrDefault();
        }
        public GroupSessionInvitations GetGroupSessionByUserId(int userId, TokenModel tokenModel)
        {
            return _context.GroupSessionInvitations
                .Where(x =>
                x.UserID==userId
                && x.OrganizationId == tokenModel.OrganizationID
                && x.IsActive == true
                && x.IsDeleted == false)
                .FirstOrDefault();
        }

    }
}
