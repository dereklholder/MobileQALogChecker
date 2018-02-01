using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.IO;

namespace MobileQALogChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string LogFileContent;
        
        public MainWindow()
        {
            InitializeComponent();
            fieldsToValidateListView.DataContext = new DataTools.FieldDataViewModel();
        }
        
        /// <summary>
        /// Allows User to browse for a log file, and sets it to the value used for Validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logFileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var someStuff = fieldsToValidateListView.ItemsSource;
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LogFileContent = System.IO.File.ReadAllText(ofd.FileName);
                    logFileBrowserPathText.Text = ofd.FileName;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("An Error Occured Opening the log File", "Error", MessageBoxButton.OK);
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            else
            {
                logFileBrowserPathText.Text = String.Empty;
            }
        }
    }
}
