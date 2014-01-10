<%@ Page Language="C#" AutoEventWireup="true" Inherits="NTTData.Redirector.ClientUI.RedirectManagerPage" %>

<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.HtmlControls"
    TagPrefix="sc" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" TagPrefix="sc" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls.Ribbons"
    TagPrefix="sc" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ca" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Sitecore</title>
    <sc:Stylesheet Src="Content Manager.css" DeviceDependant="true" runat="server" />
    <sc:Stylesheet Src="Ribbon.css" DeviceDependant="true" runat="server" />
    <sc:Stylesheet Src="Grid.css" DeviceDependant="true" runat="server" />
    <sc:Script Src="/sitecore/shell/Controls/InternetExplorer.js" runat="server" />
    <sc:Script Src="/sitecore/shell/Controls/Sitecore.js" runat="server" />
    <sc:Script Src="/sitecore/shell/Controls/SitecoreObjects.js" runat="server" />
    <sc:Script Src="/sitecore/shell/Applications/Content Manager/Content Editor.js" runat="server" />
    <style type="text/css">
        html body
        {
            overflow: hidden;
        }
    </style>
    <script type="text/javascript" language="javascript">

        function Redirects_onDoubleClick(sender, eventArgs) {
            scForm.postRequest("", "", "", "redirects:edit");
        }

        function OnResize() {
            var doc = $(document.documentElement);
            var ribbon = $("RibbonContainer");
            var grid = $("GridContainer");

            grid.style.height = doc.getHeight() - ribbon.getHeight() + 'px';
            grid.style.width = doc.getWidth() + 'px';

            Redirects.render();

            /* re-render again after some "magic amount of time" - without this second re-render grid doesn't pick correct width sometimes */
            setTimeout("Redirects.render()", 150);
        }

        function refresh() {
            Redirects.scHandler.refresh();
        }

        window.onresize = OnResize;
  
    </script>
