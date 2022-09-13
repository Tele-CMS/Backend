using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI271ParserService:IBaseService
    {
        JsonModel ReadEDI271(TokenModel token);
    }
}
