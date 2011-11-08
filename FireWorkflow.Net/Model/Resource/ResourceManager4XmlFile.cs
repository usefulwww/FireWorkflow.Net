using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Model.Resource
{
    /// <summary>
    /// 资源管理器（1.0未用到）
    /// </summary>
    public class ResourceManager4XmlFile : IResourceManager 
    {
        List<Form> forms = null;
        List<Participant> participants = null;
        List<Application> applications = null;  

        #region IResourceManager Members

        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <returns></returns>
        public List<Application> getApplications()
        {
            return this.applications; 
        }

        /// <summary>
        /// Gets the participants.
        /// </summary>
        /// <returns></returns>
        public List<Participant> getParticipants()
        {
            return this.participants;
        }

        /// <summary>
        /// Gets the forms.
        /// </summary>
        /// <returns></returns>
        public List<Form> getForms()
        {
            return this.forms; 
        }

        #endregion
    }
}