</head>
<body style="background: transparent; height: 100%">
    <form id="RedirectManagerForm" runat="server">
    <sc:AjaxScriptManager runat="server" />
    <sc:ContinuationManager runat="server" />
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td id="RibbonContainer">
                <sc:Ribbon runat="server" ID="Ribbon" />
            </td>
        </tr>
        <tr>
            <td id="GridCell" height="100%" valign="top" style="background: #e9e9e9">
                <sc:Border runat="server" ID="GridContainer">
                    <ca:Grid ID="Redirects" AutoFocusSearchBox="false" RunningMode="Callback" CssClass="Grid"
                        ShowHeader="true" HeaderCssClass="GridHeader" FillContainer="true" FooterCssClass="GridFooter"
                        GroupByText="" GroupingNotificationText="" GroupByCssClass="GroupByCell" GroupByTextCssClass="GroupByText"
                        GroupBySortAscendingImageUrl="group_asc.gif" GroupBySortDescendingImageUrl="group_desc.gif"
                        GroupBySortImageWidth="10" GroupBySortImageHeight="10" GroupingNotificationTextCssClass="GridHeaderText"
                        GroupingPageSize="5" ManualPaging="true" PageSize="15" PagerStyle="Slider" PagerTextCssClass="GridFooterText"
                        PagerButtonWidth="41" PagerButtonHeight="22" PagerImagesFolderUrl="/sitecore/shell/themes/standard/componentart/grid/pager/"
                        ShowSearchBox="true" SearchTextCssClass="GridHeaderText" SearchBoxCssClass="SearchBox"
                        SliderHeight="20" SliderWidth="150" SliderGripWidth="9" SliderPopupOffsetX="20"
                        SliderPopupClientTemplateId="SliderTemplate" TreeLineImagesFolderUrl="/sitecore/shell/themes/standard/componentart/grid/lines/"
                        TreeLineImageWidth="22" TreeLineImageHeight="19" PreExpandOnGroup="false" ImagesBaseUrl="/sitecore/shell/themes/standard/componentart/grid/"
                        IndentCellWidth="22" LoadingPanelClientTemplateId="LoadingFeedbackTemplate" LoadingPanelPosition="MiddleCenter"
                        Width="100%" Height="100%" runat="server">
                        <Levels>
                            <ca:GridLevel DataKeyField="scGridID" ShowTableHeading="false" ShowSelectorCells="false"
                                RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif" DataCellCssClass="DataCell"
                                HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="13"
                                SortImageHeight="13">
                                <Columns>
                                    <ca:GridColumn DataField="scGridID" Visible="false" IsSearchable="false" />
                                    <ca:GridColumn DataField="Name" Visible="false" IsSearchable="false" />
                                    <ca:GridColumn DataField="ItemIcon" Visible="false" IsSearchable="false" />
                                    <ca:GridColumn DataField="NewPath" AllowSorting="false" IsSearchable="true" AllowGrouping="false"
                                        SortedDataCellCssClass="SortedDataCell" HeadingText="Item" DataCellClientTemplateId="LocalNameTemplate" />
                                    <ca:GridColumn DataField="Site" AllowSorting="false" IsSearchable="true" AllowGrouping="false"
                                        SortedDataCellCssClass="SortedDataCell" HeadingText="Site" />
                                    <ca:GridColumn DataField="OldPath" AllowSorting="false" IsSearchable="true" AllowGrouping="false"
                                        SortedDataCellCssClass="SortedDataCell" HeadingText="Old Url" />
                                    <ca:GridColumn DataField="QueryString" AllowSorting="false" IsSearchable="true" AllowGrouping="false"
                                        SortedDataCellCssClass="SortedDataCell" HeadingText="Query String" />
                                    <%--
                                    <ca:GridColumn DataField="Portrait" Visible="false" IsSearchable="false" />
                                    <ca:GridColumn DataField="LocalName" AllowSorting="false" IsSearchable="true" AllowGrouping="false"
                                        SortedDataCellCssClass="SortedDataCell" HeadingText="User Name" DataCellClientTemplateId="LocalNameTemplate" />
                                    <ca:GridColumn DataField="Domain" AllowSorting="false" IsSearchable="false" AllowGrouping="false"
                                        SortedDataCellCssClass="SortedDataCell" HeadingText="Domain" />
                                    <ca:GridColumn DataField="DisplayName" AllowSorting="false" IsSearchable="false"
                                        AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Fully Qualified Name" />
                                    <ca:GridColumn DataField="Profile.FullName" AllowSorting="false" IsSearchable="false"
                                        AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Full Name"
                                        DataCellServerTemplateId="FullNameTemplate" />
                                    <ca:GridColumn DataField="Profile.Email" AllowSorting="false" IsSearchable="false"
                                        AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Email" />
                                    <ca:GridColumn DataField="Profile.Comment" AllowSorting="false" IsSearchable="false"
                                        AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Comment"
                                        DataCellServerTemplateId="CommentTemplate" />
                                    <ca:GridColumn DataField="Profile.ClientLanguage" AllowSorting="false" IsSearchable="false"
                                        AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Language" />
                                    <ca:GridColumn DataField="Profile.State" AllowSorting="false" IsSearchable="false"
                                        AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Locked" />
                                    <ca:GridColumn DataField="ItemIcon" Visible="false" IsSearchable="false" />--%>
                                </Columns>
                            </ca:GridLevel>
                        </Levels>
                        <ClientEvents>
                            <ItemDoubleClick EventHandler="Redirects_onDoubleClick" />
                        </ClientEvents>
                        <ServerTemplates>
                            <ca:GridServerTemplate ID="CommentTemplate">
                                <Template>
                                    <asp:Label ID="CommentLabel" runat="server" />
                                </Template>
                            </ca:GridServerTemplate>
                            <ca:GridServerTemplate ID="FullNameTemplate">
                                <Template>
                                    <asp:Label ID="FullNameLabel" runat="server" />
                                </Template>
                            </ca:GridServerTemplate>
                        </ServerTemplates>
                        <ClientTemplates>
                            <ca:ClientTemplate ID="LocalNameTemplate">
                                <img src="## DataItem.GetMember('ItemIcon').Value ##" width="16"
                                    height="16" border="0" alt="" align="absmiddle" />
                                ## DataItem.GetMember('NewPath').Value ##
                            </ca:ClientTemplate>
                            <ca:ClientTemplate ID="LoadingFeedbackTemplate">
                                <table cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td style="font-size: 10px;">
                                            <sc:Literal Text="Loading..." runat="server" />
                                            ;
                                        </td>
                                        <td>
                                            <img src="/sitecore/shell/themes/standard/componentart/grid/spinner.gif" width="16"
                                                height="16" border="0">
                                        </td>
                                    </tr>
                                </table>
                            </ca:ClientTemplate>
                            <ca:ClientTemplate ID="SliderTemplate">
                                <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                            <div style="padding: 4px; font: 8pt tahoma; white-space: nowrap; overflow: hidden">
                                                ## DataItem.GetMember('NewPath').Value ##</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 14px; background-color: #666666; padding: 1px 8px 1px 8px; color: white">
                                            ## DataItem.PageIndex + 1 ## / ## Redirects.PageCount ##
                                        </td>
                                    </tr>
                                </table>
                            </ca:ClientTemplate>
                        </ClientTemplates>
                    </ca:Grid>
                </sc:Border>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
