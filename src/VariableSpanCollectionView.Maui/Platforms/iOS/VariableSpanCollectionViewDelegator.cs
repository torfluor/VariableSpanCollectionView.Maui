using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;

namespace VariableSpanCollectionView.Maui
{
	public class VariableSpanCollectionViewDelegator<TItemsView, TViewController> : ReorderableItemsViewDelegator<TItemsView, TViewController>
		where TItemsView : VariableSpanCollectionView
		where TViewController : VariableSpanCollectionViewController<TItemsView>
	{
		NSIndexPath _highlightedIndexPath; 
		public VariableSpanCollectionViewDelegator(ItemsViewLayout itemsViewLayout, TViewController itemsViewController)
			: base(itemsViewLayout, itemsViewController)
		{
		}

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			if (ItemsViewLayout is VariableSpanGridViewLayout variableSpanLayout)
			{
				var itemsLayout = variableSpanLayout.ItemsLayout;
				var columnSpan = itemsLayout.ItemSpanLookup?.GetColumnSpan(ViewController.ItemsSource[indexPath]) ?? 1;
				if (columnSpan > 1)
				{
					var itemWidth = (itemsLayout.ItemWidth * columnSpan) + itemsLayout.HorizontalItemSpacing * (columnSpan - 1);
					return new CGSize(itemWidth, itemsLayout.ItemHeight);
				}
				else
				{
					return variableSpanLayout.ItemSize;
				}
			}
			else
			{
				return base.GetSizeForItem(collectionView, layout, indexPath);
			}
		}

		public override UITargetedPreview GetPreviewForHighlightingContextMenu(UICollectionView collectionView, UIContextMenuConfiguration configuration)
		{
			if(configuration != null && configuration.Identifier is NSIndexPath indexPath)
			{
				_highlightedIndexPath = indexPath;
			}
			return MakeTargetedPreview(collectionView, configuration);
		}

		public override UITargetedPreview GetPreviewForDismissingContextMenu(UICollectionView collectionView, UIContextMenuConfiguration configuration)
		{
			if (_highlightedIndexPath == null)
			{
				//return null;
			}
			var preview = MakeTargetedPreview(collectionView, configuration);
			_highlightedIndexPath = null;
			return preview;
		}

		UITargetedPreview MakeTargetedPreview(UICollectionView collectionView, UIContextMenuConfiguration configuration)
		{
			var item = ViewController.ItemsSource[_highlightedIndexPath];
			var cell = collectionView.CellForItem(_highlightedIndexPath);

			return ContextMenu.CreateTargetedPreview(cell, configuration, item, ViewController.ItemsView);
		}

		public override UIContextMenuConfiguration GetContextMenuConfiguration(UICollectionView collectionView, NSIndexPath indexPath, CGPoint point)
		{
			var menuTemplate = ContextMenu.GetMenu(ViewController.ItemsView);

			if (menuTemplate == null)
			{
				return null;
			}

			var content = menuTemplate.CreateContent();

			if (content is Menu menu)
			{
				var item = ViewController.ItemsSource[indexPath];
				BindableObject.SetInheritedBindingContext(menu, item);
				return UIContextMenuConfiguration.Create(indexPath, null, action =>
				{
					return ContextMenu.CreateMenu(menu);
				});
			}
			return null;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{

			var item = ViewController.ItemsSource[indexPath];
			ContextMenu.ExecuteClickCommand(ViewController.ItemsView, item);
		}
		public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
		{

		}
	}
}