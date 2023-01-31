using DataAccess;
using Models.ResponseModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TimesheetRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private TimesheetDataAccess instance;
        #region "=================== Constructor =============================="
        public TimesheetRepository()
        {
            this.instance = new TimesheetDataAccess();
        }

        ~TimesheetRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseCollectionModel<TimesheetModel> GetTimesheet(string projectId, string employeeId, string startDate, string endDate, long userId, out string message)
        {
            var result = new ResponseCollectionModel<TimesheetModel>();
            var dt = instance.GetTimesheet(projectId, employeeId , startDate, endDate, userId, out message);
            var lst = DataAccessUtility.ConvertToList<TimesheetModel>(dt);
            IEnumerable<TimesheetModel> tlist = lst;
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<string> CreateTimesheet(TimesheetModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateTimesheet(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> DeleteTimesheet(int id, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.DeleteTimesheet(id, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<TimesheetModel> GetTimesheetById(long id)
        {
            var result = new ResponseSingleModel<TimesheetModel>();
            var dt = instance.GetTimesheetById(id);
            var lst = DataAccessUtility.ConvertToList<TimesheetModel>(dt);
            TimesheetModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }

        public ResponseCollectionModel<HolidayModel> GetRemainingDays(int employeeId)
        {
            var result = new ResponseCollectionModel<HolidayModel>();
            var dt = instance.GetRemainingDays(employeeId);
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<string> GetJoiningDate(int id)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.GetJoiningDate(id, out message);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<string> GetLastDayOftimeSheet(int id)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.GetLastDayOftimeSheet(id, out message);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> GetTotalHoursOfDay(int employeeId, decimal hours, string? date)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.GetTotalHoursOfDay(employeeId,date, hours,out message);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> GetTotalHours(long employeeId ,DateTime? date)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.GetTotalHours(employeeId, date, out message);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<string> GetWorkingHours(string? date)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.GetWorkingHours(date, out message);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseCollectionModel<TimesheetModel> ListReasons(long userId, out string message)
        {
            var result = new ResponseCollectionModel<TimesheetModel>();
            var dt = instance.ListReasons(userId, out message);
            var lst = DataAccessUtility.ConvertToList<TimesheetModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }

       

        //public ResponseSingleModel<bool> CheckCategoryExists(string category)
        //{
        //    var result = new ResponseSingleModel<bool>();
        //    var message = string.Empty;
        //    result.Response = instance.CheckCategoryExists(category, out message);
        //    result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = message;
        //    return result;
        //}

        //public ResponseSingleModel<bool> CheckProjectInUse(int id)
        //{
        //    var result = new ResponseSingleModel<bool>();
        //    var message = string.Empty;
        //    result.Response = instance.CheckProjectInUse(id, out message);
        //    result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = message;
        //    return result;
        //}
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
