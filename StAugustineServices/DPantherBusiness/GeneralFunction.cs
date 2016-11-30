using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Xml;

using System.Collections;

using System.IO;

using System.Data;

using System.Web;

using System.Web.Script.Serialization;

using DPantherModels;

using DPantherModels.dpModels;

using System.Configuration;

using CsQuery;

using CsQuery.Web;

using System.Net;





namespace DPantherBusiness

{

    /// <summary>

    /// general functions used in dPanther

    /// </summary>

    public static class GeneralFunction

    {

        //public static string dirPath = "E:\\Projects\\DPanther\\DPanther_Server\\content";

        /// <summary>

        /// directory for digital collection repository

        /// </summary>

        public static string dirPath = dpDirectory.getDIRbyTypeAppcode("colRepo", "dpAdmin"); //"\\\\hexa\\content";



        public static string dirWebUrl = dpDirectory.getDIRWebbyTypeAppcode("colRepo", "dpAdmin");

        /// <summary>

        /// user name

        /// </summary>

        private static string username = ConfigurationManager.AppSettings["domainuser"];



        /// <summary>

        /// domain name

        /// </summary>

        private static string domain = ConfigurationManager.AppSettings["domainname"];



        /// <summary>

        /// user password

        /// </summary>

        private static string password = ConfigurationManager.AppSettings["domainuserpw"];



        /// <summary>

        /// method to get a list of image path from a mets file

        /// </summary>

        /// <param name="xmlPath">

        /// the sub path to the mets file: "\\FI\\11\\07\\07\\21\\00001\\FI11070721_00001.mets.xml"

        /// </param>

        /// <returns>

        /// return a ArrayList of images

        /// </returns>

        public static ArrayList getPath(string xmlPath)

        {

            ArrayList jpgPath = new ArrayList();

            //string fillPath = "E:\\Projects\\DPanther\\DPanther_Server\\content\\FI\\11\\07\\07\\21\\00001\\FI11070721_00001.mets.xml";

            string fillPath = "";//"\\\\192.168.3.77\\content\\FI\\11\\07\\07\\21\\00001\\FI11070721_00001.mets.xml";

            using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())

            {

                if (unc.NetUseWithCredentials(dirPath,

                                              username,

                                              domain,

                                              password))

                {

                    if (File.Exists(dirPath + xmlPath))

                    {

                        fillPath = dirPath + xmlPath;

                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.Load(fillPath);

                        XmlNodeList xnl = xmlDoc.GetElementsByTagName("METS:FLocat");

                        foreach (XmlNode node in xnl)

                        {

                            if (!jpgPath.Contains(node.Attributes["xlink:href"].Value))

                            {

                                jpgPath.Add(node.Attributes["xlink:href"].Value);

                            }

                        }

                    }

                }

            }



            return jpgPath;

        }



        //get treeview for file list Dec 13

        /// <summary>

        /// method to read structure map from a mets file and genereate the HTML treeview code

        /// </summary>

        /// <param name="xmlPath">

        /// the sub path to the mets file: "\\FI\\11\\07\\07\\21\\00001\\FI11070721_00001.mets.xml"

        /// </param>

        /// <returns>

        /// return html code for the structure map. 

        /// e.g. 

        /// <ul id="uldownloadTree">

        ///   <li>Granada Entrance. Coral Gables, Florida

        ///    <ul>

        ///       <li><a class="CitationSectionTitle2-link">Recto</a></li>

        ///       <li><a class="CitationSectionTitle2-link">Verso</a></li>

        ///     </ul>

        ///   </li>

        /// </ul> 

        /// </returns>

        public static string getPathNew(string xmlPath)

        {

            StringBuilder sb = new StringBuilder();



            ArrayList jpgPath = new ArrayList();

            //string fillPath = "E:\\Projects\\DPanther\\DPanther_Server\\content\\FI\\11\\07\\07\\21\\00001\\FI11070721_00001.mets.xml";

            string fillPath = ""; //"\\\\hexa\\content\\FI\\11\\07\\07\\21\\00001\\FI11070721_00001.mets.xml";



            using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())

            {

                if (unc.NetUseWithCredentials(dirPath,

                                              username,

                                              domain,

                                              password))

                {

                    if (File.Exists(dirPath + xmlPath))

                    {

                        fillPath = dirPath + xmlPath;

                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.Load(fillPath);                        

                        XmlNode xnode = xmlDoc.GetElementsByTagName("METS:structMap")[0];



                        XmlNodeList xnl_fileList = xmlDoc.GetElementsByTagName("METS:FLocat");                       

                       

                            XmlNodeList xnl = xnode.ChildNodes;

                            sb.Append("<ul id='uldownloadTree'>");

                            readxml(xnl, sb);

                            sb.Append("</ul>");

                        



                    }

                }

            }



