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

				// BruteForce
				Stopwatch sw = new Stopwatch();
				sw.Start();
				int bruteForce = BruteForce(lines, SearchPhrase);
				sw.Stop();
				Label lbl = (Label)FindName("BFlabel");
				lbl.Content = "Count: " + bruteForce + ", Time (s): " + sw.Elapsed.TotalSeconds;


				// KMP
				string text = File.ReadAllText("../../../sonnets.txt");
				Stopwatch sw2 = new Stopwatch();
				sw2.Start();
				int[] kmpResults = KMP(text, SearchPhrase);
				sw2.Stop();
				Label kmpLbl = (Label)FindName("KMPlabel");
				kmpLbl.Content = "Count: " + kmpResults.Length + ", Time (s): " + sw2.Elapsed.TotalSeconds;


				// Boyer-Moore 
				/*Stopwatch sw3 = new Stopwatch();
				sw3.Start();
				int bmResult = BoyerMoore(SearchPhrase, text);
				sw3.Stop();
				Label bmLbl = (Label)FindName("BMlabel");
				bmLbl.Content = "Count: " + bmResult + ", Time (s): " + sw3.Elapsed.TotalSeconds;*/
			}
		}
		private int BruteForce(string[] lines, string searchPhrase)
		{
			int count = 0;
			for (int i = 0; i < lines.Length; i++)
			{
				string l = lines[i];
				int r = l.IndexOf(searchPhrase);
				while (r > -1)
				{
					count++;
					l = l.Substring(r + searchPhrase.Length);
					r = l.IndexOf(searchPhrase);
				}
			}

			return count;
		}
		public static int[] KMP(string line, string searchPhrase)
		{
			List<int> retVal = new List<int>();
			int M = searchPhrase.Length;
			int N = line.Length;
			int i = 0;
			int j = 0;
			int[] lps = new int[M];

			ComputeLPS(searchPhrase, M, lps);

			while (i < N)
			{
				if (searchPhrase[j] == line[i])
				{
					j++;
					i++;
				}

				if (j == M)
				{
					retVal.Add(i - j);
					j = lps[j - 1];
				}

				else if (i < N && searchPhrase[j] != line[i])
				{
					if (j != 0)
						j = lps[j - 1];
					else
						i = i + 1;
				}
			}

			return retVal.ToArray();
		}

		private static void ComputeLPS(string searchPhrase, int m, int[] lps)
		{
			int len = 0;
			int i = 1;

			lps[0] = 0;

			while (i < m)
			{
				if (searchPhrase[i] == searchPhrase[len])
				{
					len++;
					lps[i] = len;
					i++;
				}
				else
				{
					if (len != 0)
					{
						len = lps[len - 1];
					}
					else
					{
						lps[i] = 0;
						i++;
					}
				}
			}
		}
		/*private static int[] BuildBadCharTable2(char[] needle)
		{
			int[] badShift = new int[256];
			for (int i = 0; i < 256; i++)
			{
				badShift[i] = needle.Length;
			}
			int last = needle.Length - 1;
			for (int i = 0; i < last; i++)
			{
				badShift[(int)needle[i]] = last - i;
			}
			return badShift;
		}

		public static int BoyerMoore(String pattern, String text)
		{
			char[] needle = pattern.ToCharArray();
			char[] haystack = text.ToCharArray();

			if (needle.Length > haystack.Length)
			{
				return -1;
			}
			int[] badShift = BuildBadCharTable2(needle);
			int offset = 1;
			int scan = 0;
			int last = needle.Length - 1;
			int maxoffset = haystack.Length - needle.Length;
			while (offset <= maxoffset)
			{
				for (scan = last; (needle[scan] == haystack[scan + offset]); scan--)
				{
					if (scan == 0)
					{
						return offset;
					}
				}
				offset += badShift[(int)haystack[offset + last]];
			}
			return -1;
		}*/
	}
}
