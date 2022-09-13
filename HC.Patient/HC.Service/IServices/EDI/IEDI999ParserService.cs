using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI999ParserService : IBaseService
    {
        JsonModel ReadEDI999(TokenModel token);
    }
}
