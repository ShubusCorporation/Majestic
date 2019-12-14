<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<script runat="server">

    protected void ContractList_Deleting(object sender, GridViewDeleteEventArgs e)
    {
        int id;

        var row = (sender as GridView).Rows[e.RowIndex];

        if (int.TryParse(row.Cells[0].Text, out id))
        {
            MyContractProvider pr = new MyContractProvider();
            pr.Delete(id);
        }
    }

    protected void ContractList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        (sender as GridView).EditIndex = e.NewEditIndex;
    }
    
    protected void ContractList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id;
        var row = (sender as GridView).Rows[e.RowIndex];

        if (int.TryParse(row.Cells[0].Text, out id))
        {
            MyContractProvider pr = new MyContractProvider();
            pr.Update(id);
        }
    }

    protected void ContractList_CancelingEdit(object sender, GridViewCancelEditEventArgs e) {}

    protected void ContractList_Init(object sender, EventArgs e)
    {
        foreach (var col in (sender as GridView).Columns)
        {
            if (col is CommandField)
            {
                var cf = col as CommandField;
                var roles = Roles.GetRolesForUser();
                
                if (roles.Contains("Operator"))
                {
                    cf.Visible = false;
                }
                else
                {
                    cf.ShowEditButton = roles.Contains("Coordinator");
                    cf.ShowDeleteButton = roles.Contains("Administrator");
                }
            }
        }
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>ContractsView</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Contracts</h2>
    
        <form id="ContractForm" method="post" runat="server">
        
         <% if (Roles.IsUserInRole("Operator"))
            {
                using (Html.BeginForm())
                { %>
            <div>
                    <label for="username">Add new contract here:</label>
                    <hr/>
                    <%= Html.TextBox("contract", "", new { style = "width:70%" })%>
                    <%= Html.ValidationMessage("contract")%>
                    <input type="submit" value="&#1044;&#1086;&#1073;&#1072;&#1074;&#1080;&#1090;&#1100;" />
            </div> <hr /> <% }
            } else { %> <hr /> <% } %>
                    
        <asp:GridView ID="ContractList" runat="server"
         DataSourceID="ContractListDS"
         AllowPaging="True"
         PageSize="15"
         AutoGenerateColumns="False"
         OnRowDeleting="ContractList_Deleting"
         OnRowEditing="ContractList_RowEditing"
         OnRowUpdating="ContractList_RowUpdating"
         OnRowCancelingEdit="ContractList_CancelingEdit"
         EnableViewState="False" oninit="ContractList_Init">
         
     <Columns>
        <asp:BoundField HeaderText="&#1048;&#1044; &#1079;&#1072;&#1087;&#1080;&#1089;&#1080;"
                        DataField="ID"
                        SortExpression="ID"
                        ReadOnly="True">
            <ItemStyle Width="5%"></ItemStyle>
        </asp:BoundField>
        
        <asp:BoundField HeaderText="&#1055;&#1086;&#1083;&#1100;&#1079;&#1086;&#1074;&#1072;&#1090;&#1077;&#1083;&#1100;"
                        DataField="User"
                        SortExpression="User"
                        ReadOnly="True">
            <ItemStyle Width="15%"></ItemStyle>
        </asp:BoundField>
        
        <asp:BoundField HeaderText="&#1053;&#1072;&#1079;&#1074;&#1072;&#1085;&#1080;&#1077;"
                        DataField="Title"
                        SortExpression="Title"
                        ReadOnly="True">
            <ItemStyle Width="55%"></ItemStyle>
        </asp:BoundField>
            
        <asp:TemplateField HeaderText="&#1057;&#1090;&#1072;&#1090;&#1091;&#1089;" SortExpression="Status">
            <EditItemTemplate>
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    DataTextField="Status" DataValueField="Role">
                <asp:ListItem Value="Accepted" Text="&#1059;&#1090;&#1074;&#1077;&#1088;&#1078;&#1076;&#1077;&#1085;"></asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:CommandField ShowEditButton="true" ShowDeleteButton="True" ShowCancelButton="True">
            <ItemStyle Width="15%"></ItemStyle>
        </asp:CommandField>
                
    </Columns>
    
    <EmptyDataTemplate>&#1047;&#1072;&#1087;&#1080;&#1089;&#1077;&#1081; &#1085;&#1077;&#1090;</EmptyDataTemplate>
    </asp:GridView>
    
    <asp:ObjectDataSource ID="ContractListDS" runat="server"
         SelectMethod="GetData"
         DeleteMethod="Delete"
         UpdateMethod="Update"
         TypeName="Majestic.Models.ContractRepository"
         EnableViewState="false">                
    </asp:ObjectDataSource>

</form>
</asp:Content>
