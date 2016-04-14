﻿#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using SobekCM.Engine_Library.IpRangeUtilities;

#endregion

namespace SobekCM.Engine_Library.Microservices
{
    /// <summary> Enumeration indicates the type of HTTP request expected for this microservice endpoint </summary>
    public enum Microservice_Endpoint_RequestType_Enum : byte
    {
        /// <summary> Endpoint is called with a HTTP DELETE call, which should remove a resource or link </summary>
        DELETE,

        /// <summary> Endpoint is called with a standard HTTP GET, perhaps including arguments in the command line </summary>
        GET,

        /// <summary> Endpoint is called with a HTTP POST call, including some object in the body of the post request and 
        /// usually adds a new resource </summary>
        POST,

        /// <summary> Endpoint is called with a HTTP PUT call, including some object in the body of the post request and 
        /// usually updates a new resource </summary>
        PUT,

        /// <summary> Invalid HTTP verb found </summary>
        ERROR
    }

    /// <summary> Enumeration indicates the type of protocol utilized by this endpoint</summary>
    public enum Microservice_Endpoint_Protocol_Enum : byte
    {
        /// <summary> Output of this endpoint is a binary stream </summary>
        BINARY,

        /// <summary> Nothing is output ( serialized, deserialized ) but the engine places the object
        /// within the (shared) cache </summary>
        CACHE,

        /// <summary> Output of this endpoint is standard JSON </summary>
        JSON,

        /// <summary> Output of this endpoint is JSON-P </summary>
        JSON_P,

        /// <summary> Output of this endpoint is Protocol Buffer octet-stream </summary>
        PROTOBUF,

        /// <summary> Serve the object, via SOAP </summary>
        SOAP,

        /// <summary> Output in XML format </summary>
        XML
    }

    /// <summary> Class defines an microservice endpoint within a collection of path or URI segments </summary>
    public class Microservice_Endpoint : Microservice_Path
    {
        public Microservice_VerbMapping GetMapping;

        public Microservice_VerbMapping DeleteMapping;

        public Microservice_VerbMapping PostMapping;

        public Microservice_VerbMapping PutMapping;


        /// <summary> Flag indicates if this path actually defines a single endpoint </summary>
        /// <remarks> This always returns 'TRUE' in this class </remarks>
        public override bool IsEndpoint
        {
            get { return true; }
        }

        /// <summary> Returns flag if a C# method is mapped to the provided HTTP verb/method </summary>
        /// <param name="Method"> Method, as upper-case string (i.e., 'DELETE', 'GET', 'POST', 'PUT', etc..) </param>
        /// <returns> TRUE if a mapping exists for the provided HTTP verb/method </returns>
        public bool VerbMappingExists(string Method)
        {
            switch (Method)
            {
                case "DELETE":
                case "delete":
                    return ((DeleteMapping != null) && (DeleteMapping.Enabled));

                case "GET":
                case "get":
                    return ((GetMapping != null) && (GetMapping.Enabled));

                case "POST":
                case "post":
                    return ((PostMapping != null) && (PostMapping.Enabled));

                case "PUT":
                case "put":
                    return ((PutMapping != null) && (PutMapping.Enabled));

                default:
                    return false;
            }
        }

        /// <summary> Gets a flag indicating if any verb mapping exists for this endpoint </summary>
        public bool HasVerbMapping
        {
            get { return ((GetMapping != null) || (PostMapping != null) || (PutMapping != null) || (DeleteMapping != null)); }
        }

        /// <summary> Get a single verb mapping, by HTTP verb/method  </summary>
        /// <param name="Method"> Method, as upper-case string (i.e., 'DELETE', 'GET', 'POST', 'PUT', etc..)</param>
        /// <returns> Matching verb mapping, or NULL </returns>
        public Microservice_VerbMapping this[string Method]
        {
            get
            {
                switch (Method)
                {
                    case "DELETE":
                    case "delete":
                        return DeleteMapping;

                    case "GET":
                    case "get":
                        return GetMapping;

                    case "POST":
                    case "post":
                        return PostMapping;

                    case "PUT":
                    case "put":
                        return PutMapping;

                    default:
                        return null;
                }
            }
        }

        /// <summary> Get the list of all verb mappings included in this endpoint </summary>
        public List<Microservice_VerbMapping> AllVerbMappings
        {
            get
            {
                // Most common case
                if ((GetMapping != null) && (PostMapping == null) && (PutMapping == null) && (DeleteMapping == null))
                    return new List<Microservice_VerbMapping> {GetMapping};

                // Build the list 
                List<Microservice_VerbMapping> returnValue = new List<Microservice_VerbMapping>();
                if (GetMapping != null) returnValue.Add(GetMapping);
                if (PostMapping != null) returnValue.Add(PostMapping);
                if (PutMapping != null) returnValue.Add(PutMapping);
                if (DeleteMapping != null) returnValue.Add(DeleteMapping);
                return returnValue;
            }
        }
    }
}