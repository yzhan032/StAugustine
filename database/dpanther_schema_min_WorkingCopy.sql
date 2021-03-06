/****** Object:  StoredProcedure [dbo].[DPanther_BrifDisplay]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<MM>
-- Create date: <07/01/2012>
-- Description:	<Display item info by citation>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_BrifDisplay]
(
@citation		nvarchar(500),
@Type			varchar(50),
@StartYear		varchar(8)=1800,
@EndYear		varchar(8)=2050

)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @str varchar(8000)
	if ISNULL(@citation,'')=''
	set @citation='""'
	--ISNULL(@Type,'""')
	
	
	set @str='SELECT b.[ItemID],[Author],b.[Spatial_KML],b.[PubDate],b.[Donor] as contributor
      ,[CreateDate],b.[VID],a.[Notes] as description     
      ,[AssocFilePath],[Link] as downloadLink,a.[Format],[Identifier]
      ,[Publication_Place] as publishPlace,[PubYear] as publishYear,a.[Other_Citation] as copyright
      ,[Subject_Keyword] as subjects,[FullCitation],[MainThumbnail] as thumbnail
      ,b.[Title],a.[Type] ,[MainLatitude],[MainLongitude],a.[BibID],b.[Publisher]
		FROM [dbo].[SobekCM_Item] b
		left join [dbo].[SobekCM_Metadata_Basic_Search_Table] a
		on a.ItemID=b.ItemID
		left join SobekCM_Item_Group c on b.GroupID=c.GroupID
	   where CONTAINS(a.FullCitation,'' '+@citation+''')'	
		
	if(@Type<>'')
	begin	
		if (@StartYear<>'')
		
		begin	
		    --cast(@StartYear as int)	
		     
			if (@EndYear<>'')
			begin
			set @str=@str+				
				' and CONTAINS(a.Type,'' '+@type+ ''')'+
				' and b.PubYear>='''+@StartYear+''' and b.PubYear<='''+@EndYear+''''
			end
			--end year=''
			else
			begin
			set @str=@str+				
				' and CONTAINS(a.Type,'' '+@type+ ''')'+
				' and b.PubYear>='''+@StartYear+''''
			end
		end		
	end
	else
	begin
		if (@StartYear<>'')
		
		begin
			if (@EndYear<>'')
			begin
			set @str=@str+' and b.PubYear>='''+@StartYear+''' and b.PubYear<='''+@EndYear+''''
			end
			--end year=''
			else
			begin
			set @str=@str+' and b.PubYear>='''+@StartYear+''''
			end
		end
	end
	print @str
	EXEC(@str)
END



GO
/****** Object:  StoredProcedure [dbo].[DPanther_DirectoryGetByAppType]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_DirectoryGetByAppType]	
(@dirType int,
@appID int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select [dirPath] from [dbo].[DP_Directory]
	where [dirType] = @dirType and [appID] = @appID
END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_DirectoryGetByAppTypeName]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_DirectoryGetByAppTypeName]	
(@applicationCode nvarchar(50),
@dirType varchar(150))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from dpView_appDirs 
	where applicationCode=@applicationCode AND dirType=@dirType
	
END




GO
/****** Object:  StoredProcedure [dbo].[DPanther_FacetAggregation]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		MM
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_FacetAggregation]
	
AS
BEGIN
SELECT a.name,a.code,l.AggregationID, Count(l.AggregationID) as Num FROM SobekCM_Item_Aggregation_Item_Link l 
left join SobekCM_Item_Aggregation a on l.aggregationID=a.aggregationid
where a.type='collection'
 GROUP BY a.name,a.code, l.AggregationID order by name
 
SELECT a.name,a.code,h.parentid as AggregationID, Count(l.AggregationID) as Num FROM SobekCM_Item_Aggregation_Item_Link l 
left join SobekCM_Item_Aggregation a on l.aggregationID=a.aggregationid
left join SobekCM_Item_Aggregation_Hierarchy h on a.aggregationID=h.childid
where a.type='subcollection'
 GROUP BY a.name,a.code, h.parentid order by name
 

END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_FacetSearch_Paged]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		MM
-- Create date: 07/27/2013
-- Description:	FacetSearch based on Sobek
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_FacetSearch_Paged]
	@term1 nvarchar(255),
	@field1 int=null,
	@link2 int,
	@term2 nvarchar(255)=null,
	@field2 int=null,
	@link3 int=0,
	@term3 nvarchar(255)=null,
	@field3 int=null,
	@link4 int=0,
	@term4 nvarchar(255)=null,
	@field4 int=null,
	@link5 int=0,
	@term5 nvarchar(255)=null,
	@field5 int=null,
	@link6 int=0,
	@term6 nvarchar(255)=null,
	@field6 int=null,
	@link7 int=0,
	@term7 nvarchar(255)=null,
	@field7 int=null,
	@link8 int=0,
	@term8 nvarchar(255)=null,
	@field8 int=null,
	@link9 int=0,
	@term9 nvarchar(255)=null,
	@field9 int=null,
	@link10 int=0,
	@term10 nvarchar(255)=null,
	@field10 int=null,
	@include_private bit,
	@aggregationcode varchar(20),	
	@pagesize int, 
	@pagenumber int,
	@sort int,
	@minpagelookahead int,
	@maxpagelookahead int,
	@lookahead_factor float,
	@include_facets bit,
	@facettype1 smallint,
	@facettype2 smallint,
	@facettype3 smallint,
	@facettype4 smallint,
	@facettype5 smallint,
	@facettype6 smallint=null,
	@facettype7 smallint=null,
	@facettype8 smallint=null,
	@startYear  nvarchar(4),
	@endYear	nvarchar(4),
	@pageOrder	bit='false',
	@total_items int output
	
	
AS
BEGIN
	-- No need to perform any locks here, especially given the possible
	-- length of this search
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	SET NOCOUNT ON;
	
	-- Field#'s indicate which metadata field (if any).  These are numbers from the 
	-- SobekCM_Metadata_Types table.  A field# of -1, means all fields are included.
	
	-- Link#'s indicate if this is an AND-joiner ( intersect ) or an OR-joiner ( union )
	-- 0 = AND, 1 = OR, 2 = AND NOT
	
	
	-- exec SobekCM_Metadata_Search 'haiti',1,1,'kesse',-1,0,'',0
	-- This searches for materials which have haiti in the title OR kesse anywhere
	
	-- Create the temporary table variables first
	-- Create the temporary table to hold all the item id's
	--setup startYear and endYear
	if(@startYear='1')
	begin
		set @startYear=' '
	end
	if(@endYear='1')
	begin
		set @endYear='ZZZZ'
	end
	
	create table #TEMPZERO ( ItemID int primary key );
	create table #TEMP_ITEMS ( ItemID int primary key, fk_TitleID int, Hit_Count int, SortDate bigint, PubYear nvarchar(100));
		    
	-- declare both the sql query and the parameter definitions
	declare @SQLQuery AS nvarchar(max);
	declare @rankselection AS nvarchar(1000);
    declare @ParamDefinition AS NVarchar(2000);
    
    -- Determine the aggregationid
	declare @aggregationid int;
	set @aggregationid = ( select ISNULL(AggregationID,-1) from SobekCM_Item_Aggregation where Code=@aggregationcode );
	
    -- Set value for filtering privates
	declare @lower_mask int;
	set @lower_mask = 0;
	if ( @include_private = 'true' )
	begin
		set @lower_mask = -256;
	end;
	    
    -- Start to build the main bulk of the query   
	set @SQLQuery = '';
    
    --if request for full citation
    if(@term1 = '1')
		begin
			-- Search the full citation then
			
			set @SQLQuery = @SQLQuery + ' 1=1 ';	
		end
    else
    begin
    -- Was a field listed?
		if (( @field1 > 0 ) and ( @field1 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field1_name varchar(100);
			set @field1_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field1 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field1_name + ', @innerterm1 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm1 )';	
		end
		 -- Start to build the query which will do ranking over the results which match this search
		set @rankselection = @term1;
	end;
            
   

	-- Add the second term, if there is one
	if (( LEN( ISNULL(@term2,'')) > 0 ) and (( @link2 = 0 ) or ( @link2 = 1 ) or ( @link2 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link2 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link2 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link2 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field2 > 0 ) and ( @field2 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field2_name varchar(100);
			set @field2_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field2 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field2_name + ', @innerterm2 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm2 )';	
		end;			
		
		-- Build the ranking query
		if ( @link2 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term2;	
		end
	end;    
	
	-- Add the third term, if there is one
	if (( LEN( ISNULL(@term3,'')) > 0 ) and (( @link3 = 0 ) or ( @link3 = 1 ) or ( @link3 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link3 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link3 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link3 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field3 > 0 ) and ( @field3 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field3_name varchar(100);
			set @field3_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field3 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field3_name + ', @innerterm3 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm3 )';	
		end;	
		
		-- Build the ranking query
		if ( @link3 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term3;		
		end
	end;   
	
	-- Add the fourth term, if there is one
	if (( LEN( ISNULL(@term4,'')) > 0 ) and (( @link4 = 0 ) or ( @link4 = 1 ) or ( @link4 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link4 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link4 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link4 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field4 > 0 ) and ( @field4 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field4_name varchar(100);
			set @field4_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field4 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field4_name + ', @innerterm4 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm4 )';	
		end;	
			
		-- Build the ranking query
		if ( @link4 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term4;		
		end
	end;
	
	-- Add the fifth term, if there is one
	if (( LEN( ISNULL(@term5,'')) > 0 ) and (( @link5 = 0 ) or ( @link5 = 1 ) or ( @link5 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link5 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link5 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link5 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field5 > 0 ) and ( @field5 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field5_name varchar(100);
			set @field5_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field5 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field5_name + ', @innerterm5 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm5 )';	
		end;
			
		-- Build the ranking query
		if ( @link5 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term5;		
		end
	end;
	
	-- Add the sixth term, if there is one
	if (( LEN( ISNULL(@term6,'')) > 0 ) and (( @link6 = 0 ) or ( @link6 = 1 ) or ( @link6 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link6 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link6 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link6 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field6 > 0 ) and ( @field6 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field6_name varchar(100);
			set @field6_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field6 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field6_name + ', @innerterm6 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm6 )';	
		end;
		
		-- Build the ranking query
		if ( @link6 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term6;		
		end
	end; 
	
	-- Add the seventh term, if there is one
	if (( LEN( ISNULL(@term7,'')) > 0 ) and (( @link7 = 0 ) or ( @link7 = 1 ) or ( @link7 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link7 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link7 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link7 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field7 > 0 ) and ( @field7 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field7_name varchar(100);
			set @field7_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field7 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field7_name + ', @innerterm7 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm7 )';	
		end;
		
		-- Build the ranking query
		if ( @link7 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term7;		
		end
	end;
	
	-- Add the eighth term, if there is one
	if (( LEN( ISNULL(@term8,'')) > 0 ) and (( @link8 = 0 ) or ( @link8 = 1 ) or ( @link8 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link8 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link8 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link8 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field8 > 0 ) and ( @field8 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field8_name varchar(100);
			set @field8_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field8 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field8_name + ', @innerterm8 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm8 )';	
		end;
		
		-- Build the ranking query
		if ( @link8 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term8;		
		end
	end;
	
	-- Add the ninth term, if there is one
	if (( LEN( ISNULL(@term9,'')) > 0 ) and (( @link9 = 0 ) or ( @link9 = 1 ) or ( @link9 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link9 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link9 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link9 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field9 > 0 ) and ( @field9 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field9_name varchar(100);
			set @field9_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field9 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field9_name + ', @innerterm9 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm9 )';	
		end;
		
		-- Build the ranking query
		if ( @link9 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term9;		
		end
	end;
	
	-- Add the tenth term, if there is one
	if (( LEN( ISNULL(@term10,'')) > 0 ) and (( @link10 = 0 ) or ( @link10 = 1 ) or ( @link10 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link10 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link10 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link10 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field10 > 0 ) and ( @field10 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field10_name varchar(100);
			set @field10_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field10 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field10_name + ', @innerterm10 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm10 )';	
		end;
		
		-- Build the ranking query
		if ( @link10 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term10;		
		end		
	end;
	
	-- Add the recompile option
	--set @SQLQuery = @SQLQuery + ' option (RECOMPILE)';

    -- Add the first term and start to build the query which will provide the items which match the search
    declare @mainquery nvarchar(max);
    set @mainquery = 'select L.Itemid from SobekCM_Metadata_Basic_Search_Table AS L';
    
    -- Do we need to limit by aggregation id as well?
    if ( @aggregationid > 0 )
    begin
		set @mainquery = @mainquery + '  join SobekCM_Item_Aggregation_Item_Link AS A ON ( A.ItemID = L.ItemID ) and ( A.AggregationID = ' + CAST( @aggregationid as varchar(5) ) + ')';   
    end    
    
    -- Add the full text search portion here
    set @mainquery = @mainquery + ' where ' + @SQLQuery;
	
	-- Set the parameter definition
	set @ParamDefinition = ' @innerterm1 nvarchar(255), @innerterm2 nvarchar(255), @innerterm3 nvarchar(255), @innerterm4 nvarchar(255), @innerterm5 nvarchar(255), @innerterm6 nvarchar(255), @innerterm7 nvarchar(255), @innerterm8 nvarchar(255), @innerterm9 nvarchar(255), @innerterm10 nvarchar(255)';
		
	-- Execute this stored procedure
	insert #TEMPZERO execute sp_Executesql @mainquery, @ParamDefinition, @term1, @term2, @term3, @term4, @term5, @term6, @term7, @term8, @term9, @term10;
			
	-- Perform ranking against the items and insert into another temporary table 
	-- with all the possible data elements needed for applying the user's sort
	if(@term1 = '1')
	Begin
	insert into #TEMP_ITEMS ( ItemID, fk_TitleID, SortDate, Hit_Count, PubYear)
		select I.ItemID, I.GroupID, SortDate=isnull( I.SortDate,-1), 0, i.PubDate
		from SobekCM_Item I, #TEMPZERO AS T1 join
		  SobekCM_Metadata_Basic_Search_Table AS KEY_TBL on KEY_TBL.ItemID = T1.ItemID
		where ( T1.ItemID = I.ItemID )
		  and ( I.Deleted = 'false' )
		  and ( I.IP_Restriction_Mask='0' )	
		  and ( I.IncludeInAll = 'true' )
		  and LEFT([PubDate], 4)>=@startYear
		  and LEFT([PubDate], 4)<=@endYear; 	
      end
      else
      begin
		  insert into #TEMP_ITEMS ( ItemID, fk_TitleID, SortDate, Hit_Count, PubYear)
			select I.ItemID, I.GroupID, SortDate=isnull( I.SortDate,-1), isnull(KEY_TBL.RANK, 0 ), i.PubDate
			from SobekCM_Item I, #TEMPZERO AS T1 join
			  CONTAINSTABLE(SobekCM_Metadata_Basic_Search_Table, FullCitation, @rankselection ) AS KEY_TBL on KEY_TBL.[KEY] = T1.ItemID
			where ( T1.ItemID = I.ItemID )
			  and ( I.Deleted = 'false' )
			  and ( I.IP_Restriction_Mask ='0' )	
			  and ( I.IncludeInAll = 'true' )
			  and LEFT([PubDate], 4)>=@startYear
			  and LEFT([PubDate], 4)<=@endYear; 
      end   

	-- Determine the start and end rows
	declare @rowstart int;
	declare @rowend int;
	set @rowstart = (@pagesize * ( @pagenumber - 1 )) + 1;
	set @rowend = @rowstart + @pagesize - 1; 
	
	-- If there were no results at all, check the count in the entire library
	if ( ( select COUNT(*) from #TEMP_ITEMS ) = 0 )
	begin
		-- Set the items and titles correctly
		set @total_items = 0;
		
		
		-- If there was an aggregation id, just return the counts for the whole library
		if ( @aggregationid > 0 )	
		begin
		
			-- Truncate the table and repull the data
			truncate table #TEMPZERO;
			
			-- Query against ALL aggregations this time
			declare @allquery nvarchar(max);
			set @allquery = 'select L.Itemid from SobekCM_Metadata_Basic_Search_Table AS L where ' + @SQLQuery;
			
			-- Execute this stored procedure
			insert #TEMPZERO execute sp_Executesql @allquery, @ParamDefinition, @term1, @term2, @term3, @term4, @term5, @term6, @term7, @term8, @term9, @term10;
			
			-- Get all items in the entire library then		  
			insert into #TEMP_ITEMS ( ItemID, fk_TitleID )
			select I.ItemID, I.GroupID
			from #TEMPZERO T1, SobekCM_Item I
			where ( T1.ItemID = I.ItemID )
			  and ( I.Deleted = 'false' )
			  and ( I.IP_Restriction_Mask ='0' )	
			  and ( I.IncludeInAll = 'true' );  
			  
			
		end;
		
		-- Drop the big temporary table
		drop table #TEMPZERO;
	
	end
	else
	begin	
	
		-- Drop the big temporary table
		drop table #TEMPZERO;	
		  
		-- There are essentially two major paths of execution, depending on whether this should
		-- be grouped as items within the page requested titles ( sorting by title or the basic
		-- sorting by rank, which ranks this way ) or whether each item should be
		-- returned by itself, such as sorting by individual publication dates, etc..
		
		if ( @sort < 10 )
		begin	
			-- create the temporary title table definition
			declare @TEMP_TITLES table ( TitleID int, BibID varchar(10), RowNumber int );	
			
			-- Return these counts
			select @total_items=COUNT(*)
			from #TEMP_ITEMS;
			
			-- Now, calculate the actual ending row, based on the ration, page information,
			-- and the lookahead factor
			if (( @total_items > 0 ))
			begin		
				-- Compute equation to determine possible page value ( max - log(factor, (items/title)/2))
				declare @computed_value int;
				select @computed_value = (@maxpagelookahead - CEILING( LOG10( ((cast(@total_items as float)) / (cast(@total_items as float)))/@lookahead_factor)));
				
				-- Compute the minimum value.  This cannot be less than @minpagelookahead.
				declare @floored_value int;
				select @floored_value = 0.5 * ((@computed_value + @minpagelookahead) + ABS(@computed_value - @minpagelookahead));
				
				-- Compute the maximum value.  This cannot be more than @maxpagelookahead.
				declare @actual_pages int;
				select @actual_pages = 0.5 * ((@floored_value + @maxpagelookahead) - ABS(@floored_value - @maxpagelookahead));

				-- Set the final row again then
				set @rowend = @rowstart + ( @pagesize * @actual_pages ) - 1; 
			end;	
					  
			-- Create saved select across titles for row numbers
			with TITLES_SELECT AS
				(	select GroupID, G.BibID, 
						ROW_NUMBER() OVER (order by case when @sort=0 THEN (SUM(Hit_COunt)/COUNT(*)) end DESC,
													case when @sort=1 THEN G.SortTitle end ASC,												
													case when @sort=2 THEN BibID end ASC,
													case when @sort=3 THEN BibID end DESC) as RowNumber
					from #TEMP_ITEMS I, SobekCM_Item_Group G
					where I.fk_TitleID = G.GroupID
					group by G.GroupID, G.BibID, G.SortTitle )

			-- Insert the correct rows into the temp title table	
			insert into @TEMP_TITLES ( TitleID, BibID, RowNumber )
			select GroupID, BibID, RowNumber
			from TITLES_SELECT
			where RowNumber >= @rowstart
			  and RowNumber <= @rowend;
		
						
			-- Return the item information for this page
			if ( @pageOrder = 'true' )
			begin
			
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID, i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
				--order by i.ItemID, T.RowNumber;
				order by i.Title desc;
			end
			else
			begin
			
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID, i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
				--order by i.ItemID, T.RowNumber;
				order by i.Title, T.RowNumber;
			end
			
		end
		else
		begin
			-- Create the temporary item table for paging purposes
			declare @TEMP_PAGED_ITEMS table ( ItemID int, RowNumber int );
			
			-- Since these sorts make each item paired with a single title row,
			-- number of items and titles are equal
			select @total_items=COUNT(*)
			from #TEMP_ITEMS; 
			
			-- In addition, always return the max lookahead pages
			set @rowend = @rowstart + ( @pagesize * @maxpagelookahead ) - 1; 
			
			-- Create saved select across items for row numbers
			with ITEMS_SELECT AS
			 (	select I.ItemID, 
					ROW_NUMBER() OVER (order by case when @sort=10 THEN isnull(SortDate,9223372036854775807)  end ASC,
												case when @sort=11 THEN isnull(SortDate,-1) end DESC) as RowNumber
					from #TEMP_ITEMS I
					group by I.ItemID, SortDate )
						  
			-- Insert the correct rows into the temp item table	
			insert into @TEMP_PAGED_ITEMS ( ItemID, RowNumber )
			select ItemID, RowNumber
			from ITEMS_SELECT
			where RowNumber >= @rowstart
			  and RowNumber <= @rowend;
			
			-- Return the item information for this page descending
			if ( @pageOrder = 'true' )
			begin
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID,  i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
				--order by i.ItemID, T.RowNumber;
				order by i.Title desc;
			end
			-- Return the item information for this page ascending
			else
			begin
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID,  i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
				--order by i.ItemID, T.RowNumber;
				order by i.Title, T.RowNumber;
			end
		end;

		-- Return the facets if asked for
		if ( @include_facets = 'true' )
		begin	
			if (( LEN( isnull( @aggregationcode, '')) = 0 ) or ( @aggregationcode = 'all' ))
			begin
				-- Build the aggregation list
				select A.Code as MetadataValue, A.ShortName, Metadata_Count=Count(*)
				from SobekCM_Item_Aggregation A, SobekCM_Item_Aggregation_Item_Link L, SobekCM_Item I, #TEMP_ITEMS T
				where ( T.ItemID = I.ItemID )
				  and ( I.ItemID = L.ItemID )
				  and ( L.AggregationID = A.AggregationID )
				  and ( A.Hidden = 'false' )
				  and ( A.isActive = 'true' )
				  and ( A.Include_In_Collection_Facet = 'true' )
				group by A.Code, A.ShortName
				order by Metadata_Count DESC, ShortName ASC;	
			end;
			
			-- Return the FIRST facet
			if ( @facettype1 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype1 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the SECOND facet
			if ( @facettype2 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype2 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the THIRD facet
			if ( @facettype3 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype3 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the FOURTH facet
			if ( @facettype4 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype4 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the FIFTH facet
			if ( @facettype5 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype5 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the SIXTH facet
			if ( @facettype6 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype6 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the SEVENTH facet
			if ( @facettype7 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype7 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the EIGHTH facet
			if ( @facettype8 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype8 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
		end; -- End overall FACET block
	end; -- End else statement entered if there are any results to return
	
	-- return the query string as well, for debuggins
	select Query = @mainquery, RankSelection = @rankselection;
	
	-- drop the temporary tables
	drop table #TEMP_ITEMS;
	
	Set NoCount OFF;
			
	If @@ERROR <> 0 GoTo ErrorHandler;
    Return(0);
  
ErrorHandler:
    Return(@@ERROR);
	
END;

GO
/****** Object:  StoredProcedure [dbo].[DPanther_FacetSearch_PagedByDate]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_FacetSearch_PagedByDate]
	@term1 nvarchar(255),
	@field1 int=null,
	@link2 int,
	@term2 nvarchar(255)=null,
	@field2 int=null,
	@link3 int=0,
	@term3 nvarchar(255)=null,
	@field3 int=null,
	@link4 int=0,
	@term4 nvarchar(255)=null,
	@field4 int=null,
	@link5 int=0,
	@term5 nvarchar(255)=null,
	@field5 int=null,
	@link6 int=0,
	@term6 nvarchar(255)=null,
	@field6 int=null,
	@link7 int=0,
	@term7 nvarchar(255)=null,
	@field7 int=null,
	@link8 int=0,
	@term8 nvarchar(255)=null,
	@field8 int=null,
	@link9 int=0,
	@term9 nvarchar(255)=null,
	@field9 int=null,
	@link10 int=0,
	@term10 nvarchar(255)=null,
	@field10 int=null,
	@include_private bit,
	@aggregationcode varchar(20),	
	@pagesize int, 
	@pagenumber int,
	@sort int,
	@minpagelookahead int,
	@maxpagelookahead int,
	@lookahead_factor float,
	@include_facets bit,
	@facettype1 smallint,
	@facettype2 smallint,
	@facettype3 smallint,
	@facettype4 smallint,
	@facettype5 smallint,
	@facettype6 smallint=null,
	@facettype7 smallint=null,
	@facettype8 smallint=null,
	@startYear  nvarchar(4),
	@endYear	nvarchar(4),
	@pageOrder	bit='false',
	@total_items int output
	
	
AS
BEGIN
	-- No need to perform any locks here, especially given the possible
	-- length of this search
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	SET NOCOUNT ON;
	
	-- Field#'s indicate which metadata field (if any).  These are numbers from the 
	-- SobekCM_Metadata_Types table.  A field# of -1, means all fields are included.
	
	-- Link#'s indicate if this is an AND-joiner ( intersect ) or an OR-joiner ( union )
	-- 0 = AND, 1 = OR, 2 = AND NOT
	
	
	-- exec SobekCM_Metadata_Search 'haiti',1,1,'kesse',-1,0,'',0
	-- This searches for materials which have haiti in the title OR kesse anywhere
	
	-- Create the temporary table variables first
	-- Create the temporary table to hold all the item id's
	--setup startYear and endYear
	if(@startYear='1')
	begin
		set @startYear=' '
	end
	if(@endYear='1')
	begin
		set @endYear='ZZZZ'
	end
	
	create table #TEMPZERO ( ItemID int primary key );
	create table #TEMP_ITEMS ( ItemID int primary key, fk_TitleID int, Hit_Count int, SortDate bigint, PubYear nvarchar(100));
		    
	-- declare both the sql query and the parameter definitions
	declare @SQLQuery AS nvarchar(max);
	declare @rankselection AS nvarchar(1000);
    declare @ParamDefinition AS NVarchar(2000);
    
    -- Determine the aggregationid
	declare @aggregationid int;
	set @aggregationid = ( select ISNULL(AggregationID,-1) from SobekCM_Item_Aggregation where Code=@aggregationcode );
	
    -- Set value for filtering privates
	declare @lower_mask int;
	set @lower_mask = 0;
	if ( @include_private = 'true' )
	begin
		set @lower_mask = -256;
	end;
	    
    -- Start to build the main bulk of the query   
	set @SQLQuery = '';
    
    --if request for full citation
    if(@term1 = '1')
		begin
			-- Search the full citation then
			
			set @SQLQuery = @SQLQuery + ' 1=1 ';	
		end
    else
    begin
    -- Was a field listed?
		if (( @field1 > 0 ) and ( @field1 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field1_name varchar(100);
			set @field1_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field1 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field1_name + ', @innerterm1 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm1 )';	
		end
		 -- Start to build the query which will do ranking over the results which match this search
		set @rankselection = @term1;
	end;
            
   

	-- Add the second term, if there is one
	if (( LEN( ISNULL(@term2,'')) > 0 ) and (( @link2 = 0 ) or ( @link2 = 1 ) or ( @link2 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link2 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link2 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link2 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field2 > 0 ) and ( @field2 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field2_name varchar(100);
			set @field2_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field2 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field2_name + ', @innerterm2 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm2 )';	
		end;			
		
		-- Build the ranking query
		if ( @link2 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term2;	
		end
	end;    
	
	-- Add the third term, if there is one
	if (( LEN( ISNULL(@term3,'')) > 0 ) and (( @link3 = 0 ) or ( @link3 = 1 ) or ( @link3 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link3 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link3 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link3 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field3 > 0 ) and ( @field3 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field3_name varchar(100);
			set @field3_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field3 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field3_name + ', @innerterm3 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm3 )';	
		end;	
		
		-- Build the ranking query
		if ( @link3 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term3;		
		end
	end;   
	
	-- Add the fourth term, if there is one
	if (( LEN( ISNULL(@term4,'')) > 0 ) and (( @link4 = 0 ) or ( @link4 = 1 ) or ( @link4 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link4 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link4 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link4 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field4 > 0 ) and ( @field4 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field4_name varchar(100);
			set @field4_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field4 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field4_name + ', @innerterm4 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm4 )';	
		end;	
			
		-- Build the ranking query
		if ( @link4 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term4;		
		end
	end;
	
	-- Add the fifth term, if there is one
	if (( LEN( ISNULL(@term5,'')) > 0 ) and (( @link5 = 0 ) or ( @link5 = 1 ) or ( @link5 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link5 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link5 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link5 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field5 > 0 ) and ( @field5 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field5_name varchar(100);
			set @field5_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field5 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field5_name + ', @innerterm5 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm5 )';	
		end;
			
		-- Build the ranking query
		if ( @link5 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term5;		
		end
	end;
	
	-- Add the sixth term, if there is one
	if (( LEN( ISNULL(@term6,'')) > 0 ) and (( @link6 = 0 ) or ( @link6 = 1 ) or ( @link6 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link6 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link6 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link6 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field6 > 0 ) and ( @field6 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field6_name varchar(100);
			set @field6_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field6 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field6_name + ', @innerterm6 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm6 )';	
		end;
		
		-- Build the ranking query
		if ( @link6 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term6;		
		end
	end; 
	
	-- Add the seventh term, if there is one
	if (( LEN( ISNULL(@term7,'')) > 0 ) and (( @link7 = 0 ) or ( @link7 = 1 ) or ( @link7 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link7 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link7 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link7 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field7 > 0 ) and ( @field7 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field7_name varchar(100);
			set @field7_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field7 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field7_name + ', @innerterm7 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm7 )';	
		end;
		
		-- Build the ranking query
		if ( @link7 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term7;		
		end
	end;
	
	-- Add the eighth term, if there is one
	if (( LEN( ISNULL(@term8,'')) > 0 ) and (( @link8 = 0 ) or ( @link8 = 1 ) or ( @link8 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link8 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link8 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link8 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field8 > 0 ) and ( @field8 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field8_name varchar(100);
			set @field8_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field8 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field8_name + ', @innerterm8 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm8 )';	
		end;
		
		-- Build the ranking query
		if ( @link8 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term8;		
		end
	end;
	
	-- Add the ninth term, if there is one
	if (( LEN( ISNULL(@term9,'')) > 0 ) and (( @link9 = 0 ) or ( @link9 = 1 ) or ( @link9 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link9 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link9 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link9 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field9 > 0 ) and ( @field9 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field9_name varchar(100);
			set @field9_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field9 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field9_name + ', @innerterm9 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm9 )';	
		end;
		
		-- Build the ranking query
		if ( @link9 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term9;		
		end
	end;
	
	-- Add the tenth term, if there is one
	if (( LEN( ISNULL(@term10,'')) > 0 ) and (( @link10 = 0 ) or ( @link10 = 1 ) or ( @link10 = 2 )))
	begin	
		-- Was this an AND, OR, or AND NOT?
		if ( @link10 = 0 ) set @SQLQuery = @SQLQuery + ' and';
		if ( @link10 = 1 ) set @SQLQuery = @SQLQuery + ' or';
		if ( @link10 = 2 ) set @SQLQuery = @SQLQuery + ' and not';
		
		-- Was a field listed?
		if (( @field10 > 0 ) and ( @field10 in ( select MetadataTypeID from SobekCM_Metadata_Types )))
		begin
			-- Get the name of this column then
			declare @field10_name varchar(100);
			set @field10_name = ( select REPLACE(MetadataName, ' ', '_') from SobekCM_Metadata_Types where MetadataTypeID = @field10 );

			-- Add this search then
			set @SQLQuery = @SQLQuery + ' contains ( ' + @field10_name + ', @innerterm10 )';
		end
		else
		begin
			-- Search the full citation then
			set @SQLQuery = @SQLQuery + ' contains ( FullCitation, @innerterm10 )';	
		end;
		
		-- Build the ranking query
		if ( @link10 != 2 )
		begin
			set @rankselection = @rankselection + ' or ' + @term10;		
		end		
	end;
	
	-- Add the recompile option
	--set @SQLQuery = @SQLQuery + ' option (RECOMPILE)';

    -- Add the first term and start to build the query which will provide the items which match the search
    declare @mainquery nvarchar(max);
    set @mainquery = 'select L.Itemid from SobekCM_Metadata_Basic_Search_Table AS L';
    
    -- Do we need to limit by aggregation id as well?
    if ( @aggregationid > 0 )
    begin
		set @mainquery = @mainquery + '  join SobekCM_Item_Aggregation_Item_Link AS A ON ( A.ItemID = L.ItemID ) and ( A.AggregationID = ' + CAST( @aggregationid as varchar(5) ) + ')';   
    end    
    
    -- Add the full text search portion here
    set @mainquery = @mainquery + ' where ' + @SQLQuery;
	
	-- Set the parameter definition
	set @ParamDefinition = ' @innerterm1 nvarchar(255), @innerterm2 nvarchar(255), @innerterm3 nvarchar(255), @innerterm4 nvarchar(255), @innerterm5 nvarchar(255), @innerterm6 nvarchar(255), @innerterm7 nvarchar(255), @innerterm8 nvarchar(255), @innerterm9 nvarchar(255), @innerterm10 nvarchar(255)';
		
	-- Execute this stored procedure
	insert #TEMPZERO execute sp_Executesql @mainquery, @ParamDefinition, @term1, @term2, @term3, @term4, @term5, @term6, @term7, @term8, @term9, @term10;
			
	-- Perform ranking against the items and insert into another temporary table 
	-- with all the possible data elements needed for applying the user's sort
	if(@term1 = '1')
	Begin
	insert into #TEMP_ITEMS ( ItemID, fk_TitleID, SortDate, Hit_Count, PubYear)
		select I.ItemID, I.GroupID, SortDate=isnull( I.SortDate,-1), 0, i.PubDate
		from SobekCM_Item I, #TEMPZERO AS T1 join
		  SobekCM_Metadata_Basic_Search_Table AS KEY_TBL on KEY_TBL.ItemID = T1.ItemID
		where ( T1.ItemID = I.ItemID )
		  and ( I.Deleted = 'false' )
		  and ( I.IP_Restriction_Mask ='0' )	
		  and ( I.IncludeInAll = 'true' )
		  and LEFT([PubDate], 4)>=@startYear
		  and LEFT([PubDate], 4)<=@endYear; 	
      end
      else
      begin
		  insert into #TEMP_ITEMS ( ItemID, fk_TitleID, SortDate, Hit_Count, PubYear)
			select I.ItemID, I.GroupID, SortDate=isnull( I.SortDate,-1), isnull(KEY_TBL.RANK, 0 ), i.PubDate
			from SobekCM_Item I, #TEMPZERO AS T1 join
			  CONTAINSTABLE(SobekCM_Metadata_Basic_Search_Table, FullCitation, @rankselection ) AS KEY_TBL on KEY_TBL.[KEY] = T1.ItemID
			where ( T1.ItemID = I.ItemID )
			  and ( I.Deleted = 'false' )
			  and ( I.IP_Restriction_Mask ='0' )	
			  and ( I.IncludeInAll = 'true' )
			  and LEFT([PubDate], 4)>=@startYear
			  and LEFT([PubDate], 4)<=@endYear; 
      end   

	-- Determine the start and end rows
	declare @rowstart int;
	declare @rowend int;
	set @rowstart = (@pagesize * ( @pagenumber - 1 )) + 1;
	set @rowend = @rowstart + @pagesize - 1; 
	
	-- If there were no results at all, check the count in the entire library
	if ( ( select COUNT(*) from #TEMP_ITEMS ) = 0 )
	begin
		-- Set the items and titles correctly
		set @total_items = 0;
		
		
		-- If there was an aggregation id, just return the counts for the whole library
		if ( @aggregationid > 0 )	
		begin
		
			-- Truncate the table and repull the data
			truncate table #TEMPZERO;
			
			-- Query against ALL aggregations this time
			declare @allquery nvarchar(max);
			set @allquery = 'select L.Itemid from SobekCM_Metadata_Basic_Search_Table AS L where ' + @SQLQuery;
			
			-- Execute this stored procedure
			insert #TEMPZERO execute sp_Executesql @allquery, @ParamDefinition, @term1, @term2, @term3, @term4, @term5, @term6, @term7, @term8, @term9, @term10;
			
			-- Get all items in the entire library then		  
			insert into #TEMP_ITEMS ( ItemID, fk_TitleID )
			select I.ItemID, I.GroupID
			from #TEMPZERO T1, SobekCM_Item I
			where ( T1.ItemID = I.ItemID )
			  and ( I.Deleted = 'false' )
			  and ( I.IP_Restriction_Mask ='0' )	
			  and ( I.IncludeInAll = 'true' );  
			  
			
		end;
		
		-- Drop the big temporary table
		drop table #TEMPZERO;
	
	end
	else
	begin	
	
		-- Drop the big temporary table
		drop table #TEMPZERO;	
		  
		-- There are essentially two major paths of execution, depending on whether this should
		-- be grouped as items within the page requested titles ( sorting by title or the basic
		-- sorting by rank, which ranks this way ) or whether each item should be
		-- returned by itself, such as sorting by individual publication dates, etc..
		
		if ( @sort < 10 )
		begin	
			-- create the temporary title table definition
			declare @TEMP_TITLES table ( TitleID int, BibID varchar(10), RowNumber int );	
			
			-- Return these counts
			select @total_items=COUNT(*)
			from #TEMP_ITEMS;
			
			-- Now, calculate the actual ending row, based on the ration, page information,
			-- and the lookahead factor
			if (( @total_items > 0 ))
			begin		
				-- Compute equation to determine possible page value ( max - log(factor, (items/title)/2))
				declare @computed_value int;
				select @computed_value = (@maxpagelookahead - CEILING( LOG10( ((cast(@total_items as float)) / (cast(@total_items as float)))/@lookahead_factor)));
				
				-- Compute the minimum value.  This cannot be less than @minpagelookahead.
				declare @floored_value int;
				select @floored_value = 0.5 * ((@computed_value + @minpagelookahead) + ABS(@computed_value - @minpagelookahead));
				
				-- Compute the maximum value.  This cannot be more than @maxpagelookahead.
				declare @actual_pages int;
				select @actual_pages = 0.5 * ((@floored_value + @maxpagelookahead) - ABS(@floored_value - @maxpagelookahead));

				-- Set the final row again then
				set @rowend = @rowstart + ( @pagesize * @actual_pages ) - 1; 
			end;	
					  
			-- Create saved select across titles for row numbers
			with TITLES_SELECT AS
				(	select GroupID, G.BibID, 
						ROW_NUMBER() OVER (order by case when @sort=0 THEN (SUM(Hit_COunt)/COUNT(*)) end DESC,
													case when @sort=1 THEN G.SortTitle end ASC,												
													case when @sort=2 THEN BibID end ASC,
													case when @sort=3 THEN BibID end DESC) as RowNumber
					from #TEMP_ITEMS I, SobekCM_Item_Group G
					where I.fk_TitleID = G.GroupID
					group by G.GroupID, G.BibID, G.SortTitle )

			-- Insert the correct rows into the temp title table	
			insert into @TEMP_TITLES ( TitleID, BibID, RowNumber )
			select GroupID, BibID, RowNumber
			from TITLES_SELECT
			where RowNumber >= @rowstart
			  and RowNumber <= @rowend;
		
						
			-- Return the item information for this page
			if ( @pageOrder = 'true' )
			begin
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID,  i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
			--order by i.ItemID, T.RowNumber;
				order by i.ItemID desc;
			end
			else
			begin
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID,  i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
			--order by i.ItemID, T.RowNumber;
				order by i.ItemID, T.RowNumber;
			end
		end
		else
		begin
			-- Create the temporary item table for paging purposes
			declare @TEMP_PAGED_ITEMS table ( ItemID int, RowNumber int );
			
			-- Since these sorts make each item paired with a single title row,
			-- number of items and titles are equal
			select @total_items=COUNT(*)
			from #TEMP_ITEMS; 
			
			-- In addition, always return the max lookahead pages
			set @rowend = @rowstart + ( @pagesize * @maxpagelookahead ) - 1; 
			
			-- Create saved select across items for row numbers
			with ITEMS_SELECT AS
			 (	select I.ItemID, 
					ROW_NUMBER() OVER (order by case when @sort=10 THEN isnull(SortDate,9223372036854775807)  end ASC,
												case when @sort=11 THEN isnull(SortDate,-1) end DESC) as RowNumber
					from #TEMP_ITEMS I
					group by I.ItemID, SortDate )
						  
			-- Insert the correct rows into the temp item table	
			insert into @TEMP_PAGED_ITEMS ( ItemID, RowNumber )
			select ItemID, RowNumber
			from ITEMS_SELECT
			where RowNumber >= @rowstart
			  and RowNumber <= @rowend;
			
			-- Return the item information for this page
			if ( @pageOrder = 'true' )
			begin
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID,  i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
				--order by i.ItemID, T.RowNumber;
				order by i.ItemID desc;
			end
			else
			begin
				select distinct i.ItemID as ItemID, T.RowNumber as fk_TitleID,  i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.[Spatial_Coverage.Display] as publishPlace,
				isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude, i.PubYear as publishYear,
				isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
				isnull(Subjects_Display, '') as Subjects, s.[Type]
			  
				from #TEMP_ITEMS M
				left join @TEMP_TITLES T on ( M.fk_TitleID = T.TitleID )
				left join SobekCM_Item I on ( I.ItemID = M.ItemID )
				left join SobekCM_Item_Group G on (M.fk_TitleID = G.GroupID)
				left join SobekCM_Metadata_Basic_Search_Table s  on M.ItemID=s.ItemID
				left join SobekCM_Item_Footprint f on (M.ItemID=f.itemid)
				--order by i.ItemID, T.RowNumber;
				order by i.ItemID, T.RowNumber;
			end
		end;

		-- Return the facets if asked for
		if ( @include_facets = 'true' )
		begin	
			if (( LEN( isnull( @aggregationcode, '')) = 0 ) or ( @aggregationcode = 'all' ))
			begin
				-- Build the aggregation list
				select A.Code as MetadataValue, A.ShortName, Metadata_Count=Count(*)
				from SobekCM_Item_Aggregation A, SobekCM_Item_Aggregation_Item_Link L, SobekCM_Item I, #TEMP_ITEMS T
				where ( T.ItemID = I.ItemID )
				  and ( I.ItemID = L.ItemID )
				  and ( L.AggregationID = A.AggregationID )
				  and ( A.Hidden = 'false' )
				  and ( A.isActive = 'true' )
				  and ( A.Include_In_Collection_Facet = 'true' )
				group by A.Code, A.ShortName
				order by Metadata_Count DESC, ShortName ASC;	
			end;
			
			-- Return the FIRST facet
			if ( @facettype1 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype1 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the SECOND facet
			if ( @facettype2 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype2 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the THIRD facet
			if ( @facettype3 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype3 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the FOURTH facet
			if ( @facettype4 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype4 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the FIFTH facet
			if ( @facettype5 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype5 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the SIXTH facet
			if ( @facettype6 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype6 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the SEVENTH facet
			if ( @facettype7 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype7 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
			
			-- Return the EIGHTH facet
			if ( @facettype8 > 0 )
			begin
				-- Return the first 100 values
				select MetadataValue, Metadata_Count
				from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
						from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
						where ( U.ItemID = I.ItemID )
						  and ( U.MetadataTypeID = @facettype8 )
						group by U.MetadataID
						order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
				where M.MetadataID = F.MetadataID
				order by Metadata_Count DESC, MetadataValue ASC;
			end;
		end; -- End overall FACET block
	end; -- End else statement entered if there are any results to return
	
	-- return the query string as well, for debuggins
	select Query = @mainquery, RankSelection = @rankselection;
	
	-- drop the temporary tables
	drop table #TEMP_ITEMS;
	
	Set NoCount OFF;
			
	If @@ERROR <> 0 GoTo ErrorHandler;
    Return(0);
  
ErrorHandler:
    Return(@@ERROR);
	
END;

GO
/****** Object:  StoredProcedure [dbo].[DPanther_GetCollectionByCoordinates]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_GetCollectionByCoordinates]
	-- Add the parameters for the stored procedure here
	@lat1 float,
	@long1 float,
	@lat2 float=null,
	@long2 float=null,
	@include_private bit,
	@aggregationcode varchar(20),
	@pagesize int, 
	@pagenumber int,
	@sort int,	
	@minpagelookahead int,
	@maxpagelookahead int,
	@lookahead_factor float,
	@include_facets bit,
	@facettype1 smallint,
	@facettype2 smallint,
	@facettype3 smallint,
	@facettype4 smallint,
	@facettype5 smallint,
	@facettype6 smallint=null,
	@facettype7 smallint=null,
	@facettype8 smallint=null,
	@total_items int output,
	@total_titles int output
AS
BEGIN

	-- No need to perform any locks here
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	-- Create the temporary tables first
	-- Create the temporary table to hold all the item id's
	create table #TEMPSUBZERO ( ItemID int );
	create table #TEMPZERO ( ItemID int );
	create table #TEMP_ITEMS ( ItemID int, fk_TitleID int, SortDate bigint, Spatial_KML varchar(4000), Spatial_KML_Distance float );

	-- Is this really just a point search?
	if (( isnull(@lat2,1000) = 1000 ) or ( isnull(@long2,1000) = 1000 ) or (( @lat1=@lat2 ) and ( @long1=@long2 )))
	begin

		-- Select all matching item ids
		insert into #TEMPZERO
		select distinct(itemid) 
		from SobekCM_Item_Footprint
		where (( Point_Latitude = @lat1 ) and ( Point_Longitude = @long1 ))
		   or (((( Rect_Latitude_A >= @lat1 ) and ( Rect_Latitude_B <= @lat1 )) or (( Rect_Latitude_A <= @lat1 ) and ( Rect_Latitude_B >= @lat1)))
	        and((( Rect_Longitude_A >= @long1 ) and ( Rect_Longitude_B <= @long1 )) or (( Rect_Longitude_A <= @long1 ) and ( Rect_Longitude_B >= @long1 ))));

	end
	else
	begin

		-- Select all matching item ids by rectangle
		insert into #TEMPSUBZERO
		select distinct(itemid)
		from SobekCM_Item_Footprint
		where ((( Point_Latitude <= @lat1 ) and ( Point_Latitude >= @lat2 )) or (( Point_Latitude >= @lat1 ) and ( Point_Latitude <= @lat2 )))
		  and ((( Point_Longitude <= @long1 ) and ( Point_Longitude >= @long2 )) or (( Point_Longitude >= @long1 ) and ( Point_Longitude <= @long2 )));
		
		-- Select rectangles which OVERLAP with this rectangle
		insert into #TEMPSUBZERO
		select distinct(itemid)
		from SobekCM_Item_Footprint
		where (((( Rect_Latitude_A >= @lat1 ) and ( Rect_Latitude_A <= @lat2 )) or (( Rect_Latitude_A <= @lat1 ) and ( Rect_Latitude_A >= @lat2 )))
			or ((( Rect_Latitude_B >= @lat1 ) and ( Rect_Latitude_B <= @lat2 )) or (( Rect_Latitude_B <= @lat1 ) and ( Rect_Latitude_B >= @lat2 ))))
		  and (((( Rect_Longitude_A >= @long1 ) and ( Rect_Longitude_A <= @long2 )) or (( Rect_Longitude_A <= @long1 ) and ( Rect_Longitude_A >= @long2 )))
			or ((( Rect_Longitude_B >= @long1 ) and ( Rect_Longitude_B <= @long2 )) or (( Rect_Longitude_B <= @long1 ) and ( Rect_Longitude_B >= @long2 ))));
		
		-- Select rectangles that INCLUDE this rectangle by picking overlaps with one point
		insert into #TEMPSUBZERO
		select distinct(itemid)
		from SobekCM_Item_Footprint
		where ((( @lat1 <= Rect_Latitude_A ) and ( @lat1 >= Rect_Latitude_B )) or (( @lat1 >= Rect_Latitude_A ) and ( @lat1 <= Rect_Latitude_B )))
		  and ((( @long1 <= Rect_Longitude_A ) and ( @long1 >= Rect_Longitude_B )) or (( @long1 >= Rect_Longitude_A ) and ( @long1 <= Rect_Longitude_B )));

		-- Make sure uniqueness applies here as well
		insert into #TEMPZERO
		select distinct(itemid)
		from #TEMPSUBZERO;
	end;
	
	-- Determine the start and end rows
	declare @rowstart int;
	declare @rowend int; 
	set @rowstart = (@pagesize * ( @pagenumber - 1 )) + 1;
	set @rowend = @rowstart + @pagesize - 1; 

	-- Set value for filtering privates
	declare @lower_mask int;
	set @lower_mask = 0;
	if ( @include_private = 'true' )
	begin
		set @lower_mask = -256;
	end;

	-- Determine the aggregationid
	declare @aggregationid int;
	set @aggregationid = ( select ISNULL(AggregationID,-1) from SobekCM_Item_Aggregation where Code=@aggregationcode );

			
	-- Was an aggregation included?
	if ( LEN(ISNULL( @aggregationcode,'' )) > 0 )
	begin	
		-- Look for matching the provided aggregation
		insert into #TEMP_ITEMS ( ItemID, fk_TitleID, SortDate, Spatial_KML, Spatial_KML_Distance )
		select I.ItemID, I.GroupID, SortDate=isnull( I.SortDate,-1), Spatial_KML=isnull(Spatial_KML,''), Spatial_KML_Distance
		from #TEMPZERO T1, SobekCM_Item I, SobekCM_Item_Aggregation_Item_Link CL
		where ( CL.ItemID = I.ItemID )
		  and ( CL.AggregationID = @aggregationid )
		  and ( I.Deleted = 'false' )
		  and ( T1.ItemID = I.ItemID ) 
		  and ( I.IP_Restriction_Mask >= @lower_mask );
	end
	else
	begin	
		-- Look for matching the provided aggregation
		insert into #TEMP_ITEMS ( ItemID, fk_TitleID, SortDate, Spatial_KML, Spatial_KML_Distance )
		select I.ItemID, I.GroupID, SortDate=isnull( I.SortDate,-1), Spatial_KML=isnull(Spatial_KML,''), Spatial_KML_Distance
		from #TEMPZERO T1, SobekCM_Item I
		where ( I.Deleted = 'false' )
		  and ( T1.ItemID = I.ItemID )
		  and ( I.IP_Restriction_Mask >= @lower_mask );
	end;
	
	-- There are essentially THREE major paths of execution, depending on whether this should
	-- be grouped as items within the page requested titles ( sorting by title or the basic
	-- sorting by rank, which ranks this way ) or whether each item should be
	-- returned by itself, such as sorting by individual publication dates, etc..
	-- The default sort for this search is by spatial coordiantes, in which case the same 
	-- title should appear multiple times, if the items in the volume have different coordinates
	
	if ( @sort = 0 )
	begin
		-- create the temporary title table definition
		create table #TEMP_TITLES_ITEMS ( TitleID int, BibID varchar(10), RowNumber int, Spatial_KML varchar(4000), Spatial_Distance float );
		
		-- Compute the number of seperate titles/coordinates
		select fk_TitleID, (COUNT(Spatial_KML)) as assign_value
		into #TEMP1
		from #TEMP_ITEMS I
		group by fk_TitleID, Spatial_KML;
		
		-- Get the TOTAL count of spatial_kmls
		select @total_titles = isnull(SUM(assign_value), 0) from #TEMP1;
		drop table #TEMP1;
		
		-- Total items is simpler to computer
		select @total_items = COUNT(*) from #TEMP_ITEMS;	
		
		-- For now, always return the max lookahead pages
		set @rowend = @rowstart + ( @pagesize * @maxpagelookahead ) - 1; 
		
		-- Create saved select across titles for row numbers
		with TITLES_SELECT AS
			(	select GroupID, G.BibID, Spatial_KML, Spatial_KML_Distance,
					ROW_NUMBER() OVER (order by Spatial_KML_Distance ASC, Spatial_KML ASC) as RowNumber
				from #TEMP_ITEMS I, SobekCM_Item_Group G
				where I.fk_TitleID = G.GroupID
				group by G.GroupID, G.BibID, G.SortTitle, Spatial_KML, Spatial_KML_Distance )

		-- Insert the correct rows into the temp title table	
		insert into #TEMP_TITLES_ITEMS ( TitleID, BibID, RowNumber, Spatial_KML, Spatial_Distance )
		select GroupID, BibID, RowNumber, Spatial_KML, Spatial_KML_Distance
		from TITLES_SELECT
		where RowNumber >= @rowstart
		  and RowNumber <= @rowend;
		  
		-- Return the title information for this page
		select RowNumber as TitleID, T.BibID, G.GroupTitle, G.ALEPH_Number, G.OCLC_Number, isnull(G.GroupThumbnail,'') as GroupThumbnail, G.[Type], isnull(G.Primary_Identifier_Type,'') as Primary_Identifier_Type, isnull(G.Primary_Identifier, '') as Primary_Identifier
		from #TEMP_TITLES_ITEMS T, SobekCM_Item_Group G
		where ( T.TitleID = G.GroupID )
		order by RowNumber ASC;
		
		-- Return the item information for this page
		select T.RowNumber as fk_TitleID, i.ItemID as ItemID, i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.Spatial_Coverage as publishPlace,
			  isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [MainLatitude],[MainLongitude],i.PubYear as publishYear,
			  isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
			  isnull(Subjects_Display, '') as Subjects, G.[Type]
			  
			from #TEMP_ITEMS M, #TEMP_TITLES T, SobekCM_Item I, SobekCM_Item_Group G, SobekCM_Metadata_Basic_Search_Table s 
			where ( M.fk_TitleID = T.TitleID )
			  and ( I.ItemID = M.ItemID )
			  and ( T.TitleID = G.GroupID)
			  and (t.TitleID=s.ItemID)
			order by i.ItemID, T.RowNumber;
		
		-- drop the temporary table
		drop table #TEMP_TITLES_ITEMS;	
	end;
	
	if (( @sort < 10 ) and ( @sort > 0 ))
	begin	
		-- create the temporary title table definition
		create table #TEMP_TITLES ( TitleID int, BibID varchar(10), RowNumber int );
		
		-- Get the total counts
		select @total_items=COUNT(*), @total_titles=COUNT(distinct fk_TitleID)
		from #TEMP_ITEMS; 
		
		-- Now, calculate the actual ending row, based on the ration, page information,
		-- and the lookahead factor
		
		-- Compute equation to determine possible page value ( max - log(factor, (items/title)/2))
		declare @computed_value int;
		select @computed_value = (@maxpagelookahead - CEILING( LOG10( ((cast(@total_items as float)) / (cast(@total_titles as float)))/@lookahead_factor)));
		
		-- Compute the minimum value.  This cannot be less than @minpagelookahead.
		declare @floored_value int;
		select @floored_value = 0.5 * ((@computed_value + @minpagelookahead) + ABS(@computed_value - @minpagelookahead));
		
		-- Compute the maximum value.  This cannot be more than @maxpagelookahead.
		declare @actual_pages int;
		select @actual_pages = 0.5 * ((@floored_value + @maxpagelookahead) - ABS(@floored_value - @maxpagelookahead)); 

		-- Set the final row again then
		set @rowend = @rowstart + ( @pagesize * @actual_pages ) - 1; 		
				  
		-- Create saved select across titles for row numbers
		with TITLES_SELECT AS
			(	select GroupID, G.BibID, 
					ROW_NUMBER() OVER (order by case when @sort=1 THEN G.SortTitle end ASC,											
												case when @sort=2 THEN BibID end ASC,
											    case when @sort=3 THEN BibID end DESC) as RowNumber
				from #TEMP_ITEMS I, SobekCM_Item_Group G
				where I.fk_TitleID = G.GroupID
				group by G.GroupID, G.BibID, G.SortTitle )

		-- Insert the correct rows into the temp title table	
		insert into #TEMP_TITLES ( TitleID, BibID, RowNumber )
		select GroupID, BibID, RowNumber
		from TITLES_SELECT
		where RowNumber >= @rowstart
		  and RowNumber <= @rowend;
	
		-- Return the title information for this page
		select RowNumber as TitleID, T.BibID, G.GroupTitle, G.ALEPH_Number, G.OCLC_Number, isnull(G.GroupThumbnail,'') as GroupThumbnail, G.[Type], isnull(G.Primary_Identifier_Type,'') as Primary_Identifier_Type, isnull(G.Primary_Identifier, '') as Primary_Identifier
		from #TEMP_TITLES T, SobekCM_Item_Group G
		where ( T.TitleID = G.GroupID )
		order by RowNumber ASC;
		
		-- Return the item information for this page
		select T.RowNumber as fk_TitleID, i.ItemID as ItemID, i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.Spatial_Coverage as publishPlace,
			  isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [MainLatitude],[MainLongitude],i.PubYear as publishYear,
			  isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
			  isnull(Subjects_Display, '') as Subjects, G.[Type]
			  
			from #TEMP_ITEMS M, #TEMP_TITLES T, SobekCM_Item I, SobekCM_Item_Group G, SobekCM_Metadata_Basic_Search_Table s 
			where ( M.fk_TitleID = T.TitleID )
			  and ( I.ItemID = M.ItemID )
			  and ( T.TitleID = G.GroupID)
			  and (t.TitleID=s.ItemID)
			order by i.ItemID, T.RowNumber;
		
		-- drop the temporary table
		drop table #TEMP_TITLES;
	end;
	
	if ( @sort >= 10 )
	begin
		-- Create the temporary item table for paging purposes
		create table #TEMP_PAGED_ITEMS ( ItemID int, RowNumber int );
		
		-- Since these sorts make each item paired with a single title row,
		-- number of items and titles are equal
		select @total_items=COUNT(*), @total_titles=COUNT(*)
		from #TEMP_ITEMS; 
		
		-- In addition, always return the max lookahead pages
		set @rowend = @rowstart + ( @pagesize * @maxpagelookahead ) - 1; 
		
		-- Create saved select across items for row numbers
		with ITEMS_SELECT AS
		 (	select I.ItemID, 
				ROW_NUMBER() OVER (order by case when @sort=10 THEN SortDate end ASC,
											case when @sort=11 THEN SortDate end DESC) as RowNumber
				from #TEMP_ITEMS I
				group by I.ItemID, SortDate )
					  
		-- Insert the correct rows into the temp item table	
		insert into #TEMP_PAGED_ITEMS ( ItemID, RowNumber )
		select ItemID, RowNumber
		from ITEMS_SELECT
		where RowNumber >= @rowstart
		  and RowNumber <= @rowend;
		  
		-- Return the title information for this page
		select RowNumber as TitleID, G.BibID, G.GroupTitle, G.ALEPH_Number, G.OCLC_Number, isnull(G.GroupThumbnail,'') as GroupThumbnail, G.[Type], isnull(G.Primary_Identifier_Type,'') as Primary_Identifier_Type, isnull(G.Primary_Identifier, '') as Primary_Identifier
		from #TEMP_PAGED_ITEMS T, SobekCM_Item I, SobekCM_Item_Group G
		where ( T.ItemID = I.ItemID )
		  and ( I.GroupID = G.GroupID )
		order by RowNumber ASC;
		
		-- Return the item information for this page
		select T.RowNumber as fk_TitleID, i.ItemID as ItemID, i.Title,  isnull(I.MainThumbnail,'') as thumbnail, s.Spatial_Coverage as publishPlace,
			  isnull(I.PubDate,'') as PubDate, VID, G.BibID, I.Spatial_KML, [MainLatitude],[MainLongitude],i.PubYear as publishYear,
			  isnull(I.Link,'') as downloadLink, isnull(i.Publisher,'') as Publisher, isnull(Author,'') as Author, isnull(i.Format,'') as Format,
			  isnull(Subjects_Display, '') as Subjects, G.[Type]
			  
			from #TEMP_ITEMS M, #TEMP_TITLES T, SobekCM_Item I, SobekCM_Item_Group G, SobekCM_Metadata_Basic_Search_Table s 
			where ( M.fk_TitleID = T.TitleID )
			  and ( I.ItemID = M.ItemID )
			  and ( T.TitleID = G.GroupID)
			  and (t.TitleID=s.ItemID)
			order by i.ItemID, T.RowNumber;
		  				
		-- drop the temporary paged table
		drop table #TEMP_PAGED_ITEMS;	
	end
	
	-- Return the facets if asked for
	if ( @include_facets = 'true' )
	begin	
		-- Only return the aggregation codes if this was a search across all collections	
		if (( LEN( isnull( @aggregationcode, '')) = 0 ) or ( @aggregationcode='all'))
		begin
			-- Build the aggregation list
			select A.Code, A.ShortName, Metadata_Count=Count(*)
			from SobekCM_Item_Aggregation A, SobekCM_Item_Aggregation_Item_Link L, SobekCM_Item I, #TEMP_ITEMS T
			where ( T.ItemID = I.ItemID )
			  and ( I.ItemID = L.ItemID )
			  and ( L.AggregationID = A.AggregationID )
			  and ( A.Hidden = 'false' )
			  and ( A.isActive = 'true' )
			  and ( A.Include_In_Collection_Facet = 'true' )
			group by A.Code, A.ShortName
			order by Metadata_Count DESC, ShortName ASC;	
		end;	
		
		-- Return the FIRST facet
		if ( @facettype1 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype1 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
		
		-- Return the SECOND facet
		if ( @facettype2 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype2 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
		
		-- Return the THIRD facet
		if ( @facettype3 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype3 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;	
		
		-- Return the FOURTH facet
		if ( @facettype4 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype4 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
		
		-- Return the FIFTH facet
		if ( @facettype5 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype5 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
		
		-- Return the SIXTH facet
		if ( @facettype6 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype6 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
		
		-- Return the SEVENTH facet
		if ( @facettype7 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype7 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
		
		-- Return the EIGHTH facet
		if ( @facettype8 > 0 )
		begin
			-- Return the first 100 values
			select MetadataValue, Metadata_Count
			from (	select top(100) U.MetadataID, Metadata_Count = COUNT(*)
					from #TEMP_ITEMS I, Metadata_Item_Link_Indexed_View U with (NOEXPAND)
					where ( U.ItemID = I.ItemID )
					  and ( U.MetadataTypeID = @facettype8 )
					group by U.MetadataID
					order by Metadata_Count DESC ) F, SobekCM_Metadata_Unique_Search_Table M
			where M.MetadataID = F.MetadataID
			order by Metadata_Count DESC, MetadataValue ASC;
		end;
	end;

	-- drop the temporary tables
	drop table #TEMP_ITEMS;
	drop table #TEMPZERO;
	drop table #TEMPSUBZERO;
END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_GetCollectionLatLon]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_GetCollectionLatLon]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT distinct
       [Point_Latitude]
      ,[Point_Longitude]
      
  FROM [dbo].[SobekCM_Item_Footprint]
END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_GetMetadataTypeID]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_GetMetadataTypeID]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
	
	select [MetadataTypeID],[SobekCode] from [SobekCM_Metadata_Types]
	where [SobekCode]<>''
END

GO
/****** Object:  StoredProcedure [dbo].[DPanther_GetpubYear]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MM>
-- Create date: <2015-04-08>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_GetpubYear]
	@aggcode	varchar(100)='cgm'
AS
BEGIN
	select max(publishYear) as maxYear, MIN(b.publishYear) as minYear from
	--get all 
	(SELECT cast (LEFT(subsrt, PATINDEX('%[^0-9]%', subsrt + 't') - 1) as int) as publishYear
	FROM (
		SELECT subsrt = SUBSTRING(pubdate, pos, LEN(pubdate))
		FROM (
			SELECT pubdate, pos = PATINDEX('%[0-9]%', pubdate)
			from [DPantherProd].[dbo].[SobekCM_Item] 
			where aggregationcodes like @aggcode
			and pubdate<>''			
			and pubdate not like '%-%'
			) d
			) t
	where len(LEFT(subsrt, PATINDEX('%[^0-9]%', subsrt + 't') - 1))=4 

	union

	select  RIGHT (pubdate,4) as publishYear from [DPantherProd].[dbo].[SobekCM_Item] 
	where aggregationcodes like @aggcode and pubdate<>''
	and pubdate like '%/%'
	AND RIGHT (PUBDATE,4) NOT LIKE '%/%'
	AND ISNUMERIC(RIGHT (PUBDATE,4) )=1
	) b
END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_ItemGetByFI]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_ItemGetByFI] 
	(@FINum varchar(50),
	 @vid	varchar(5)='00001'
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   	select    s.[Spatial_Coverage.Display] as publishPlace,	i.mainThumbnail,		
			  VID, S.BibID,  [Point_Latitude] as MainLatitude, [Point_Longitude] as MainLongitude			  
			 
	 FROM SobekCM_Metadata_Basic_Search_Table s 
	 left join	SobekCM_Item I on ( I.ItemID = s.ItemID )					
	 left join SobekCM_Item_Footprint f on (S.ItemID=f.itemid)  
     where S.BibID LIKE '%'+@FINum+'%' and I.vid=@vid

	select [Code] from
	[SobekCM_Item_Aggregation] a
	left join [SobekCM_Item_Aggregation_Item_Link] al
	on a.AggregationID=al.AggregationID
	where al.ItemID in
		(select s.itemid
			 FROM SobekCM_Metadata_Basic_Search_Table s
			 left join	SobekCM_Item I on ( I.ItemID = s.ItemID )	
			 where S.BibID LIKE '%'+@FINum+'%' and I.vid=@vid)

    select ic.* from
	[SobekCM_Icon] IC
	left join [SobekCM_Item_Icons] iic
	on IC.IconID=iic.IconID
	where iic.ItemID in
		(select s.itemid
			 FROM SobekCM_Metadata_Basic_Search_Table s
			 left join	SobekCM_Item I on ( I.ItemID = s.ItemID )	
			 where S.BibID LIKE '%'+@FINum+'%' and I.vid=@vid)

END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_KMLGetByFI]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_KMLGetByFI]
	-- Add the parameters for the stored procedure here
	(@FINum varchar(50))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from DP_KML 
	where FINum = @FINum
	
END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_logInsert]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_logInsert] 
	-- Add the parameters for the stored procedure here
	@userInfo		nvarchar(50),	
	@opTime			datetime,
	@action			nvarchar(150),
	@description	nvarchar(500),
	@typeID			int,
	@applicationID	int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into DP_Logs 
		(userInfo,opTime,action,description,typeID,applicationID)
	values
		(@userInfo,@opTime,@action,@description,@typeID,@applicationID)
END


GO
/****** Object:  StoredProcedure [dbo].[DPanther_SearchAgg]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_SearchAgg]
(
@aggregation    nvarchar(50),
@citation		nvarchar(500),
@Type			varchar(50),
@StartYear		varchar(8)=1800,
@EndYear		varchar(8)=2050
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @str varchar(8000)
	if ISNULL(@citation,'')=''
	set @citation='""'
	if ISNULL(@aggregation,'')=''
	set @aggregation='""'
	--ISNULL(@Type,'""')
	
	
	set @str='SELECT b.[ItemID],[Author],b.[Spatial_KML],b.AggregationCodes,b.[PubDate],b.[Donor] as contributor
      ,[CreateDate],b.[VID],a.[Notes] as description     
      ,[AssocFilePath],[Link] as downloadLink,a.[Format],[Identifier]
      ,[Spatial_Coverage] as publishPlace,[PubYear] as publishYear,a.[Other_Citation] as copyright
      ,[Subject_Keyword] as subjects,[FullCitation],[MainThumbnail] as thumbnail
      ,b.[Title],a.[Type] ,[MainLatitude],[MainLongitude],a.[BibID],b.[Publisher]
		FROM [dbo].[SobekCM_Item] b
		left join [dbo].[SobekCM_Metadata_Basic_Search_Table] a
		on a.ItemID=b.ItemID
		left join SobekCM_Item_Group c on b.GroupID=c.GroupID'
		
	if(@citation<>'""')
	begin	
		set @str=@str+	
			' where a.FullCitation LIKE ''%'+@citation+'%'' and b.AggregationCodes LIKE ''%'+@aggregation+'%'''	
	end
	else	
	begin
		set @str=@str+				
				' where b.AggregationCodes LIKE ''%'+@aggregation+'%'''
	end
		
	if(@Type<>'')
	begin	
		if (@StartYear<>'')
		
		begin	
		    --cast(@StartYear as int)	
		     
			if (@EndYear<>'')
			begin
			set @str=@str+				
				' and CONTAINS(a.Type,'' '+@type+ ''')'+
				' and b.PubYear>='''+@StartYear+''' and b.PubYear<='''+@EndYear+''''
			end
			--end year=''
			else
			begin
			set @str=@str+				
				' and CONTAINS(a.Type,'' '+@type+ ''')'+
				' and b.PubYear>='''+@StartYear+''''
			end
		end		
	end
	else
	begin
		if (@StartYear<>'')
		
		begin
			if (@EndYear<>'')
			begin
			set @str=@str+' and b.PubYear>='''+@StartYear+''' and b.PubYear<='''+@EndYear+''''
			end
			--end year=''
			else
			begin
			set @str=@str+' and b.PubYear>='''+@StartYear+''''
			end
		end
	end
	
	
	print @str
	EXEC(@str)
END



GO
/****** Object:  StoredProcedure [dbo].[DPanther_SearchByLatLng]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<MM>
-- Create date: <07/01/2012>
-- Description:	<Display item info by latitude & longitude>
-- =============================================
CREATE PROCEDURE [dbo].[DPanther_SearchByLatLng]
(
@LatStart		varchar(25),
@LatEnd			varchar(25),
@LngStart		varchar(25),
@LngEnd			varchar(25),
@Type			varchar(50),
@StartYear		varchar(8)=1800,
@EndYear		varchar(8)=2050
)
AS
BEGIN
	SET NOCOUNT ON;
	declare @str varchar(8000)
	
	set @str='SELECT b.[ItemID],[Author],b.[Spatial_KML],b.[PubDate],b.[Donor] as contributor
      ,[CreateDate],b.[VID],[User_Description]+'' ''+[Abstract] as description    
      ,[AssocFilePath],[Link] as downloadLink,a.[Format],[Identifier]
      ,[Publication_Place] as publishPlace,[PubYear] as publishYear,a.[Other_Citation] as copyright
      ,[Subject_Keyword] as subjects,[FullCitation],[MainThumbnail] as thumbnail
      ,b.[Title],a.[Type] ,[MainLatitude],[MainLongitude],a.[BibID], b.[Publisher]
		FROM [dbo].[SobekCM_Item] b
		left join [dbo].[SobekCM_Metadata_Basic_Search_Table] a
		on a.ItemID=b.ItemID
		left join SobekCM_Item_Group c on b.GroupID=c.GroupID		
		where b.MainLatitude>='''+@LatStart+''' and b.MainLatitude<='''+@LatEnd+
				''' and b.MainLongitude>='''+@LngStart+''' and b.MainLongitude<='''+@LngEnd+''''

	if(@Type<>'')
	begin	
		if (@StartYear<>'')
		
		begin	
		    --cast(@StartYear as int)	
		     
			if (@EndYear<>'')
			begin
			set @str=@str+				
				' and CONTAINS(a.Type,'' '+@type+ ''')'+
				' and b.PubYear>='''+@StartYear+''' and b.PubYear<='''+@EndYear+''''
			end
			--end year=''
			else
			begin
			set @str=@str+				
				' and CONTAINS(a.Type,'' '+@type+ ''')'+
				' and b.PubYear>='''+@StartYear+''''
			end
		end		
	end
	else
	begin
	if (@StartYear<>'')
		
		begin	
	     
			if (@EndYear<>'')
			begin
			set @str=@str+								
				' and b.PubYear>='''+@StartYear+''' and b.PubYear<='''+@EndYear+''''
			end
			--end year=''
			else
			begin
			set @str=@str+								
				' and b.PubYear>='''+@StartYear+''''
			end
		end		
	end
	
   EXEC(@str)
END



GO
/****** Object:  Table [dbo].[DP_Applicatons]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DP_Applicatons](
	[applicationID] [int] IDENTITY(1,1) NOT NULL,
	[applicationName] [nvarchar](150) NULL,
	[description] [nvarchar](max) NULL,
	[url] [nvarchar](450) NULL,
	[applicationCode] [nvarchar](50) NULL,
	[centerPoint_X] [nvarchar](max) NULL,
	[centerPoint_Y] [nvarchar](max) NULL,
	[appLogo] [nvarchar](max) NULL,
	[appTitle] [varchar](250) NULL,
	[appType] [varchar](50) NULL,
	[appFooter] [nvarchar](max) NULL,
	[aggregationCode] [nvarchar](50) NULL,
	[appImg] [nvarchar](max) NULL,
 CONSTRAINT [PK_DP_Applicatons] PRIMARY KEY CLUSTERED 
(
	[applicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DP_Directory]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DP_Directory](
	[dirID] [int] IDENTITY(1,1) NOT NULL,
	[dirPath] [varchar](max) NULL,
	[dirName] [varchar](150) NULL,
	[dirDescription] [varchar](500) NULL,
	[webAccess] [bit] NULL,
	[webUrl] [varchar](max) NULL,
	[appID] [int] NULL,
	[dirType] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DP_DirectoryType]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DP_DirectoryType](
	[dirTypeID] [int] IDENTITY(1,1) NOT NULL,
	[dirType] [varchar](150) NULL,
	[dirTypeDesc] [varchar](500) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DP_Identifier]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DP_Identifier](
	[sysID] [int] IDENTITY(1,1) NOT NULL,
	[identifier] [varchar](50) NULL,
	[createDate] [date] NULL,
	[createUser] [varchar](150) NULL,
	[description] [varchar](max) NULL,
	[appID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DP_KML]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DP_KML](
	[kmlid] [int] IDENTITY(1,1) NOT NULL,
	[path] [varchar](150) NULL,
	[description] [varchar](max) NULL,
	[keyword] [varchar](400) NULL,
	[userCreated] [varchar](200) NULL,
	[dateCreated] [varchar](200) NULL,
	[FINum] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DP_Logs]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DP_Logs](
	[idLog] [int] IDENTITY(1,1) NOT NULL,
	[userInfo] [nvarchar](50) NULL,
	[opTime] [datetime] NULL,
	[action] [nvarchar](150) NULL,
	[description] [nvarchar](500) NULL,
	[typeID] [int] NULL,
	[applicationID] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DP_LogTypes]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DP_LogTypes](
	[typeID] [int] IDENTITY(1,1) NOT NULL,
	[type] [nvarchar](50) NULL,
	[description] [nvarchar](250) NULL,
 CONSTRAINT [PK_DP_LogTypes] PRIMARY KEY CLUSTERED 
(
	[typeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[dpView_appCollection]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[dpView_appCollection]
AS
SELECT        dbo.SobekCM_Item_Aggregation.Code, dbo.SobekCM_Item_Aggregation.Name, dbo.SobekCM_Item_Aggregation.ShortName, 
                         dbo.SobekCM_Item_Aggregation_Hierarchy.Search_Parent_Only, dbo.SobekCM_Item_Aggregation_Hierarchy.ChildID, 
                         dbo.SobekCM_Item_Aggregation_Hierarchy.ParentID, dbo.DP_Applicatons.applicationName, dbo.DP_Applicatons.description AS appDesc, dbo.DP_Applicatons.url, 
                         dbo.DP_Applicatons.centerPoint_X, dbo.DP_Applicatons.centerPoint_Y, dbo.SobekCM_Item_Aggregation.Description AS colDesc, dbo.DP_Applicatons.applicationID, 
                         dbo.SobekCM_Item_Aggregation.Type, dbo.DP_Applicatons.appLogo, dbo.DP_Applicatons.appImg, dbo.SobekCM_Item_Aggregation.DateAdded, 
                         dbo.SobekCM_Item_Aggregation.AggregationID
FROM            dbo.SobekCM_Item_Aggregation_Hierarchy INNER JOIN
                         dbo.SobekCM_Item_Aggregation ON dbo.SobekCM_Item_Aggregation_Hierarchy.ChildID = dbo.SobekCM_Item_Aggregation.AggregationID LEFT OUTER JOIN
                         dbo.DP_Applicatons ON dbo.DP_Applicatons.aggregationCode = dbo.SobekCM_Item_Aggregation.Code
WHERE        (dbo.SobekCM_Item_Aggregation.Type <> 'Institution') AND (dbo.SobekCM_Item_Aggregation.Deleted <> '1')



GO
/****** Object:  View [dbo].[dpView_appDirs]    Script Date: 11/19/2015 11:09:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[dpView_appDirs]
AS
SELECT     dbo.DP_Directory.dirPath, dbo.DP_Directory.webUrl, dbo.DP_DirectoryType.dirType, dbo.DP_Applicatons.applicationName, dbo.DP_Applicatons.applicationCode, 
                      dbo.DP_Directory.webAccess, dbo.DP_Directory.dirName
FROM         dbo.DP_Directory INNER JOIN
                      dbo.DP_DirectoryType ON dbo.DP_Directory.dirType = dbo.DP_DirectoryType.dirTypeID INNER JOIN
                      dbo.DP_Applicatons ON dbo.DP_Directory.appID = dbo.DP_Applicatons.applicationID


GO
SET IDENTITY_INSERT [dbo].[DP_Applicatons] ON 

INSERT [dbo].[DP_Applicatons] ([applicationID], [applicationName], [description], [url], [applicationCode], [centerPoint_X], [centerPoint_Y], [appLogo], [appTitle], [appType], [appFooter], [aggregationCode], [appImg]) VALUES (19, N'DPantherAdmin', N'DPanther Administrator site ', NULL, N'dpAdmin', NULL, NULL, NULL, N'DPanther Administrator Site', N'1', NULL, NULL, NULL)
INSERT [dbo].[DP_Applicatons] ([applicationID], [applicationName], [description], [url], [applicationCode], [centerPoint_X], [centerPoint_Y], [appLogo], [appTitle], [appType], [appFooter], [aggregationCode], [appImg]) VALUES (92, N'Unearthing St. Augustine''s Colonial Heritage', N'Unearthing St. Augustine''s Colonial Heritage is a National Endowment for the Humanities grant-funded project to digitally preserve a collection of hidden and fragile resources related to colonial St. Augustine, consisting of 10,000 maps, drawings, photographs and documents available freely online. Partnering with the George A. Smathers Libraries at the University of Florida (UF) to realize this project are the City of St. Augustine departments of Heritage Tourism and the Archaeology Program, the St. Augustine Historical Society, and Government House, which is managed by UF. The project will be a resource for researchers, historians, archaeologists, architects, and historic preservationists, and will help in telling St. Augustine''s unique "story" on a global scale.', N'', N'usach', N'', N'', N'', NULL, N'Collection', NULL, N'usach', N'st.augustine.2.JPG')
SET IDENTITY_INSERT [dbo].[DP_Applicatons] OFF
SET IDENTITY_INSERT [dbo].[DP_Directory] ON 

INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (20, N'\\dpanther01\dpContent\collection', N'dpcollectionTacking', N'Location to upload collection files to the tracking system', 0, N'http://dpanther.fiu.edu/dpContent/collection/', 19, 13)
INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (8, N'\\DPANTHER01\content', N'dpContent', N'digital repository of dpanther', 1, N'http://dpanther.fiu.edu/sobek/content/', 19, 8)
INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (9, N'', N'dpPurlService', N'url to generate purl page by FI number', 1, N'http://dpanther.fiu.edu/dpService/dpPurlService/purl/', 19, 9)
INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (10, N'', N'dpAppService', N'Services to get applications, create, update and delete applications ', 1, N'http://dpanther.fiu.edu/dpService/dpAppService/application/', 19, 9)
INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (11, N'', N'dpCollectionService', N'Services to get digital collections,aggregation code and item statistics info', 1, N'http://dpanther.fiu.edu/dpService/dpCollectionService/collections/', 19, 9)
INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (28, N'/sobek/', N'sobekItemContent', N'location to get the sobek citation content', 1, N'/sobek/', 19, 19)
INSERT [dbo].[DP_Directory] ([dirID], [dirPath], [dirName], [dirDescription], [webAccess], [webUrl], [appID], [dirType]) VALUES (29, N'/dpanther/items/itemdetail', NULL, NULL, 1, N'/dpanther/items/itemdetail', 19, 20)
SET IDENTITY_INSERT [dbo].[DP_Directory] OFF
SET IDENTITY_INSERT [dbo].[DP_DirectoryType] ON 

INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (1, N'kmlHarvest', N'directory for harvesting function')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (2, N'kmlPublish ', N'directory for publishing the content')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (3, N'kmlPublishURL', N'Web URL for end user to retrive the kml files')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (4, N'pubRequest', N'directory for external / cross department publication request')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (5, N'pubTpl', N'directory for publication template')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (6, N'geoporatlTemp', N'directory for geoportal metadata / page template')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (7, N'purlTemp', N'directory for purl page template')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (8, N'colRepo', N'directory for digital collection repository')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (9, N'services', N'directory for services')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (10, N'pubRequestTmp', N'temp file upload directory for external / cross department publication request')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (11, N'imageServer', N'directory for image server root')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (12, N'appGalleryImg', N'directory for application gallery images upload')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (13, N'collectionTacking', N'directory for collections upload to tracking system')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (14, N'sitemapTemp', N'directory for sitemap template')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (15, N'dpProjectTemp', N'directory for dpanther project page')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (16, N'dpWebContent', N'directory for web content when use dPanther as a CMS')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (17, N'dpLog', N'directory for dPanther logs')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (18, N'gisTemp', N'directory for gis project template')
INSERT [dbo].[DP_DirectoryType] ([dirTypeID], [dirType], [dirTypeDesc]) VALUES (19, N'sobekCitationUrl', N'directory to get sobek citation html content')
SET IDENTITY_INSERT [dbo].[DP_DirectoryType] OFF
SET IDENTITY_INSERT [dbo].[DP_LogTypes] ON 

INSERT [dbo].[DP_LogTypes] ([typeID], [type], [description]) VALUES (1, N'dbLog', N'Tacking changes on database')
INSERT [dbo].[DP_LogTypes] ([typeID], [type], [description]) VALUES (2, N'searchLog', N'Tracking changes for search request from cliend side')
INSERT [dbo].[DP_LogTypes] ([typeID], [type], [description]) VALUES (3, N'userLog', N'Tracking user access foot prints')
INSERT [dbo].[DP_LogTypes] ([typeID], [type], [description]) VALUES (4, N'operationLog', N'Tracking regular event during DPanther system operating')
INSERT [dbo].[DP_LogTypes] ([typeID], [type], [description]) VALUES (5, N'fileLog', N'Tracking Files changing on DPanther')
INSERT [dbo].[DP_LogTypes] ([typeID], [type], [description]) VALUES (6, N'errorLog', N'Record system logs and the posted error messages')
SET IDENTITY_INSERT [dbo].[DP_LogTypes] OFF
ALTER TABLE [dbo].[DP_Identifier]  WITH CHECK ADD  CONSTRAINT [FK_DP_Identifier_DP_Applicatons] FOREIGN KEY([appID])
REFERENCES [dbo].[DP_Applicatons] ([applicationID])
GO
ALTER TABLE [dbo].[DP_Identifier] CHECK CONSTRAINT [FK_DP_Identifier_DP_Applicatons]
GO
ALTER TABLE [dbo].[DP_Logs]  WITH CHECK ADD  CONSTRAINT [FK_DP_Logs_DP_Applicatons] FOREIGN KEY([applicationID])
REFERENCES [dbo].[DP_Applicatons] ([applicationID])
GO
ALTER TABLE [dbo].[DP_Logs] CHECK CONSTRAINT [FK_DP_Logs_DP_Applicatons]
GO
ALTER TABLE [dbo].[DP_Logs]  WITH CHECK ADD  CONSTRAINT [FK_DP_Logs_DP_LogTypes] FOREIGN KEY([typeID])
REFERENCES [dbo].[DP_LogTypes] ([typeID])
GO
ALTER TABLE [dbo].[DP_Logs] CHECK CONSTRAINT [FK_DP_Logs_DP_LogTypes]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[39] 4[20] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "SobekCM_Item_Aggregation_Hierarchy"
            Begin Extent = 
               Top = 29
               Left = 652
               Bottom = 208
               Right = 1004
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SobekCM_Item_Aggregation"
            Begin Extent = 
               Top = 25
               Left = 310
               Bottom = 315
               Right = 621
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "DP_Applicatons"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 294
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 19
         Width = 284
         Width = 1500
         Width = 2580
         Width = 2580
         Width = 1020
         Width = 1500
         Width = 915
         Width = 1500
         Width = 1500
         Width = 2790
         Width = 1500
         Width = 1500
         Width = 570
         Width = 1500
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 150
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'dpView_appCollection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'dpView_appCollection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'dpView_appCollection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[20] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DP_Directory"
            Begin Extent = 
               Top = 22
               Left = 372
               Bottom = 284
               Right = 532
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "DP_DirectoryType"
            Begin Extent = 
               Top = 7
               Left = 109
               Bottom = 270
               Right = 269
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_Applicatons"
            Begin Extent = 
               Top = 17
               Left = 646
               Bottom = 263
               Right = 813
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1530
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'dpView_appDirs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'dpView_appDirs'
GO
