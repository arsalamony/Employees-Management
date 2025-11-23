
namespace EmployeesManagement.Gui.HomeGui
{
    public partial class HomeUserControl : UserControl
    {
        private static  HomeUserControl? homeUserControl;
        public HomeUserControl()
        {
            InitializeComponent();
            //labelWelcome.Text = $"مرحبا بك {LocalUser.FullName}";
           // labelCompnayName.Text = Properties.Settings.Default.CompanyName;
        }

        public static HomeUserControl Instance()
        {
            return homeUserControl??(homeUserControl= new HomeUserControl());
        }
    }
}
