//using Audit.SqlServer.Providers;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HC.Patient.Web
{
    //public class FileUploadOperation : IOperationFilter
    public class FileUploadOperation 
    {
        //public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        //{
        //    if (operation.OperationId.ToLower() == "apivaluesuploadpost")
        //    {
        //        operation.Parameters.Clear();
        //        operation.Parameters.Add(new NonBodyParameter
        //        {
        //            Name = "uploadedFile",
        //            In = "formData",
        //            Description = "Upload File",
        //            Required = true,
        //            Type = "file"
        //        });
        //        operation.Consumes.Add("multipart/form-data");
        //    }
        //}
    }
}