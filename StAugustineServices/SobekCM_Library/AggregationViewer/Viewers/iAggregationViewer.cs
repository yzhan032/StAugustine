﻿#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using SobekCM.Core.Aggregations;
using SobekCM.Core.Navigation;
using SobekCM.Library.HTML;
using SobekCM.Library.MainWriters;
using SobekCM.Tools;

#endregion

namespace SobekCM.Library.AggregationViewer.Viewers
{
    /// <summary> Enumeration specifies if the subaggregation selection panel is displayed for a particular collection viewer </summary>
    public enum Selection_Panel_Display_Enum : byte
    {
        /// <summary> This collection viewer never shows the subaggregation selection panel </summary>
        Never = 0,

        /// <summary> This collection viewer allows the user to select to display or hide the subaggregation selection panel </summary>
        Selectable = 1,

        /// <summary> This collection viewer always displays the subaggregation selection panel </summary>
        Always = 2
    }

    /// <summary> Interface which all collection viewers must implement </summary>
    /// <remarks> Collection viewers are used when displaying collection home pages, searches, browses, and information pages.<br /><br />
    /// During a valid html request, the following steps occur:
    /// <ul>
    /// <li>Application state is built/verified by the Application_State_Builder </li>
    /// <li>Request is analyzed by the QueryString_Analyzer and output as a <see cref="Navigation_Object"/> </li>
    /// <li>Main writer is created for rendering the output, in his case the <see cref="Html_MainWriter"/> </li>
    /// <li>The HTML writer will create the necessary subwriter.  For a collection-level request, an instance of the  <see cref="Aggregation_HtmlSubwriter"/> class is created. </li>
    /// <li>To display the requested collection view, the collection subwriter will create one or more collection viewers ( implementing this class )</li>
    /// </ul></remarks>
    public interface iAggregationViewer
    {
        /// <summary> Gets the type of collection view or search </summary>
        Item_Aggregation_Views_Searches_Enum Type { get; }

        /// <summary> Gets the reference to the javascript file to be included in the HTML </summary>
        string Search_Script_Reference { get; }

        /// <summary> Gets the reference to the javascript method to be called </summary>
        string Search_Script_Action { get; }

        /// <summary> Gets flag which indicates whether the secondary text requires controls </summary>
        bool Secondary_Text_Requires_Controls { get; }

        /// <summary> Gets flag which indicates whether this is an internal view, which may have a 
        /// slightly different design feel </summary>
        bool Is_Internal_View { get;  }

        /// <summary> Title for the page that displays this viewer, this is shown in the search box at the top of the page, just below the banner </summary>
        string Viewer_Title { get; }

        /// <summary> Gets the URL for the icon related to this aggregational viewer task </summary>
        string Viewer_Icon { get; }

        /// <summary> Gets the collection of special behaviors which this aggregation viewer
        /// requests from the main HTML subwriter. </summary>
        List<HtmlSubwriter_Behaviors_Enum> AggregationViewer_Behaviors { get; }

        /// <summary> Gets flag which indicates whether the selection panel should be displayed </summary>
        Selection_Panel_Display_Enum Selection_Panel_Display { get; }

        /// <summary> Add the HTML to be displayed in the search box </summary>
        /// <param name="Output"> Textwriter to write the HTML for this viewer </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering</param>
        void Add_Search_Box_HTML(TextWriter Output, Custom_Tracer Tracer);

        /// <summary> Add the HTML to be displayed below the search box </summary>
        /// <param name="Output"> Textwriter to write the HTML for this viewer </param>
        /// <param name="Tracer">Trace object keeps a list of each method executed and important milestones in rendering</param>
        void Add_Secondary_HTML(TextWriter Output, Custom_Tracer Tracer);

        /// <summary> Add the HTML and controls to the section below the search box </summary>
        /// <param name="MainPlaceHolder">Place holder to add html and controls to</param>
        /// <param name="Tracer">Trace object keeps a list of each method executed and important milestones in rendering</param>
        void Add_Secondary_Controls(PlaceHolder MainPlaceHolder, Custom_Tracer Tracer);
    }
}
