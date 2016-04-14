﻿#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using SobekCM.Core.Configuration;
using SobekCM.Core.Navigation;
using SobekCM.Library.Database;
using SobekCM.Library.HTML;
using SobekCM.Library.MainWriters;
using SobekCM.Library.Settings;
using SobekCM.Library.UI;
using SobekCM.Tools;

#endregion

namespace SobekCM.Library.AdminViewer
{
    /// <summary> Class allows an authenticated system admin to view and edit the library-wide system settings in this library </summary>
    /// <remarks> This class extends the <see cref="abstract_AdminViewer"/> class.<br /><br />
    /// MySobek Viewers are used for registration and authentication with mySobek, as well as performing any task which requires
    /// authentication, such as online submittal, metadata editing, and system administrative tasks.<br /><br />
    /// During a valid html request, the following steps occur:
    /// <ul>
    /// <li>Application state is built/verified by the Application_State_Builder </li>
    /// <li>Request is analyzed by the QueryString_Analyzer and output as a <see cref="Navigation_Object"/>  </li>
    /// <li>Main writer is created for rendering the output, in his case the <see cref="Html_MainWriter"/> </li>
    /// <li>The HTML writer will create the necessary subwriter.  Since this action requires authentication, an instance of the  <see cref="MySobek_HtmlSubwriter"/> class is created. </li>
    /// <li>The mySobek subwriter creates an instance of this viewer to show the library-wide system settings in this digital library</li>
    /// </ul></remarks>
    class Settings_AdminViewer : abstract_AdminViewer
    {
        private readonly string actionMessage;
        private readonly StringBuilder errorBuilder;
        private readonly Dictionary<int, Setting_Info> idToSetting;
	    private readonly Dictionary<Setting_Info, int> settingToId;
        private bool isValid;
        private int settingCounter;
        private Dictionary<string, string> settings;
	    private List<string> standardSettingKeys; 
	    private readonly Dictionary<string, List<Setting_Info>> categorizedSettings;
	    private bool odd_row;
	    private readonly bool category_view;
        private bool limitedRightsMode;
        private List<string> omitSettingsKeys;


        /// <summary> Constructor for a new instance of the Thematic_Headings_AdminViewer class </summary>
        /// <param name="RequestSpecificValues"> All the necessary, non-global data specific to the current request </param>
        /// <remarks> Postback from handling saving the new settings is handled here in the constructor </remarks>
        public Settings_AdminViewer(RequestCache RequestSpecificValues) : base(RequestSpecificValues)
        {
            // If the RequestSpecificValues.Current_User cannot edit this, go back
            if (( RequestSpecificValues.Current_User == null ) || ((!RequestSpecificValues.Current_User.Is_System_Admin) && (!RequestSpecificValues.Current_User.Is_Portal_Admin)))
            {
                RequestSpecificValues.Current_Mode.Mode = Display_Mode_Enum.My_Sobek;
                RequestSpecificValues.Current_Mode.My_Sobek_Type = My_Sobek_Type_Enum.Home;
                UrlWriterHelper.Redirect(RequestSpecificValues.Current_Mode);
            }

            // Establish some default, starting values
            idToSetting = new Dictionary<int, Setting_Info>();
			settingToId = new Dictionary<Setting_Info, int>();
			categorizedSettings = new Dictionary<string, List<Setting_Info>>();
            settingCounter = 1;
            actionMessage = String.Empty;
			category_view = Convert.ToBoolean(RequestSpecificValues.Current_User.Get_Setting("Settings_AdminViewer:Category_View", "false"));

            standardSettingKeys = new List<string> { "Builder Last Message", "Builder Last Run Finished", "Builder Version", "Builder Operation Flag", "Spreadsheet Library License" };
            omitSettingsKeys = new List<string>();



            // Determine if full rights are provided here, or limited
            limitedRightsMode = true;
            if (((RequestSpecificValues.Current_User.Is_System_Admin) && (!UI_ApplicationCache_Gateway.Settings.isHosted)) ||
                (RequestSpecificValues.Current_User.Is_Host_Admin))
            {
                limitedRightsMode = false;
            }
            else
            {
                // Add some keys, which are stored in this portion of the database, 
                // but are not really setting values so shouldn't show here (or are hidden)
                omitSettingsKeys = new List<string> { "Builder Last Message", "Builder Last Run Finished", "Builder Version", "Builder Operation Flag", "Spreadsheet Library License", 
                "Archive DropBox", "Log Files Directory", "Log Files URL", "Main Builder Input Folder", "MarcXML Feed Location", "Application Server Network", "Caching Server", 
                "Document Solr Index URL", "Image Server Network", "JPEG2000 Server", "Log Files Directory", "Log Files URL", "Main Builder Input Folder", "MarcXML Feed Location", 
                "Page Solr Index URL", "SobekCM Web Server IP", "Static Pages Location", "Web In Process Submission Location", "Application Server URL",
                "Builder Operation Flag", "Builder Seconds Between Polls", "Email Default From Address", "Email Default From Name", "Email Method", "Email SMTP Port", "Email SMTP Server",
                "Files To Exclude From Downloads", "FDA Report DropBox", "Image Server URL", "JPEG2000 Server Type", "PostArchive Files To Delete", "PreArchive Files To Delete", 
                "System Base Abbreviation", "System Base Name", "System Base URL", "Upload File Types", "Upload Image Types"
            };
            }

            // Build the setting values
            build_setting_objects_for_display();
  
            // Is this a post-back requesting to save all this data?
            if (RequestSpecificValues.Current_Mode.isPostBack) 
            {
                NameValueCollection form = HttpContext.Current.Request.Form;

	            if (form["admin_settings_order"] == "category")
	            {
		            RequestSpecificValues.Current_User.Add_Setting("Settings_AdminViewer:Category_View", "true");
                    SobekCM_Database.Set_User_Setting(RequestSpecificValues.Current_User.UserID, "Settings_AdminViewer:Category_View", "true");
		            category_view = true;
	            }

				if (form["admin_settings_order"] == "alphabetical")
				{
					RequestSpecificValues.Current_User.Add_Setting("Settings_AdminViewer:Category_View", "false");
                    SobekCM_Database.Set_User_Setting(RequestSpecificValues.Current_User.UserID, "Settings_AdminViewer:Category_View", "false");
					category_view = false;
				}

	            string action_value = form["admin_settings_action"];
                if ((action_value == "save") && ( RequestSpecificValues.Current_User.Is_System_Admin ))
                {
                    // Populate all the new settings
                    Dictionary<string, string> newSettings = new Dictionary<string, string>();
					List<string> customSettingKeys = new List<string>();

                    int errors = 0;
                    // Step through each setting
                    foreach (int thisSettingCounter in idToSetting.Keys)
                    {
                        Setting_Info thisSetting = idToSetting[thisSettingCounter];
                        if (!thisSetting.ReadOnly)
                        {
                            string setting_key = thisSetting.Key;
                            if (form["setting" + thisSettingCounter] != null)
                            {
                                string setting_value = form["setting" + thisSettingCounter];
                                newSettings[setting_key] = setting_value;
                            }
                        }
                    }

					// Step through each possible custom setting
                    if (!limitedRightsMode)
                    {
                        for (int i = 0; i < (settings.Count - standardSettingKeys.Count) + 20; i++)
                        {
                            string key = form["admin_customkey_" + i];
                            if (!String.IsNullOrEmpty(key))
                            {
                                string value = form["admin_customvalue_" + i];
                                if (!String.IsNullOrEmpty(value))
                                {
                                    newSettings[key] = value;
                                    customSettingKeys.Add(key);
                                }
                            }
                        }
                    }

                    // Determine which settings need to be DELETED from the database
					foreach (KeyValuePair<string, string> customSetting in settings)
					{
						if ((!standardSettingKeys.Contains(customSetting.Key)) && ( !customSettingKeys.Contains(customSetting.Key)) && ( !omitSettingsKeys.Contains(customSetting.Key)))
						{
							SobekCM_Database.Delete_Setting(customSetting.Key);
						}
					}
					
					// Now, validate all this
                    errorBuilder = new StringBuilder();
                    isValid = true;
                    if (validate_update_entered_data(newSettings))
                    {
                        // Try to save each setting
                        errors += newSettings.Keys.Count(ThisKey => !SobekCM_Database.Set_Setting(ThisKey, newSettings[ThisKey]));

                        // Prepare the action message
                        if (errors > 0)
                            actionMessage = "Save completed, but with " + errors + " errors.";
                        else
                            actionMessage = "Settings saved";

                        // Assign this to be used by the system
                        UI_ApplicationCache_Gateway.ResetSettings();

                        // Also, reset the source for static files, as thatmay have changed
                        Static_Resources.Config_Read_Attempted = false;

                        // Get all the settings again 
                        build_setting_objects_for_display();
                    }
                    else
                    {
                        actionMessage = errorBuilder.ToString().Replace("\n", "<br />");
                        settings = newSettings;
                    }
                }
            }
        }

