Public Class frmLaporanDetail

    Dim Tampung As New DataTable
    Public NilaiKiriman As Integer
    Public ID As Integer

    Sub TampilTable()
        Select Case NilaiKiriman
            Case 1
                'untuk detail pembelian
                Tampung = EksekusiSQL("select pembelian_detil.ID_BARANG,NAMA_BARANG,SATUAN,HARGA_SATUAN,JUMLAH,TOTAL_HARGA from pembelian_detil,barang where pembelian_detil.ID_BARANG=barang.ID_BARANG and ID_PEMBELIAN='" & ID & "'")
                GridControl1.DataSource = Tampung
                GridView1.Columns("HARGA_SATUAN").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                GridView1.Columns("HARGA_SATUAN").DisplayFormat.FormatString = "c0"
                GridView1.Columns("JUMLAH").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                GridView1.Columns("JUMLAH").DisplayFormat.FormatString = "n0"
                GridView1.Columns("TOTAL_HARGA").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                GridView1.Columns("TOTAL_HARGA").DisplayFormat.FormatString = "c0"
                GridView1.Columns("TOTAL_HARGA").SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:c0}")
                GridView1.BestFitColumns()
            Case 2
                'untuk ditail penjualan
                Tampung = EksekusiSQL("select penjualan_detil.ID_BARANG,NAMA_BARANG,SATUAN,HARGA_SATUAN,JUMLAH,TOTAL_HARGA from penjualan_detil,barang where penjualan_detil.ID_BARANG=barang.ID_BARANG and ID_PENJUALAN='" & ID & "'")
                GridControl1.DataSource = Tampung
                GridView1.Columns("HARGA_SATUAN").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                GridView1.Columns("HARGA_SATUAN").DisplayFormat.FormatString = "c0"
                GridView1.Columns("JUMLAH").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                GridView1.Columns("JUMLAH").DisplayFormat.FormatString = "n0"
                GridView1.Columns("TOTAL_HARGA").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                GridView1.Columns("TOTAL_HARGA").DisplayFormat.FormatString = "c0"
                GridView1.Columns("TOTAL_HARGA").SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:c0}")
                GridView1.BestFitColumns()
        End Select
    End Sub

    Sub CetakData()
        Select Case NilaiKiriman
            Case 1
                'laporan cetak detil pembelian
                GridView1.OptionsPrint.PrintFilterInfo = True
                GridView1.OptionsPrint.RtfReportHeader = "DETAIL LAPORAN PEMBELIAN" & vbCrLf & "NOMOR TRANSAKSI" & ID & vbCrLf
                TTD(GridView1)
                ExecuteGridControlPreview(GridControl1, GridControl1.LookAndFeel, True, Printing.PaperKind.A4, 15, 15, 15, 15)
            Case 2
                'laporan cetak detil penjualan
                GridView1.OptionsPrint.PrintFilterInfo = True
                GridView1.OptionsPrint.RtfReportHeader = "DETAIL LAPORAN PENJUALAN" & vbCrLf & "NOMOR TRANSAKSI" & ID & vbCrLf
                TTD(GridView1)
                ExecuteGridControlPreview(GridControl1, GridControl1.LookAndFeel, True, Printing.PaperKind.A4, 15, 15, 15, 15)
        End Select
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub frmLaporanDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.P
                CetakData()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub

    Private Sub frmLaporanDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        GridView1.OptionsView.ShowFooter = True 'aktifkan report footer
        TampilTable()

    End Sub

    Private Sub BarButtonItem7_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        CetakData()

    End Sub

    Private Sub BarButtonItem6_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        Tutup()
    End Sub
End Class