Imports MySql.Data.MySqlClient
Imports DevExpress.XtraPrinting
Imports DevExpress.LookAndFeel
Imports System.Drawing.Printing
Module Module1
    Function EksekusiSQL(ByVal PerintahSQL) As DataTable
        Dim Alamat As String = "server=" & My.Settings.KonNamaServer & ";user id=" & My.Settings.KonNamaUser & ";password=" & My.Settings.KonKataSandi & ";database=" & My.Settings.KonNamaDatabase & ";port=" & My.Settings.KonPortal & ""
        Dim Koneksi As New MySqlConnection(Alamat)
        Dim Eksekusi As New MySqlDataAdapter(PerintahSQL, Koneksi)
        Dim Tampung As New DataTable
        Eksekusi.Fill(Tampung)
        Return Tampung

    End Function


    Sub EksekusiRapikanGridView(ByVal NamaGridView As DevExpress.XtraGrid.Views.Grid.GridView)

        NamaGridView.OptionsBehavior.Editable = False 'supaya tidak bisa diedit pada tabel
        NamaGridView.OptionsView.ColumnAutoWidth = False 'supaya kolom tidak mengikuti ukuran tabel
        NamaGridView.OptionsView.ShowGroupPanel = False 'menghilangkan grip panel
        NamaGridView.OptionsView.ShowAutoFilterRow = True 'tampilkan pencarian berdasarka kolom
        NamaGridView.OptionsFind.AlwaysVisible = True 'tampilkan keseluruhan kolom

    End Sub


    Sub TTD(ByVal GridViewName As DevExpress.XtraGrid.Views.Grid.GridView)
        GridViewName.OptionsPrint.RtfReportFooter = vbCrLf & IIf(My.Settings.LokasiLaporan <> Nothing, My.Settings.LokasiLaporan, ".........................") & ", " & Format(Now, "dd MMMM yyyy") & vbCrLf & "Mengesahkan/Mengetahui," & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "_" & vbCrLf & "Pimpinan"
    End Sub


    Public Sub ExecuteGridControlPreview(ByVal GridControl As IPrintable, ByVal GridControlLookAndFeel As UserLookAndFeel, Optional ByVal StatusLandscape As Boolean = False, Optional ByVal PaperKind As System.Drawing.Printing.PaperKind = PaperKind.A4, Optional ByVal LeftMargin As Double = 50, Optional ByVal RightMargin As Double = 50, Optional ByVal TopMargin As Double = 50, Optional ByVal BottomMargin As Double = 50)
        Dim Link As New PrintableComponentLink() With {.PrintingSystemBase = New PrintingSystem(), .Component = GridControl, .Landscape = StatusLandscape, .PaperKind = PaperKind, .Margins = New Margins(LeftMargin, RightMargin, TopMargin, BottomMargin)}
        Link.ShowRibbonPreview(GridControlLookAndFeel)
    End Sub

    Sub PortalMenu(ByVal sBarang As Boolean, ByVal sMenu As Boolean, ByVal sPemasok As Boolean, ByVal sPelanggan As Boolean, ByVal sPembelian As Boolean, ByVal sPenjualan As Boolean, ByVal sRekapPenjualan As Boolean, ByVal sRekapPembelian As Boolean, ByVal sKonfigurasiKoneksi As Boolean, ByVal sKonfigurasiSistem As Boolean, ByVal sPengguna As Boolean)
        With frmSetUpMenuUtama
            'menu master
            .btnBarang.Enabled = sBarang
            .btnPemasok.Enabled = sPemasok
            .btnPelanggan.Enabled = sPelanggan
           


            'menu transaksi
            .btnTransaksiPembelian.Enabled = sPembelian
            .btnTransaksiPenjualan.Enabled = sPenjualan


            'menu laporan
            .btnRekapPembelian.Enabled = sRekapPembelian
            .btnRekapPenjualan.Enabled = sRekapPenjualan

            'menu pengaturan
            .btnKoneksi.Enabled = sKonfigurasiKoneksi
            .btnSistem.Enabled = sKonfigurasiSistem
            .btnPengguna.Enabled = sPengguna

        End With
    End Sub
End Module
