using Maui.BindableProperty.Generator.Core;


namespace VariableSpanCollectionView.Maui;

public partial class MenuElement : Element
{
    [AutoBindable]
    readonly string title;
}
