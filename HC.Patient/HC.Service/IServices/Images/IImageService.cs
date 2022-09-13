using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Images
{
    public interface IImageService :IBaseService
    {
        string SaveImages(string base64String, string directory, string folderName);
        Patients ConvertBase64ToImage(Patients entity);
        Staffs ConvertBase64ToImageForUser(Staffs entity);
        GlobalCodeModel ConvertBase64ToImageForDocSpeciality(GlobalCodeModel entity);
        PatientInsuranceDetails ConvertBase64ToImageForInsurance(PatientInsuranceDetails entity);
    }
}
