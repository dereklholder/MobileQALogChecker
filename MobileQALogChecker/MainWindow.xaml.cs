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
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

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
        }

        /// <summary>
        /// Allows User to browse for a log file, and sets it to the value used for Validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logFileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
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

        /// <summary>
        /// Searched throught he log file for any transactiosn matching the ID provided.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(transactionIDOrderIDBox.Text))
            {
                try
                {
                    xmlTreeView.Items.Clear();

                    DataTools.LogFileParser parser = new DataTools.LogFileParser(logFileBrowserPathText.Text);

                    transactionXMLTextBox.Text = parser.GetXmlEntries(transactionIDOrderIDBox.Text, out string[] arrayOfXmlStrings);

                    string xmlString = @"<TransactionsMatchingID>" + String.Join(Environment.NewLine, arrayOfXmlStrings) + @"</TransactionsMatchingID>";


                    buildTree(xmlTreeView, XDocument.Parse(xmlString));
                }
                catch (IndexOutOfRangeException)
                {
                    System.Windows.MessageBox.Show("Invalid ID provided for Transaction", "Error", MessageBoxButton.OK);

                }
                catch (Exception ex)
                {
                    //Some other exception Occured...
                    System.Windows.MessageBox.Show("Unexpected Error Occured" + Environment.NewLine + ex.Message, "Error", MessageBoxButton.OK);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("You Must Provide an OrderID or TransactionID", "Error", MessageBoxButton.OK);
            }
                    
        }

        /// <summary>
        /// Creates the XmlTree used to Display the response XML in an easily readable format
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="doc"></param>
        private void buildTree(System.Windows.Controls.TreeView treeView, XDocument doc)
        {
            TreeViewItem treeNode = new TreeViewItem
            {
                Header = doc.Root.Name.LocalName,
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            buildNodes(treeNode, doc.Root, doc);
        }
        //Iterates through this to Build Nodes
        private void buildNodes(TreeViewItem treeNode, XElement element, XDocument doc)
        {
            foreach (XNode child in element.Nodes())
            {
                switch (child.NodeType)
                {
                    case XmlNodeType.Element:
                        XElement childElement = child as XElement;
                        TreeViewItem childTreeNode = new TreeViewItem
                        {
                            //Get First attribute where it is equal to value
                            Header = childElement.Name,
                            //Automatically expand elements
                            IsExpanded = false
                        };
                        treeNode.Items.Add(childTreeNode);
                        //If its the Gateway Response Node, Get the TransactionType (if Possible) and add it to the label, for clarity.
                        if (childTreeNode.Header.ToString() == "GatewayResponse")
                        {
                            try
                            {
                                var childNodeList = childElement.Descendants().ToList();
                                string tranTypeString = childNodeList.Find(x => x.Name.ToString().Contains("TransactionType")).Value.ToString();
                                childTreeNode.Header = "GatewayResponse - TransactionType: " + tranTypeString;
                            }
                            catch
                            {
                                childTreeNode.Header = "GatewayResponse - TransactionType: Update";
                            }

                        }
                        buildNodes(childTreeNode, childElement, doc);
                        break;
                    case XmlNodeType.Text:
                        XText childText = child as XText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value, });
                        break;
                }
            }

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            xmlTreeView.Items.Clear();
            transactionXMLTextBox.Text = "";
        }
    }

}
