using HC.Model;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.IServices.PatientDiseaseManagementProgram
{
    public interface IPatientDiseaseManagementProgramService :IBaseService
    {
        JsonModel GetPatientDiseaseManagementProgramList(int patientId, FilterModel filterModel, TokenModel token);
        JsonModel AssignNewPrograms(AssignNewProgramModel assignNewProgramModel, TokenModel token);
        JsonModel EnrollmentPatientInDiseaseManagementProgram(int patientDiseaseManagementProgramId,DateTime enrollmentDate, bool isEnrolled, TokenModel token);
        JsonModel GetPatientDiseaseManagementProgramDetails(int Id, TokenModel token);
        JsonModel deleteDiseaseManagementProgram(int Id, TokenModel token);
        JsonModel GetAllPatientDiseaseManagementProgramsList(ProgramsFilterModel filterModel, TokenModel token);
        MemoryStream GetProgramsEnrollPatientsForPDF(ProgramsFilterModel filterModel, TokenModel token);
        JsonModel GetReportHRAProgram(HRALogFilterModel filterModel, TokenModel tokenModel);
    }
}
