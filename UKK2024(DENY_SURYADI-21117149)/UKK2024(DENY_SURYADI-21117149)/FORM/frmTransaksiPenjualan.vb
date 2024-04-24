Public Class frmTransaksiPenjualan


    Dim Tampung As New DataTable
    Dim TotalPenjualan As Double

    Sub TampilTotalHarga()
        'Menjumlahkan dan menampilkan kolom total harga pada tabel

        Dim Total As Double = 0
        For Each isi In Tampung.Select()
            Total = Total + isi.Item("TOTAL_HARGA")
        Next
        TotalPenjualan = Total
        txtTotal.Text = "Total Rp. " & Format(Total, "#,##0") & ",-"
        GridView1.BestFitColumns()
    End Sub

    Sub TampilPelanggan()
        Dim Pelanggan = EksekusiSQL("select * From pelanggan").Select()
        txtPelanggan.Properties.Items.Clear()
        For Each isi In Pelanggan
            txtPelanggan.Properties.Items.Add(isi.Item("NAMA_PELANGGAN") & ">" & isi.Item("ID_PELANGGAN"))
        Next


    End Sub

    Sub TampilTabel()
        Tampung = EksekusiSQL("select JUMLAH,penjualan_detil.ID_BARANG,NAMA_BARANG,SATUAN,HARGA_SATUAN,TOTAL_HARGA from penjualan_detil, barang where penjualan_detil.ID_BARANG=barang.ID_BARANG and ID_PENJUALAN=''")
        GridControl1.DataSource = Tampung

    End Sub

    Sub Segarkan()
        TampilTabel()
        txtCari.Text = ""
        txtTanggal.DateTime = Now
        TampilPelanggan()
        txtPelanggan.SelectedIndex = -1
        txtPotongan.Value = 0
        txtMetodePembayaran.SelectedIndex = 0
        txtKeterangan.Text = ""
        TotalPenjualan = 0
        TampilTotalHarga()
        txtCari.Focus()



    End Sub

    Sub TampilItemBarang()
        Dim CariBarang = EksekusiSQL("select * from barang where ID_BARANG='" & txtCari.Text & "'or BARCODE='" & txtCari.Text & "'").Select()
        If CariBarang.Length > 0 Then 'pengecekan barang ada atau tidak 
            'disini lanjut


            Dim CariDataTabel = Tampung.Select("ID_BARANG='" & CariBarang(0).Item("ID_BARANG") & "'")
            If CariDataTabel.Length <= 0 Then
                'tambah ke tabel
                'pengecekan stok sebelum ditambahkan
                If CariBarang(0).Item("STOK") <= 0 Then
                    MessageBox.Show("stok tidak ada.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
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
                If CariBarang(0).Item("STOK") < CariDataTabel(0).Item("JUMLAH") + 1 Then
                    MessageBox.Show("stok tersedia " & CariBarang(0).Item("STOK") & ".", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
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
        frmListBarang.NilaiKiriman = 2
        frmListBarang.WindowState = FormWindowState.Maximized
        frmListBarang.ShowDialog()




    End Sub

    Sub Simpan()
        'pengecekan data
        If txtTanggal.Text = "" Or txtPelanggan.Text = "" Or txtMetodePembayaran.Text = "" Then
            MessageBox.Show("Tanggal, Pelanggan, dan Metode Pembayaran wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Data item belum ditambahkan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'pembuatan kode otomatis (auto increment)
        Dim NomorTransaksi As Integer
        Try
            NomorTransaksi = EksekusiSQL("select max(ID_PENJUALAN) as ID_PENJUALAN from penjualan").Select()(0).Item("ID_PENJUALAN") + 1
        Catch ex As Exception
            NomorTransaksi = 1
        End Try
        'mengisi tabel pembelian
        EksekusiSQL("insert into penjualan(ID_PENJUALAN,TANGGAL,ID_PELANGGAN,TOTAL_PENJUALAN,POTONGAN,METODE_PEMBAYARAN,KETERANGAN,ID_PENGGUNA) values('" & NomorTransaksi & "','" & Format(txtTanggal.DateTime, "yyyy-MM-dd") & "','" & txtPelanggan.Text.Split(">")(1) & "','" & TotalPenjualan & "','" & txtPotongan.Value & "','" & txtMetodePembayaran.Text & "','" & txtKeterangan.Text & "','" & My.Settings.lgnID & "')")
        'mengisi tabel penjualan detil
        For Each isi In Tampung.Select()
            EksekusiSQL("insert into penjualan_detil(ID_PENJUALAN,ID_BARANG,JUMLAH,HARGA_SATUAN,TOTAL_HARGA) values('" & NomorTransaksi & "','" & isi.Item("ID_BARANG") & "','" & isi.Item("JUMLAH") & "','" & isi.Item("HARGA_SATUAN") & "','" & isi.Item("TOTAL_HARGA") & "')")
            'koding ngojek untuk merubah/menambah stok pada barang

            'triger pak arif
            'Dim Stok = EksekusiSQL("select STOK from barang where ID_BARANG='" & isi.Item("ID_BARANG") & "'").Select()(0).Item("STOK")
            'Dim StokBaru = Stok - isi.Item("JUMLAH")
            'mengubah data stok dengan stok baru 
            'EksekusiSQL("update barang set STOK='" & StokBaru & "' where ID_BARANG='" & isi.Item("ID_BARANG") & "'")
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

    Private Sub frmTransaksiPenjualan_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                TampilItemBarang()
            Case e.Control And Keys.S
                Simpan()
            Case Keys.F5
                Segarkan()
            Case e.Control And Keys.D
                HapusItemBarang()
            Case Keys.F1
                ListBarang()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub

    Private Sub frmTransaksiPenjualan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        GridView1.OptionsView.ColumnAutoWidth = False 'supaya kolom tidak mengikuti ukuran tabel
        GridView1.OptionsView.ShowGroupPanel = False 'menghilangkan group panel
        txtPelanggan.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        txtMetodePembayaran.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Segarkan()
        GridView1.Columns("ID_BARANG").OptionsColumn.AllowEdit = False
        GridView1.Columns("NAMA_BARANG").OptionsColumn.AllowEdit = False
        GridView1.Columns("TOTAL_HARGA").OptionsColumn.AllowEdit = False
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        Simpan()

    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        Segarkan()

    End Sub

    Private Sub BarButtonItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        ListBarang()
        frmListBarang.NilaiKiriman = 1
        frmListBarang.WindowState = FormWindowState.Maximized
        frmListBarang.ShowDialog()

    End Sub

    Private Sub BarButtonItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        Tutup()

    End Sub


    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        'pengecekan kolom yang akan dirubah
        Select Case e.Column.GetTextCaption
            Case "JUMLAH"
                Dim Barang = EksekusiSQL("select * from barang where ID_BARANG='" & GridView1.GetFocusedRowCellValue("ID_BARANG") & "'").Select()
                If Barang(0).Item("STOK") < GridView1.GetFocusedRowCellValue("JUMLAH") Then
                    MessageBox.Show("stok tersedia " & Barang(0).Item("STOK") & ".", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    GridView1.SetFocusedRowCellValue("JUMLAH", Barang(0).Item("STOK"))
                    Exit Sub
                End If
                Dim TotalHarga = GridView1.GetFocusedRowCellValue("JUMLAH") * GridView1.GetFocusedRowCellValue("HARGA_SATUAN")
                GridView1.SetFocusedRowCellValue("TOTAL_HARGA", TotalHarga)
            Case "HARGA_SATUAN"
                'HITUNG TOTAL HARGA
                Dim TotalHarga = GridView1.GetFocusedRowCellValue("JUMLAH") * GridView1.GetFocusedRowCellValue("HARGA_SATUAN")
                GridView1.SetFocusedRowCellValue("TOTAL_HARGA", TotalHarga)
        End Select
        TampilTotalHarga()

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        HapusItemBarang()

    End Sub

    Private Sub BarButtonItem1_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Simpan()
    End Sub

    Private Sub BarButtonItem2_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Segarkan()
    End Sub

    Private Sub BarButtonItem3_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        HapusItemBarang()
    End Sub

    Private Sub BarButtonItem4_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        ListBarang()
    End Sub

    Private Sub BarButtonItem5_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        Tutup()
    End Sub
End Class