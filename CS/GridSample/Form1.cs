using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Tile.ViewInfo;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;


namespace GridSample
{
    public partial class Form1 : XtraForm
    {
        TileViewHitInfo tileDownHitInfo = null;
        GridHitInfo gridDownHitInfo = null;
        public Form1()
        {
            InitializeComponent();

            SetUpTileGrid(gridControl1);
            SetUpGrid(gridControl2);
            gridControl1.DataSource = GetTable();
            gridControl2.DataSource = GetTable();
        }

        static DataTable GetTable() {
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            return table;
        }

        public void SetUpTileGrid(GridControl grid) {
            grid.AllowDrop = true;
            grid.DragOver += new System.Windows.Forms.DragEventHandler(grid_DragOver);
            grid.DragDrop += new System.Windows.Forms.DragEventHandler(grid_DragDrop);
            TileView view = grid.MainView as TileView;
            view.OptionsBehavior.Editable = false;
            view.MouseMove += new System.Windows.Forms.MouseEventHandler(tileView_MouseMove);
            view.MouseDown += new System.Windows.Forms.MouseEventHandler(tileView_MouseDown);
        }

        public void SetUpGrid(GridControl grid) {
            grid.AllowDrop = true;
            grid.DragOver += new System.Windows.Forms.DragEventHandler(grid_DragOver);
            grid.DragDrop += new System.Windows.Forms.DragEventHandler(grid_DragDrop);
            GridView view = grid.MainView as GridView;
            view.OptionsBehavior.Editable = false;
            view.MouseMove += new System.Windows.Forms.MouseEventHandler(gridView_MouseMove);
            view.MouseDown += new System.Windows.Forms.MouseEventHandler(gridView_MouseDown);
        }
        
        private void tileView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            TileView view = sender as TileView;
            if (view == null) return;
            tileDownHitInfo = null;
            TileViewHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                tileDownHitInfo = hitInfo;
        }

        private void tileView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            TileView view = sender as TileView;
            if (view == null) return;
            if (e.Button == MouseButtons.Left && tileDownHitInfo != null) {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(tileDownHitInfo.HitPoint.X - dragSize.Width / 2,
                    tileDownHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y))) {
                    DataRow row = view.GetDataRow(tileDownHitInfo.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    tileDownHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }
        private void gridView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            GridView view = sender as GridView;
            if (view == null) return;
            gridDownHitInfo = null;
            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                gridDownHitInfo = hitInfo;
        }

        private void gridView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            GridView view = sender as GridView;
            if (view == null) return;
            if (e.Button == MouseButtons.Left && gridDownHitInfo != null) {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(gridDownHitInfo.HitPoint.X - dragSize.Width / 2,
                    gridDownHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y))) {
                    DataRow row = view.GetDataRow(gridDownHitInfo.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    gridDownHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }
        private void grid_DragOver(object sender, System.Windows.Forms.DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(DataRow)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void grid_DragDrop(object sender, System.Windows.Forms.DragEventArgs e) {
            GridControl grid = sender as GridControl;
            DataTable table = grid.DataSource as DataTable;
            DataRow row = e.Data.GetData(typeof(DataRow)) as DataRow;
            if (row != null && table != null && row.Table != table) {
                table.ImportRow(row);
                row.Delete();
            }
        }
    }
}