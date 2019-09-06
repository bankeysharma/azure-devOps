using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using web_api_library.Extensions;

namespace web_api_library
{
    public class AzureContext: IDisposable
    {

        protected bool isInitialsed = false;

        protected Uri AzureProjectUrl { get; set; }

        protected VssCredentials Credentials { get; set; }

        protected AzureConnection Connection { get; set; }

        #region Initialisers
        private AzureContext Initialise(string defaultProjectName) {

            if (isInitialsed) return this;

            AzureHttpLogger loggerHandler = new AzureHttpLogger();

            VssHttpMessageHandler vssHandler = new VssHttpMessageHandler(
                Credentials,
                VssClientHttpRequestSettings.Default.Clone());

            this.Connection = new AzureConnection(
                this.AzureProjectUrl,
                vssHandler,
                new DelegatingHandler[] { loggerHandler });

            //* Establishing connection, which may fail also :)
            this.Connection.ConnectAsync().SyncResult();

            #region Configuring for project

            TeamProjectReference defaultProject = this.FindProject(defaultProjectName);

            if (defaultProject == null) throw new Exception(string.Format("Failed to initised - Missing project '{0}'", defaultProjectName));
            SetValue("$defautProject", defaultProject);
            SetValue("$allAssignToUsers", this.GetAllTeamMembers().Select(item => item.Identity.DisplayName).ToArray());
            
            #endregion

            isInitialsed = true;

            return this;
        }

        public AzureContext(Uri url, string defaultProjectName) : this(url, defaultProjectName, null) {
        }

        public AzureContext(Uri url, string defaultProjectName, VssCredentials credentials) {
            this.AzureProjectUrl = url;

            if (credentials == null)
            {
                this.Credentials = new VssClientCredentials();
            }
            else
            {
                this.Credentials = credentials;
            }

            Initialise(defaultProjectName);
        }

        #endregion

        #region HttpClients
        public WorkItemTrackingHttpClient GetHttpClientWorkItem() {
            return this.Connection.GetClient<WorkItemTrackingHttpClient>();
        }

        public ProjectHttpClient GetHttpClientProject() {
            return this.Connection.GetClient<ProjectHttpClient>();
        }

        public TeamHttpClient GetHttpClientTeam()
        {
            return this.Connection.GetClient<TeamHttpClient>();
        }
        #endregion

        #region Member Functions
        public TeamProjectReference GetDefaultProject() {
            return this.GetValue<TeamProjectReference>("$defautProject");
        }

        public string[] GetAssignToUsers() {
            return this.GetValue<string[]>("$allAssignToUsers");
        }

        #endregion

        #region Context Properties 
        protected Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();


        public T GetValue<T>(string name) {
            return (T)Properties[name];
        }

        public bool TryGetValue<T>(string name, out T result) {
            return Properties.TryGetValue<T>(name, out result);
        }

        public void SetValue<T>(string name, T value)
        {
            Properties[name] = value;
        }

        public void RemoveValue(string name)
        {
            Properties.Remove(name);
        }

        #endregion

        public void Log(String message)
        {
            this.Log(message, null);
        }

        public void Log(String message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.Connection.Dispose();
                this.Connection = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AzureWorkbench() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
