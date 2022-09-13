using HC.Common;
using HC.Model;
using HC.Patient.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HC.Common.HC.Common;
using Microsoft.Extensions.Primitives;

namespace HC.Patient.Data
{
    public class ConfigurationBuilderContext
    {
        public string GetNewConnection(HttpContext httpContext)
        {
            ConstantString constantString = new ConstantString();
            string host = string.Empty;
            //#if !Release
#if !Debug
            StringValues authorizationToken;
            var tokenExist = httpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);// get host name from token
            if (tokenExist)
            {
                //get the host name from request
                TokenModel token = CommonMethods.GetTokenDataModel(httpContext);
                host = token.DomainName;
            }
            else if (httpContext.Request.QueryString.Value.Contains("Business")) // before login get verified host name from query string
            {
                host = httpContext.Request.QueryString.Value.Split("=")[1];
            }
            else if (httpContext.Request.QueryString.Value.Contains("access_token")) // this is for chathub access_token is passed from frontend to access dyanmic database
            {
                host = httpContext.Request.QueryString.Value.Split("=")[2];
                httpContext.Request.Headers.Add("Authorization", $"Bearer {host}");
                TokenModel token = CommonMethods.GetTokenDataModel(httpContext);
                host = token.DomainName;
            }
            else
            {
                httpContext.Request.Headers.TryGetValue("BusinessToken", out authorizationToken); //get host name from header
                host = CommonMethods.Decrypt(authorizationToken);
            }
#else
            //its only for debug mode
            host = HCOrganizationConnectionStringEnum.Host; //its Merging db
#endif
            //return new connetion string which made from request host
            return GetDomain(host);

        }
        public string GetDomain(string host)
        {
            //TO DO Master connection string should be from app-setings
            var optionsBuilder = new DbContextOptionsBuilder<HCMasterContext>();
            optionsBuilder.UseSqlServer(CreateMasterConnectionString());
            HCMasterContext _masterContext = new HCMasterContext(optionsBuilder.Options);

            string con = string.Empty;

            //get the organization from Business-Name
            MasterOrganization org = _masterContext.MasterOrganization.Where(a => a.BusinessName == host && a.IsDeleted == false).FirstOrDefault();
            if (org != null)
            {
                //get the db credentials for new connection
                OrganizationDatabaseDetail orgData = _masterContext.OrganizationDatabaseDetail.Where(a => a.Id == org.DatabaseDetailId && a.IsDeleted == false).FirstOrDefault();

                //initialize Domain token model to create new connection string
                DomainToken domainData = new DomainToken();
                domainData.ServerName = orgData.ServerName;
                domainData.DatabaseName = orgData.DatabaseName;
                domainData.Password = orgData.Password;
                domainData.UserName = orgData.UserName;
                con = ConnectionString(domainData);
            }
            //return new connection string
            return con;
        }
        /// <summary>
        /// create dynamically new connection string
        /// </summary>
        /// <param name="domainToken"></param>
        /// <returns></returns>
        public string ConnectionString(DomainToken domainToken)
        {
            string conn = @"Server=" + domainToken.ServerName + ";Database=" + domainToken.DatabaseName + ";Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;User ID=" + domainToken.UserName + ";Password=" + domainToken.Password + ";Connection Timeout=3600;";
            return conn;
        }

        public string CreateOrganizationConnectionString()
        {
            DomainToken domainToken = new DomainToken();
            domainToken.ServerName = HCOrganizationConnectionStringEnum.Server;
            domainToken.DatabaseName = HCOrganizationConnectionStringEnum.Database;
            domainToken.UserName = HCOrganizationConnectionStringEnum.User;
            domainToken.Password = HCOrganizationConnectionStringEnum.Password;
            return ConnectionString(domainToken);
        }

        public string CreateMasterConnectionString()
        {
            DomainToken domainToken = new DomainToken();
            domainToken.ServerName = HCMasterConnectionStringEnum.Server;
            domainToken.DatabaseName = HCMasterConnectionStringEnum.Database;
            domainToken.UserName = HCMasterConnectionStringEnum.User;
            domainToken.Password = HCMasterConnectionStringEnum.Password;
            return ConnectionString(domainToken);
        }
    }
}