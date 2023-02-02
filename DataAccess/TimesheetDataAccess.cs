using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TimesheetDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public TimesheetDataAccess()
        {
        }

        ~TimesheetDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public DataTable GetTimesheet(string projectId, string employeeId, string startDate, string endDate, long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTimesheet", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@startDate", startDate);
                connector.AddInParameterWithValue("@endDate", endDate);
                connector.AddInParameterWithValue("@projectId", projectId);
                connector.AddInParameterWithValue("@employeeId",employeeId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable GetTimesheetById(long id)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTimesheetById", true))
            {
                connector.AddInParameterWithValue("@timesheetId", id);
                dt = connector.GetDataTable();
            }
            return dt;
        }


        public string GetTotalHoursOfDay(long employeeId, string? date, decimal hours, out string message)
        {
            DataTable dt = null;
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_GetTotalHoursOfDay", true))
            {
                connector.AddInParameterWithValue("@employeeId", Convert.ToInt32(employeeId));
                connector.AddInParameterWithValue("@hours", hours);
                connector.AddInParameterWithValue("@date", date);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }
            return message;
        }

        public string GetWorkingHours(string? date, out string message)
        {
            DataTable dt = null;
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_GetWorkingHours", true))
            {
                connector.AddInParameterWithValue("@date",date);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }
            return message;
        }
        public string GetTotalHours(long employeeId, DateTime? date,out string message)
        {
            DataTable dt = null;
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_GetTotalHours", true))
            {
                connector.AddInParameterWithValue("@employeeId", Convert.ToInt32(employeeId));
                connector.AddInParameterWithValue("@date", date);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }
            return message;
        }

        public DataTable ListReasons(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListReasons", true))
            {
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable GetRemainingDays(int employeeId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetRemainingDays", true))
            {
                connector.AddInParameterWithValue("@employeeId", employeeId);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetRemainingTimeSheet(int employeeId, string date)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetRemainingTimeSheet", true))
            {
                connector.AddInParameterWithValue("@employeeId", employeeId);
                connector.AddInParameterWithValue("@date", date);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public string CreateTimesheet(TimesheetModel timesheet, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateTimesheet", true))
            {
                if (timesheet.employeeId == 0 || timesheet.employeeId == null)
                    connector.AddInParameterWithValue("@employeeId", userId);
                else
                    connector.AddInParameterWithValue("@employeeId", timesheet.employeeId);
                if (timesheet.projectId == 0 || timesheet.projectId == null)
                connector.AddInParameterWithValue("@projectId",DBNull.Value);
                else
                connector.AddInParameterWithValue("@projectId", timesheet.projectId);
                if (timesheet.taskId == 0 || timesheet.taskId == null)
                    connector.AddInParameterWithValue("@taskID", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@taskID", timesheet.taskId);
                if (timesheet.date == null)
                    connector.AddInParameterWithValue("@date", DBNull.Value);
                else
                {
                    connector.AddInParameterWithValue("@date", timesheet.date);
                }
                  
                if(timesheet.trackerFlag == 0 || timesheet.trackerFlag == null)
                connector.AddInParameterWithValue("@trackerFlag",DBNull.Value);
                else
                connector.AddInParameterWithValue("@trackerFlag", timesheet.trackerFlag);
                connector.AddInParameterWithValue("@type", timesheet.type);
                if (timesheet.hours == 0 || timesheet.hours == null )
                    connector.AddInParameterWithValue("@hours",DBNull.Value);
                else
                    connector.AddInParameterWithValue("@hours", timesheet.hours);
                if (timesheet.leavehours == null)
                    connector.AddInParameterWithValue("@leavehours", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@leavehours", timesheet.leavehours);
                if (timesheet.reasonCode == null || timesheet.reasonCode =="")
                connector.AddInParameterWithValue("@reasonCode", DBNull.Value);
                else
                connector.AddInParameterWithValue("@reasonCode", timesheet.reasonCode);
                if (timesheet.notes == null || timesheet.notes=="")
                    connector.AddInParameterWithValue("@notes", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@notes", timesheet.notes);
                if (timesheet.leavenotes == null || timesheet.leavenotes == "")
                    connector.AddInParameterWithValue("@leaveNotes", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@leaveNotes", timesheet.leavenotes);
                //if (timesheet.LeaveType == null)
                //    connector.AddInParameterWithValue("@leaveType", DBNull.Value);
                //else
                //    connector.AddInParameterWithValue("@leaveType", timesheet.LeaveType);
                connector.AddInParameterWithValue("@isDeleted", false);
                connector.AddInParameterWithValue("@createdBy", userId);
                connector.AddInParameterWithValue("@createdDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

       

        public string DeleteTimesheet(long id, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteTimesheet", true))
            {
                connector.AddInParameterWithValue("@timesheetId", id);
                connector.AddInParameterWithValue("@deletedBy", userId);
                connector.AddInParameterWithValue("@deletedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }
        public string GetJoiningDate(long id, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_GetJoiningDate", true))
            {
                connector.AddInParameterWithValue("@employeeId", id);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string GetLastDayOftimeSheet(long id, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_GetLastDayOftimeSheet", true))
            {
                connector.AddInParameterWithValue("@employeeId", id);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        #region ========================= Dispose Method ==============
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
