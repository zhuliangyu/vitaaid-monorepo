using MyToolkit.Base.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MyToolkit.Base.Extensions
{
    public static class DataGridExtension
    {
        public static IEnumerable<DataGridRow> GetDataGridRows(this DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
        /// <summary>
        /// Gets the specified cell of the DataGrid
        /// </summary>
        /// <param name="dataGrid1">The DataGrid instance</param>
        /// <param name="row">The row of the cell</param>
        /// <param name="column">The column index of the cell</param>
        /// <returns>A cell of the DataGrid</returns>
        public static DataGridCell GetCell(this DataGrid dataGrid1, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = UIElementHelper.GetVisualChild<DataGridCellsPresenter>(row);
                                                   
                if (presenter == null)
                {
                    dataGrid1.ScrollIntoView(row, dataGrid1.Columns[column]);
                    presenter = UIElementHelper.GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);

                return cell;
            }
            return null;
        }
        /// <summary>
        /// Gets the specified cell of the DataGrid
        /// </summary>
        /// <param name="grid">The DataGrid instance</param>
        /// <param name="row">The row index of the cell</param>
        /// <param name="column">The column index of the cell</param>
        /// <returns>A cell of the DataGrid</returns>
        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            DataGridRow rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }
        /// <summary>
        /// Gets the specified row of the DataGrid
        /// </summary>
        /// <param name="grid">The DataGrid instance</param>
        /// <param name="index">The index of the row</param>
        /// <returns>A row of the DataGrid</returns>
        public static DataGridRow GetRow(this DataGrid grid, int index)
        {
            if (index < 0) return null;
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        /// <summary>
        /// Gets the selected row of the DataGrid
        /// </summary>
        /// <param name="grid">The DataGrid instance</param>
        /// <returns></returns>
        public static DataGridRow GetSelectedRow(this DataGrid grid)
        {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }
        public static DataGridCell TryToFindGridCell(this DataGrid grid, DataGridCellInfo cellInfo)
        {
            DataGridCell result = null;
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(cellInfo.Item);
            if (row != null)
            {
                int columnIndex = grid.Columns.IndexOf(cellInfo.Column);
                if (columnIndex > -1)
                {
                    DataGridCellsPresenter presenter = UIElementHelper.GetVisualChild<DataGridCellsPresenter>(row);
                    result = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                }
            }
            return result;
        }

        public static void HideFields(this DataGridAutoGeneratingColumnEventArgs e, IList<string> Fields)
        {
            e.Cancel = Fields.Contains(e.PropertyName);
        }
        public static void ShowFields(this DataGridAutoGeneratingColumnEventArgs e, IList<string> Fields)
        {
            e.Cancel = !Fields.Contains(e.PropertyName);
        }

    }
}
