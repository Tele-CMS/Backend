﻿using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientPrescriptionRepository : IRepositoryBase<PatientPrescription>
    {
        IQueryable<T> GetprescriptionDrugList<T>() where T : class, new();
        IQueryable<T> GetPatientPrescriptions<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMasterprescriptionDrugsList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMasterPharmacyList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetPatientSentPrescriptions<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
