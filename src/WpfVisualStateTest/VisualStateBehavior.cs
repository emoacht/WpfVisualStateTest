using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace WpfVisualStateTest;

[TypeConstraint(typeof(ToggleButton))]
public class VisualStateBehavior : Behavior<ToggleButton>
{
	protected override void OnAttached()
	{
		base.OnAttached();

		this.AssociatedObject.MouseEnter += OnIsMouseOverChanged;
		this.AssociatedObject.MouseLeave += OnIsMouseOverChanged;
		this.AssociatedObject.Checked += OnIsCheckedChanged;
		this.AssociatedObject.Unchecked += OnIsCheckedChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();

		this.AssociatedObject.MouseEnter -= OnIsMouseOverChanged;
		this.AssociatedObject.MouseLeave -= OnIsMouseOverChanged;
		this.AssociatedObject.Checked -= OnIsCheckedChanged;
		this.AssociatedObject.Unchecked -= OnIsCheckedChanged;
	}

	private void OnIsMouseOverChanged(object sender, MouseEventArgs e)
	{
		Trace.WriteLine($"IsMouseOver Property: {this.AssociatedObject.IsMouseOver}");
		CheckVisualState(this.AssociatedObject);
	}

	private void OnIsCheckedChanged(object sender, RoutedEventArgs e)
	{
		Trace.WriteLine($"IsChecked Property: {this.AssociatedObject.IsChecked}");
		CheckVisualState(this.AssociatedObject);
	}

	private static void CheckVisualState(FrameworkElement element)
	{
		foreach (var group in EnumerateVisualStateGroups(element))
			Trace.WriteLine($"VisualStateGroup: {group.Name} VisualState: {group.CurrentState?.Name}");
	}

	private static IEnumerable<VisualStateGroup> EnumerateVisualStateGroups(FrameworkElement element)
	{
		if (VisualTreeHelper.GetChildrenCount(element) <= 0) // If the ControlTemplate has not been applied yet
			yield break;

		foreach (var descendant in EnumerateDescendants(element).OfType<FrameworkElement>())
		{
			var groups = VisualStateManager.GetVisualStateGroups(descendant)?.Cast<VisualStateGroup>();
			if (groups is not null)
			{
				foreach (var group in groups)
					yield return group;
			}
		}
	}

	private static IEnumerable<DependencyObject> EnumerateDescendants(DependencyObject reference)
	{
		if (reference is null)
			yield break;

		var queue = new Queue<DependencyObject>();

		do
		{
			var parent = (queue.Count == 0) ? reference : queue.Dequeue();

			var count = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				queue.Enqueue(child);

				yield return child;
			}
		}
		while (queue.Count > 0);
	}
}