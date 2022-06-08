using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace searchstring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
			string[] lines = File.ReadAllLines("../../../sonnets.txt");
			TextBox searchBox = (TextBox)FindName("SearchBox");
			string SearchPhrase = searchBox.Text;

			if (SearchPhrase != null && SearchPhrase != "")
			{

				// BruteSearch
				Stopwatch sw = new Stopwatch();
				sw.Start();
				int bruteForce = BruteForce(lines, SearchPhrase);
				sw.Stop();
				Label lbl = (Label)FindName("BFlabel");
				lbl.Content = "Count: " + bruteForce + ", Time (sec): " + sw.Elapsed.TotalSeconds;


				// KMP
				string text = File.ReadAllText("../../../shakespeare-sonnets.txt");
				Stopwatch sw2 = new Stopwatch();
				sw2.Start();
				int[] kmpResults = KMP(text, SearchPhrase);
				sw2.Stop();
				Label kmpLbl = (Label)FindName("KMPlabel");
				kmpLbl.Content = "Count: " + kmpResults.Length + ", Time (sec): " + sw2.Elapsed.TotalSeconds;


				// Boyer-Moore
				Stopwatch sw3 = new Stopwatch();
				sw3.Start();
				int bmResult = BoyerMoore(SearchPhrase, text);
				sw3.Stop();
				Label bmLbl = (Label)FindName("BMlabel");
				bmLbl.Content = "Count: " + bmResult + ", Time (sec): " + sw3.Elapsed.TotalSeconds;
			}

		}
	}
}
