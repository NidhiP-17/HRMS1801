using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ResponseModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is a Response class. We used this class to send response from service layer to presentation layer.
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    public class ResponseCollectionModel<T>
    {
        /// <summary>
        ///  Gets or sets property. We will use it to assign response of user request.
        /// </summary>
        public IEnumerable<T> Response { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether status. We will use it to assign status of the response of user request.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets property. We will use it to assign exception which occurred during the execution of process.
        /// </summary>
        public string Message { get; set; }
    }
}
