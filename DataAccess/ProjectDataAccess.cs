using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProjectDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public ProjectDataAccess()
        {
        }

        ~ProjectDataAccess()
        {
            this.Dispose(false);
        }
        #endregion


        public DataTable GetProject(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetProject", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }
        public DataTable GetProjectList(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetProjectList", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }
        public DataTable ListProjects(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListProjects", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable GetProjectById(long id)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetProjectById", true))
            {
                connector.AddInParameterWithValue("@projectId", id);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetTechnology()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTechnology", true))
            {
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public string CreateProject(ProjectModel project, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateProject", true))
            {
                connector.AddInParameterWithValue("@projectName", project.projectName);
                connector.AddInParameterWithValue("@technology", project.technologies[0]);
                connector.AddInParameterWithValue("@clientName", project.clientName);
                connector.AddInParameterWithValue("@status", "Pending");
                connector.AddInParameterWithValue("@startDate", project.startDate);
                connector.AddInParameterWithValue("@isDeleted", 0);
                connector.AddInParameterWithValue("@insertedBy", userId);
                connector.AddInParameterWithValue("@insertedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string UpdateProject(ProjectModel project, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateProject", true))
            {
                connector.AddInParameterWithValue("@projectId", project.projectId);
                connector.AddInParameterWithValue("@projectCode", project.projectCode);
                connector.AddInParameterWithValue("@projectName", project.projectName);
                if (project.technologies[0] != null)
                    connector.AddInParameterWithValue("@technology", project.technologies[0]);
                else
                    connector.AddInParameterWithValue("@technology", project.technology);
                connector.AddInParameterWithValue("@clientName", project.clientName);
                connector.AddInParameterWithValue("@status",project.status);
                connector.AddInParameterWithValue("@startDate", project.startDate);
                connector.AddInParameterWithValue("@isDeleted", 0);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddInParameterWithValue("@updatedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string DeleteProject(long id, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteProject", true))
            {
                connector.AddInParameterWithValue("@projectId", id);
                connector.AddInParameterWithValue("@deletedBy", userId);
                connector.AddInParameterWithValue("@deletedDate", DateTime.Now);
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
