# How to apply search and filter for one column in WPF DataGrid (SfDataGrid)

You can apply the searching operation for one column by inheriting SearchHelper class in [WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid) and overriding the SearchCell and FilterRecords methods.

In the sample, the searching operation has been performed for Country column based on the text entered in the text box and when the text box lost its focus or pressed the Enter key in the text box.

Refer to the following code snippets to apply the search filter for one column in SfDataGrid.

**XAML**
```xml
<Grid>
    <Grid.RowDefinitions>
 
        <RowDefinition Height="50"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>
    <WrapPanel Grid.Row="0">
        <StackPanel Width="50" />
        <TextBox x:Name="TextBox" Height="30" Width="100" />
        <StackPanel Width="10" />
    </WrapPanel>
 
    <syncfusion:SfDataGrid x:Name="dataGrid" Grid.Row="1"
                            AutoGenerateColumns="True" 
                            AllowFiltering="True" 
                            AllowResizingColumns="True" 
                            EnableDataVirtualization="True"
                            ShowBusyIndicator="True"
                            AllowEditing="True"              
                            ItemsSource="{Binding Path=ItemSource}"/>
</Grid>
```

**SerachHelperExt.cs**
```csharp
public class SearchHelperExt : SearchHelper
{
    public SearchHelperExt(SfDataGrid datagrid) : base(datagrid)
    {
    }
        
    protected override bool SearchCell(DataColumnBase column, object record, bool ApplySearchHighlightBrush)
    {
        if (column == null)
            return false;
        if (column.GridColumn.MappingName == "Country")
        {
            return base.SearchCell(column, record, ApplySearchHighlightBrush);
        }
        else
            return false;
    }
 
    protected override bool FilterRecords(object dataRow)
    {
        if (string.IsNullOrEmpty(SearchText))
            return true;           
 
        if (this.Provider == null)
            Provider = this.DataGrid.View.GetPropertyAccessProvider();
 
           
        if (MatchSearchTextOpimized("Country", dataRow))
            return true;
            
        return false;
    }
 
    protected virtual bool MatchSearchTextOpimized(string mappingName, object record)
    {
        if (this.DataGrid.View == null || string.IsNullOrEmpty(SearchText))
            return false;
 
        var cellvalue = Provider.GetFormattedValue(record, mappingName);
        if (this.SearchType == SearchType.Contains)
        {
            if (!AllowCaseSensitiveSearch)
                return cellvalue != null && cellvalue.ToString().ToLower().Contains(SearchText.ToString().ToLower());
            else
                return cellvalue != null && cellvalue.ToString().Contains(SearchText.ToString());
        }
        else if (this.SearchType == SearchType.StartsWith)
        {
            if (!AllowCaseSensitiveSearch)
                return cellvalue != null && cellvalue.ToString().ToLower().StartsWith(SearchText.ToString().ToLower());
            else
                return cellvalue != null && cellvalue.ToString().StartsWith(SearchText.ToString());
        }
        else
        {
            if (!AllowCaseSensitiveSearch)
                return cellvalue != null && cellvalue.ToString().ToLower().EndsWith(SearchText.ToString().ToLower());
            else
                return cellvalue != null && cellvalue.ToString().EndsWith(SearchText.ToString());
        }
    }
}
```

**MainWindow.xaml.cs**
```csharp
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.dataGrid.SearchHelper = new SearchHelperExt(this.dataGrid);
        this.TextBox.LostFocus += TextBox_LostFocus;
        this.TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
    }
    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        PerformSearch();
    }
 
    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            PerformSearch();
    }
 
    private void PerformSearch()
    {
        if (this.dataGrid.SearchHelper.SearchText.Equals(this.TextBox.Text))
            return;
 
        var text = TextBox.Text;
        this.dataGrid.SearchHelper.AllowCaseSensitiveSearch = true;
        this.dataGrid.SearchHelper.SearchType = SearchType.StartsWith;
        this.dataGrid.SearchHelper.AllowFiltering = true;
        this.dataGrid.SearchHelper.Search(text);
    }
}
```

Take a moment to peruse the [WPF SfDataGrid Documentation](https://help.syncfusion.com/wpf/datagrid/overview), where you can find about SfDataGrid with code examples.