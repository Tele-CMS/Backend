﻿// <auto-generated />
using HC.Patient.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace HC.Patient.Web.Migrations.HCMaster
{
    [DbContext(typeof(HCMasterContext))]
    [Migration("20180221055811_AddedNullableFieldsInOrganization")]
    partial class AddedNullableFieldsInOrganization
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HC.Patient.Entity.MasterAppConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("ConfigType");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Key");

                    b.Property<string>("Label");

                    b.Property<int?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("MasterAppConfiguration");
                });

            modelBuilder.Entity("HC.Patient.Entity.MasterOrganization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OrganizationID");

                    b.Property<string>("Address1")
                        .HasMaxLength(200);

                    b.Property<string>("Address2")
                        .HasMaxLength(200);

                    b.Property<string>("BusinessName")
                        .HasMaxLength(100);

                    b.Property<string>("City")
                        .HasMaxLength(100);

                    b.Property<string>("ContactPersonFirstName")
                        .HasMaxLength(50);

                    b.Property<string>("ContactPersonLastName")
                        .HasMaxLength(50);

                    b.Property<string>("ContactPersonMiddleName")
                        .HasMaxLength(50);

                    b.Property<string>("ContactPersonPhoneNumber")
                        .HasMaxLength(15);

                    b.Property<int?>("CountryID");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int>("DatabaseDetailId");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("Email")
                        .HasMaxLength(250);

                    b.Property<byte[]>("Favicon");

                    b.Property<string>("Fax")
                        .HasMaxLength(40);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<byte[]>("Logo");

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .HasMaxLength(100);

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.Property<int?>("StateID");

                    b.Property<int?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<string>("UserName")
                        .HasMaxLength(100);

                    b.Property<string>("Zip")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("MasterOrganization");
                });

            modelBuilder.Entity("HC.Patient.Entity.MasterSecurityQuestions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Question")
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("MasterSecurityQuestions");
                });

            modelBuilder.Entity("HC.Patient.Entity.OrganizationDatabaseDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("DatabaseName")
                        .HasMaxLength(100);

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsCentralised");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Password")
                        .HasMaxLength(100);

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<string>("UserName")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("OrganizationDatabaseDetail");
                });

            modelBuilder.Entity("HC.Patient.Entity.SuperUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime?>("BlockDateTime");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsBlock");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Password");

                    b.Property<string>("RoleName");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("SuperUser");
                });
#pragma warning restore 612, 618
        }
    }
}
