using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetObjects.Core;
using NetObjects.Enums;

namespace StartCalculating
{
	public partial class MainWindow : Window
	{
		private static int _nodecount;
		private static int _countofiteration;
		private static int _maxVirus;
		private static int _pofInfective;
		private static int taskcount;
		private static bool _isAdresDiff;
		private static bool _isAppend;
		private static int _addLinks;
		private static int _minLinks;
		//private static string _nettype;
		private static List<CalculationTask> _c;

		public MainWindow()
		{
			InitializeComponent();
			_c = new List<CalculationTask>();
		}

		private void ExpCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			taskcount = (int)ExpCount.Value;
			ExpCountView.Text = taskcount.ToString();
		}

		private void IterationCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_countofiteration = (int)IterationCount.Value;
			IterationCountView.Text = _countofiteration.ToString();
		}

		private void PofinfectiveValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_pofInfective = (int)pofinfective.Value;
			pofinfectiveView.Text = _pofInfective.ToString();
		}

		private void StartCalculate_Click(object sender, RoutedEventArgs e)
		{
		
		}

		private void NodeCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_nodecount = (int)NodeCountSlider.Value;
			NodeCounttextBox.Text = _nodecount.ToString();
		}

		private void IsAdresDifftrue_Checked(object sender, RoutedEventArgs e)
		{
			AdresDifflabel.Visibility = Visibility.Visible;
			CodeRad2radioButton.Visibility = Visibility.Visible;
			SqrtradioButton.Visibility = Visibility.Visible;
			granicalabel.Visibility = Visibility.Visible;
			granicatextbox.Visibility = Visibility.Visible;
			_isAdresDiff = true;
		}

		private void IsAdresDifffalse_Checked(object sender, RoutedEventArgs e)
		{
			AdresDifflabel.Visibility = Visibility.Collapsed;
			CodeRad2radioButton.Visibility = Visibility.Collapsed;
			SqrtradioButton.Visibility = Visibility.Collapsed;
			granicalabel.Visibility = Visibility.Collapsed;
			granicatextbox.Visibility = Visibility.Collapsed;
			_isAdresDiff = false;
		}

		private void SqrtradioButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void CodeRad2radioButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void KeilyMultyLink_Checked(object sender, RoutedEventArgs e)
		{
			AddLinkstextBox.Visibility = Visibility.Visible;
			MinLinkstextBox.Visibility = Visibility.Visible;
			MinLinkslabel.Visibility = Visibility.Visible;
			AddLinkslabel.Visibility = Visibility.Visible;
		}

		private void NotKeilyMultyLink_Checked(object sender, RoutedEventArgs e)
		{
			AddLinkstextBox.Visibility = Visibility.Collapsed;
			MinLinkstextBox.Visibility = Visibility.Collapsed;
			MinLinkslabel.Visibility = Visibility.Collapsed;
			AddLinkslabel.Visibility = Visibility.Collapsed;
		}

		private void SaveExp_Click(object sender, RoutedEventArgs e)
		{
			CalculationTask[] temp = new CalculationTask[(int) ExpCount.Value];
			string nettype = "";
			if (MultyLink.IsChecked != null && (bool) MultyLink.IsChecked)
				nettype = MultyLink.Name;
			if (Keily.IsChecked != null && (bool) Keily.IsChecked)
				nettype = Keily.Name;
			if (Quadro.IsChecked != null && (bool) Quadro.IsChecked)
				nettype = Quadro.Name;
			if (KeilyRegular.IsChecked != null && (bool) KeilyRegular.IsChecked)
				nettype = KeilyRegular.Name;
			if (TriangleIrregular.IsChecked != null && (bool) TriangleIrregular.IsChecked)
				nettype = TriangleIrregular.Name;
			if (TriangleRegular.IsChecked != null && (bool) TriangleRegular.IsChecked)
				nettype = TriangleRegular.Name;
			if (Hexagon.IsChecked != null && (bool) Hexagon.IsChecked)
				nettype = Hexagon.Name;
			if (Net3122.IsChecked != null && (bool) Net3122.IsChecked)
				nettype = Net3122.Name;
			if (nettype == "")
				return;

            //for (int i = 0; i < taskcount; i++)
            //{
            //    temp[i] = new CalculationTask
            //                  {
            //                      PofInfective = _pofInfective,
            //                      NodeCount = _nodecount,
            //                      NetType = nettype,
            //                      IsAdresDiff = _isAdresDiff,
            //                      IsAppend = _isAppend,
            //                      MaxVirus = _maxVirus
            //                  };
            //    if (!_isAdresDiff)
            //    {
            //        temp[i].VirusStrategy = VirusSendStrategy.None.ToString();
            //    }
            //    else
            //    {
            //        //temp[i].VirusStrategy 
            //    }
            //    if (nettype != NetType.Keily.ToString() || nettype != NetType.MultyLink.ToString())
            //    {
            //        temp[i].MinLink = 0;
            //        temp[i].MaxLink = 0;
            //        temp[i].AddToMaxLinkCount = 0;
            //    }
            //    else
            //    {
            //        temp[i].MinLink = _minLinks;
            //        temp[i].AddToMaxLinkCount = _addLinks;
            //        temp[i].MaxLink = _addLinks + _minLinks;
            //    }
            //}
		}

		private void IsAppendtrueChecked(object sender, RoutedEventArgs e)
		{
			_isAppend = true;
		}

		private void IsAppendfalseChecked(object sender, RoutedEventArgs e)
		{
			_isAppend = false;
		}

		private void MaxVirusSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_maxVirus = (int)MaxVirusSlider.Value;
			MaxVirustextBox.Text = _maxVirus.ToString();
		}

		private void MinLinkstextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_minLinks = Convert.ToInt32(MinLinkstextBox.Text);
		}

		private void AddLinkstextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_addLinks = Convert.ToInt32(AddLinkstextBox.Text);
		}
	
	}
}
