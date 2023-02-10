using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TaskDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public TaskDataAccess()
        {
        }

        ~TaskDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public DataTable GetTask(int pId,long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTask", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@projectId", pId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable ListTasks(int pId, long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListTasks", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@projectId", pId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }
        public DataTable GetTaskProjectWise(int projectId, long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTaskByProjectId", true))
            {
                //connector.AddInParameterWithValue("@userId", userId);
                connector.AddInParameterWithValue("@projectId", projectId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable GetTaskById(long id)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTaskById", true))
            {
                connector.AddInParameterWithValue("@taskId", id);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetTaskByProjectId(int projectId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetTaskByProjectId", true))
            {
                connector.AddInParameterWithValue("@projectId", projectId);
                dt = connector.GetDataTable();
            }
            return dt;
        }
        public string CreateTask(TaskModel task, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateTask", true))
            {
                connector.AddInParameterWithValue("@projectId", task.projectId);
                connector.AddInParameterWithValue("@employeeId", task.employeeId);
                connector.AddInParameterWithValue("@taskName", task.taskName);
                connector.AddInParameterWithValue("@status",task.status);
                connector.AddInParameterWithValue("@isDeleted", task.isDeleted);
                connector.AddInParameterWithValue("@createdBy", userId);
                connector.AddInParameterWithValue("@createdDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string UpdateTask(TaskModel task, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateTask", true))
            {
                connector.AddInParameterWithValue("@taskId", task.taskId);
                connector.AddInParameterWithValue("@projectId", task.projectId);
                connector.AddInParameterWithValue("@employeeId", task.employeeId);
                connector.AddInParameterWithValue("@taskName", task.taskName);
                connector.AddInParameterWithValue("@status", task.status);
                connector.AddInParameterWithValue("@isDeleted", task.isDeleted);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddInParameterWithValue("@updatedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string DeleteTask(long id, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteTask", true))
            {
                connector.AddInParameterWithValue("@taskId", id);
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
