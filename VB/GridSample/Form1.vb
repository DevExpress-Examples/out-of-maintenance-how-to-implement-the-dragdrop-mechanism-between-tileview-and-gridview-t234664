Imports System
Imports System.Data
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base

Namespace GridSample
	Partial Public Class Form1
		Inherits XtraForm

		Public Sub New()
			InitializeComponent()

			SetUpGrid(gridControl1)
			SetUpGrid(gridControl2)
			gridControl1.DataSource = GetTable()
			gridControl2.DataSource = GetTable()
		End Sub
		Private Shared Function GetTable() As DataTable
			Dim table As New DataTable()
			table.Columns.Add("Dosage", GetType(Integer))
			table.Columns.Add("Drug", GetType(String))
			table.Columns.Add("Patient", GetType(String))
			table.Columns.Add("Date", GetType(DateTime))

			table.Rows.Add(25, "Indocin", "David", DateTime.Now)
			table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now)
			table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now)
			table.Rows.Add(21, "Combivent", "Janet", DateTime.Now)
			table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now)
			Return table
		End Function
		Public Sub SetUpGrid(ByVal grid As GridControl)
			Dim view As ColumnView = TryCast(grid.MainView, ColumnView)
			view.OptionsBehavior.Editable = False
		End Sub
	End Class
End Namespace