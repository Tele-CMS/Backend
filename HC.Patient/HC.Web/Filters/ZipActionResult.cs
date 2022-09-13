using Ionic.Zip;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC.Patient.Web.Filters
{
    public class ZipFileResult : ActionResult
    {
        public ZipFile zip { get; set; }
        public string filename { get; set; }

        public ZipFileResult(ZipFile zip)
        {
            this.zip = zip;
            this.filename = null;
        }
        public ZipFileResult(ZipFile zip, string filename)
        {
            this.zip = zip;
            this.filename = filename;
        }

        //public override void ExecuteResult(ActionContext context)
        //{
        //    var Response = context.HttpContext.Response;

        //    Response.ContentType = "application/zip";
        //    Response.AddHeader("Content-Disposition", "attachment;" + (string.IsNullOrEmpty(filename) ? "" : "filename=" + filename));


        //    zip.Save(Response.OutputStream);

        //    Response.End();
        //}
    }
}
