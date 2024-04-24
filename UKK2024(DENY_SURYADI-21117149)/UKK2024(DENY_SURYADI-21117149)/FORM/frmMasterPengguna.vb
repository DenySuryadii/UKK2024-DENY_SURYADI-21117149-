Public Class frmMasterPengguna

    Dim Tampung As New DataTable
    Dim ID As Integer

    Sub TampilData()
        Tampung = EksekusiSQL("select * from pengguna")
        GridControl1.DataSource = Tampung
        GridView1.Columns("PASSWORD").Visible = False 'sembunyikan kolom PASSWORD
        GridView1.BestFitColumns() 'merapikan tampilan isi kolom dan baris
    End Sub

    Sub Segarkan()
        TampilData()
        ID = 0
        txtNama.Text = ""
        txtAlamat.Text = ""
        txtNomorTelepon.Text = ""
        txtUsername.Text = ""
        txtPassword.Text = ""
        txtHakAkses.SelectedIndex = -1
        txtNama.Focus() 'arahkan kursor fokus ke txtNama

    End Sub
    Sub Simpan()
        'Validasi Inputan
        If txtNama.Text = "" Or txtUsername.Text = "" Or txtPassword.Text = "" Or txtHakAkses.Text = "" Then
            MessageBox.Show("Nama, Username, Password, dan Hak Akses Wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If ID = 0 Then
            'input data baru
            EksekusiSQL("INSERT INTO `pengguna`(`NAMA_PENGGUNA`, `ALAMAT`, `NOMOR_TELEPON`, `USERNAME`, `PASSWORD`, `HAK_AKSES`) VALUES ('" & txtNama.Text & "','" & txtAlamat.Text & "','" & txtNomorTelepon.Text & "','" & txtUsername.Text & "','" & txtPassword.Text & "','" & txtHakAkses.Text & "')")
            Segarkan()
            MessageBox.Show("Data sukses ditambahkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'ubah data lama
            EksekusiSQL("UPDATE `pengguna` SET `NAMA_PENGGUNA`='" & txtNama.Text & "',`ALAMAT`='" & txtAlamat.Text & "',`NOMOR_TELEPON`=" & txtNomorTelepon.Text & ",`USERNAME`='" & txtUsername.Text & "',`PASSWORD`='" & txtPassword.Text & "',`HAK_AKSES`='" & txtHakAkses.Text & "' WHERE `ID_PENGGUNA`='" & ID & "'")
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
        ID = GridView1.GetFocusedRowCellValue("ID_PENGGUNA")
        txtNama.Text = GridView1.GetFocusedRowCellValue("NAMA_PENGGUNA")
        txtAlamat.Text = GridView1.GetFocusedRowCellValue("ALAMAT")
        txtNomorTelepon.Text = GridView1.GetFocusedRowCellValue("NOMOR_TELEPON")
        txtUsername.Text = GridView1.GetFocusedRowCellValue("USERNAME")
        txtPassword.Text = GridView1.GetFocusedRowCellValue("PASSWORD")
        txtHakAkses.Text = GridView1.GetFocusedRowCellValue("HAK_AKSES")
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
            EksekusiSQL("delete from pengguna where ID_PENGGUNA='" & GridView1.GetFocusedRowCellValue("ID_PENGGUNA") & "'")
            Segarkan()

        End If
    End Sub

    Sub Cetak()
        GridControl1.ShowPrintPreview()
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub frmMasterPengguna_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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

    Private Sub frmMasterPengguna_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.KeyPreview = True 'Mengaktifkan shortcut
        EksekusiRapikanGridView(GridView1)
        txtPassword.Properties.UseSystemPasswordChar = True
        txtHakAkses.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Segarkan()
    End Sub
    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Ubah()

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

    Private Sub BarButtonItem1_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Simpan()
    End Sub
End Class