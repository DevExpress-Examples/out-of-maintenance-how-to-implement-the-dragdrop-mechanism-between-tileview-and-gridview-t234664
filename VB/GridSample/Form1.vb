Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Globalization
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Tile.ViewInfo
Imports DevExpress.XtraGrid.Views.Tile
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo


Namespace GridSample
    Partial Public Class Form1
        Inherits XtraForm

        Private tileDownHitInfo As TileViewHitInfo = Nothing
        Private gridDownHitInfo As GridHitInfo = Nothing
        Public Sub New()
            InitializeComponent()

            SetUpTileGrid(gridControl1)
            SetUpGrid(gridControl2)
            gridControl1.DataSource = GetTable()
            gridControl2.DataSource = GetTable()
        End Sub

        Private Shared Function GetTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("Dosage", GetType(Integer))
            table.Columns.Add("Drug", GetType(String))
            table.Columns.Add("Patient", GetType(String))
            table.Columns.Add("Date", GetType(Date))

            table.Rows.Add(25, "Indocin", "David", Date.Now)
            table.Rows.Add(50, "Enebrel", "Sam", Date.Now)
            table.Rows.Add(10, "Hydralazine", "Christoff", Date.Now)
            table.Rows.Add(21, "Combivent", "Janet", Date.Now)
            table.Rows.Add(100, "Dilantin", "Melanie", Date.Now)
            Return table
        End Function

        Public Sub SetUpTileGrid(ByVal grid As GridControl)
            grid.AllowDrop = True
            AddHandler grid.DragOver, AddressOf grid_DragOver
            AddHandler grid.DragDrop, AddressOf grid_DragDrop
            Dim view As TileView = TryCast(grid.MainView, TileView)
            view.OptionsBehavior.Editable = False
            AddHandler view.MouseMove, AddressOf tileView_MouseMove
            AddHandler view.MouseDown, AddressOf tileView_MouseDown
        End Sub

        Public Sub SetUpGrid(ByVal grid As GridControl)
            grid.AllowDrop = True
            AddHandler grid.DragOver, AddressOf grid_DragOver
            AddHandler grid.DragDrop, AddressOf grid_DragDrop
            Dim view As GridView = TryCast(grid.MainView, GridView)
            view.OptionsBehavior.Editable = False
            AddHandler view.MouseMove, AddressOf gridView_MouseMove
            AddHandler view.MouseDown, AddressOf gridView_MouseDown
        End Sub

        Private Sub tileView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            Dim view As TileView = TryCast(sender, TileView)
            If view Is Nothing Then
                Return
            End If
            tileDownHitInfo = Nothing
            Dim hitInfo As TileViewHitInfo = view.CalcHitInfo(New Point(e.X, e.Y))
            If Control.ModifierKeys <> Keys.None Then
                Return
            End If
            If e.Button = MouseButtons.Left AndAlso hitInfo.RowHandle >= 0 Then
                tileDownHitInfo = hitInfo
            End If
        End Sub

        Private Sub tileView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            Dim view As TileView = TryCast(sender, TileView)
            If view Is Nothing Then
                Return
            End If
            If e.Button = MouseButtons.Left AndAlso tileDownHitInfo IsNot Nothing Then
                Dim dragSize As Size = SystemInformation.DragSize
                Dim dragRect As New Rectangle(New Point(tileDownHitInfo.HitPoint.X - dragSize.Width \ 2, tileDownHitInfo.HitPoint.Y - dragSize.Height \ 2), dragSize)

                If Not dragRect.Contains(New Point(e.X, e.Y)) Then
                    Dim row As DataRow = view.GetDataRow(tileDownHitInfo.RowHandle)
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move)
                    tileDownHitInfo = Nothing
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = True
                End If
            End If
        End Sub
        Private Sub gridView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            Dim view As GridView = TryCast(sender, GridView)
            If view Is Nothing Then
                Return
            End If
            gridDownHitInfo = Nothing
            Dim hitInfo As GridHitInfo = view.CalcHitInfo(New Point(e.X, e.Y))
            If Control.ModifierKeys <> Keys.None Then
                Return
            End If
            If e.Button = MouseButtons.Left AndAlso hitInfo.RowHandle >= 0 Then
                gridDownHitInfo = hitInfo
            End If
        End Sub

        Private Sub gridView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            Dim view As GridView = TryCast(sender, GridView)
            If view Is Nothing Then
                Return
            End If
            If e.Button = MouseButtons.Left AndAlso gridDownHitInfo IsNot Nothing Then
                Dim dragSize As Size = SystemInformation.DragSize
                Dim dragRect As New Rectangle(New Point(gridDownHitInfo.HitPoint.X - dragSize.Width \ 2, gridDownHitInfo.HitPoint.Y - dragSize.Height \ 2), dragSize)

                If Not dragRect.Contains(New Point(e.X, e.Y)) Then
                    Dim row As DataRow = view.GetDataRow(gridDownHitInfo.RowHandle)
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move)
                    gridDownHitInfo = Nothing
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = True
                End If
            End If
        End Sub
        Private Sub grid_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
            If e.Data.GetDataPresent(GetType(DataRow)) Then
                e.Effect = DragDropEffects.Move
            Else
                e.Effect = DragDropEffects.None
            End If
        End Sub

        Private Sub grid_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
            Dim grid As GridControl = TryCast(sender, GridControl)
            Dim table As DataTable = TryCast(grid.DataSource, DataTable)
            Dim row As DataRow = TryCast(e.Data.GetData(GetType(DataRow)), DataRow)
            If row IsNot Nothing AndAlso table IsNot Nothing AndAlso row.Table IsNot table Then
                table.ImportRow(row)
                row.Delete()
            End If
        End Sub
    End Class
End Namespace