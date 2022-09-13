using HC.Model;
using HC.Patient.Model.Claim;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDIGenerator.IServices
{
    public interface IEDI837Service
    {
        string GenerateEDI837_005010X222A1(EDI837FileModel ediFileModel, string payerpreference, string submissionType,string type);
        string GenerateSingleEDI837(EDI837FileModel ediFileModel);
        string GenerateBatchEDI837(EDI837FileModel ediFileModel);
        string GenerateSingleEDI837_Secondary(EDI837FileModel ediFileModel);
        string GenerateBatchEDI837_Secondary(EDI837FileModel ediFileModel);
        string GenerateSingleEDI837_Tertiary(EDI837FileModel ediFileModel);
        string GenerateBatchEDI837_Tertiary(EDI837FileModel ediFileModel);
    }
}
