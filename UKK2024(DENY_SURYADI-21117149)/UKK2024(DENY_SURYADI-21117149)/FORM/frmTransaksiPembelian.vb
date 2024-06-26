﻿Public Class frmTransaksiPembelian

    Dim Tampung As New DataTable
    Dim TotalPembelian As Double

    Sub TampilTotalHarga()
        'Menjumlahkan dan menampilkan kolom total harga pada tabel

        Dim Total As Double = 0
        For Each isi In Tampung.Select()
            Total = Total + isi.Item("TOTAL_HARGA")
        Next
        TotalPembelian = Total
        txtTotal.Text = "Total Rp. " & Format(Total, "#,##0") & ",-"
        GridView1.BestFitColumns()
    End Sub

    Sub TampilPemasok()
        Dim Pemasok = EksekusiSQL("select * From pemasok").Select()
        txtPemasok.Properties.Items.Clear()
        For Each isi In Pemasok
            txtPemasok.Properties.Items.Add(isi.Item("NAMA_PEMASOK") & ">" & isi.Item("ID_PEMASOK"))
        Next


    End Sub

    Sub TampilTabel()
        Tampung = EksekusiSQL("select JUMLAH,Pembelian_detil.ID_BARANG,NAMA_BARANG,SATUAN,HARGA_SATUAN,TOTAL_HARGA from pembelian_detil, barang where pembelian_detil.ID_BARANG=barang.ID_BARANG and ID_PEMBELIAN=''")
        GridControl1.DataSource = Tampung

    End Sub

    Sub Segarkan()
        TampilTabel()
        txtCari.Text = ""
        txtTanggal.DateTime = Now
        TampilPemasok()
        txtPemasok.SelectedIndex = -1
        txtPotongan.Value = 0
        txtMetodePembayaran.SelectedIndex = 0
        txtKeterangan.Text = ""
        TotalPembelian = 0
        TampilTotalHarga()
        txtCari.Focus()



    End Sub

    Sub TampilItemBarang()
        Dim CariBarang = EksekusiSQL("select * from barang where ID_BARANG='" & txtCari.Text & "'or BARCODE='" & txtCari.Text & "'").Select()
        If CariBarang.Length > 0 Then 'pengecekan bar\ang ada atau tidak 
            'disini lanjut


            Dim CariDataTabel = Tampung.Select("ID_BARANG='" & CariBarang(0).Item("ID_BARANG") & "'")
            If CariDataTabel.Length <= 0 Then
                'tambah ke tabel
                Dim DataBaru = Tampung.NewRow
                DataBaru.Item("JUMLAH") = 1
                DataBaru.Item("ID_BARANG") = CariBarang(0).Item("ID_BARANG")
                DataBaru.Item("NAMA_BARANG") = CariBarang(0).Item("NAMA_BARANG")
                DataBaru.Item("SATUAN") = CariBarang(0).Item("SATUAN")
                DataBaru.Item("HARGA_SATUAN") = CariBarang(0).Item("HARGA_BELI")
                DataBaru.Item("TOTAL_HARGA") = DataBaru.Item("JUMLAH") * DataBaru.Item("HARGA_SATUAN")
                Tampung.Rows.Add(DataBaru) 'tambahkan isian baris baru pada tampung
                TampilTotalHarga()
                txtCari.Text = ""
                txtCari.Focus()

            Else
                'tambah jumlahnya dan hitung ulang TOTAL_HARGA
                CariDataTabel(0).Item("JUMLAH") = CariDataTabel(0).Item("JUMLAH") + 1
                CariDataTabel(0).Item("TOTAL_HARGA") = CariDataTabel(0).Item("JUMLAH") * CariDataTabel(0).Item("HARGA_SATUAN")
                TampilTotalHarga()
                txtCari.Text = ""
                txtCari.Focus()
            End If
        Else
            MessageBox.Show("Barang tidak ditemukan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCari.Text = ""
            txtCari.Focus()
        End If


    End Sub

    Sub HapusItemBarang()
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Item barang belum ditambahkan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If MessageBox.Show("Item barang akan dihapus. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            GridView1.DeleteSelectedRows()
            TampilTotalHarga()
            txtCari.Text = ""
            txtCari.Focus()
        End If
    End Sub

    Sub ListBarang()
        frmListBarang.NilaiKiriman = 1
        frmListBarang.WindowState = FormWindowState.Maximized
        frmListBarang.ShowDialog()

    End Sub

    Sub Simpan()
        'pengecekan data
        If txtTanggal.Text = "" Or txtPemasok.Text = "" Or txtMetodePembayaran.Text = "" Then
            MessageBox.Show("Tanggal, Pemasok, dan Metode Pembayaran wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Data item belum ditambahkan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'pembuatan kode otomatis (auto increment)
        Dim NomorTransaksi As Integer
        Try
            NomorTransaksi = EksekusiSQL("select max(ID_PEMBELIAN) as ID_PEMBELIAN from pembelian").Select()(0).Item("ID_PEMBELIAN") + 1
        Catch ex As Exception
            NomorTransaksi = 1
        End Try
        'mengisi tabel pembelian
        EksekusiSQL("insert into pembelian(ID_PEMBELIAN,TANGGAL,ID_PEMASOK,TOTAL_PEMBELIAN,POTONGAN,METODE_PEMBAYARAN,KETERANGAN,ID_PENGGUNA) values('" & NomorTransaksi & "','" & Format(txtTanggal.DateTime, "yyyy-MM-dd") & "','" & txtPemasok.Text.Split(">")(1) & "','" & TotalPembelian & "','" & txtPotongan.Value & "','" & txtMetodePembayaran.Text & "','" & txtKeterangan.Text & "','" & My.Settings.lgnID & "')")
        'mengisi tabel pembelian detil
        For Each isi In Tampung.Select()
            EksekusiSQL("insert into pembelian_detil(ID_PEMBELIAN,ID_BARANG,JUMLAH,HARGA_SATUAN,TOTAL_HARGA) values('" & NomorTransaksi & "','" & isi.Item("ID_BARANG") & "','" & isi.Item("JUMLAH") & "','" & isi.Item("HARGA_SATUAN") & "','" & isi.Item("TOTAL_HARGA") & "')")
            'koding ngojek untuk merubah/menambah stok pada barang
            'triger pak arif
            ' Dim Stok = EksekusiSQL("select STOK from barang where ID_BARANG='" & isi.Item("ID_BARANG") & "'").Select()(0).Item("STOK")
            '  Dim StokBaru = isi.Item("JUMLAH") + Stok
            'mengubah data stok dengan stok baru 
            ' EksekusiSQL("update barang set STOK='" & StokBaru & "' where ID_BARANG='" & isi.Item("ID_BARANG") & "'")
        Next
        'pembersihan ulang
        Segarkan()
        MessageBox.Show("Transaksi Berhasil.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'akan ada koding cetak di sini lain waktu
    End Sub


    Sub Tutup()
        If GridView1.RowCount > 0 Then
            If MessageBox.Show("Transaksi akan ditutup. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub


    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        TampilItemBarang()
    End Sub

    Private Sub frmTransaksiPembelian_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                TampilItemBarang()
            Case e.Control And Keys.S
                Simpan()
            Case Keys.F5
                Segarkan()
            Case e.Control And Keys.D
                HapusItemBarang()
            Case e.Control And Keys.L
                ListBarang()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub

    Private Sub frmTransaksiPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        GridView1.OptionsView.ColumnAutoWidth = False 'supaya kolom tidak mengikuti ukuran tabel
        GridView1.OptionsView.ShowGroupPanel = False 'menghilangkan group panel
        txtPemasok.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        txtMetodePembayaran.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Segarkan()
        GridView1.Columns("ID_BARANG").OptionsColumn.AllowEdit = False
        GridView1.Columns("NAMA_BARANG").OptionsColumn.AllowEdit = False
        GridView1.Columns("TOTAL_HARGA").OptionsColumn.AllowEdit = False
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Simpan()

    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Segarkan()

    End Sub

    Private Sub BarButtonItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        ListBarang()


    End Sub

    Private Sub BarButtonItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        Tutup()

    End Sub



    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        'pengecekan kolom yang akan dirubah
        Select Case e.Column.GetTextCaption
            Case "JUMLAH"
                'HITUNG TOTAL HARGA
                Dim TotalHarga = GridView1.GetFocusedRowCellValue("JUMLAH") * GridView1.GetFocusedRowCellValue("HARGA_SATUAN")
                GridView1.SetFocusedRowCellValue("TOTAL_HARGA", TotalHarga)
            Case "HARGA_SATUAN"
                'HITUNG TOTAL HARGA
                Dim TotalHarga = GridView1.GetFocusedRowCellValue("JUMLAH") * GridView1.GetFocusedRowCellValue("HARGA_SATUAN")
                GridView1.SetFocusedRowCellValue("TOTAL_HARGA", TotalHarga)
        End Select
        TampilTotalHarga()

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        HapusItemBarang()
    End Sub

    Private Sub txtPemasok_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtPemasok.SelectedIndexChanged

    End Sub
End Class