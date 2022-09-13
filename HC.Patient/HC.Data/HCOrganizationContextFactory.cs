//using Audit.SqlServer.Providers;
using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Linq;
using System.Net.Http;

namespace HC.Patient.Web
{
    public class ApplicationDbContextFactory
    {
        //private HttpContext _httpContext;
        public IConfigurationRoot Configuration { get; }
        //public ApplicationDbContextFactory(HttpContext httpContext) { _httpContext = httpContext; }

        //public HCOrganizationContext CreateApplicationDbContext(HttpContext htcontext)
        //{

        //    if(htcontext != null)
        //    {
        //        //TODO Something clever to create correct ApplicationDbContext with ConnectionString you need.
        //        CommonMethods commonMethods = new CommonMethods();

        //        TokenModel token = CommonMethods.GetTokenDataModel(htcontext);

        //        string host = token.DomainName;//"apollo";
        //        var optionsBuilder = new DbContextOptionsBuilder<HCOrganizationContext>();
        //        optionsBuilder.UseSqlServer(GetDomain(host));
        //        HCOrganizationContext _context = new HCOrganizationContext(optionsBuilder.Options, htcontext);
        //        return _context;                
        //    }
        //    else
        //    {
        //        var optionsBuilder = new DbContextOptionsBuilder<HCOrganizationContext>();                
        //        optionsBuilder.UseSqlServer(@"Server=108.168.203.227,7007;Database=HC_Patient_Merging;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;User ID=HC_Patient;Password=HC_Patient;");
        //        HCOrganizationContext _context = new HCOrganizationContext(optionsBuilder.Options);
        //        return _context;
        //    }
        //}

        //private string GetDomain(string host)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<HCMasterContext>();
        //    optionsBuilder.UseSqlServer(@"Server=108.168.203.227,7007;Database=HC_Patient_Master;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;User ID=HC_Patient;Password=HC_Patient;");
        //    HCMasterContext _masterContext = new HCMasterContext(optionsBuilder.Options);

        //    MasterOrganization org = _masterContext.MasterOrganization.Where(a => a.BusinessName == host).FirstOrDefault();
        //    OrganizationDatabaseDetail orgData = _masterContext.OrganizationDatabaseDetail.Where(a => a.Id == org.DatabaseDetailId).FirstOrDefault();
        //    DomainToken domainData = new DomainToken();
        //    domainData.ServerName = orgData.ServerName;
        //    domainData.DatabaseName = orgData.DatabaseName;
        //    domainData.Password = orgData.Password;
        //    domainData.UserName = orgData.UserName;
        //    string con = ConnectionString(domainData);
        //    return con;
        //}

        //private string ConnectionString(DomainToken domainToken)
        //{
        //    string conn = @"Server=" + domainToken.ServerName + ";Database=" + domainToken.DatabaseName + ";Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;User ID=" + domainToken.UserName + ";Password=" + domainToken.Password + ";";
        //    return conn;
        //}
    }
}