Public Class frmMasterBarang

    Dim Tampung As New DataTable
    Dim ID As Integer


    Sub TampilData()
        Tampung = EksekusiSQL("select * from barang")
        GridControl1.DataSource = Tampung
        GridView1.BestFitColumns() 'MERapikan tampilan isi kolom
    End Sub


    Sub Segarkan()
        TampilData()
        ID = 0
        txtNama.Text = ""
        txtKategori.Text = ""
        txtSatuan.Text = ""
        txtHargaBeli.Value = 0
        txtHargaJual.Value = 0
        txtStok.Value = 0
        txtKeterangan.Text = ""
        txtBarcode.Text = ""
        txtNama.Focus() 'arahkan kursor ke txtnama
        TampilPilihanKategori()
        TampilPilihanSatuan()


    End Sub

    Sub Simpan()
        'validasi inputan
        If txtNama.Text = "" Or txtKategori.Text = "" Or txtSatuan.Text = "" Then
            MessageBox.Show("Nama Barang, Kategori, Satuan wajib di isi.", "validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            Exit Sub
        End If
        If ID = 0 Then
            'input data baru
            EksekusiSQL("INSERT INTO `barang`(`NAMA_BARANG`, `KATEGORI`, `SATUAN`, `HARGA_BELI`, `HARGA_JUAL`, `STOK`, `KETERANGAN`, `BARCODE` ) VALUES ('" & txtNama.Text & "','" & txtKategori.Text & "','" & txtSatuan.Text & "','" & txtHargaBeli.Value & "','" & txtHargaJual.Value & "','" & txtStok.Value & "','" & txtKeterangan.Text & "','" & txtBarcode.Text & "')")
            Segarkan()
            MessageBox.Show("Data sukses ditambahkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            'ubah data lama
            EksekusiSQL("UPDATE `barang` SET `NAMA_BARANG`='" & txtNama.Text & "',`KATEGORI`='" & txtKategori.Text & "',`SATUAN`='" & txtSatuan.Text & "',`HARGA_BELI`='" & txtHargaBeli.Value & "',`HARGA_JUAL`='" & txtHargaJual.Value & "',`STOK`='" & txtStok.Value & "',`KETERANGAN`='" & txtKeterangan.Text & "',`BARCODE`='" & txtBarcode.Text & "' WHERE `ID_BARANG`='" & ID & "'")
            Segarkan()
            MessageBox.Show("Data sukses diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Sub Ubah()
        'pengecekan data pada tabel ada atau tidak 
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("data tidak ada.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        End If
        'MENGISIKAN DATA YANG DI PILIH PADA TABEL KE INPUTAN
        ID = GridView1.GetFocusedRowCellValue("ID_BARANG")
        txtNama.Text = GridView1.GetFocusedRowCellValue("NAMA_BARANG")
        txtKategori.Text = GridView1.GetFocusedRowCellValue("KATEGORI")
        txtSatuan.Text = GridView1.GetFocusedRowCellValue("SATUAN")
        txtHargaBeli.Value = GridView1.GetFocusedRowCellValue("HARGA_BELI")
        txtHargaJual.Value = GridView1.GetFocusedRowCellValue("HARGA_JUAL")
        txtStok.Value = GridView1.GetFocusedRowCellValue("STOK")
        txtKeterangan.Text = GridView1.GetFocusedRowCellValue("KETERANGAN")
        txtBarcode.Text = GridView1.GetFocusedRowCellValue("BARCODE")
        txtNama.Focus()
    End Sub

    Sub Hapus()
        'pengecekan data pada tabel ada atau tidak 
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("data tidak ada.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        End If
        If MessageBox.Show("data akan di hapus . lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            'melakukan pengahapusan data
            EksekusiSQL("delete from barang where ID_BARANG = '" & GridView1.GetFocusedRowCellValue("ID_BARANG") & "'")
            Segarkan()

        End If
    End Sub

    Sub Cetak()
        GridControl1.ShowRibbonPrintPreview()

    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Sub TampilPilihanKategori()
        Dim Kategori = EksekusiSQL("select distinct(KATEGORI) from barang").Select()
        txtKategori.Properties.Items.Clear() 'bersihkan isian pilihan pada txt kategori
        For Each Isi In Kategori
            txtKategori.Properties.Items.Add(Isi.Item("KATEGORI"))
        Next

    End Sub

    Sub TampilPilihanSatuan()
        Dim Satuan = EksekusiSQL("select distinct(SATUAN) from barang").Select()
        txtSatuan.Properties.Items.Clear() 'bersihkan isian pada pilihan txtsatuan
        For Each Isi In Satuan
            txtSatuan.Properties.Items.Add(Isi.Item("SATUAN"))

        Next
    End Sub
    Private Sub frmMasterBarang_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.S
                Simpan()
            Case Keys.F5
                Segarkan()
            Case Keys.F2
                Ubah()
            Case e.Control And Keys.D
                Hapus()
            Case e.Control And Keys.P
                Cetak()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub

    Private Sub frmMasterBarang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True 'mengaktifkan shortcut
        EksekusiRapikanGridView(GridView1)
        txtStok.ReadOnly = True 'menonaktifkan editan txtstok
        Segarkan()

    End Sub

 

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Ubah()

    End Sub

    Private Sub BarButtonItem1_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Simpan()
    End Sub

    Private Sub BarButtonItem2_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Segarkan()
    End Sub

    Private Sub BarButtonItem3_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Ubah()
    End Sub

    Private Sub BarButtonItem4_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Hapus()
    End Sub

    Private Sub BarButtonItem5_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        Cetak()
    End Sub

    Private Sub BarButtonItem6_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        Tutup()
    End Sub
End Class