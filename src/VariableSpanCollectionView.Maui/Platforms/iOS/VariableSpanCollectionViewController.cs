using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;
using UIKit;

namespace VariableSpanCollectionView.Maui
{
	public class VariableSpanCollectionViewController<TItemsView> : ReorderableItemsViewController<TItemsView>
		where TItemsView : VariableSpanCollectionView
	{
		public VariableSpanCollectionViewController(TItemsView variableSpanCollectionView, ItemsViewLayout layout)
			: base(variableSpanCollectionView, layout)
		{
		}

		protected override UICollectionViewDelegateFlowLayout CreateDelegator()
		{
			return new VariableSpanCollectionViewDelegator<TItemsView, VariableSpanCollectionViewController<TItemsView>>(ItemsViewLayout, this);
		}

		protected override void UpdateDefaultCell(DefaultCell cell, NSIndexPath indexPath)
		{
			base.UpdateDefaultCell(cell, indexPath);
			if (ItemsView.SelectionMode == SelectionMode.None)
			{
				cell.SelectedBackgroundView = null;
			}
		}

		protected override void UpdateTemplatedCell(TemplatedCell cell, NSIndexPath indexPath)
		{
			base.UpdateTemplatedCell(cell, indexPath);
			if (ItemsView.SelectionMode == SelectionMode.None)
			{
				cell.SelectedBackgroundView = null;
			}
		}
	}
}