using HC.Model;
using HC.Patient.Model.Payroll;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Payroll
{
    public interface IAgencyHolidaysService : IBaseService
    {
        JsonModel SaveAgencyHolidays(HolidaysModel holidaysModel, TokenModel token);
        JsonModel GetAgencyHolidaysById(int Id, TokenModel token);
        JsonModel DeleteAgencyHolidays(int Id, TokenModel token);
        JsonModel GetAgencyHolidaysList(int pageNumber, int pageSize, TokenModel token);
    }
}
