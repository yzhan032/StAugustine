﻿#region Using directives

using System;
using System.Runtime.Serialization;
using EngineAgnosticLayerDbAccess;

#endregion

namespace SobekCM.Core.Configuration
{
	/// <summary> Contains all the information to connect to a single SobekCM instance's database </summary>
	/// <remarks> This was added to allow the SobekCM builder to process pending requests for multiple instances </remarks>
	[DataContract]
	public class Database_Instance_Configuration
	{
		/// <summary> Constructor for a new instance of the Database_Instance_Configuration class </summary>
		public Database_Instance_Configuration()
		{
			Database_Type = EalDbTypeEnum.MSSQL;
			Is_Active = true;
			Can_Abort = true;
			Name = String.Empty;
		}

		/// <summary> Database connection string includes all the information to connect to a single instance </summary>
		[DataMember]
		public string Connection_String { get; set;  }

		/// <summary> Database type </summary>
		[DataMember]
        public EalDbTypeEnum Database_Type { get; set;  }

		/// <summary> Flag indicates if this database instance is active for the builder </summary>
		/// <remarks> The configuration file for the builder may have multiple database settings to allow a single
		/// SobekCM builder to support multiple SobekCM instances.  </remarks>
		[DataMember]
		public bool Is_Active { get; set; }

		/// <summary> Flag indicates if this database instance can force an abort
		/// of the SobekCM system, or a NO BUILDING REQUESTED. </summary>
		/// <remarks> Any system can pause itself, but only certain ones may request
		/// a full abort through the web interface </remarks>
		[DataMember]
		public bool Can_Abort { get; set;  }

		/// <summary> Name for this database instance </summary>
		/// <remarks> This is only used by the SobekCM builder to be able to report the instance
		/// name, in the event that the database referenced is inaccessible. </remarks>
        [DataMember]
		public string Name { get; set;  }

        /// <summary> Database type (as a string) </summary>
        public string Database_Type_String
        {
            get
            {
				switch (Database_Type)
                {
                    case EalDbTypeEnum.MSSQL:
                        return "Microsoft SQL Server";
                       
                    case EalDbTypeEnum.PostgreSQL:
                        return "PostgreSQL";
                }
                return "Unrecognized";
            }
        }
	}
}
