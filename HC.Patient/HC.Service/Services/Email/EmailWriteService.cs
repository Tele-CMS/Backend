using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Service.IServices;
using HC.Service;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services
{
    public class EmailWriteService : BaseService, IEmailWriteService
    {
        private JsonModel response;
        private readonly IEmailRepository _emailRepository;
        private readonly IMapper _mapper;
        public EmailWriteService(IEmailRepository emailRepository, IMapper mapper)
        {
            _emailRepository = emailRepository;
            _mapper = mapper;
        }
        public JsonModel SaveEmailLog(EmailModel emailModel, TokenModel tokenModel)
        {
            if (emailModel == null)
                response = new JsonModel(new object(), StatusMessage.EmailLogNotFound, (int)HttpStatusCodes.BadRequest);
            var emailLog = _mapper.Map<EmailLog>(emailModel);
            response = new JsonModel();
            EmailLog email = _emailRepository.SaveEmailLog(emailLog);
            if (email != null)
            {
                response = new JsonModel(email, StatusMessage.EmailLogSavedSuccessfully, (int)HttpStatusCodes.OK);
            }
            else
                response = new JsonModel(new object(), StatusMessage.EmailLogNotSaved, (int)HttpStatusCodes.InternalServerError);
            return response;
        }
    }
}
