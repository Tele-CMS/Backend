using HC.Patient.Model.Claim;
using HC.Patient.Model.Patient;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDIGenerator.IServices
{
    public interface IEDI270Service
    {
        string GenerateEDI270(EDI270FileModel ediFileModel);
    }
}
