using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HC.Patient.Data
{
    public class HCMasterContext : DbContext
    {
        /// <summary>
        /// Db context for Patient module
        /// </summary>
        /// <param name="options"></param>
        public HCMasterContext(DbContextOptions<HCMasterContext> options) : base(options) { }
        public DbSet<MasterOrganization> MasterOrganization { get; set; }
        public DbSet<OrganizationDatabaseDetail> OrganizationDatabaseDetail { get; set; }
        public DbSet<MasterAppConfiguration> MasterAppConfiguration { get; set; }
        public DbSet<MasterSecurityQuestions> MasterSecurityQuestions { get; set; }
        public DbSet<SuperUser> SuperUser { get; set; }
        //IpAddress log
        public DbSet<IpAddressLog> IpAddressLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Organization
            modelBuilder.Entity<MasterOrganization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterOrganization>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterOrganization>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<SuperUser>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<SuperUser>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<SuperUser>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            // //OrganizationDatabaseDetail
            // modelBuilder.Entity<OrganizationDatabaseDetail>()
            //  .Property(b => b.IsDeleted)
            //  .HasDefaultValue(false);

            // modelBuilder.Entity<OrganizationDatabaseDetail>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

            // modelBuilder.Entity<OrganizationDatabaseDetail>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //MasterAppConfirguration
            modelBuilder.Entity<MasterAppConfiguration>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterAppConfiguration>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterAppConfiguration>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterSecurityQuestion
            modelBuilder.Entity<MasterSecurityQuestions>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterSecurityQuestions>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterSecurityQuestions>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //OrganizationDatabaseDetail
            modelBuilder.Entity<OrganizationDatabaseDetail>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<OrganizationDatabaseDetail>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<OrganizationDatabaseDetail>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

        }

        public IList<TEntity> ExecStoredProcedureListWithOutput<TEntity>(string commandText, int totalOutputParams, params object[] parameters) where TEntity : class, new()
        {
            var connection = this.Database.GetDbConnection();

            //  var context = ((Microsoft.AspNetCore. AspNet.DynamicData.ModelProviders.EFDataModelProvide)(this)).ObjectContext;
            IList<TEntity> result = new List<TEntity>();
            try
            {
                totalOutputParams = totalOutputParams == 0 ? 1 : totalOutputParams;
                //int o = 0;

                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000;
                    if (parameters != null)
                    {
                        // move parameters to command object
                        foreach (var p in parameters)
                        {
                            if (p != null)
                            {
                                cmd.Parameters.Add(p);
                            }
                        }
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = DataReaderMapToList<TEntity>(reader);
                        reader.NextResult();
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static IList<T> DataReaderMapToList<T>(IDataReader dr)
        {
            IList<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())               //Solution - Check if property is there in the reader and then try to remove try catch code
                {
                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch { continue; }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
