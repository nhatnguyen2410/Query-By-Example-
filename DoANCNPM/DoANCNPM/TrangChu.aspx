<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="DoANCNPM.TrangChu" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="wrapper-all">

        <div id="create-show_query">
            <div class="title_baocao">
                <label>Tiêu đề báo cáo:</label>
                <asp:TextBox ID="txtTieuDeBaoCao"  runat="server" CssClass="InPutTitleBaoCao"></asp:TextBox>
            </div>

            <asp:TextBox ID="txtNDQuery" runat="server" CssClass="form_query"  TextMode="MultiLine" placeholder="Query created will appear here..." OnTextChanged="txtNDQuery_TextChanged"></asp:TextBox>&nbsp;
            <asp:Label ID="labelLoi" CssClass="lbLoi" runat="server" Text="Label" Visible="False"></asp:Label>
            <div class="button-xuly">
                <span class="list-button">
                    <asp:Button ID="btnQuery" runat="server" Text="Tạo Query" CssClass="btn btn-success btn-taoQuery" OnClick="btnQuery_Click" />
                    <asp:Button ID="btnRP" runat="server" Text="Tạo Report" CssClass="btn btn-success btn-taoReport" OnClick="btnRP_Click" />
                </span>
            </div>
        </div>
        <div class="wrapper-column-table">
            <div id="table-list" aria-expanded="undefined">
                <label class="lb-listTable">
                    L<span class="list-button"><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QLVT_DHConnectionString %>" SelectCommand="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE 'sys%' AND TABLE_NAME NOT LIKE 'MS%'" OnSelecting="SqlDataSource1_Selecting"></asp:SqlDataSource>
                    </span>ist Table</label>
                <asp:CheckBoxList ID="CheckBoxListTable" runat="server" BorderStyle="Ridge" DataSourceID="SqlDataSource1" DataTextField="TABLE_NAME" DataValueField="TABLE_NAME" Height="307px" Style="margin-left: 22px; margin-top: 7px" ViewStateMode="Enabled" Width="153px" BackColor="White" AutoPostBack="True" OnSelectedIndexChanged="CheckBoxListTable_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                </asp:CheckBoxList>

            </div>
            <div id="column-list">
                <label class="lb-listColumn">List Column</label>
                <asp:CheckBoxList ID="CheckBoxListColumn" runat="server" AutoPostBack="True" Height="342px" OnSelectedIndexChanged="CheckBoxListColumn_SelectedIndexChanged" Style="margin-left: 17px; margin-top: 9px; color: #fff" Width="947px">
                </asp:CheckBoxList>
            </div>
        </div>
        <div id="show_column-choose" style="margin-right: 4px">
            <asp:GridView ID="TBColumnChoosed" class="table-wrapper-scroll-y" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Height="245px" Style="margin: 2% 5%;width:90%;"  >
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="Trạng Thái">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server">
                                <asp:ListItem Text="NONE" Value="None"></asp:ListItem>
                                <asp:ListItem Text="COUNT" Value="COUNT"></asp:ListItem>
                                <asp:ListItem Text="MIN" Value="MIN"></asp:ListItem>
                                <asp:ListItem Text="MAX" Value="MAX"></asp:ListItem>
                                <asp:ListItem Text="AVG" Value="AVG"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Điều Kiện">
                        <ItemTemplate>
                                 <asp:TextBox ID="TextBoxDieuKien" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sắp xếp">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList2" runat="server">
                             
                                <asp:ListItem Text="NONE" Value="NONE"></asp:ListItem>                             
                                <asp:ListItem Text="Tăng Dần" Value="ASC"></asp:ListItem>
                                <asp:ListItem Text="Giảm Dần" Value="DESC"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Height="32px" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="32px" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>
    </div>
    <script>
       
    </script>
</asp:Content>
