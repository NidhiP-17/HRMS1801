using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HolidayDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public HolidayDataAccess()
        {
        }

        ~HolidayDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public string CreateHoliday(HolidayModel holiday, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateHoliday", true))
            {
                connector.AddInParameterWithValue("@date", holiday.date);
                connector.AddInParameterWithValue("@day", holiday.day);
                connector.AddInParameterWithValue("@name", holiday.name);
                connector.AddInParameterWithValue("@type", holiday.type);
                connector.AddInParameterWithValue("@hours", holiday.hours);
                connector.AddInParameterWithValue("@createdBy", userId);
                connector.AddInParameterWithValue("@createdDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public DataTable ListHolidays(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListHolidays", true))
            {
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }
        public DataTable ListHolidaysandWorkingDays(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListHolidaysAndWorkingdays", true))
            {
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable ListSaturdays(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListWorkingSaturdays", true))
            {
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }


        public string UpdateHoliday(HolidayModel holiday, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateHoliday", true))
            {
                connector.AddInParameterWithValue("@holidayId", holiday.holidayId);
                connector.AddInParameterWithValue("@date",holiday.date);
                connector.AddInParameterWithValue("@day", holiday.day);
                connector.AddInParameterWithValue("@name", holiday.name);
                connector.AddInParameterWithValue("@type", holiday.type);
                connector.AddInParameterWithValue("@hours", holiday.hours);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddInParameterWithValue("@updatedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }
        public string DeleteHoliday(int holidayId, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteHoliday", true))
            {
                connector.AddInParameterWithValue("@holidayId", holidayId);
                connector.AddInParameterWithValue("@deletedBy", userId);
                connector.AddInParameterWithValue("@deletedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public DataTable GetHolidayById(int holidayId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetHolidayById", true))
            {
                connector.AddInParameterWithValue("@holidayId", holidayId);
                dt = connector.GetDataTable();
            }
            return dt;
        }
        public DataTable GetholidayType(string date)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetHolidayType", true))
            {
                connector.AddInParameterWithValue("@date", date);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable Listtypes()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetHolidayTypes", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
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
