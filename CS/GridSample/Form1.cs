using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;

namespace GridSample {
    public partial class Form1 : XtraForm {
        public Form1() {
            InitializeComponent();

            SetUpGrid(gridControl1);
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
        public void SetUpGrid(GridControl grid) {
            ColumnView view = grid.MainView as ColumnView;
            view.OptionsBehavior.Editable = false;
        }
    }
}