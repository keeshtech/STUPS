﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 7/18/2014
 * Time: 11:37 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Tmx.Client
{
    using System;
    using System.Diagnostics;
	using System.Net;
    using PSTestLib.Helpers;
	using Spring.Http;
	using Spring.Rest.Client;
    using Tmx.Core.Types.Remoting;
	using Tmx.Interfaces.Exceptions;
	using Tmx.Interfaces.Server;
	using Tmx.Core;
	using Tmx.Interfaces.Remoting;
    
    /// <summary>
    /// Description of Registration.
    /// </summary>
    public class Registration
    {
        // volatile RestTemplate _restTemplate;
        RestTemplate _restTemplate;
        
        public Registration(RestRequestCreator requestCreator)
        {
            _restTemplate = requestCreator.GetRestTemplate();
            
            // 20141211
            // temporary
            // TODO: think about where to move it
            var tracingControl = new TracingControl("TmxClient_");
        }
        
        public virtual Guid SendRegistrationInfoAndGetClientId(string customClientString)
        {
            var registrationResponse = _restTemplate.PostForMessage<TestClient>(UrlList.TestClientRegistrationPoint_absPath, getNewTestClient(customClientString));
            
            if (HttpStatusCode.Created == registrationResponse.StatusCode)
                ClientSettings.Instance.CurrentClient = registrationResponse.Body;
            
            if (HttpStatusCode.Created == registrationResponse.StatusCode)
                return registrationResponse.Body.Id;
            
            // TODO: AOP
            Trace.TraceWarning("SendRegistrationInfoAndGetClientId(string customClientString)");
            Trace.TraceWarning("Failed to register a client. "+ registrationResponse.StatusCode);
            throw new Exception("Failed to register a client. "+ registrationResponse.StatusCode); // TODO: new type!
        }
        
        public virtual void UnregisterClient()
        {
            closeCurrentTaskIfAny();
            try {
                _restTemplate.Delete(UrlList.TestClients_Root + "/" + ClientSettings.Instance.ClientId);
                ClientSettings.Instance.ResetData();
            }
            catch (RestClientException eUnregisteringClient) {
                // TODO: AOP
                Trace.TraceError("UnregisterClient()");
                Trace.TraceError(eUnregisteringClient.Message);
                throw new ClientDeregistrationException("Failed to unregister the client. " + eUnregisteringClient.Message);
            }
        }
        
        ITestClient getNewTestClient(string customClientString)
        {
            return new TestClient { CustomString = customClientString };
        }
        
        void closeCurrentTaskIfAny()
        {
            var task = ClientSettings.Instance.CurrentTask;
            if (null == task) return;
            var taskUpdater = new TaskUpdater(new RestRequestCreator());
            task.TaskFinished = true;
            task.TaskStatus = TestTaskStatuses.CompletedSuccessfully;
            taskUpdater.UpdateTask(task);
        }
    }
}