        private void build_setting_objects_for_display()
        {
            // Get the current settings from the database
            settings = SobekCM_Database.Get_Settings(RequestSpecificValues.Tracer);

            // Get the default URL and default system location
            string default_url = RequestSpecificValues.Current_Mode.Base_URL;
            string default_location = HttpContext.Current.Request.PhysicalApplicationPath;

            // Clear the lists
            // First, save this in the setting dictionary
            idToSetting.Clear();
            settingToId.Clear();
            settingCounter = 0;
            categorizedSettings.Clear();
            standardSettingKeys.Clear();

            // NOTE: This code is exactly the same as found in the SobekCM_Configuration tool.  That is why
            // this feels a little strange, to be loading information about all the settings, and then handling
            // the display portion late.  This allows the settings to be added or subtracted from rather easily 
            // in code and also keeps the exact same code chunk to keep the configuration tool and the web 
            // app both kept current simultaneously.
            string[] empty_options = new string[] { };
            string[] boolean_options = new[] { "true", "false" };
            string[] language_options = Web_Language_Enum_Converter.Language_Name_Array;



            if (!limitedRightsMode) Add_Setting_UI("Application Server Network", "Server Configuration", -1, empty_options, "Server share for the web application's network location.\n\nExample: '\\\\lib-sandbox\\Production\\'", false, default_location);
            Add_Setting_UI("Application Server URL", "Server Configuration", -1, empty_options, "Base URL which points to the web application.\n\nExamples: 'http://localhost/sobekcm/', 'http://ufdc.ufl.edu/', etc..", false, default_url).Set_ReadOnly(limitedRightsMode); 
            if (!limitedRightsMode ) Add_Setting_UI("Archive DropBox", "Archiving", -1, empty_options, "Network location for the archive drop box.  If this is set to a value, the builder/bulk loader will place a copy of the package in this folder for archiving purposes.  This folder is where any of your archiving processes should look for new packages.", false);
            if (!limitedRightsMode) Add_Setting_UI("Builder Add PageTurner ItemViewer", "Builder", 70, boolean_options, "Flag indicates if the page turner view should be added automatically to all items with four or more pages.", false);
            if (!limitedRightsMode) Add_Setting_UI("Builder IIS Logs Directory", "Builder", -1, empty_options, "IIS web log location (usually a network share) for the builder to read the logs and add the usage statistics to the database.", false );
            Add_Setting_UI("Builder Log Expiration in Days", "Builder", 200, new[] {"10", "30", "365", "99999"}, "Number of days the SobekCM Builder logs are retained.", false, "10").Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Builder Operation Flag", "Builder", 200, new[] { "STANDARD OPERATION", "PAUSE REQUESTED", "ABORT REQUESTED", "NO BUILDER REQUESTED" }, "Last flag set when the builder/bulk loader ran.", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Builder Seconds Between Polls", "Builder", 200, new[] { "15", "60", "300", "600" }, "Number of seconds the builder remains idle before checking for new incoming package again.", false, "60").Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Builder Send Usage Emails", "Builder", 70, boolean_options, "Flag indicates is usage emails should be sent automatically after the stats usage has been calculated and added to the database.", false);
            if (!limitedRightsMode) Add_Setting_UI("Caching Server", "Server Configuration", -1, empty_options, "URL for the AppFabric Cache host machine, if a caching server/cluster is in use in this system.", false);
            Add_Setting_UI("Can Remove Single Search Term", "General Appearance", 70, boolean_options, "When this is set to TRUE, users can remove a single search term from their current search.  Setting this to FALSE, makes the display slightly cleaner.", false);
            Add_Setting_UI("Can Submit Items Online", "Resource Files", 70, boolean_options, "Flag dictates if users can submit items online, or if this is disabled in this system.", false);
            Add_Setting_UI("Convert Office Files to PDF", "Resource Files", 70, boolean_options, "Flag dictates if users can submit items online, or if this is disabled in this system.", false, "false");
            Add_Setting_UI("Create MARC Feed By Default", "Interoperability", 70, boolean_options, "Flag indicates if the builder/bulk loader should create the MARC feed by default when operating in background mode.", false);
            if (!limitedRightsMode) Add_Config_Setting("Database Type", "Server Configuration", UI_ApplicationCache_Gateway.Settings.Database_Connections[0].Database_Type_String, "Type of database used to drive the SobekCM system.\n\nCurrently, only Microsoft SQL Server is allowed with plans to add PostgreSQL to the supported database system.\n\nThis value resides in the configuration on the web server.  See your database and web server administrator to change this value.");
            if (!limitedRightsMode) Add_Config_Setting("Database Connection String", "Server Configuration", UI_ApplicationCache_Gateway.Settings.Database_Connections[0].Connection_String, "Connection string used to connect to the SobekCM database\n\nThis value resides in the configuration file on the web server.  See your database and web server administrator to change this value.");
            Add_Setting_UI("Detailed User Permissions", "System Configuration", -1, boolean_options, "Flag indicates if more refined user permissions can be assigned, such as if a user can edit behaviors of an item in a collection vs. a more general flag that says a RequestSpecificValues.Current_User can make all changes to an item in a collection.", false);

            if (!limitedRightsMode)
            {
                Add_Setting_UI("Disable Standard User Logon Flag", "System Configuration", -1, boolean_options, "Flag indicates if non system administrators are temporarily barred from logging on.", false);
                Add_Setting_UI("Disable Standard User Logon Message", "System Configuration", -1, empty_options, "Message displayed if non syste administrators are temporarily barred from logging on.", false);
            }

            if (!limitedRightsMode) Add_Setting_UI("Document Solr Index URL", "Server Configuration", -1, empty_options, "URL for the document-level solr index.\n\nExample: 'http://localhost:8080/documents'", false);
            Add_Config_Setting("Error Emails", "Emails", UI_ApplicationCache_Gateway.Settings.System_Error_Email, "Email address for the web application to mail for any errors encountered while executing requests.\n\nThis account will be notified of inabilities to connect to servers, potential attacks, missing files, etc..\n\nIf the system is able to connect to the database, the 'System Error Email' address listed there, if there is one, will be used instead.\n\nUse a semi-colon betwen email addresses if multiple addresses are included.\n\nExample: 'person1@corp.edu;person2@corp2.edu'.\n\nThis value resides in the web.config file on the web server.  See your web server administrator to change this value.");
            Add_Config_Setting("Error HTML Page", "System Configuration", UI_ApplicationCache_Gateway.Settings.System_Error_URL, "Static page the user should be redirected towards if an unexpected exception occurs which cannot be handled by the web application.\n\nExample: 'http://ufdc.ufl.edu/error.html'.\n\nThis value resides in the web.config file on the web server.  See your web server administrator to change this value.");

            Add_Setting_UI("Email Default From Address", "Emails", -1, empty_options, "Email address that emails from this system should utilize", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Email Default From Name", "Emails", -1, empty_options, "Display name to associate with emails sent from this system (otherwise the instance/portal name will be used)", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Email Method", "Emails", -1, new string[] { "DATABASE MAIL", "SMTP DIRECT" }, "Indicated whether the database mail system or the SMTP direct email system should be utilizied", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Email SMTP Port", "Emails", 70, empty_options, "If direct SMTP email sending is used, the port to utilize.  This must be numeric.", false, "25").Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Email SMTP Server", "Emails", -1, empty_options, "If direct SMTP email sending is used, the server name to send emails to.", false).Set_ReadOnly(limitedRightsMode);

