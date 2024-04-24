Public Class frmMasterPemasok

    Dim Tampung As New DataTable
    Dim ID As Integer

    Sub TampilData()
        Tampung = EksekusiSQL("select * from pemasok")
        GridControl1.DataSource = Tampung
        'sembunyikan kolom PASSWORD

    End Sub

    Sub Segarkan()
        TampilData()
        ID = 0
        txtNama.Text = ""
        txtAlamat.Text = ""
        txtNomorTelepon.Text = ""
        txtNama.Focus() 'arahkan kursor fokus ke txtNama
    End Sub
    Sub Simpan()
        'Validasi Inputan
        If txtNama.Text = "" Then
            MessageBox.Show("Nama,  Wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If ID = 0 Then
            'input data baru
            EksekusiSQL("INSERT INTO `pemasok`(`NAMA_PEMASOK`, `ALAMAT`, `NOMOR_TELEPON`) VALUES ('" & txtNama.Text & "','" & txtAlamat.Text & "','" & txtNomorTelepon.Text & "')")
            Segarkan()
            MessageBox.Show("Data sukses ditambahkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'ubah data lama
            EksekusiSQL("UPDATE `pemasok` SET `NAMA_PEMASOK`='" & txtNama.Text & "',`ALAMAT`='" & txtAlamat.Text & "',`NOMOR_TELEPON`=" & txtNomorTelepon.Text & "' WHERE `ID_PEMASOK`='" & ID & "'")
            Segarkan()
            MessageBox.Show("Data sukses diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)


        End If
    End Sub


    Sub Ubah()
        'Pengecekan data pada tabel ada atau tidak 
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Data tidak ada.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        End If
        'mengisikan data yanmg dipilih pada tabel ke inputan
        ID = GridView1.GetFocusedRowCellValue("ID_PEMASOK")
        txtNama.Text = GridView1.GetFocusedRowCellValue("NAMA_PEMASOK")
        txtAlamat.Text = GridView1.GetFocusedRowCellValue("ALAMAT")
        txtNomorTelepon.Text = GridView1.GetFocusedRowCellValue("NOMOR_TELEPON")

        txtNama.Focus()



    End Sub

    Sub Hapus()
        'Pengecekan data pada tabel ada atau tidak 
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Data tidak ada.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        End If
        If MessageBox.Show("Data Akan Dihapus. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            'melakukan penghapusan data
            EksekusiSQL("delete from pemasok where ID_PEMASOK='" & GridView1.GetFocusedRowCellValue("ID_PEMASOK") & "'")
            Segarkan()

        End If
    End Sub

    Sub Cetak()
        GridControl1.ShowPrintPreview()
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub frmMasterPemasok_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.Keycode
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

    Private Sub frmMasterPemasok_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.KeyPreview = True 'Mengaktifkan shortcut
        EksekusiRapikanGridView(GridView1)

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