using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service
{
    public class Enum
    {
    }
    public enum EmailTemplateType
    {
        CreateAppointment = 1,
        DeleteAppointment = 2,
        CancelAppointment = 3,
        RequestAppointment = 4,
        ApprovedAppointment = 5,
        UpdateAppointment = 6,
        UpdateRequestAppointment = 7,
        ActivateAppointment = 8,
        TentativeAppointment = 9,
        AcceptAppointment = 10,
        RejectAppointment = 11,
        AutoConfirmTentativeAppointment = 12,
        TaskAssign = 13
    }

    public static class EmailNotificationSubject
    {
        public static string AppointmentNotification { get; set; } = "Appointment Notification";
        public static string TaskAssignNotification { get; set; } = "Task Assign Notification";

    }
    public enum ErrorLogType
    {
        EmailNotification = 1,
        PushNotification = 2
    }
}
