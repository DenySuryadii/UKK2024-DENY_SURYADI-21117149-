Public Class frmSetupMenuUtama

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        frmSetupGantiPassword.ShowDialog()
    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        If MessageBox.Show("Akses aplikasi akan ditutup. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            frmSetupLogin.Show()
            Me.Close()
        End If

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        If MessageBox.Show("Akses aplikasi akan ditutup. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            frmSetupLogin.Show()
            Application.Exit()
        End If
    End Sub

    Private Sub BarButtonItem9_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPengguna.ItemClick
        frmMasterPengguna.MdiParent = Me
        frmMasterPengguna.WindowState = FormWindowState.Maximized
        frmMasterPengguna.Show()
    End Sub

    Private Sub BarButtonItem10_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnKoneksi.ItemClick
        frmSetupKonfigurasiKoneksi.ShowDialog()
    End Sub

    Private Sub frmSetupMenuUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BarButtonItem5.Caption = "Pengguna: " & My.Settings.lgnNama
        BarButtonItem6.Caption = "Hak Akses: " & My.Settings.lgnAkses
        'pengaturan wallpaper berdasarkan simpan
        Try
            Me.BackgroundImageLayout = ImageLayout.Stretch
            Me.BackgroundImage = Image.FromFile(My.Settings.LokasiWallpaper)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim Tgl = Now
        BarButtonItem7.Caption = "Tanggal " & Format(Tgl, "dd MMMM yyyy") & ", Pukul " & Format(Tgl, "HH:MM:ss") & ""
    End Sub




    Private Sub BarButtonItem11_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPelanggan.ItemClick
        frmMasterPelanggan.MdiParent = Me
        frmMasterPelanggan.WindowState = FormWindowState.Maximized
        frmMasterPelanggan.Show()
    End Sub

    Private Sub BarButtonItem12_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPemasok.ItemClick
        frmMasterPemasok.MdiParent = Me
        frmMasterPemasok.WindowState = FormWindowState.Maximized
        frmMasterPemasok.Show()
    End Sub

    Private Sub BarButtonItem13_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnBarang.ItemClick
        frmMasterBarang.MdiParent = Me
        frmMasterBarang.WindowState = FormWindowState.Maximized
        frmMasterBarang.Show()
    End Sub

    Private Sub BarButtonItem14_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnTransaksiPembelian.ItemClick
        frmTransaksiPembelian.MdiParent = Me
        frmTransaksiPembelian.WindowState = FormWindowState.Maximized
        frmTransaksiPembelian.Show()
    End Sub

    Private Sub BarButtonItem17_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnRekapPenjualan.ItemClick
        frmLaporanPenjualan.MdiParent = Me
        frmLaporanPenjualan.WindowState = FormWindowState.Maximized
        frmLaporanPenjualan.Show()
    End Sub

    Private Sub BarButtonItem15_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnTransaksiPenjualan.ItemClick
        frmTransaksiPenjualan.MdiParent = Me
        frmTransaksiPenjualan.WindowState = FormWindowState.Maximized
        frmTransaksiPenjualan.Show()
    End Sub

    Private Sub BarButtonItem16_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnRekapPembelian.ItemClick
        frmLaporanPembelian.MdiParent = Me
        frmLaporanPembelian.WindowState = FormWindowState.Maximized
        frmLaporanPembelian.Show()
    End Sub

    Private Sub ko_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnSistem.ItemClick
        frmSetupKonfigurasiSistem.ShowDialog()
    End Sub
End Class