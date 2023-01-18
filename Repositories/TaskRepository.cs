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
    public class TaskRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private TaskDataAccess instance;
        #region "=================== Constructor =============================="
        public TaskRepository()
        {
            this.instance = new TaskDataAccess();
        }

        ~TaskRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseCollectionModel<TaskModel> GetList(int pId,long userId, out string message)
        {
            var result = new ResponseCollectionModel<TaskModel>();
            var dt = instance.GetTask(pId, userId, out message);
            var lst = DataAccessUtility.ConvertToList<TaskModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<TaskModel> ListTasks(int pId,long userId, out string message)
        {
            var result = new ResponseCollectionModel<TaskModel>();
            var dt = instance.ListTasks(pId,userId, out message);
            var lst = DataAccessUtility.ConvertToList<TaskModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<TaskModel> GetTaskProjectWise(int projectId, long userId, out string message)
        {
            var result = new ResponseCollectionModel<TaskModel>();
            var dt = instance.GetTaskProjectWise(projectId, userId, out message);
            var lst = DataAccessUtility.ConvertToList<TaskModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<string> Create(TaskModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateTask(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Update(TaskModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.UpdateTask(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Delete(int id, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.DeleteTask(id, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<TaskModel> GetDetail(long id)
        {
            var result = new ResponseSingleModel<TaskModel>();
            var dt = instance.GetTaskById(id);
            var lst = DataAccessUtility.ConvertToList<TaskModel>(dt);
            TaskModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }
        public ResponseCollectionModel<TaskModel> GetTaskByProjectId(int projectId)
        {
            var result = new ResponseCollectionModel<TaskModel>();
            var dt = instance.GetTaskByProjectId(projectId);
            var lst = DataAccessUtility.ConvertToList<TaskModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "OK";
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
