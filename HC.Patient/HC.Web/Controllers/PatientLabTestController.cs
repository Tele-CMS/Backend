using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.PatientCommon.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class PatientLabTestController : CustomJsonApiController<Entity.PatientLabTest, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IPatientCommonService _patientCommonService;
        private readonly IHostingEnvironment _env;

        #region Construtor of the class
        public PatientLabTestController(
        IJsonApiContext jsonApiContext,
        IResourceService<Entity.PatientLabTest, int> resourceService,
        ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IHostingEnvironment env, IUserCommonRepository userCommonRepository)
        : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _env = env;
                _dbContextResolver = jsonApiContext.GetDbContextResolver();                
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                this._patientCommonService = patientCommonService;
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

                }
            }
            catch
            {

            }
        }
        #endregion

        //#region Class Methods
        ///// <summary>
        ///// this method is used for update patient social history
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="patientInfo"></param>
        ///// <returns></returns>
        //[HttpPatch("{id}")]
        //public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientLabTest patientLabTest)
        //{
        //    var hl7 = GenerateHL7(patientLabTest.PatientID);
        //    patientLabTest.Hl7Url = hl7.Keys.FirstOrDefault();
        //    patientLabTest.Hl7 = hl7.Keys.FirstOrDefault();
        //    return await base.PatchAsync(id, patientLabTest);
        //}

        ////[ValidateModel]
        //[HttpPost]
        //public override async Task<IActionResult> PostAsync([FromBody]PatientLabTest patientLabTest)
        //{
        //    patientLabTest.OrderNumber = GetRandomNumber(5).ToString() + GetRandomString(2).ToString();
        //    patientLabTest.FillerOrderNumber = GetRandomNumber(11).ToString();
        //    var asyncPatientLabTest = await base.PostAsync(patientLabTest);
        //    var hl7 = GenerateHL7(patientLabTest.PatientID);
        //    patientLabTest = (PatientLabTest)((ObjectResult)asyncPatientLabTest).Value;
        //    patientLabTest.Hl7Url = hl7.Keys.FirstOrDefault();
        //    patientLabTest.Hl7 = hl7.Keys.FirstOrDefault();
        //    return await base.PatchAsync(patientLabTest.Id, patientLabTest);
        //}

        //private string GetRandomNumber(int length)
        //{
        //    Random getrandom = new Random();
        //    string pool = "0123456789";
        //    var builder = new System.Text.StringBuilder();

        //    for (var i = 0; i < length; i++)
        //    {
        //        var c = pool[getrandom.Next(0, pool.Length)];
        //        builder.Append(c);
        //    }

        //    return builder.ToString();
        //}

        //private string GetRandomString(int length)
        //{
        //    Random getrandom = new Random();
        //    string pool = "abcdefghijklmnopqrstuvwxyz";
        //    var builder = new System.Text.StringBuilder();

        //    for (var i = 0; i < length; i++)
        //    {
        //        var c = pool[getrandom.Next(0, pool.Length)];
        //        builder.Append(c);
        //    }

        //    return builder.ToString();
        //}

        //private Dictionary<string,string> GenerateHL7(int PatientID)
        //{
        //    //try
        //    //{
        //        return null;
        //        //var Patient = _dbContextResolver.GetDbSet<Patients>().Include(p=>p.Staff).Include(l => l.Organization).Include(l => l.PatientLabTest).Where(l => l.Id == PatientID).FirstOrDefault();
        //       // string hl7PathOriginal = "";
        //        //string hl7PathToSave = "";


        //        //hl7PathOriginal = Directory.GetCurrentDirectory().Replace("HC_Patient","HC_Photos").Replace("HCPatient_test", "HC_Photos") + "/hl7Messages/" + Patient.FirstName + Patient.LastName + Patient.CreatedDate.ToString("yyyyMMddhhmmss") + ".hl7";

        //        //hl7PathToSave = "http://108.168.203.227/HC_Photos/hl7Messages/" +Patient.FirstName + Patient.LastName + Patient.CreatedDate.ToString("yyyyMMddhhmmss") + ".hl7";



        //    //    Message message = new Message();
        //    //    Encoding encoding1 = new Encoding();
        //    //    Segment segment1 = new Segment("MSH", encoding1);
        //    //    Field field1 = new Field(Patient.OrganizationID.ToString(), encoding1);
        //    //    segment1.AddNewField(field1, 4);
        //    //    Field field2 = new Field("ORU", encoding1);
        //    //    Component com1 = new Component("R01", encoding1);
        //    //    field2.AddNewComponent(com1);
        //    //    segment1.AddNewField(field2, 9);

        //    //    message.AddNewSegment(segment1);
        //    //    Encoding encoding2 = new Encoding();
        //    //    Segment segment2 = new Segment("PID", encoding2);
        //    //    Field field3 = new Field(Patient.MRN, encoding2);
        //    //    Field field4 = new Field(Patient.LastName, encoding2);
        //    //    Component component1 = new Component(Patient.FirstName, encoding2);
        //    //    Component component2 = new Component(Patient.MiddleName, encoding2);
        //    //    field4.AddNewComponent(component1);
        //    //    field4.AddNewComponent(component2);

        //    //    segment2.AddNewField(field3, 5);
        //    //    segment2.AddNewField(field4, 8);
        //    //    message.AddNewSegment(segment2);

        //    //    Patient.PatientLabTest.ForEach(labtest =>
        //    //    {
        //    //        int labTestIndex = Patient.PatientLabTest.IndexOf(labtest);
        //    //        var loincCode = _dbContextResolver.GetDbSet<MasterLonic>().Where(k => k.Id == labtest.LonicCodeID).FirstOrDefault().LonicCode;
        //    //        Encoding encoding3 = new Encoding();
        //    //        Segment segment3 = new Segment("OBR", encoding3);
        //    //        segment3.AddNewField((labTestIndex + 1).ToString(), 2);
        //    //        segment3.AddNewField(labtest.OrderNumber, 4);
        //    //        segment3.AddNewField(labtest.FillerOrderNumber, 6);

        //    //        Field field5 = new Field(labtest.OrderNumber, encoding3);
        //    //        Component component3 = new Component(labtest.TestName, encoding3);
        //    //        Component component4 = new Component(loincCode, encoding3);
        //    //        field5.AddNewComponent(component3);
        //    //        field5.AddNewComponent(component4);
        //    //        segment3.AddNewField(field5, 8);
        //    //        segment3.AddNewField(MessageHelper.LongDateWithFractionOfSecond(labtest.OrderDate), 10);
        //    //        segment3.AddNewField(MessageHelper.LongDateWithFractionOfSecond(labtest.OrderDate), 12);

        //    //        Field field6 = new Field(Patient.OrganizationID.ToString(), encoding3);
        //    //        Component comp5 = new Component(Patient.Organization.OrganizationDescription, encoding3);
        //    //        Component comp6 = new Component(Patient.Organization.OrganizationName, encoding3);
        //    //        field6.AddNewComponent(comp5);
        //    //        field6.AddNewComponent(comp6);
        //    //        segment3.AddNewField(field6, 14);

        //    //        segment3.AddNewField(MessageHelper.LongDateWithFractionOfSecond(labtest.OrderDate.AddDays(7)), 16);
        //    //        segment3.AddNewField("F", 18);
        //    //        if (labtest.HL7Result != null)
        //    //        {
        //    //            segment3.Value = labtest.HL7Result;
        //    //        }
        //    //        message.AddNewSegment(segment3);
        //    //    });



        //    //    Encoding encoding4 = new Encoding();
        //    //    Segment segment4 = new Segment("ORC", encoding4);
        //    //    Field field7 = new Field("RE", encoding4);
        //    //    segment4.AddNewField(field7, 2);
        //    //    Field field8 = new Field(MessageHelper.LongDateWithFractionOfSecond(DateTime.UtcNow), encoding4);

        //    //    segment4.AddNewField(field8, 4);

        //    //    segment4.AddNewField("F", 6);

        //    //    message.AddNewSegment(segment4);

        //    //    message.MessageControlID = "MessageID123";
        //    //    message.ProcessingID = "P";
        //    //    message.Version = "2.5.1";
        //    //    message = message.HL7MessageFunction(Patient);

        //    //    var HL7MessageBase64 = EncodeTo64(message.HL7Message);

        //    //    //if (!System.IO.File.Exists(hl7PathOriginal))
        //    //    //{
        //    //    //    System.IO.File.WriteAllBytes(hl7PathOriginal, Convert.FromBase64String(HL7MessageBase64));
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    System.IO.File.Delete(hl7PathOriginal);
        //    //    //    System.IO.File.WriteAllBytes(hl7PathOriginal, Convert.FromBase64String(HL7MessageBase64));
        //    //    //}
        //    //    Dictionary<string, string> returnHL7 = new Dictionary<string, string>();
        //    //    var bytes = Convert.FromBase64String(HL7MessageBase64);
        //    //    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        //    //    returnHL7.Add(base64String, message.HL7Message);

        //    //    return returnHL7;
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    throw;
        //    //}

        //        Message message = new Message();
        //        Encoding encoding1 = new Encoding();
        //        Segment segment1 = new Segment("MSH", encoding1);
        //        Field field1 = new Field(Patient.OrganizationID.ToString(), encoding1);
        //        segment1.AddNewField(field1, 4);
        //        Field field2 = new Field("ORU", encoding1);
        //        Component com1 = new Component("R01", encoding1);
        //        field2.AddNewComponent(com1);
        //        segment1.AddNewField(field2, 9);

        //        message.AddNewSegment(segment1);
        //        Encoding encoding2 = new Encoding();
        //        Segment segment2 = new Segment("PID", encoding2);
        //        Field field3 = new Field(Patient.MRN, encoding2);
        //        Field field4 = new Field(Patient.LastName, encoding2);
        //        Component component1 = new Component(Patient.FirstName, encoding2);
        //        Component component2 = new Component(Patient.MiddleName, encoding2);
        //        field4.AddNewComponent(component1);
        //        field4.AddNewComponent(component2);

        //        segment2.AddNewField(field3, 5);
        //        segment2.AddNewField(field4, 8);
        //        message.AddNewSegment(segment2);

        //        Patient.PatientLabTest.ForEach(labtest =>
        //        {
        //            int labTestIndex = Patient.PatientLabTest.IndexOf(labtest);
        //            var loincCode = _dbContextResolver.GetDbSet<MasterLonic>().Where(k => k.Id == labtest.LonicCodeID).FirstOrDefault().LonicCode;
        //            Encoding encoding3 = new Encoding();
        //            Segment segment3 = new Segment("OBR", encoding3);
        //            segment3.AddNewField((labTestIndex + 1).ToString(), 2);
        //            segment3.AddNewField(labtest.OrderNumber, 4);
        //            segment3.AddNewField(labtest.FillerOrderNumber, 6);

        //            Field field5 = new Field(labtest.OrderNumber, encoding3);
        //            Component component3 = new Component(labtest.TestName, encoding3);
        //            Component component4 = new Component(loincCode, encoding3);
        //            field5.AddNewComponent(component3);
        //            field5.AddNewComponent(component4);
        //            segment3.AddNewField(field5, 8);
        //            segment3.AddNewField(MessageHelper.LongDateWithFractionOfSecond(labtest.OrderDate), 10);
        //            segment3.AddNewField(MessageHelper.LongDateWithFractionOfSecond(labtest.OrderDate), 12);

        //            Field field6 = new Field(Patient.OrganizationID.ToString(), encoding3);
        //            Component comp5 = new Component(Patient.Organization.Description, encoding3);
        //            Component comp6 = new Component(Patient.Organization.OrganizationName, encoding3);
        //            field6.AddNewComponent(comp5);
        //            field6.AddNewComponent(comp6);
        //            segment3.AddNewField(field6, 14);

        //            segment3.AddNewField(MessageHelper.LongDateWithFractionOfSecond(labtest.OrderDate.AddDays(7)), 16);
        //            segment3.AddNewField("F", 18);
        //            if (labtest.HL7Result != null)
        //            {
        //                segment3.Value = labtest.HL7Result;
        //            }
        //            message.AddNewSegment(segment3);
        //        });



        //        Encoding encoding4 = new Encoding();
        //        Segment segment4 = new Segment("ORC", encoding4);
        //        Field field7 = new Field("RE", encoding4);
        //        segment4.AddNewField(field7, 2);
        //        Field field8 = new Field(MessageHelper.LongDateWithFractionOfSecond(DateTime.UtcNow), encoding4);

        //        segment4.AddNewField(field8, 4);

        //        segment4.AddNewField("F", 6);

        //        message.AddNewSegment(segment4);

        //        message.MessageControlID = "MessageID123";
        //        message.ProcessingID = "P";
        //        message.Version = "2.5.1";
        //        message = message.HL7MessageFunction(Patient);

        //        var HL7MessageBase64 = EncodeTo64(message.HL7Message);

        //        //if (!System.IO.File.Exists(hl7PathOriginal))
        //        //{
        //        //    System.IO.File.WriteAllBytes(hl7PathOriginal, Convert.FromBase64String(HL7MessageBase64));
        //        //}
        //        //else
        //        //{
        //        //    System.IO.File.Delete(hl7PathOriginal);
        //        //    System.IO.File.WriteAllBytes(hl7PathOriginal, Convert.FromBase64String(HL7MessageBase64));
        //        //}
        //        Dictionary<string, string> returnHL7 = new Dictionary<string, string>();
        //        var bytes = Convert.FromBase64String(HL7MessageBase64);
        //        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        //        returnHL7.Add(base64String, message.HL7Message);

        //        return returnHL7;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //public static string EncodeTo64(string toEncode)
        //{
        //    byte[] toEncodeAsBytes
        //          = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
        //    string returnValue
        //          = System.Convert.ToBase64String(toEncodeAsBytes);
        //    return returnValue;
        //}

        //#endregion

        //#region Helping Methods
        //#endregion

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

    }
}