            return sb.ToString();

        }

                

        /// <summary>

        /// Recursive method to generate the treeview structure of file list

        /// </summary>

        /// <param name="xmlnl">

        /// list of child Nodes of current xml node,

        /// e.g.    XmlNode xnode = xmlDoc.GetElementsByTagName("METS:structMap")[0];

        ///         XmlNodeList xmlnl = xnode.ChildNodes;

        /// </param>

        /// <param name="sb_">

        /// genereated HTML structure, this function will append the childnodes to the treeview structure 

        /// e.g. [input]

        ///      <ul id='uldownloadTree'>

        ///      [output]

        ///      <ul id="uldownloadTree">

        ///         <li>Granada Entrance. Coral Gables, Florida</li>  

        /// </param>

        private static void readxml(XmlNodeList xmlnl, StringBuilder sb_)

        {

            foreach (XmlNode xl in xmlnl)

            {

                if (xl.HasChildNodes)

                {

                    //sb_.Append("<li><a>" + xl.Attributes["LABEL"].Value + "</a></li>");

                    XmlElement xlelement = (XmlElement)xl;

                    if (xlelement.HasAttribute("LABEL"))

                    {

                        xlelement = (XmlElement)xl.FirstChild;

                        if (xlelement.HasAttribute("FILEID"))

                        {

                            sb_.Append("<li><a href='#dvFilePanel' class='CitationSectionTitle2-link'>" + xl.Attributes["LABEL"].Value + "</a><ul>");

                        }

                        else

                        {

                            sb_.Append("<li>" + xl.Attributes["LABEL"].Value + "<ul>");

                        }

                        readxml(xl.ChildNodes, sb_);

                        sb_.Append("</ul></li>");

                    }

                    else

                    {

                        xlelement = (XmlElement)xl.FirstChild;

                        if (xlelement.HasAttribute("FILEID"))

                        {

                            sb_.Append("<li><a href='#dvFilePanel' class='CitationSectionTitle2-link'>" + xlelement.Attributes["FILEID"].Value + "</a><ul>");

                            //sb_.Append("<li><a class='CitationSectionTitle2-link'>" + xl.ParentNode.Attributes["LABEL"].Value + "</a><ul>");

                        }

                        else

                        {

                            //sb_.Append("<li>" + xl.ParentNode.Attributes["LABEL"].Value + "<ul>");

                        }

                        readxml(xl.ChildNodes, sb_);

                        sb_.Append("</ul></li>");

                    }

                }

            }

        }



      

        public static string getSobekContentHTML(string htmlPath)

        {

            string strHTML = "";



            htmlPath = htmlPath.Replace("/","\\");

                FileInfo fi = new FileInfo(dirPath + htmlPath);

                if (!fi.Exists)

                {

                    htmlPath = htmlPath.Replace("sobek_files\\", "\\");

                    FileInfo fitemp = new FileInfo(dirPath + htmlPath);

                    if (!fitemp.Exists)

                    {

                        return strHTML;

                    }

                }

                

                    CQ dom = CQ.CreateFromUrl(dirWebUrl + htmlPath);

                    // dom.Document.GetElementById("sbkCiv_Citation").InnerHTML;



                    string[] strarrPath = htmlPath.Split('\\');

                    if (strarrPath.Length == 1)

                    {

                        strarrPath = htmlPath.Split('/');

                    }

                    string strFInum = strarrPath[strarrPath.Length - 1].Split('_')[0];

                    string strPurl = dpDirectory.getServiceURLbyTypeAppcode("services", "dpAdmin", "dpPurlService");

                    string strLink = strPurl + strFInum;

                    //update permanent Link to dPanther purl page

                    if (dom[".sbk_CivPERMANENT_LINK_Element"].Length > 1)

                    {

                        dom[".sbk_CivPERMANENT_LINK_Element a"].Attr("href", strLink);

                        dom[".sbk_CivPERMANENT_LINK_Element a"][0].InnerHTML = strLink;

                        dom[".sbk_CivPERMANENT_LINK_Element a"].Attr("onclick", "VTMain.pageEvent.SpotlightDownloadWindow('" + strLink + "')");

                    }

                    //if there is no permanent link, append permanent link nodes to external link

                    else if (dom[".sbk_CivEXTERNAL_LINK_Element"].Length > 1)

                    {

                        //open new window by click the ecternal link

                        dom[".sbk_CivEXTERNAL_LINK_Element a"].Attr("href", dom[".sbk_CivEXTERNAL_LINK_Element a"][0].InnerHTML);

                        dom[".sbk_CivEXTERNAL_LINK_Element a"].Attr("onclick", "VTMain.pageEvent.SpotlightDownloadWindow('" + dom[".sbk_CivEXTERNAL_LINK_Element a"][0].InnerHTML + "')");



                        CQ domPerLink = CQ.Create("<dt class='sbk_CivPERMANENT_LINK_Element' style='width:180px;'>Permanent Link: </dt>" +

                            "<dd class='sbk_CivPERMANENT_LINK_Element' style='margin-left:180px;'><a href='" + strLink + "' onclick='VTMain.pageEvent.SpotlightDownloadWindow(\""

                            + strLink + "\")'><span itemprop='url'>" + strLink +

                            "</span></a></dd>");

                        foreach (IDomObject domPL in domPerLink)

                        {

                            dom["dl"][0].AppendChild(domPL);

                        }

                    }

                    //change the img link

                    if (dom["#Sbk_CivThumbnailDiv"].Length > 0)

                    {

                        dom["#Sbk_CivThumbnailDiv a"].Attr("href", "#");

                    }

                    //add video frame instead of video link

                    int numofVideo = dom[".sbk_CivHOST_MATERIAL_Element a"].Length;

                    if (numofVideo > 0)

                    {

                        

                        foreach (CsQuery.Implementation.HtmlAnchorElement item in dom[".sbk_CivHOST_MATERIAL_Element a"])

                        {

                            if (item.Attributes["href"].Contains("http://libtube.fiu.edu"))

                            {

                                item.ParentNode.InnerHTML = "<label>" + item.InnerHTML + "</label></br>" + "<iframe width='330' height='270' src='" + item.Attributes["href"] + "' frameborder='0' scrolling='no'></iframe>";

                            }

 

                        }

                    }

                    strHTML = dom["#sbkCiv_Citation"].Html().Replace("&lt;", "<").Replace("&gt;", ">");



              

            

            return strHTML;

        }



        public static string getSobekContentHTMLfromURL(string url, string hostName)

        {

            string strHTML = "";



            using (WebClient client = new WebClient())

            {

                strHTML = client.DownloadString(url);

            }

                      



            CQ dom = CQ.CreateFromUrl(url);



            string[] strarrPath = url.Split('\\');

            if (strarrPath.Length == 1)

            {

                strarrPath = url.Split('/');

            }

            string strFInum = strarrPath[strarrPath.Length - 3];

            string strVid = strarrPath[strarrPath.Length - 2];

            //hostName="https://urldefense.proofpoint.com/v2/url?u=http-3A__ufdcimages.uflib.ufl.edu_&d=AwIGaQ&c=1QsCMERiq7JOmEnKpsSyjg&r=23qocRX19TwDIb8HQ8eFfQ&m=Qwkk7QhYkF_ofNYa3ae6KaACcbh4LGvxMWFSNyD_vKk&s=lL-G0kZCQxWQKA-_WQkNZBICPIn6qWcXlXVHGADTQsk&e= ";

            string strPurl = "";

            if (hostName.Contains("https://urldefense.proofpoint.com/v2/url?u=http-3A__&d=AwIGaQ&c=1QsCMERiq7JOmEnKpsSyjg&r=23qocRX19TwDIb8HQ8eFfQ&m=Qwkk7QhYkF_ofNYa3ae6KaACcbh4LGvxMWFSNyD_vKk&s=v7OxMms1kmGl6Ytc3JZZIB-JYKRbk63DTaAslINfxMw&e= "))

                strPurl =  dpDirectory.getServiceURLbyTypeAppcode("services", "dpAdmin", "dpPurlService");

            else

            { 

                strPurl = "https://urldefense.proofpoint.com/v2/url?u=http-3A__&d=AwIGaQ&c=1QsCMERiq7JOmEnKpsSyjg&r=23qocRX19TwDIb8HQ8eFfQ&m=Qwkk7QhYkF_ofNYa3ae6KaACcbh4LGvxMWFSNyD_vKk&s=v7OxMms1kmGl6Ytc3JZZIB-JYKRbk63DTaAslINfxMw&e= " + hostName + dpDirectory.getServiceURLbyTypeAppcode("services", "dpAdmin", "dpPurlService");

            }

            string strLink = strPurl + strFInum + "/" + strVid;

            //update permanent Link to dPanther purl page

            if (dom[".sbk_CivPERMANENT_LINK_Element"].Length > 1)

            {
                if (dom[".sbk_CivEXTERNAL_LINK_Element a"].Length > 0)
                {

                    dom[".sbk_CivPERMANENT_LINK_Element a"].Attr("href", strLink);

                    dom[".sbk_CivPERMANENT_LINK_Element a"][0].InnerHTML = strLink;

                    dom[".sbk_CivPERMANENT_LINK_Element a"].Attr("onclick", "VTMain.pageEvent.SpotlightDownloadWindow('" + strLink + "')");
                }

            }

            //if there is no permanent link, append permanent link nodes to external link

            else if (dom[".sbk_CivEXTERNAL_LINK_Element"].Length > 1)

            {

                //open new window by click the ecternal link
                if (dom[".sbk_CivEXTERNAL_LINK_Element a"].Length > 0)
                {
                    dom[".sbk_CivEXTERNAL_LINK_Element a"].Attr("href", dom[".sbk_CivEXTERNAL_LINK_Element a"][0].InnerHTML);

                    dom[".sbk_CivEXTERNAL_LINK_Element a"].Attr("onclick", "VTMain.pageEvent.SpotlightDownloadWindow('" + dom[".sbk_CivEXTERNAL_LINK_Element a"][0].InnerHTML + "')");
                }


                CQ domPerLink = CQ.Create("<dt class='sbk_CivPERMANENT_LINK_Element' style='width:180px;'>Permanent Link: </dt>" +

                    "<dd class='sbk_CivPERMANENT_LINK_Element' style='margin-left:180px;'><a href='" + strLink + "' onclick='VTMain.pageEvent.SpotlightDownloadWindow(\""

                    + strLink + "\")'><span itemprop='url'>" + strLink +

                    "</span></a></dd>");

                foreach (IDomObject domPL in domPerLink)

                {

                    dom["dl"][0].AppendChild(domPL);

                }

            }

            //change the img link

            if (dom["#Sbk_CivThumbnailDiv"].Length > 0)

            {

                dom["#Sbk_CivThumbnailDiv a"].Attr("href", "#");

            }

            //update all the url to a dPanther click event

            if (dom["a"].Length > 0)

            {

                for (int i = 0; i < dom["a"].Length; i++)

                {

                    CQ domlink = dom["a"][i].Cq();

                    string strPath = "";

                    if (domlink.Attr("href") != null)

                    {

                        strPath = domlink.Attr("href").ToString();

                    }

                    if (strPath.Contains("/"))

                    {

                        string[] s = strPath.Split('/');

                        if (s[s.Length - 1].StartsWith("?"))

                        {

                            string[] condition = s[s.Length - 1].Split('&');

                            string strValue = condition[0].Split('=')[1].Replace("\"", "").Replace("&quot;", "").Replace("&ldquo", "").Trim();

                            string strField = condition[1].Split('=')[1];



                            domlink.Attr("onClick", "DPanther.pageEvent.Para_term_search('" + strValue + "','" + strField + "')");

                            domlink.Attr("href", "#");

                            domlink.Attr("target", "_self");

                        }

                        if (s[s.Length - 1] == "TEST" || s[s.Length - 1] == "iFIU")

                        {

                            domlink.Attr("href", "#");

                        }

                    }

                }

            }

            //check if the top url existed, if so, remove it

            if (dom["#sbkCiv_ViewSelectRow"].Length > 0)

            {

                CQ domTab = dom["#sbkCiv_ViewSelectRow"][0].Cq();

                domTab.Attr("style", "display:none");

            }

            //add video frame instead of video link

            int numofVideo = dom[".sbk_CivHOST_MATERIAL_Element a"].Length;

            if (numofVideo > 0)

            {

               

                foreach (CsQuery.Implementation.HtmlAnchorElement item in dom[".sbk_CivHOST_MATERIAL_Element a"])

                {

                    if (item.Attributes["href"].Contains("http://libtube.fiu.edu"))

                    {

                        item.ParentNode.InnerHTML = "<label>" + item.InnerHTML + "</label></br>" + "<iframe width='330' height='270' src='" + item.Attributes["href"] + "' frameborder='0' scrolling='no'></iframe>";

                    }



                }

            }

            strHTML = dom["#sbkCiv_Citation"].Html().Replace("&lt;", "<").Replace("&gt;", ">");







            return strHTML;

        }



       



        /// <summary>

        /// serialize the server end array into a javascript array

        /// </summary>

        /// <param name="o">

        /// The array on server end

        /// </param>

        /// <returns>

        /// serialized javascript array

        /// </returns>

        public static string Serialize(object o)

        {

            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Serialize(o);

        }





        public static string getFIString(string strRaw)

        {

            string strRetuen = strRaw.Trim().Trim('|').Trim();

                    

            return strRetuen;

        }

    }

}

