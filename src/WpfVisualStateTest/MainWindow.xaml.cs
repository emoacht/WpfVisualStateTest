using System;
using System.Diagnostics;
using System.Windows;

namespace WpfVisualStateTest;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void MouseOver_Storyboard_Completed(object sender, EventArgs e)
	{
		Trace.WriteLine("MouseOver Storyboard: Completed");
	}

	private void Unchecked_Storyboard_Completed(object sender, EventArgs e)
	{
		Trace.WriteLine("Unchecked Storyboard: Completed");
	}

	private void Checked_Storyboard_Completed(object sender, EventArgs e)
	{
		Trace.WriteLine("Checked Storyboard: Completed");
	}
}
