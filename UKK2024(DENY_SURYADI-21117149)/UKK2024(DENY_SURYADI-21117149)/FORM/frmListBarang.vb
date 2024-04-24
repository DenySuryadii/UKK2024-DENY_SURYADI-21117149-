Public Class frmListBarang

    Dim Tampung As New DataTable
    Public NilaiKiriman As Integer

    Sub TampilTabel()
        Tampung = EksekusiSQL("select * from barang")
        GridControl1.DataSource = Tampung
        GridView1.BestFitColumns()

    End Sub


    Sub Pilih()
        Select Case NilaiKiriman
            Case 1
                'kembalikan data ke pembelian
                frmTransaksiPembelian.txtCari.Text = GridView1.GetFocusedRowCellValue("ID_BARANG")
                frmTransaksiPembelian.TampilItemBarang()
                Me.Close()

            Case 2
                frmTransaksiPembelian.txtCari.Text = GridView1.GetFocusedRowCellValue("ID_BARANG")
                frmTransaksiPembelian.TampilItemBarang()
                Me.Close()
        End Select
    End Sub

    Sub Tutup()
        Me.Close()


    End Sub

    Private Sub frmListBarang_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                Pilih()
            Case Keys.Escape
                Tutup()


        End Select
    End Sub

    Private Sub frmListBarang_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        EksekusiRapikanGridView(GridView1)
        GridView1.FindFilterText = Nothing 'kosongin pencarian
        TampilTabel()


    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Pilih()

    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Pilih()

    End Sub

    Private Sub BarButtonItem6_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        Tutup()
    End Sub
End Class