﻿using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGrid_Searching
{ 
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


}
