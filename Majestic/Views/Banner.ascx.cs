using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Majestic.Views
{
    public partial class Banner : System.Web.Mvc.ViewUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkTime_Click(object sender, EventArgs e)
        {
            tc1.BackColor = Color.Green;
            tc1.Width = Unit.Percentage(95);
        }
    }
}