            Add_Setting_UI("Facets Collapsible", "General Appearance", 70, boolean_options, "Flag determines if the facets are collapsible like an accordian, or if they all start fully expanded.", false);
            Add_Setting_UI("FDA Report DropBox", "Florida SUS", -1, empty_options, "Location for the builder/bulk loader to look for incoming Florida Dark Archive XML reports to process and add to the history of digital resources.", true).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Files To Exclude From Downloads", "Resource Files", -1, empty_options, "Regular expressions used to exclude files from being added by default to the downloads of resources.\n\nExample: '((.*?)\\.(jpg|tif|jp2|jpx|bmp|jpeg|gif|png|txt|pro|mets|db|xml|bak|job)$|qc_error.html)'", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Help URL", "Help", -1, empty_options, "URL used for the main help pages about this system's basic functionality.\n\nExample (and default): 'http://ufdc.ufl.edu/'", false);
            Add_Setting_UI("Help Metadata URL", "Help", -1, empty_options, "URL used for the help pages when users request help on metadata elements during online submit and editing.\n\nExample (and default): 'http://ufdc.ufl.edu/'", false);
            if (!limitedRightsMode) Add_Setting_UI("Image Server Network", "Server Configuration", -1, empty_options, "Network location to the content for all of the digital resources (images, metadata, etc.).\n\nExample: 'C:\\inetpub\\wwwroot\\UFDC Web\\SobekCM\\content\\' or '\\\\ufdc-images\\content\\'", false, default_location + "content\\");
            Add_Setting_UI("Image Server URL", "Server Configuration", -1, empty_options, "URL which points to the digital resource images.\n\nExample: 'http://localhost/sobekcm/content/' or 'http://ufdcimages.uflib.ufl.edu/'", false, default_url + "content/").Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Include Partners On System Home", "System Configuration", 70, boolean_options, "This option controls whether a PARTNERS option appears on the main system home page, assuming there are multiple institutional aggregations.", false, "false");
            Add_Setting_UI("Include TreeView On System Home", "System Configuration", 70, boolean_options, "This option controls whether a TREE VIEW option appears on the main system home page which displays all the active aggregations hierarchically in a tree view.", false, "false");
            Add_Setting_UI("JPEG Height", "Resource Files", 60, empty_options, "Restriction on the size of the jpeg page images' height (in pixels) when generated automatically by the builder/bulk loader.\n\nDefault: '1000'", false);
            Add_Setting_UI("JPEG Width", "Resource Files", 60, empty_options, "Restriction on the size of the jpeg page images' width (in pixels) when generated automatically by the builder/bulk loader.\n\nDefault: '630'", false);
            if (!limitedRightsMode) Add_Setting_UI("JPEG2000 Server", "Server Configuration", -1, empty_options, "URL for the Aware JPEG2000 Server for displaying and zooming into JPEG2000 images.", false);
            Add_Setting_UI("JPEG2000 Server Type", "Server Configuration", -1, new[] { "Built-In IIPImage", "None" }, "Type of the JPEG2000 server found at the URL above.", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Kakadu JPEG2000 Create Command", "Resource Files", -1, empty_options, "Kakadu JPEG2000 script will override the specifications used when creating zoomable images.\n\nIf this is blank, the default specifications will be used which match those used by the National Digital Newspaper Program and University of Florida Digital Collections.", false);
            if (!limitedRightsMode) Add_Setting_UI("Log Files Directory", "Builder", -1, empty_options, "Network location for the share within which the builder/bulk loader logs should be copied to become web accessible.\n\nExample: '\\\\lib-sandbox\\Design\\extra\\logs\\'", false, default_location + "design\\extra\\logs\\");
            if (!limitedRightsMode) Add_Setting_UI("Log Files URL", "Builder", -1, empty_options, "URL for the builder/bulk loader logs files.\n\nExample: 'http://ufdc.ufl.edu/design/extra/logs/'", false, default_url + "design/exra/logs/");
            if (!limitedRightsMode) Add_Setting_UI("Main Builder Input Folder", "Builder", -1, empty_options, "This is the network location to the SobekCM Builder's main incoming folder.\n\nThis is used by the SMaRT tool when doing bulk imports from spreadsheet or MARC records.", false);
            Add_Setting_UI("Mango Union Search Base URL", "Florida SUS", -1, empty_options, "Florida SUS state-wide catalog base URL for determining the number of physical holdings which match a given search.\n\nExample: 'http://solrcits.fcla.edu/citsZ.jsp?type=search&base=uf'", true);
            Add_Setting_UI("Mango Union Search Text", "Florida SUS", -1, empty_options, "Text to display the number of hits found in the Florida SUS-wide catalog.\n\nUse the value '%1' in the string where the number of hits should be inserted.\n\nExample: '%1 matches found in the statewide catalog'", true);
            Add_Setting_UI("MARC Cataloging Source Code", "Interoperability", 60, empty_options, "Cataloging source code for the 040 field, ( for example 'FUG' for University of Florida )", false);
            Add_Setting_UI("MARC Location Code", "Interoperability", 60, empty_options, "Location code for the 852 |a - if none is given the system abbreviation will be used. Otherwise, the system abbreviation will be put in the 852 |b field.", false);
            Add_Setting_UI("MARC Reproduction Agency", "Interoperability", -1, empty_options, "Agency responsible for reproduction, or primary agency associated with the SobekCM instance ( for the added 533 |c field )\n\nThis 533 is not added for born digital items.", false);
            Add_Setting_UI("MARC Reproduction Place", "Interoperability", -1, empty_options, "Place of reproduction, or primary location associated with the SobekCM instance ( for the added 533 |b field ).\n\nThis 533 is not added for born digital items.", false);
            Add_Setting_UI("MARC XSLT File", "Interoperability", -1, empty_options, "XSLT file to use as a final transform, after the standard MarcXML file is written.\n\nThis only affects generated MarcXML ( for the feeds and OAI ) not the dispayed in-system MARC ( as of January 2015 ).  This file should appear in the config/users folder.", false);
            if (!limitedRightsMode) Add_Setting_UI("MarcXML Feed Location", "Interoperability", -1, empty_options, "Network location or share where any geneated MarcXML feed should be written.\n\nExample: '\\\\lib-sandbox\\Data\\'", false, default_location + "data\\");
            Add_Setting_UI("OCR Engine Command", "Resource Files", -1, empty_options, "If you wish to utilize an OCR engine in the builder/bulk loader, add the command-line call to the engine here.\n\nUse %1 as a place holder for the ingoing image file name and %2 as a placeholder for the output text file name.\n\nExample: 'C:\\OCR\\Engine.exe -in %1 -out %2'", false);
            if (!limitedRightsMode) Add_Setting_UI("Page Solr Index URL", "Server Configuration", -1, empty_options, "URL for the resource-level solr index used when searching for matching pages within a single document.\n\nExample: 'http://localhost:8080/pages'", false);
            Add_Setting_UI("PostArchive Files To Delete", "Archiving", -1, empty_options, "Regular expression indicates which files should be deleted AFTER being archived by the builder/bulk loader.\n\nExample: '(.*?)\\.(tif)'", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("PreArchive Files To Delete", "Archiving", -1, empty_options, "Regular expression indicates which files should be deleted BEFORE being archived by the builder/bulk loader.\n\nExample: '(.*?)\\.(QC.jpg)'", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Privacy Email Address", "Emails", -1, empty_options, "Email address which receives notification if personal information (such as Social Security Numbers) is potentially found while loading or post-processing an item.\n\nIf you are using multiple email addresses, seperate them with a semi-colon.\n\nExample: 'person1@corp.edu;person2@corp.edu'", false);
            Add_Setting_UI("Send Email On Added Aggregation", "System Configuration", 100, new[] { "Always", "Never" }, "Flag indicates when emails should be sent after new item aggregations are added through the web interface.", false, "Always");
            Add_Setting_UI("Show Florida SUS Settings", "Florida SUS", 70, boolean_options, "Some system settings are only applicable to institutions which are part of the Florida State University System.  Setting this value to TRUE will show these settings, while FALSE will suppress them.\n\nIf this value is changed, you willl need to save the settings for it to reload and reflect the change.", false, "false");
            if (!limitedRightsMode) Add_Setting_UI("SobekCM Web Server IP", "Server Configuration", 200, empty_options, "IP address for the web server running this web repository software.\n\nThis is used for setting restricted or dark material to only be available for the web server, which then acts as a proxy/web server to serve that content to authenticated users.", false);
            if (!limitedRightsMode) Add_Setting_UI("Static Pages Location", "Server Configuration", -1, empty_options, "Location where the static files are located for providing the full citation and text for indexing, either on the same server as the web application or as a network share.\n\nIt is recommended that these files be on the same server as the web server, rather than remote storage, to increase the speed in which requests from search engine indexers can be fulfilled.\n\nExample: 'C:\\inetpub\\wwwroot\\UFDC Web\\SobekCM\\data\\'.", false, default_location + "data\\");
            if (!limitedRightsMode)
            {
                string[] files = Directory.GetFiles(UI_ApplicationCache_Gateway.Settings.Base_Directory + "\\config\\default", "sobekcm_static_resources_*.config");
                List<string> config_file_names = new List<string>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                foreach (string thisFile in files)
                {
                    string name = (Path.GetFileName(thisFile)).ToLower().Replace("sobekcm_static_resources_", "").Replace(".config", "");
                    name = textInfo.ToTitleCase(name.Replace("_", " "));
                    if (name.IndexOf("Cdn") == 0)
                        name = name.Replace("Cdn", "CDN");
                    config_file_names.Add(name);
                }
                Add_Setting_UI("Static Resources Source", "System Configuration", 100, config_file_names.ToArray(), "Indicates the general source of all the static resources, such as javascript, system default stylesheets, images, and included libraries.\n\nUsing CDN will result in better performance, but can only be used when users will have access to the database.\n\nThis actually indicates which configuration file to read to determine the base location of the default resources.", false, "CDN");
            }
            Add_Setting_UI("Statistics Caching Enabled", "System Configuration", 70, boolean_options, "Flag indicates if the basic usage and item count information should be cached for up to 24 hours as static XML files written in the web server's temp directory.\n\nThis should be enabled if your library is quite large as it can take a fair amount of time to retrieve this information and these screens are made available for search engine index robots for indexing.", false);
            Add_Setting_UI("System Base Abbreviation", "System Configuration", 100, empty_options, "Base abbreviation to be used when the system refers to itself to the RequestSpecificValues.Current_User, such as the main tabs to take a RequestSpecificValues.Current_User to the home pages.\n\nThis abbreviation should be kept as short as possible.\n\nExamples: 'UFDC', 'dLOC', 'Sobek', etc..", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("System Base Name", "System Configuration", -1, empty_options, "Overall name of the system, to be used when creating MARC records and in several other locations.", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("System Base URL", "System Configuration", -1, empty_options, "Base URL which points to the web application.\n\nExamples: 'http://localhost/sobekcm/', 'http://ufdc.ufl.edu/', etc..", false, default_url).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("System Default Language", "System Configuration", 150, language_options, "Default system RequestSpecificValues.Current_User interface language.  If the RequestSpecificValues.Current_User's HTML request does not include a language supported by the interface or which does not include specific translations for a field, this default language is utilized.", false, "English");
            Add_Setting_UI("System Email", "Emails", -1, empty_options, "Default email address for the system, which is sent emails when users opt to contact the administrators.\n\nThis can be changed for individual aggregations but at least one email is required for the overall system.\n\nIf you are using multiple email addresses, seperate them with a semi-colon.\n\nExample: 'person1@corp.edu;person2@corp.edu'", false);
            Add_Setting_UI("System Error Email", "Emails", -1, empty_options, "Email address used when a critical system error occurs which may require investigation or correction.\n\nIf you are using multiple email addresses, seperate them with a semi-colon.\n\nExample: 'person1@corp.edu;person2@corp.edu'", false);
            Add_Setting_UI("Thumbnail Height", "Resource Files", 60, empty_options, "Restriction on the size of the page image thumbnails' height (in pixels) when generated automatically by the builder/bulk loader.\n\nDefault: '300'", false);
            Add_Setting_UI("Thumbnail Width", "Resource Files", 60, empty_options, "Restriction on the size of the page image thumbnails' width (in pixels) when generated automatically by the builder/bulk loader.\n\nDefault: '150'", false);
            Add_Setting_UI("Upload File Types", "Resource Files", -1, empty_options, "List of non-image extensions which are allowed to be uploaded into a digital resource.\n\nList should be the extensions, with the period, separated by commas.\n\nExample: .aif,.aifc,.aiff,.au,.avi,.bz2,.c,.c++,.css,.dbf,.ddl,...", false).Set_ReadOnly(limitedRightsMode);
            Add_Setting_UI("Upload Image Types", "Resource Files", -1, empty_options, "List of page image extensions which are allowed to be uploaded into a digital resource to display as page images.\n\nList should be the extensions, with the period, separated by commas.\n\nExample: .txt,.tif,.jpg,.jp2,.pro", false).Set_ReadOnly(limitedRightsMode);
            if (!limitedRightsMode) Add_Setting_UI("Web In Process Submission Location", "System Configuration", -1, empty_options, "Location where packages are built by users during online submissions and metadata updates.\n\nThis generally needs to be on the web server and have appropriate access for read/write.\n\nIf nothing is indicated in this field, the system will automatically use the 'mySobek\\InProcess' subfolder under the web application.", false, default_location + "mySobek\\InProcess\\");
            Add_Setting_UI("Web Output Caching Minutes", "System Configuration", 200, new[] { "0", "1", "2", "3", "5", "10", "15" }, "This setting controls how long the client's browser is instructed to cache the served web page.\n\nSetting this value higher removes the round-trip when requesting a recently requested page.  It also means that some changes may not be reflected until the refresh button is pressed.\n\nIn general, this setting is only applied to public-style pages, and not personalized pages, such as the bookshelf views.", false, "0");

        }

        /// <summary> Title for the page that displays this viewer, this is shown in the search box at the top of the page, just below the banner </summary>
        /// <value> This always returns the value 'System-Wide Settings' </value>
        public override string Web_Title
        {
            get { return "System-Wide Settings"; }
        }
        
        /// <summary> Gets the URL for the icon related to this administrative task </summary>
        public override string Viewer_Icon
        {
            get { return Static_Resources.Settings_Img; }
        }

        private bool validate_update_entered_data( Dictionary<string, string> NewSettings )
        {
            isValid = true;
            List<string> keys = NewSettings.Keys.ToList();
            foreach( string key in keys )
            {
                string value = NewSettings[key];

                switch (key)
                {
                    case "Application Server Network":
                        must_end_with(value, key, "\\", NewSettings);
                        break;

                    case "Application Server URL":
                        must_start_end_with(value, key, new string[] {"http://", "https://" }, "/", NewSettings);
                        break;

                    case "Document Solr Index URL":
                        must_start_end_with(value, key, new string[] { "http://", "https://" }, "/", NewSettings);
                        break;

                    case "Files To Exclude From Downloads":
                        must_be_valid_regular_expression(value, key);
                        break;

                    case "Image Server Network":
                        must_end_with(value, key, "\\", NewSettings);
                        break;

                    case "Image Server URL":
                        must_start_end_with(value, key, new string[] { "http://", "https://" }, "/", NewSettings);
                        break;

                    case "JPEG Height":
                        must_be_positive_number(value, key);
                        break;

                    case "JPEG Width":
                        must_be_positive_number(value, key);
                        break;

                    case "Log Files Directory":
                        must_end_with(value, key, "\\", NewSettings);
                        break;

                    case "Log Files URL":
                        must_start_end_with(value, key, new string[] { "http://", "https://" }, "/", NewSettings);
                        break;

                    case "Mango Union Search Base URL":
                        must_start_with(value, key, new string[] { "http://", "https://" }, NewSettings);
                        break;

                    case "Mango Union Search Text":
                        if (value.Trim().Length > 0)
                        {
                            if (value.IndexOf("%1") < 0)
                            {
                                isValid = false;
                                errorBuilder.AppendLine( key + ": Value must contain the '%1' string.  See help for more information.");
                            }
                        }
                        break;

                    case "MarcXML Feed Location":
                        must_end_with(value, key, "\\", NewSettings);
                        break;

                    case "OAI Resource Identifier Base":
                        must_end_with(value, key, ":", NewSettings);
                        break;

                    case "Page Solr Index URL":
                        must_start_end_with(value, key, new string[] { "http://", "https://" }, "/", NewSettings);
                        break;

                    case "PostArchive Files To Delete":
                        must_be_valid_regular_expression(value, key);
                        break;

                    case "PreArchive Files To Delete":
                        must_be_valid_regular_expression(value, key);
                        break;

                    case "Static Pages Location":
                        must_end_with(value, key, "\\", NewSettings);
                        break;

                    case "System Base Abbreviation":
                        if (value.Trim().Length == 0)
                        {
                            isValid = false;
                            errorBuilder.AppendLine( key + ": Field is required.");
                        }
                        break;

                    case "System Base URL":
                        if (value.Trim().Length == 0)
                        {
                            isValid = false;
                            errorBuilder.AppendLine( key + ": Field is required.");
                        }
                        else
                        {
                            must_start_end_with(value, key, new string[] { "http://", "https://" }, "/", NewSettings);
                        }
                        break;

                    case "Thumbnail Height":
                        must_be_positive_number(value, key);
                        break;

                    case "Thumbnail Width":
                        must_be_positive_number(value, key);
                        break;

                    case "Web In Process Submission Location":
                        must_end_with(value, key, "\\", NewSettings);
                        break;
                }
            }

            return isValid;
        }

        private Variable_Setting_Info Add_Setting_UI(string Key, string Category, int FixedInputSize, string[] Options, string HelpMessage, bool IsFloridaSusSetting)
        {
            // Create the settings option struct
            Variable_Setting_Info newSettingInfo = new Variable_Setting_Info(Key, FixedInputSize, Options, HelpMessage, IsFloridaSusSetting);

            // First, save this in the setting dictionary
            idToSetting[settingCounter] = newSettingInfo;
	        settingToId[newSettingInfo] = settingCounter;

			// Does this category already exist?
			if (!categorizedSettings.ContainsKey(Category))
			{
				List<Setting_Info> settingsList = new List<Setting_Info>();
				categorizedSettings[Category] = settingsList;
			}

			// Add this to the categorized settings
			categorizedSettings[Category].Add(newSettingInfo);

            // Increment in preparation for any next setting
            settingCounter++;

			// Add this key as a standard key, so they are left out of the custom settings portion
			standardSettingKeys.Add(Key);

            return newSettingInfo;
        }

        private Variable_Setting_Info Add_Setting_UI(string Key, string Category, int FixedInputSize, string[] Options, string HelpMessage, bool IsFloridaSusSetting, string DefaultValue)
        {
            // Create the settings option struct
            Variable_Setting_Info newSettingInfo = new Variable_Setting_Info(Key, FixedInputSize, Options, HelpMessage, IsFloridaSusSetting, DefaultValue );

            // First, save this in the setting dictionary
            idToSetting[settingCounter] = newSettingInfo;
			settingToId[newSettingInfo] = settingCounter;

			// Does this category already exist?
			if (!categorizedSettings.ContainsKey(Category))
			{
				List<Setting_Info> settingsList = new List<Setting_Info>();
				categorizedSettings[Category] = settingsList;
			}

			// Add this to the categorized settings
			categorizedSettings[Category].Add(newSettingInfo);

            // Increment in preparation for any next setting
            settingCounter++;

			// Add this key as a standard key, so they are left out of the custom settings portion
			standardSettingKeys.Add(Key);

            return newSettingInfo;
        }

        private void Add_Config_Setting(string Key, string Category, string Value, string HelpMessage)
        {
            // Create the settings option struct
            Constant_Setting_Info newSettingInfo = new Constant_Setting_Info(Key, Value, HelpMessage);

            // First, save this in the setting dictionary
            idToSetting[settingCounter] = newSettingInfo;
			settingToId[newSettingInfo] = settingCounter;

			// Does this category already exist?
			if (!categorizedSettings.ContainsKey(Category))
			{
				List<Setting_Info> settingsList = new List<Setting_Info>();
				categorizedSettings[Category] = settingsList;
			}

			// Add this to the categorized settings
			categorizedSettings[Category].Add(newSettingInfo);

            // Increment in preparation for any next setting
            settingCounter++;

			// Add this key as a standard key, so they are left out of the custom settings portion
			standardSettingKeys.Add(Key);
        }

        /// <summary> Add the HTML to be displayed in the main SobekCM viewer area </summary>
        /// <param name="Output"> Textwriter to write the HTML for this viewer</param>
        /// <param name="Tracer">Trace object keeps a list of each method executed and important milestones in rendering</param>
        /// <remarks> This class does nothing, since the themes list is added as controls, not HTML </remarks>
        public override void Write_HTML(TextWriter Output, Custom_Tracer Tracer)
        {
            Tracer.Add_Trace("Settings_AdminViewer.Write_HTML", "Do nothing");
        }

        /// <summary> This is an opportunity to write HTML directly into the main form, without
        /// using the pop-up html form architecture </summary>
        /// <param name="Output"> Textwriter to write the pop-up form HTML for this viewer </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering</param>
        /// <remarks> This text will appear within the ItemNavForm form tags </remarks>
		public override void Write_ItemNavForm_Closing(TextWriter Output, Custom_Tracer Tracer)
        {
            Tracer.Add_Trace("Settings_AdminViewer.Write_ItemNavForm_Closing", "Write the rest of the form ");


            // Add the hidden field
            Output.WriteLine("<!-- Hidden field is used for postbacks to indicate what to save and reset -->");
            Output.WriteLine("<input type=\"hidden\" id=\"admin_settings_action\" name=\"admin_settings_action\" value=\"\" />");
			if ( category_view )	
				Output.WriteLine("<input type=\"hidden\" id=\"admin_settings_order\" name=\"admin_settings_order\" value=\"category\" />");
			else
				Output.WriteLine("<input type=\"hidden\" id=\"admin_settings_order\" name=\"admin_settings_order\" value=\"alphabetical\" />");

            Output.WriteLine();

            Output.WriteLine("<!-- Settings_AdminViewer.Write_ItemNavForm_Closing -->");
            Output.WriteLine("<script src=\"" + Static_Resources.Sobekcm_Admin_Js + "\" type=\"text/javascript\"></script>");
			Output.WriteLine();

			Output.WriteLine("<div class=\"sbkAdm_HomeText\">");

			if (actionMessage.Length > 0)
			{
				Output.WriteLine("  <br />");
				Output.WriteLine("  <div id=\"sbkAdm_ActionMessage\">" + actionMessage + "</div>");
			}

            // Determine if the Florida SUS settings should be displayed
	        bool show_florida_sus = (settings.ContainsKey("Show Florida SUS Settings")) && (String.Compare(settings["Show Florida SUS Settings"], "true", StringComparison.OrdinalIgnoreCase) == 0);

	        Output.WriteLine("  <div id=\"sbkSeav_Explanation\">");
	        Output.WriteLine("    <p>This form allows a user to view and edit all the main system-wide settings which allow the SobekCM web application and assorted related applications to function correctly within each custom architecture and each institution.</p>");
            Output.WriteLine("    <p>For more information about these settings, <a href=\"" + UI_ApplicationCache_Gateway.Settings.Help_URL(RequestSpecificValues.Current_Mode.Base_URL) + "adminhelp/settings\" target=\"ADMIN_USER_HELP\" >click here to view the help page</a>.</p>");
	        Output.WriteLine("  </div>");
            Output.WriteLine();

			Output.WriteLine("  <div class=\"sbkSeav_ButtonsDiv\">");
	        if (RequestSpecificValues.Current_User.Is_System_Admin)
	        {

		        Output.WriteLine("    <button title=\"Do not apply changes\" class=\"sbkAdm_RoundButton\" onclick=\"window.location.href='" + RequestSpecificValues.Current_Mode.Base_URL + "my/admin'; return false;\"><img src=\"" + Static_Resources.Button_Previous_Arrow_Png + "\" class=\"sbkAdm_RoundButton_LeftImg\" alt=\"\" /> CANCEL</button> &nbsp; &nbsp; ");
		        Output.WriteLine("    <button title=\"Save changes to this existing web skin\" class=\"sbkAdm_RoundButton\" onclick=\"admin_settings_save(); return false;\">SAVE <img src=\"" + Static_Resources.Button_Next_Arrow_Png + "\" class=\"sbkAdm_RoundButton_RightImg\" alt=\"\" /></button>");
	        }
	        else
	        {
				Output.WriteLine("    <button class=\"sbkAdm_RoundButton\" onclick=\"window.location.href='" + RequestSpecificValues.Current_Mode.Base_URL + "my/admin'; return false;\"><img src=\"" + Static_Resources.Button_Previous_Arrow_Png + "\" class=\"sbkAdm_RoundButton_LeftImg\" alt=\"\" /> BACK</button> &nbsp; &nbsp; ");
	        }
			Output.WriteLine("  </div>");
			Output.WriteLine();


			// Add portal admin message
			string readonly_tag = String.Empty;
	        if (!RequestSpecificValues.Current_User.Is_System_Admin)
			{
				Output.WriteLine("<p>Portal Admins have rights to see these settings. System Admins can change these settings.</p>");
				readonly_tag = " readonly=\"readonly\"";
			}

	        Output.WriteLine("  <h2>Current System-Wide Settings</h2>");
			Output.WriteLine();


			Output.WriteLine("  <div id=\"tabContainer\" class=\"sbkSeav_TabContainer\">");
			Output.WriteLine("    <div class=\"tabs\">");
			Output.WriteLine("      <ul>");
			Output.WriteLine("        <li id=\"tabHeader_1\">Standard Settings</li>");
            if (!limitedRightsMode)
            {
                Output.WriteLine("        <li id=\"tabHeader_2\">Custom Settings</li>");
            }
            Output.WriteLine("      </ul>");
			Output.WriteLine("    </div>");
			Output.WriteLine("    <div class=\"tabscontent\">");
			Output.WriteLine("    	<div class=\"tabpage\" id=\"tabpage_1\">");
			Output.WriteLine("        <h3>Standard Settings</h3>");
			Output.WriteLine("        <p>These are the main standard settings utilized by all the components of a SobekCM digital repository and management system.</p>");

	        if (category_view)
	        {
				Output.WriteLine("        <table class=\"sbkSeav_SettingsTable\">");

		        // Write the data for each interface
		        odd_row = true;
	            bool first_row = !Add_Categorized_Settings("Archiving", "Archiving Settings", Output, true);
                if (Add_Categorized_Settings("Authentication", "Authentication Settings", Output, first_row)) first_row = false;
				if (Add_Categorized_Settings("Builder", "Builder Settings", Output, first_row)) first_row = false;
				if (Add_Categorized_Settings("Emails", "Email Settings", Output, first_row)) first_row = false;
	            if (show_florida_sus)
	            {
	                if (Add_Categorized_Settings("Florida SUS", "Florida SUS-Specific Settings", Output, first_row)) first_row = false;
	            }
	            if (Add_Categorized_Settings("Help", "Help Settings", Output, first_row)) first_row = false;
				if (Add_Categorized_Settings("General Appearance", "General Appearance Settings", Output, first_row)) first_row = false;
				if (Add_Categorized_Settings("Interoperability", "Interoperability Settings", Output, first_row)) first_row = false;
				if (Add_Categorized_Settings("Resource Files", "Resource File Settings", Output, first_row)) first_row = false;
                if (Add_Categorized_Settings("Server Configuration", "Server Configuration", Output, first_row)) first_row = false;
		        Add_Categorized_Settings("System Configuration", "System Configuration", Output, first_row);

				Output.WriteLine("        </table>");

	        }
	        else
	        {
		        Add_Alphabetical_Settings(Output, show_florida_sus);
	        }

			Output.WriteLine("    	</div>");

            if (!limitedRightsMode)
            {
                Output.WriteLine("    	<div class=\"tabpage\" id=\"tabpage_2\">");
                Output.WriteLine("        <h3>Custom Settings</h3>");
                Output.WriteLine("        <p>Additional custom settings can be entered or edited here.  These may be settings used by the core system but not exposed as standard settings, or these may be setting utilized by extensions added to the main core system.</p>");


                Output.WriteLine("        <table class=\"sbkSeav_CustomSettingsTable\">");
                Output.WriteLine("          <tr>");
                if (RequestSpecificValues.Current_User.Is_System_Admin)
                    Output.WriteLine("            <th style=\"width: 105px; text-align:center;\" >ACTION</th>");
                Output.WriteLine("            <th>SETTING KEY</th>");
                Output.WriteLine("            <th>SETTING VALUE</th>");
                Output.WriteLine("          </tr>");

                int counter = 1;

                // Add all the existing custom settings
                foreach (KeyValuePair<string, string> customSetting in settings)
                {
                    if (!standardSettingKeys.Contains(customSetting.Key))
                    {
                        Output.WriteLine("          <tr>");
                        if (RequestSpecificValues.Current_User.Is_System_Admin)
                            Output.WriteLine("            <td class=\"sbkAdm_ActionLink\" style=\"text-align:center\" >( <a title=\"Click to clear this setting\" href=\"" + RequestSpecificValues.Current_Mode.Base_URL + "l/technical/javascriptrequired\" onclick=\"return clear_setting('" + counter + "');\">clear</a> )</td>");
                        Output.WriteLine("            <td><input class=\"sbkSeav_CustomKey_Input sbkAdmin_Focusable\" name=\"admin_customkey_" + counter + "\" id=\"admin_customkey_" + counter + "\" type=\"text\" value=\"" + HttpUtility.HtmlEncode(customSetting.Key) + "\"" + readonly_tag + " /></td>");
                        Output.WriteLine("            <td><input class=\"sbkSeav_CustomValue_Input sbkAdmin_Focusable\" name=\"admin_customvalue_" + counter + "\" id=\"admin_customvalue_" + counter + "\" type=\"text\" value=\"" + HttpUtility.HtmlEncode(customSetting.Value) + "\"" + readonly_tag + " /></td>");
                        Output.WriteLine("          </tr>");

                        counter++;
                    }
                }

                // Now, add 10 blank lines at the bottom
                if (RequestSpecificValues.Current_User.Is_System_Admin)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Output.WriteLine("          <tr>");
                        Output.WriteLine("            <td class=\"sbkAdm_ActionLink\" style=\"text-align:center\" >( <a title=\"Click to clear this setting\" href=\"" + RequestSpecificValues.Current_Mode.Base_URL + "l/technical/javascriptrequired\" onclick=\"return clear_setting('" + counter + "');\">clear</a> )</td>");
                        Output.WriteLine("            <td><input class=\"sbkSeav_CustomKey_Input sbkAdmin_Focusable\" name=\"admin_customkey_" + counter + "\" id=\"admin_customkey_" + counter + "\" type=\"text\" value=\"\" /></td>");
                        Output.WriteLine("            <td><input class=\"sbkSeav_CustomValue_Input sbkAdmin_Focusable\" name=\"admin_customvalue_" + counter + "\" id=\"admin_customvalue_" + counter + "\" type=\"text\" value=\"\" /></td>");
                        Output.WriteLine("          </tr>");

                        counter++;
                    }
                }

                Output.WriteLine("        </table>");

                Output.WriteLine("    	</div>");
            }
            Output.WriteLine("   </div>");
			Output.WriteLine("  </div>");

			Output.WriteLine("<br />");

            Output.WriteLine();

			Output.WriteLine("  <div class=\"sbkSeav_ButtonsDiv\">");

	        if (RequestSpecificValues.Current_User.Is_System_Admin)
	        {
		        Output.WriteLine("    <button title=\"Do not apply changes\" class=\"sbkAdm_RoundButton\" onclick=\"window.location.href='" + RequestSpecificValues.Current_Mode.Base_URL + "my/admin'; return false;\"><img src=\"" + Static_Resources.Button_Previous_Arrow_Png + "\" class=\"sbkAdm_RoundButton_LeftImg\" alt=\"\" /> CANCEL</button> &nbsp; &nbsp; ");
		        Output.WriteLine("    <button title=\"Save changes to this existing web skin\" class=\"sbkAdm_RoundButton\" onclick=\"admin_settings_save(); return false;\">SAVE <img src=\"" + Static_Resources.Button_Next_Arrow_Png + "\" class=\"sbkAdm_RoundButton_RightImg\" alt=\"\" /></button>");
	        }
	        else
	        {
				Output.WriteLine("    <button class=\"sbkAdm_RoundButton\" onclick=\"window.location.href='" + RequestSpecificValues.Current_Mode.Base_URL + "my/admin'; return false;\"><img src=\"" + Static_Resources.Button_Previous_Arrow_Png + "\" class=\"sbkAdm_RoundButton_LeftImg\" alt=\"\" /> BACK</button> &nbsp; &nbsp; ");
	        }
	        Output.WriteLine("  </div>");
			Output.WriteLine();

			Output.WriteLine("<br />");
			Output.WriteLine("<br />");

			Output.WriteLine("  <script>");
			Output.WriteLine("    $(document).ready(function(){");
	        Output.WriteLine("      $(\"#tabContainer\").acidTabs();");
			Output.WriteLine("    });");
			Output.WriteLine("  </script>");

            Output.WriteLine("  <br />");
            Output.WriteLine("</div>");
            Output.WriteLine();
        }

		private void Add_Alphabetical_Settings(TextWriter Output, bool ShowFloridaSUS )
		{
			Output.WriteLine("        <table class=\"sbkSeav_SettingsTable\">");

			Output.WriteLine("          <tr>");
			Output.WriteLine("            <th>ALL SETTINGS</th>");
			Output.WriteLine("            <th style=\"text-align:right; padding-right: 40px;text-transform:none\">Order: <select id=\"reorder_select\" name=\"reorder_select\" onchange=\"settings_reorder(this);\"><option value=\"alphabetical\" selected=\"selected\">Alphabetical</option><option value=\"category\">Categories</option></select></th>");
			Output.WriteLine("          </tr>");
		//	Output.WriteLine("    <tr><td bgcolor=\"#e7e7e7\" colspan=\"2\"></td></tr>");

			// Write the data for each interface
			foreach (int setting_counter in idToSetting.Keys)
			{
				// Get the current setting information
				Setting_Info settingInfo = idToSetting[setting_counter];

				// If no Florida SUS-specific settings should be displayed, check to see
				// if this setting should be skipped
				if ((ShowFloridaSUS) || (!settingInfo.Is_Florida_SUS_Setting))
				{
					// Also, look for this value in the current settings
					string setting_value = String.Empty;
					if (!settingInfo.ReadOnly)
					{
						if (settings.ContainsKey(settingInfo.Key))
							setting_value = settings[settingInfo.Key];
						if ((setting_value.Length == 0) && (settingInfo.Default_Value.Length > 0))
							setting_value = settingInfo.Default_Value;
					}
					else
					{
					    Constant_Setting_Info settingCast = settingInfo as Constant_Setting_Info;
					    if (settingCast != null)
					    {
					        setting_value = settingCast.Value;
					    }
					    else
					    {
                            if (settings.ContainsKey(settingInfo.Key))
                                setting_value = settings[settingInfo.Key];
                            if ((setting_value.Length == 0) && (settingInfo.Default_Value.Length > 0))
                                setting_value = settingInfo.Default_Value;
					    }
					}

                    // Ensure setting is not NULL
                    if ( setting_value == null )
                        setting_value = String.Empty;

					// Build the row
					Output.WriteLine(odd_row
										 ? "          <tr class=\"sbkSeav_TableEvenRow\">"
										 : "          <tr class=\"sbkSeav_TableOddRow\">");
					Output.WriteLine("            <td class=\"sbkSeav_TableKeyCell\">" + settingInfo.Key + ":</td>");
					Output.WriteLine("            <td>");
					if ((!RequestSpecificValues.Current_User.Is_System_Admin) && (!settingInfo.ReadOnly) && ( setting_value.Trim().Length > 0 ))
						Output.WriteLine("              <table class=\"sbkSeav_InnerTableConstant\">");
					else
						Output.WriteLine("              <table class=\"sbkSeav_InnerTable\">");
					Output.WriteLine("                <tr style=\"vertical-align:middle;border:0;\">");
					Output.WriteLine("                  <td>");

					// Is this a variable setting, which the RequestSpecificValues.Current_User can change?
					if (( RequestSpecificValues.Current_User.Is_System_Admin ) && ( !settingInfo.ReadOnly))
					{
						Variable_Setting_Info varSettingInfo = (Variable_Setting_Info)settingInfo;
						if (varSettingInfo.Options.Length > 0)
						{
							Output.WriteLine("                    <select id=\"setting" + setting_counter + "\" name=\"setting" + setting_counter + "\" class=\"sbkSeav_select\" >");

							bool option_found = false;
							foreach (string thisValue in varSettingInfo.Options)
							{
								if (String.Compare(thisValue, setting_value, StringComparison.OrdinalIgnoreCase) == 0)
								{
									option_found = true;
									Output.WriteLine("                      <option selected=\"selected\">" + setting_value + "</option>");
								}
								else
								{
									Output.WriteLine("                      <option>" + thisValue + "</option>");
								}
							}

							if (!option_found)
							{
								Output.WriteLine("                      <option selected=\"selected\">" + setting_value + "</option>");
							}
							Output.WriteLine("                    </select>");
						}
						else
						{
							if ((varSettingInfo.Fixed_Input_Size > 0) && (varSettingInfo.Fixed_Input_Size < 360))
								Output.WriteLine("                    <input id=\"setting" + setting_counter + "\" name=\"setting" + setting_counter + "\" class=\"sbkSeav_input sbkAdmin_Focusable\" type=\"text\"  style=\"width: " + varSettingInfo.Fixed_Input_Size + "px;\" value=\"" + HttpUtility.HtmlEncode(setting_value) + "\" />");
							else
								Output.WriteLine("                    <input id=\"setting" + setting_counter + "\" name=\"setting" + setting_counter + "\" class=\"sbkSeav_input sbkAdmin_Focusable\" type=\"text\" value=\"" + HttpUtility.HtmlEncode(setting_value) + "\" />");
						}
					}
					else
					{
						if (setting_value.Trim().Length == 0)
						{
							Output.WriteLine("                    <em>( no value )</em>");
						}
						else
						{
							Output.WriteLine("                    " + HttpUtility.HtmlEncode(setting_value).Replace(",",", "));
						}
					}

					Output.WriteLine("                  </td>");
					Output.WriteLine("                  <td>");
					Output.WriteLine("                    <img  class=\"sbkSeav_HelpButton\" src=\"" + Static_Resources.Help_Button_Jpg + "\" onclick=\"alert('" + settingInfo.Help_Message.Replace("'", "").Replace("\\", "\\\\").Replace("\n", "\\n") + "');\"  title=\"" + settingInfo.Help_Message.Replace("\\", "\\\\").Replace("\n", "") + "\" />");
					Output.WriteLine("                  </td>");
					Output.WriteLine("                </tr>");
					Output.WriteLine("              </table>");
					Output.WriteLine("            </td>");
					Output.WriteLine("          </tr>");
				//	Output.WriteLine("    <tr><td bgcolor=\"#e7e7e7\" colspan=\"2\"></td></tr>");

					odd_row = !odd_row;
				}
			}

			Output.WriteLine("        </table>");
		}

		private bool Add_Categorized_Settings(string Category, string Title, TextWriter Output, bool FirstCategory )
		{
            if (!categorizedSettings.ContainsKey(Category))
                return false;

			Output.WriteLine("          <tr>");
			if (FirstCategory)
			{
				Output.WriteLine("            <th colspan=\"2\">" + Title + "</th>");
				Output.WriteLine("            <th style=\"text-align:right; padding-right: 40px;text-transform:none\">Order: <select id=\"reorder_select\" name=\"reorder_select\" onchange=\"settings_reorder(this);\"><option value=\"alphabetical\">Alphabetical</option><option value=\"category\" selected=\"selected\">Categories</option></select></th>");
			}
			else
			{
				Output.WriteLine("            <th colspan=\"3\">" + Title + "</th>");
			}

			Output.WriteLine("          </tr>");

			foreach (Setting_Info settingInfo in categorizedSettings[Category])
			{
				// Get the setting count
				int setting_counter = settingToId[settingInfo];

				// Also, look for this value in the current settings
				string setting_value = String.Empty;
                if (!settingInfo.ReadOnly)
				{
					if (settings.ContainsKey(settingInfo.Key))
						setting_value = settings[settingInfo.Key];
					if ((setting_value.Length == 0) && (settingInfo.Default_Value.Length > 0))
						setting_value = settingInfo.Default_Value;
				}
				else
				{
                    Constant_Setting_Info settingCast = settingInfo as Constant_Setting_Info;
                    if (settingCast != null)
                    {
                        setting_value = settingCast.Value;
                    }
                    else
                    {
                        if (settings.ContainsKey(settingInfo.Key))
                            setting_value = settings[settingInfo.Key];
                        if ((setting_value.Length == 0) && (settingInfo.Default_Value.Length > 0))
                            setting_value = settingInfo.Default_Value;
                    }
				}


				// Build the row
				Output.WriteLine(odd_row
									 ? "          <tr class=\"sbkSeav_TableEvenRow\">"
									 : "          <tr class=\"sbkSeav_TableOddRow\">");
				Output.WriteLine("            <td style=\"width: 40px\"></td>");
				Output.WriteLine("            <td class=\"sbkSeav_TableKeyCell\">" + settingInfo.Key + ":</td>");
				Output.WriteLine("            <td>");
				if ((!RequestSpecificValues.Current_User.Is_System_Admin) && (!settingInfo.ReadOnly) && (setting_value.Trim().Length > 0))
					Output.WriteLine("              <table class=\"sbkSeav_InnerTableConstant\">");
				else
					Output.WriteLine("              <table class=\"sbkSeav_InnerTable\">");
				Output.WriteLine("                <tr style=\"vertical-align:middle;border:0;\">");
				Output.WriteLine("                  <td>");

				// Is this a variable setting, which the RequestSpecificValues.Current_User can change?
                if ((RequestSpecificValues.Current_User.Is_System_Admin) && (!settingInfo.ReadOnly))
				{
					Variable_Setting_Info varSettingInfo = (Variable_Setting_Info) settingInfo;
					if (varSettingInfo.Options.Length > 0)
					{
						Output.WriteLine("                    <select id=\"setting" + setting_counter + "\" name=\"setting" + setting_counter + "\" class=\"sbkSeav_select\" >");

						bool option_found = false;
						foreach (string thisValue in varSettingInfo.Options)
						{
							if (String.Compare(thisValue, setting_value, StringComparison.OrdinalIgnoreCase) == 0)
							{
								option_found = true;
								Output.WriteLine("                      <option selected=\"selected\">" + setting_value + "</option>");
							}
							else
							{
								Output.WriteLine("                      <option>" + thisValue + "</option>");
							}
						}

						if (!option_found)
						{
							Output.WriteLine("                      <option selected=\"selected\">" + setting_value + "</option>");
						}
						Output.WriteLine("                    </select>");
					}
					else
					{
						if ((varSettingInfo.Fixed_Input_Size > 0) && (varSettingInfo.Fixed_Input_Size < 360))
							Output.WriteLine("                    <input id=\"setting" + setting_counter + "\" name=\"setting" + setting_counter + "\" class=\"sbkSeav_input sbkAdmin_Focusable\" type=\"text\"  style=\"width: " + varSettingInfo.Fixed_Input_Size + "px;\" value=\"" + HttpUtility.HtmlEncode(setting_value) + "\" />");
						else
							Output.WriteLine("                    <input id=\"setting" + setting_counter + "\" name=\"setting" + setting_counter + "\" class=\"sbkSeav_input sbkAdmin_Focusable\" type=\"text\" style=\"width: 360px;\" value=\"" + HttpUtility.HtmlEncode(setting_value) + "\" />");
					}
				}
				else
				{
					if (( setting_value == null ) || (setting_value.Trim().Length == 0))
					{
						Output.WriteLine("                    <em>( no value )</em>");
					}
					else
					{
						Output.WriteLine("                    " + HttpUtility.HtmlEncode(setting_value).Replace(",", ", "));
					}
				}

				Output.WriteLine("                  </td>");
				Output.WriteLine("                  <td>");
				Output.WriteLine("                    <img class=\"sbkSeav_HelpButton\" src=\"" + Static_Resources.Help_Button_Jpg + "\" onclick=\"alert('" + settingInfo.Help_Message.Replace("'", "").Replace("\\", "\\\\").Replace("\n", "\\n") + "');\" title=\"" + settingInfo.Help_Message.Replace("\\", "\\\\").Replace("\n", "") + "\" />");
				Output.WriteLine("                  </td>");
				Output.WriteLine("                </tr>");
				Output.WriteLine("              </table>");
				Output.WriteLine("            </td>");
				Output.WriteLine("          </tr>");

				odd_row = !odd_row;
			}

            return true;
		}

        private void must_be_positive_number(string Value, string Key)
        {
            bool appears_valid = false;
            int number;
            if ((Int32.TryParse(Value, out number)) && (number >= 0))
            {
                appears_valid = true;
            }

            if (!appears_valid)
            {
                isValid = false;
                errorBuilder.AppendLine(Key + ": Value must be a positive integer or zero.");
            }
        }

        private void must_be_valid_regular_expression(string Value, string Key)
        {
            if (Value.Length == 0)
                return;

            try
            {
                Regex.Match("any_old_file.tif", Value);
            }
            catch (ArgumentException)
            {
                isValid = false;
                errorBuilder.AppendLine(Key + ": Value must be empty or a valid regular expression.");
            }
        }

        private static void must_start_with(string Value, string Key, string StartsWith, Dictionary<string, string> NewSettings)
        {
            if (Value.Length == 0)
                return;

            if (!Value.StartsWith(StartsWith, StringComparison.InvariantCultureIgnoreCase))
            {
                NewSettings[Key] = StartsWith + Value;
            }
        }

        private static void must_start_with(string Value, string Key, string[] StartsWith, Dictionary<string, string> NewSettings)
        {
            if (Value.Length == 0)
                return;

            // Check for the start against all possible combinations
            bool missing_start = true;
            foreach (string possibleStart in StartsWith)
            {
                if (Value.StartsWith(possibleStart, StringComparison.InvariantCultureIgnoreCase))
                {
                    missing_start = false;
                    break;
                }
            }

            if (missing_start)
            {
                NewSettings[Key] = StartsWith[0] + Value;
            }
        }

        private static void must_end_with(string Value, string Key, string EndsWith, Dictionary<string, string> NewSettings)
        {
            if (Value.Length == 0)
                return;

            if (!Value.EndsWith(EndsWith, StringComparison.InvariantCultureIgnoreCase))
            {
                NewSettings[Key] = Value + EndsWith;
            }
        }

        private static void must_start_end_with(string Value, string Key, string StartsWith, string EndsWith, Dictionary<string, string> NewSettings)
        {
            if (Value.Length == 0)
                return;

            if ((!Value.StartsWith(StartsWith, StringComparison.InvariantCultureIgnoreCase)) || (!Value.EndsWith(EndsWith, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!Value.StartsWith(StartsWith, StringComparison.InvariantCultureIgnoreCase))
                    Value = StartsWith + Value;
                if (!Value.EndsWith(EndsWith, StringComparison.InvariantCultureIgnoreCase))
                    Value = Value + EndsWith;
                NewSettings[Key] = Value;
            }
        }

        private static void must_start_end_with(string Value, string Key, string[] StartsWith, string EndsWith, Dictionary<string, string> NewSettings)
        {
            if (Value.Length == 0)
                return;

            // Check for the start against all possible combinations
            bool missing_start = true;
            foreach (string possibleStart in StartsWith)
            {
                if (Value.StartsWith(possibleStart, StringComparison.InvariantCultureIgnoreCase))
                {
                    missing_start = false;
                    break;
                }
            }

            if ((missing_start) || (!Value.EndsWith(EndsWith, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (missing_start)
                    Value = StartsWith[0] + Value;
                if (!Value.EndsWith(EndsWith, StringComparison.InvariantCultureIgnoreCase))
                    Value = Value + EndsWith;
                NewSettings[Key] = Value;
            }
        }

        #region Nested type: Constant_Setting_Info

        /// <summary> Structure holds the basic information about a single constant setting read from the web configuration file</summary>
        protected class Constant_Setting_Info : Setting_Info
        {
            /// <summary> Value for this constant setting from the web configuration file </summary>
            public readonly string Value;

            /// <summary> Contstructor for a new object of this type </summary>
            /// <param name="Key"> Key for this constant setting from the web configuration file</param>
            /// <param name="Value">Value for this constant setting from the web configuration file </param>
            /// <param name="Help_Message"> Message to be displayed if RequestSpecificValues.Current_User requests help on this setting </param>
            public Constant_Setting_Info(string Key, string Value, string Help_Message) : base(Key, Help_Message, false, String.Empty )
            {
                this.Value = Value;
            }

            /// <summary> Flag indicates if this is a variable setting which will display, but not be editable </summary>
            /// <value> 'FALSE' is always returned for obejcts of this  class </value>
            public override bool ReadOnly
            {
                get { return true; }
            }
        }

        #endregion

        #region Nested type: Setting_Info

        /// <summary> Structure holds the basic information about a single setting which is held in the database and can be changed from the web application's admin screens </summary>
        protected abstract class Setting_Info
        {
            /// <summary> Message to be displayed if RequestSpecificValues.Current_User requests help on this setting </summary>
            public readonly string Help_Message;

            /// <summary> Key for this setting </summary>
            public readonly string Key;

            /// <summary> Default value to use if the key is not found in the currrent settings </summary>
            public readonly string Default_Value;

            /// <summary> Flag indicates if this is a Florida SUS setting, to be generally suppressed from displaying </summary>
            public readonly bool Is_Florida_SUS_Setting;

	        /// <summary> Contstructor for a new object of this type </summary>
	        /// <param name="Key"> Key for this setting </param>
	        /// <param name="Help_Message"> Message to be displayed if RequestSpecificValues.Current_User requests help on this setting </param>
	        /// <param name="Is_Florida_SUS_Setting"></param>
	        /// <param name="Default_Value"></param>
	        protected Setting_Info(string Key, string Help_Message, bool Is_Florida_SUS_Setting, string Default_Value)
            {
                this.Key = Key;
                this.Help_Message = Help_Message;
                this.Is_Florida_SUS_Setting = Is_Florida_SUS_Setting;
                this.Default_Value = Default_Value;
            }

            /// <summary> Flag indicates if this is a variable setting which will display, but not be editable </summary>
            public abstract bool ReadOnly { get; }
        }

        #endregion

        #region Nested type: Variable_Setting_Info

        /// <summary> Structure holds the basic information about a single setting which is held in the database and can be changed from the web application's admin screens </summary>
        protected class Variable_Setting_Info : Setting_Info
        {
            private bool readOnly;

            /// <summary> If the input should be a fixed size, size in pixels </summary>
            public readonly int Fixed_Input_Size;

            /// <summary> If there are only a limited number of options for this setting, this holds all possible options </summary>
            public readonly string[] Options;

	        /// <summary> Contstructor for a new object of this type </summary>
	        /// <param name="Key"> Key for this setting in the database (also the display label) </param>
	        /// <param name="Fixed_Input_Size"> If the input should be a fixed size, size in pixels </param>
	        /// <param name="Options"> If there are only a limited number of options for this setting, this holds all possible options </param>
	        /// <param name="Help_Message"> Message to be displayed if RequestSpecificValues.Current_User requests help on this setting </param>
	        /// <param name="Is_Florida_SUS_Setting"></param>
	        public Variable_Setting_Info(string Key, int Fixed_Input_Size, string[] Options, string Help_Message, bool Is_Florida_SUS_Setting ):base(Key, Help_Message, Is_Florida_SUS_Setting, String.Empty )
            {
                this.Fixed_Input_Size = Fixed_Input_Size;
                this.Options = Options;
                readOnly = false;
            }

	        /// <summary> Contstructor for a new object of this type </summary>
	        /// <param name="Key"> Key for this setting in the database (also the display label) </param>
	        /// <param name="Fixed_Input_Size"> If the input should be a fixed size, size in pixels </param>
	        /// <param name="Options"> If there are only a limited number of options for this setting, this holds all possible options </param>
	        /// <param name="Help_Message"> Message to be displayed if RequestSpecificValues.Current_User requests help on this setting </param>
	        /// <param name="Is_Florida_SUS_Setting"></param>
	        /// <param name="Default_Value">Default value to use if the key is not found in the currrent settings</param>
	        public Variable_Setting_Info(string Key, int Fixed_Input_Size, string[] Options, string Help_Message, bool Is_Florida_SUS_Setting, string Default_Value ) : base(Key, Help_Message, Is_Florida_SUS_Setting, Default_Value)
            {
                this.Fixed_Input_Size = Fixed_Input_Size;
                this.Options = Options;
                readOnly = false;
            }

            /// <summary> Flag indicates if this is a variable setting which will display, but not be editable </summary>
            public override bool ReadOnly { get { return readOnly;  } }

            public void Set_ReadOnly(bool NewValue)
            {
                readOnly = NewValue;
            }


        }

        #endregion
    }
}