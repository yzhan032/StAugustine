using System;
using System.ServiceModel.Activation;
using System.IO;
using System.Web;
using System.Web.Routing;
using System.Collections.Generic;
//using BIKE;
//using FacultyResidence;

namespace DPantherRESTServices
{
    /// <summary>
    /// global class of DPantherRESTServices
    /// </summary>
    public class Global : HttpApplication
    {
        // Values used by the SobekCM Library, as well as the SobekCM Web Application
       
        public static Dictionary<string, string> Collection_Aliases;        
        public static DateTime? Last_Refresh;
        

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            
            // Edit the base address of Service1 by replacing the "Service1" string below            
            RouteTable.Routes.Add(new ServiceRoute("DigitalObjectService", new WebServiceHostFactory(), typeof(DigitalObjectService)));          
          
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }
               
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
