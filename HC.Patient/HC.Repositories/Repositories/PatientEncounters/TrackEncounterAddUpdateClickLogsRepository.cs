﻿using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
   public class TrackEncounterAddUpdateClickLogsRepository : RepositoryBase<EncounterClickLogs>, ITrackEncounterAddUpdateClickLogsRepository
    {
        private readonly HCOrganizationContext _context;
        public TrackEncounterAddUpdateClickLogsRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
    }
}
