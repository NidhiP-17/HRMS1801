using DataAccess;
using Models.ResponseModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Repositories
{
    public class HolidayRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private HolidayDataAccess instance;
        #region "=================== Constructor =============================="
        public HolidayRepository()
        {
            this.instance = new HolidayDataAccess();
        }

        ~HolidayRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseSingleModel<string> Create(HolidayModel holiday, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateHoliday(holiday, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseCollectionModel<HolidayModel> ListHolidays(long userId, out string message)
        {
            var result = new ResponseCollectionModel<HolidayModel>();
            var dt = instance.ListHolidays(userId, out message);
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }

        public ResponseCollectionModel<HolidayModel> ListHolidaysandWorkingDays(long userId, out string message)
        {
            var result = new ResponseCollectionModel<HolidayModel>();
            var dt = instance.ListHolidaysandWorkingDays(userId, out message);
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<HolidayModel> ListSaturdays(long userId, out string message)
        {
            var result = new ResponseCollectionModel<HolidayModel>();
            var dt = instance.ListSaturdays(userId, out message);
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }


        public ResponseSingleModel<string> Update(HolidayModel holiday, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.UpdateHoliday(holiday, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Delete(int id, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.DeleteHoliday(id, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<HolidayModel> GetHolidayById(int holidayId)
        {
            var result = new ResponseSingleModel<HolidayModel>();
            var dt = instance.GetHolidayById(holidayId);
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            HolidayModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }

        public ResponseCollectionModel<HolidayModel> GetholidayType(string date)
        {
            var result = new ResponseCollectionModel<HolidayModel>();
            var dt = instance.GetholidayType(date);
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "OK";
            return result;
        }
        public ResponseCollectionModel<HolidayModel> Listtypes()
        {
            var result = new ResponseCollectionModel<HolidayModel>();
            var dt = instance.Listtypes();
            var lst = DataAccessUtility.ConvertToList<HolidayModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
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
