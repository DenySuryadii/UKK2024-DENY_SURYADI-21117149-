Public Class frmLaporanPenjualan

    Dim Tampung As New DataTable
    Public NilaiKiriman As Integer

    Sub TampilTable()
        Tampung = EksekusiSQL("select TANGGAL,ID_PENJUALAN,penjualan.ID_PELANGGAN,NAMA_PELANGGAN,TOTAL_PENJUALAN,POTONGAN,METODE_PEMBAYARAN,KETERANGAN,penjualan.ID_PENGGUNA,NAMA_PENGGUNA from penjualan, pelanggan, pengguna where penjualan.ID_PELANGGAN=pelanggan.ID_PELANGGAN and penjualan.ID_PENGGUNA=pengguna.ID_PENGGUNA and TANGGAL between '" & Format(txtTanggalAwal.DateTime, "yyyy-MM-dd") & "' and '" & Format(txtTanggalAkhir.DateTime, "yyyy-MM-dd") & "'")
        GridControl1.DataSource = Tampung
        'disini masih banyak pengaturan
        'memberikan format  pada kolom total_penjualan numrick currency
        GridView1.Columns("TOTAL_PENJUALAN").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        GridView1.Columns("TOTAL_PENJUALAN").DisplayFormat.FormatString = "c0"

        'memberikan total pada kolom total_penjualan dan format  berupa numric currency
        GridView1.Columns("TOTAL_PENJUALAN").SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:c0}")
        GridView1.BestFitColumns()

    End Sub

    Sub DetailLaporan()
        'memanggil frmlaporandetil
        'cek dlu isi tabel
        If GridView1.RowCount > 0 Then
            frmLaporanDetail.NilaiKiriman = 2
            frmLaporanDetail.ID = GridView1.GetFocusedRowCellValue("ID_PENJUALAN")
            frmLaporanDetail.WindowState = FormWindowState.Maximized
            frmLaporanDetail.ShowDialog()

        End If
    End Sub

    Sub Cetak()
        GridView1.OptionsPrint.PrintFilterInfo = True
        GridView1.OptionsPrint.RtfReportHeader = "LAPORAN PENJUALAN" & vbCrLf & "PERIODE" & Format(txtTanggalAwal.DateTime, "dd MMMM yyyy") & " S/D " & Format(txtTanggalAkhir.DateTime, "dd MMMM yyyy") & vbCrLf
        TTD(GridView1)
        ExecuteGridControlPreview(GridControl1, GridControl1.LookAndFeel, True, Printing.PaperKind.A4, 15, 15, 15, 15)
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub frmLaporanPembelian_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Tab
                TampilTable()
            Case Keys.F1
                DetailLaporan()
            Case e.Control And Keys.P
                Cetak()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub

    Private Sub frmLaporanPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        EksekusiRapikanGridView(GridView1)
        GridView1.OptionsView.ShowFooter = True
        'menampilkan tanggal hari ke tanggal awal dan tanggal akhir
        txtTanggalAwal.DateTime = Now
        txtTanggalAkhir.DateTime = Now
        TampilTable()

    End Sub

   

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        Cetak()
    End Sub

    Private Sub BarButtonItem6_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        Tutup()
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        DetailLaporan()
    End Sub

    Private Sub BarButtonItem7_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        DetailLaporan()
    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        TampilTable()
    End Sub
End Class