<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register TagPrefix="mycontrol" TagName="Banner" Src="~/Views/Banner.ascx" %>

<script runat="server">
    protected void GvDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var row = (sender as GridView).Rows[e.RowIndex];
        string userLogin = row.Cells[1].Text;       

        if (string.Equals(userLogin, Membership.GetUser().UserName) 
           && (new UserContracts.Models.UserRepository()).GetAdminsCount() == 1)
        {
           return;
        }      
        Membership.DeleteUser(userLogin, false);
    }

    protected void GvRowEditing(object sender, GridViewEditEventArgs e)
    {
        (sender as GridView).EditIndex = e.NewEditIndex;
    }

    protected void GvRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id;
        var row = (sender as GridView).Rows[e.RowIndex];
        
        if (int.TryParse(row.Cells[0].Text, out id))
        {
            TextBox mTbox = row.FindControl("TbPsw1") as TextBox;
            DropDownList mType = row.FindControl("DropDownList1") as DropDownList;
            string role = mType.SelectedValue;
            string psw = string.IsNullOrEmpty(mTbox.Text.Trim()) ? null : mTbox.Text.Trim();

            UserContracts.Models.UserRepository _user = new UserContracts.Models.UserRepository();

            if (id == Majestic.Models.Ext.GetUserId())
            {
                if (!string.Equals(role, "Administrator") && _user.GetAdminsCount() == 1)
                {
                    return;
                }
            }
            _user.UpdateUser(id, psw, role);
        }
    }

    protected void GvCancelingEdit(object sender, GridViewCancelEditEventArgs e) {}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>AccountsView</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Accounts</h2>    
        <hr />
    <% using (Html.BeginForm()) { %>
        <div>
                    <label for="username">Login:</label>
                    <%= Html.TextBox("username") %>
                    <%= Html.ValidationMessage("username") %>
                    
                    <label for="password">Password:</label>
                    <%= Html.Password("password") %>
                    <%= Html.ValidationMessage("password") %>                    
                    
                    <label for="ddList">Role:</label>
                    <%=                         
                        Html.DropDownList("ddList"
                        , new List<SelectListItem>()
                          {
                              new SelectListItem()
                              {
                                  Text = "Administrator",
                                  Value = "Administrator"
                              },
                              new SelectListItem()
                              {
                                  Text = "Coordinator",
                                  Value = "Coordinator"
                              },
                              new SelectListItem()
                              {
                                  Text = "Operator",
                                  Value = "Operator"
                              },
                          })
                     %>
                    <input type="submit" value="&#1044;&#1086;&#1073;&#1072;&#1074;&#1080;&#1090;&#1100;" />
        </div>
    <% } %>
    <hr />
       
    <form id="Form1" method="post" runat="server">
    <mycontrol:Banner ID="Banner1" runat="server" />
    <br />
        
    <asp:GridView ID="OperationHistoryList" runat="server"
         DataSourceID="OpHistoryListDS"
         AllowPaging="True"
         PageSize="15"
         AutoGenerateColumns="False"
         OnRowDeleting="GvDeleting"
         OnRowEditing="GvRowEditing"
         OnRowUpdating="GvRowUpdating"
         OnRowCancelingEdit="GvCancelingEdit"
         EnableViewState="False">
         
     <Columns>
        <asp:BoundField HeaderText="&#1048;&#1044;"
                        DataField="ID"
                        SortExpression="ID"
                        ReadOnly="True">
            <ItemStyle Width="5%"></ItemStyle>
        </asp:BoundField>
        
        <asp:BoundField HeaderText="&#1051;&#1086;&#1075;&#1080;&#1085;"
                        DataField="Login"
                        SortExpression="UP"
                        ReadOnly="True">
            <ItemStyle Width="5%"></ItemStyle>
        </asp:BoundField>
               
        <asp:TemplateField HeaderText="&#1055;&#1072;&#1088;&#1086;&#1083;&#1100;" SortExpression="Password">
            <EditItemTemplate>
                  <asp:TextBox runat="server" ID="TbPsw1" TextMode="Password">
                  </asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="LabelPsw" runat="server" Text='<%# Bind("Password") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>        

        <asp:TemplateField HeaderText="Role" SortExpression="Role">
            <EditItemTemplate>
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    DataTextField="Role" DataValueField="Role" 
                >
                <asp:ListItem Value="Operator" Text="&#1057;&#1086;&#1090;&#1088;&#1091;&#1076;&#1085;&#1080;&#1082;"></asp:ListItem>
                <asp:ListItem Value="Coordinator" Text="&#1050;&#1086;&#1086;&#1088;&#1076;&#1080;&#1085;&#1072;&#1090;&#1086;&#1088;"></asp:ListItem>
                <asp:ListItem Value="Administrator" Text="&#1040;&#1076;&#1084;&#1080;&#1085;&#1080;&#1089;&#1090;&#1088;&#1072;&#1090;&#1086;&#1088;"></asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Role") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ShowCancelButton="True">
            <ItemStyle Width="15%"></ItemStyle>
        </asp:CommandField>
        
    </Columns>
    
    <EmptyDataTemplate>&#1047;&#1072;&#1087;&#1080;&#1089;&#1077;&#1081; &#1085;&#1077;&#1090;</EmptyDataTemplate>
    </asp:GridView>

    <asp:ObjectDataSource ID="OpHistoryListDS" runat="server"
         SelectMethod="GetData"
         DeleteMethod="DeleteStub"
         UpdateMethod="UpdateStub"
         TypeName="UserContracts.Models.UserRepository"
         EnableViewState="false">                
    </asp:ObjectDataSource>
       
</form>

</asp:Content>
