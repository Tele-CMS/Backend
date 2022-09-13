using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.PatientEncLinkedDataChanges
{
    public class PatientEncLinkedDataChangesRepository: RepositoryBase<PatientEncounterLinkedDataChanges>, IPatientEncLinkedDataChangesRepository
    {
        HCOrganizationContext _context;
        public PatientEncLinkedDataChangesRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }

        public void savePatientEncounterChanges(List<ChangesLog> changesLogs, int? recordId, int encounterId, TokenModel tokenModel)
        {
            if (changesLogs.Count > 0 && encounterId > 0)
            {
                List<PatientEncounterLinkedDataChanges> patientEncounterLinkedDataChangesList = new List<PatientEncounterLinkedDataChanges>();
                foreach (ChangesLog changes in changesLogs)
                {
                    string entityName = changes.TableName;
                    PatientEncounterLinkedDataChanges patientEncounterLinkedData = new PatientEncounterLinkedDataChanges();
                    patientEncounterLinkedData.EncounterId = encounterId;
                    patientEncounterLinkedData.OldValue = changes.OriginalValue;
                    patientEncounterLinkedData.NewValue = changes.NewValue;
                    patientEncounterLinkedData.MasterPatientEncLinkedColumnId = changes.ColumnId;
                    patientEncounterLinkedData.Action = changes.State;
                    patientEncounterLinkedData.ChangeDescription = string.Concat("[", changes.State, "] ", changes.ColumnName , " - ", changes.OriginalValue, " to ", changes.NewValue);
                    patientEncounterLinkedData.CreatedBy = tokenModel.UserID;
                    patientEncounterLinkedData.CreatedDate = DateTime.UtcNow;
                    patientEncounterLinkedData.IsActive = true;
                    patientEncounterLinkedData.IsDeleted = false;
                    switch (entityName)
                    {
                        case "PatientAllergies":
                            patientEncounterLinkedData.PatientAllergyId = recordId;
                            break;
                        case "PatientImmunization":
                            patientEncounterLinkedData.PatientImmunizationId = recordId;
                            break;
                        case "PatientSocialHistory":
                            patientEncounterLinkedData.PatientSocialHistoryId = changes.RecordID != 0 ? (int?)changes.RecordID : null;
                            break;
                        case "TrackPatientActivity":
                            patientEncounterLinkedData.TrackPatientActivityId = changes.RecordID != 0 ? (int?)changes.RecordID : null;
                            break;
                        case "PatientMedicalFamilyHistory":
                        case "PatientMedicalFamilyHistoryDiseases":
                            patientEncounterLinkedData.PatientMedicalFamilyHistoryId = recordId;
                            break;
                        case "PatientPhysician":
                            patientEncounterLinkedData.PatientPhysicianId = recordId;
                            break;
                        case "PatientLab":
                            patientEncounterLinkedData.PatientLabId = recordId;
                            break;
                        case "PatientVitals":
                            patientEncounterLinkedData.PatientVitalId = recordId;
                            break;
                        case "PatientMedication":
                            patientEncounterLinkedData.PatientMedicationId = recordId;
                            break;
                        case "Barriers":
                            patientEncounterLinkedData.PatientBarrierId = recordId;
                            break;
                        case "DFA_PatientDocuments":
                            patientEncounterLinkedData.PatientAssessmentId = changes.RecordID != 0 ? (int?)changes.RecordID : null;
                            break;
                        case "PatientCareMetric":
                        case "PatientCareMetricProgram":
                        case "PatientCareMetricsQuestionAnswer":
                            patientEncounterLinkedData.PatientCareMetricId = recordId;
                            patientEncounterLinkedData.MasterCareMetricsQuestionId = changes.RecordID> 0 ? (int?)changes.RecordID : null;
                                break;
                        case "PatientReferrals":
                            patientEncounterLinkedData.PatientReferralId = recordId>0?recordId:null;
                            break;
                        case "Tasks":
                            patientEncounterLinkedData.TaskId = recordId > 0 ? recordId : null; 
                            break;
                        case "PatientDiagnosis":
                            patientEncounterLinkedData.PatientDiagnosisId = recordId;
                            break;

                        default:
                            break;
                    }
                    patientEncounterLinkedDataChangesList.Add(patientEncounterLinkedData);
                }
                _context.AddRange(patientEncounterLinkedDataChangesList);
                _context.SaveChanges();
            }
        }
    }
}
