using DataAccess;
using Models;
using Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProjectRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private ProjectDataAccess instance;
        #region "=================== Constructor =============================="
        public ProjectRepository()
        {
            this.instance = new ProjectDataAccess();
        }

        ~ProjectRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseCollectionModel<ProjectModel> GetList(long userId,out string message)
        {
            var result = new ResponseCollectionModel<ProjectModel>();
            var dt = instance.GetProject(userId, out message);
            var lst = DataAccessUtility.ConvertToList<ProjectModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<ProjectModel> GetProjectList(long userId, out string message)
        {
            var result = new ResponseCollectionModel<ProjectModel>();
            var dt = instance.GetProjectList(userId, out message);
            var lst = DataAccessUtility.ConvertToList<ProjectModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<ProjectModel> ListProjects(long userId, out string message)
        {
            var result = new ResponseCollectionModel<ProjectModel>();
            var dt = instance.ListProjects(userId, out message);
            var lst = DataAccessUtility.ConvertToList<ProjectModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<string> Create(ProjectModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateProject(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Update(ProjectModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.UpdateProject(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Delete(int id, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.DeleteProject(id, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<ProjectModel> GetDetail(long id)
        {
            var result = new ResponseSingleModel<ProjectModel>();
            var dt = instance.GetProjectById(id);
            var lst = DataAccessUtility.ConvertToList<ProjectModel>(dt);
            ProjectModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }
        public ResponseCollectionModel<ProjectModel> GetTechnology()
        {
            var result = new ResponseCollectionModel<ProjectModel>();
            var dt = instance.GetTechnology();
            var lst = DataAccessUtility.ConvertToList<ProjectModel>(dt);
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
