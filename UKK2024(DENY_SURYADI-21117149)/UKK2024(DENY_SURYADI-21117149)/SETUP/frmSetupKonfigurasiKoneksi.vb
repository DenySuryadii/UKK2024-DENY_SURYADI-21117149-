Imports MySql.Data.MySqlClient

Public Class frmSetupKonfigurasiKoneksi

    Sub TampilMySetting()
        ' Menampilkan isi My.Settings ke komponen pada konfigurasi koneksi
        txtNamaServer.Text = My.Settings.konNamaServer
        txtNamaUser.Text = My.Settings.konNamaUser
        txtKataSandi.Text = My.Settings.konKataSandi
        txtNamaDatabase.Text = My.Settings.konNamaDatabase
        txtPortal.Value = My.Settings.konPortal
    End Sub

    Sub TesKoneksi()
        ' Pengecekan input apakah sudah terisi atau belum
        If txtNamaServer.Text = "" Or txtNamaUser.Text = "" Or txtNamaDatabase.Text = "" Or txtPortal.Value = 0 Then
            MessageBox.Show("Nama Server, Nama User, Nama Database, dan Portal wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub ' Membatalkan kodingan tes koneksi
        End If

        Try
            ' Melakukan koneksi berdasarkan inputan yang ada di komponen konfigurasi koneksi
            Dim Alamat As String = "server=" & txtNamaServer.Text & ";user id=" & txtNamaUser.Text & ";password=" & txtKataSandi.Text & ";database=" & txtNamaDatabase.Text & ";port=" & txtPortal.Value & ""
            Dim Koneksi As New MySqlConnection(Alamat)
            Koneksi.Open()
            Koneksi.Close()

            MessageBox.Show("Koneksi berhasil.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            MessageBox.Show("Koneksi Gagal. " & ex.Message, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Sub SimpanKoneksi()
        ' Pengecekan input apakah sudah terisi atau belum
        If txtNamaServer.Text = "" Or txtNamaUser.Text = "" Or txtNamaDatabase.Text = "" Or txtPortal.Value = 0 Then
            MessageBox.Show("Nama Server, Nama User, Nama Database, dan Portal wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub ' Membatalkan kodingan tes koneksi
        End If

        Try
            ' Melakukan koneksi berdasarkan inputan yang ada di komponen konfigurasi koneksi
            Dim Alamat As String = "server=" & txtNamaServer.Text & ";user id=" & txtNamaUser.Text & ";password=" & txtKataSandi.Text & ";database=" & txtNamaDatabase.Text & ";port=" & txtPortal.Value
            Dim Koneksi As New MySqlConnection(Alamat)
            Koneksi.Open()
            Koneksi.Close()

            ' Menyimpan koneksi ke pengaturan
            My.Settings.konNamaServer = txtNamaServer.Text
            My.Settings.konNamaUser = txtNamaUser.Text
            My.Settings.konKataSandi = txtKataSandi.Text
            My.Settings.konNamaDatabase = txtNamaDatabase.Text
            My.Settings.konPortal = txtPortal.Value
            My.Settings.Save()

            MessageBox.Show("Berhasil disimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Gagal disimpan. " & ex.Message, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try


    End Sub

    Private Sub frmSetupKonfigurasiKoneksi_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Tempat shortcut
        Select Case e.KeyCode
            Case e.Control And Keys.T
                TesKoneksi()
            Case e.Control And Keys.S
                SimpanKoneksi()
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub frmSetupKonfigurasiKoneksi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtKataSandi.Properties.UseSystemPasswordChar = True
        TampilMySetting()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        TesKoneksi()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        SimpanKoneksi()
    End Sub
End Class
