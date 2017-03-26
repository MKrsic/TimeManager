using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.dateText.Text = DateTime.Now.ToString("HH:mm:ss");
            }, this.Dispatcher);
        }

        databaseFunctions db = new databaseFunctions();

        private void btnPrijava_Click(object sender, RoutedEventArgs e)
        {
            string delay = txtDelay.Text;
            double dblDelay = Double.Parse(delay);
            if (dblDelay < 0)
                MessageBox.Show("Odgoda mora biti pozitivan broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                db.loginTime(-dblDelay);
        }

        private void btnOdjava_Click(object sender, RoutedEventArgs e)
        {
            string delay = txtDelay.Text;
            double dblDelay = Double.Parse(delay);
            if(dblDelay < 0)
                MessageBox.Show("Odgoda mora biti pozitivan broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                db.logoutTime(dblDelay);

        }

        private void btnUkupno_Click(object sender, RoutedEventArgs e)
        {
            int sumMinuta = db.total();
            double sumSati = Convert.ToDouble(sumMinuta / 60.0);
            txtUkupnoMinuta.Text = sumMinuta.ToString();
            txtUkupnoSati.Text = sumSati.ToString();
        }
    }
}
