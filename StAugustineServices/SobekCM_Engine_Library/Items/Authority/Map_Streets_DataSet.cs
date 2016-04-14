﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=1.1.4322.2032.
// 

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

#endregion

#pragma warning disable 1591

namespace SobekCM.Engine_Library.Items.Authority
{
    [Serializable()]
    [DesignerCategory("code")]
    [DebuggerStepThrough()]
    [ToolboxItem(true)]
    public class Map_Streets_DataSet : DataSet {
        
        private StreetsDataTable tableStreets;
        
        public Map_Streets_DataSet() {
            InitClass();
            CollectionChangeEventHandler schemaChangedHandler = new CollectionChangeEventHandler(SchemaChanged);
            Tables.CollectionChanged += schemaChangedHandler;
            Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected Map_Streets_DataSet(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new StringReader(strSchema)));
                if ((ds.Tables["Streets"] != null)) {
                    Tables.Add(new StreetsDataTable(ds.Tables["Streets"]));
                }
                DataSetName = ds.DataSetName;
                Prefix = ds.Prefix;
                Namespace = ds.Namespace;
                Locale = ds.Locale;
                CaseSensitive = ds.CaseSensitive;
                EnforceConstraints = ds.EnforceConstraints;
                Merge(ds, false, MissingSchemaAction.Add);
                InitVars();
            }
            else {
                InitClass();
            }
            GetSerializationData(info, context);
            CollectionChangeEventHandler schemaChangedHandler = new CollectionChangeEventHandler(SchemaChanged);
            Tables.CollectionChanged += schemaChangedHandler;
            Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StreetsDataTable Streets {
            get {
                return tableStreets;
            }
        }
        
        public override DataSet Clone() {
            Map_Streets_DataSet cln = ((Map_Streets_DataSet)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["Streets"] != null)) {
                Tables.Add(new StreetsDataTable(ds.Tables["Streets"]));
            }
            DataSetName = ds.DataSetName;
            Prefix = ds.Prefix;
            Namespace = ds.Namespace;
            Locale = ds.Locale;
            CaseSensitive = ds.CaseSensitive;
            EnforceConstraints = ds.EnforceConstraints;
            Merge(ds, false, MissingSchemaAction.Add);
            InitVars();
        }
        
        protected override XmlSchema GetSchemaSerializable() {
            MemoryStream stream = new MemoryStream();
            WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        public void InitVars() {
            tableStreets = ((StreetsDataTable)(Tables["Streets"]));
            if ((tableStreets != null)) {
                tableStreets.InitVars();
            }
        }
        
        private void InitClass() {
            DataSetName = "Map_Streets_DataSet";
            Prefix = "";
            Namespace = "";
            Locale = new CultureInfo("en-US");
            CaseSensitive = false;
            EnforceConstraints = true;
            tableStreets = new StreetsDataTable();
            Tables.Add(tableStreets);
        }
        
        private bool ShouldSerializeStreets() {
            return false;
        }
        
        private void SchemaChanged(object sender, CollectionChangeEventArgs e) {
            if ((e.Action == CollectionChangeAction.Remove)) {
                InitVars();
            }
        }
        
        public delegate void StreetsRowChangeEventHandler(object sender, StreetsRowChangeEvent e);
        
        [DebuggerStepThrough()]
        public class StreetsDataTable : DataTable, IEnumerable {
            
            private DataColumn columnTemporalStreetID;
            
            private DataColumn columnStreetName;
            
            private DataColumn columnStartAddress;
            
            private DataColumn columnEndAddress;
            
            private DataColumn columnStreetDirection;
            
            private DataColumn columnStreetSide;
            
            private DataColumn columnSegmentDescription;
            
            private DataColumn columnPageSequence;
            
            private DataColumn columnPageName;
            
            public StreetsDataTable() : 
                    base("Streets") {
                InitClass();
            }
            
            public StreetsDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    Namespace = table.Namespace;
                }
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
                DisplayExpression = table.DisplayExpression;
            }
            
            [Browsable(false)]
            public int Count {
                get {
                    return Rows.Count;
                }
            }
            
            public DataColumn TemporalStreetIDColumn {
                get {
                    return columnTemporalStreetID;
                }
            }
            
            public DataColumn StreetNameColumn {
                get {
                    return columnStreetName;
                }
            }
            
            public DataColumn StartAddressColumn {
                get {
                    return columnStartAddress;
                }
            }
            
            public DataColumn EndAddressColumn {
                get {
                    return columnEndAddress;
                }
            }
            
            public DataColumn StreetDirectionColumn {
                get {
                    return columnStreetDirection;
                }
            }
            
            public DataColumn StreetSideColumn {
                get {
                    return columnStreetSide;
                }
            }
            
            public DataColumn SegmentDescriptionColumn {
                get {
                    return columnSegmentDescription;
                }
            }
            
            public DataColumn PageSequenceColumn {
                get {
                    return columnPageSequence;
                }
            }
            
            public DataColumn PageNameColumn {
                get {
                    return columnPageName;
                }
            }
            
            public StreetsRow this[int index] {
                get {
                    return ((StreetsRow)(Rows[index]));
                }
            }
            
            public event StreetsRowChangeEventHandler StreetsRowChanged;
            
            public event StreetsRowChangeEventHandler StreetsRowChanging;
            
            public event StreetsRowChangeEventHandler StreetsRowDeleted;
            
            public event StreetsRowChangeEventHandler StreetsRowDeleting;
            
            public void AddStreetsRow(StreetsRow row) {
                Rows.Add(row);
            }
            
            public StreetsRow AddStreetsRow(int TemporalStreetID, string StreetName, int StartAddress, int EndAddress, string StreetDirection, string StreetSide, string SegmentDescription, int PageSequence, string PageName) {
                StreetsRow rowStreetsRow = ((StreetsRow)(NewRow()));
                rowStreetsRow.ItemArray = new object[] {
                        TemporalStreetID,
                        StreetName,
                        StartAddress,
                        EndAddress,
                        StreetDirection,
                        StreetSide,
                        SegmentDescription,
                        PageSequence,
                        PageName};
                Rows.Add(rowStreetsRow);
                return rowStreetsRow;
            }
            
            public IEnumerator GetEnumerator() {
                return Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                StreetsDataTable cln = ((StreetsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new StreetsDataTable();
            }
            
            public void InitVars() {
                columnTemporalStreetID = Columns["TemporalStreetID"];
                columnStreetName = Columns["StreetName"];
                columnStartAddress = Columns["StartAddress"];
                columnEndAddress = Columns["EndAddress"];
                columnStreetDirection = Columns["StreetDirection"];
                columnStreetSide = Columns["StreetSide"];
                columnSegmentDescription = Columns["SegmentDescription"];
                columnPageSequence = Columns["PageSequence"];
                columnPageName = Columns["PageName"];
            }
            
            private void InitClass() {
                columnTemporalStreetID = new DataColumn("TemporalStreetID", typeof(int), null, MappingType.Element);
                Columns.Add(columnTemporalStreetID);
                columnStreetName = new DataColumn("StreetName", typeof(string), null, MappingType.Element);
                Columns.Add(columnStreetName);
                columnStartAddress = new DataColumn("StartAddress", typeof(int), null, MappingType.Element);
                Columns.Add(columnStartAddress);
                columnEndAddress = new DataColumn("EndAddress", typeof(int), null, MappingType.Element);
                Columns.Add(columnEndAddress);
                columnStreetDirection = new DataColumn("StreetDirection", typeof(string), null, MappingType.Element);
                Columns.Add(columnStreetDirection);
                columnStreetSide = new DataColumn("StreetSide", typeof(string), null, MappingType.Element);
                Columns.Add(columnStreetSide);
                columnSegmentDescription = new DataColumn("SegmentDescription", typeof(string), null, MappingType.Element);
                Columns.Add(columnSegmentDescription);
                columnPageSequence = new DataColumn("PageSequence", typeof(int), null, MappingType.Element);
                Columns.Add(columnPageSequence);
                columnPageName = new DataColumn("PageName", typeof(string), null, MappingType.Element);
                Columns.Add(columnPageName);
            }
            
            public StreetsRow NewStreetsRow() {
                return ((StreetsRow)(NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new StreetsRow(builder);
            }
            
            protected override Type GetRowType() {
                return typeof(StreetsRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((StreetsRowChanged != null)) {
                    StreetsRowChanged(this, new StreetsRowChangeEvent(((StreetsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((StreetsRowChanging != null)) {
                    StreetsRowChanging(this, new StreetsRowChangeEvent(((StreetsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((StreetsRowDeleted != null)) {
                    StreetsRowDeleted(this, new StreetsRowChangeEvent(((StreetsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((StreetsRowDeleting != null)) {
                    StreetsRowDeleting(this, new StreetsRowChangeEvent(((StreetsRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveStreetsRow(StreetsRow row) {
                Rows.Remove(row);
            }
        }
        
        [DebuggerStepThrough()]
        public class StreetsRow : DataRow {
            
            private StreetsDataTable tableStreets;
            
            public StreetsRow(DataRowBuilder rb) : 
                    base(rb) {
                tableStreets = ((StreetsDataTable)(Table));
            }
            
            public int TemporalStreetID {
                get {
                    try {
                        return ((int)(this[tableStreets.TemporalStreetIDColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.TemporalStreetIDColumn] = value;
                }
            }
            
            public string StreetName {
                get {
                    try {
                        return ((string)(this[tableStreets.StreetNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.StreetNameColumn] = value;
                }
            }
            
            public int StartAddress {
                get {
                    try {
                        return ((int)(this[tableStreets.StartAddressColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.StartAddressColumn] = value;
                }
            }
            
            public int EndAddress {
                get {
                    try {
                        return ((int)(this[tableStreets.EndAddressColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.EndAddressColumn] = value;
                }
            }
            
            public string StreetDirection {
                get {
                    try {
                        return ((string)(this[tableStreets.StreetDirectionColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.StreetDirectionColumn] = value;
                }
            }
            
            public string StreetSide {
                get {
                    try {
                        return ((string)(this[tableStreets.StreetSideColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.StreetSideColumn] = value;
                }
            }
            
            public string SegmentDescription {
                get {
                    try {
                        return ((string)(this[tableStreets.SegmentDescriptionColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.SegmentDescriptionColumn] = value;
                }
            }
            
            public int PageSequence {
                get {
                    try {
                        return ((int)(this[tableStreets.PageSequenceColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.PageSequenceColumn] = value;
                }
            }
            
            public string PageName {
                get {
                    try {
                        return ((string)(this[tableStreets.PageNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[tableStreets.PageNameColumn] = value;
                }
            }
            
            public bool IsTemporalStreetIDNull() {
                return IsNull(tableStreets.TemporalStreetIDColumn);
            }
            
            public void SetTemporalStreetIDNull() {
                this[tableStreets.TemporalStreetIDColumn] = Convert.DBNull;
            }
            
            public bool IsStreetNameNull() {
                return IsNull(tableStreets.StreetNameColumn);
            }
            
            public void SetStreetNameNull() {
                this[tableStreets.StreetNameColumn] = Convert.DBNull;
            }
            
            public bool IsStartAddressNull() {
                return IsNull(tableStreets.StartAddressColumn);
            }
            
            public void SetStartAddressNull() {
                this[tableStreets.StartAddressColumn] = Convert.DBNull;
            }
            
            public bool IsEndAddressNull() {
                return IsNull(tableStreets.EndAddressColumn);
            }
            
            public void SetEndAddressNull() {
                this[tableStreets.EndAddressColumn] = Convert.DBNull;
            }
            
            public bool IsStreetDirectionNull() {
                return IsNull(tableStreets.StreetDirectionColumn);
            }
            
            public void SetStreetDirectionNull() {
                this[tableStreets.StreetDirectionColumn] = Convert.DBNull;
            }
            
            public bool IsStreetSideNull() {
                return IsNull(tableStreets.StreetSideColumn);
            }
            
            public void SetStreetSideNull() {
                this[tableStreets.StreetSideColumn] = Convert.DBNull;
            }
            
            public bool IsSegmentDescriptionNull() {
                return IsNull(tableStreets.SegmentDescriptionColumn);
            }
            
            public void SetSegmentDescriptionNull() {
                this[tableStreets.SegmentDescriptionColumn] = Convert.DBNull;
            }
            
            public bool IsPageSequenceNull() {
                return IsNull(tableStreets.PageSequenceColumn);
            }
            
            public void SetPageSequenceNull() {
                this[tableStreets.PageSequenceColumn] = Convert.DBNull;
            }
            
            public bool IsPageNameNull() {
                return IsNull(tableStreets.PageNameColumn);
            }
            
            public void SetPageNameNull() {
                this[tableStreets.PageNameColumn] = Convert.DBNull;
            }
        }
        
        [DebuggerStepThrough()]
        public class StreetsRowChangeEvent : EventArgs {
            
            private StreetsRow eventRow;
            
            private DataRowAction eventAction;
            
            public StreetsRowChangeEvent(StreetsRow row, DataRowAction action) {
                eventRow = row;
                eventAction = action;
            }
            
            public StreetsRow Row {
                get {
                    return eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return eventAction;
                }
            }
        }
    }
}

#pragma warning restore 1591