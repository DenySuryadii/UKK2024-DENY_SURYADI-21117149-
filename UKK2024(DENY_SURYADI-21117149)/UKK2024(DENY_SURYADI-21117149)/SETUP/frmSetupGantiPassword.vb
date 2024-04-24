Public Class frmSetupGantiPassword

    Sub Simpan()
        'memastikan semua inputan terisi 
        If txtPasswordLama.Text = "" Or txtNewPassword.Text = "" Or txtUlangiPassword.Text = "" Then
            MessageBox.Show("Password Lama, Password Baru dan Ulangi Password Wajib Di Isi!", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'mencocokan password lama
        Dim PasswordLama = EksekusiSQL("select password from pengguna where ID_PENGGUNA ='" & My.Settings.lgnID & "'").Select()(0).Item("PASSWORD")
        If txtPasswordLama.Text <> PasswordLama Then
            MessageBox.Show("Password yang anda tidak sesuai!", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        'mencocokan password baru
        If txtNewPassword.Text <> txtUlangiPassword.Text Then
            MessageBox.Show("Password Baru tidak sesuai!", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        'mengubah password lama menuju password baru
        EksekusiSQL("update pengguna set PASSWORD ='" & txtNewPassword.Text & "' where ID_PENGGUNA = '" & My.Settings.lgnID & "'")

        'kembali ke login
        frmSetupLogin.Show()
        frmSetupMenuUtama.Close()
        Me.Close()
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub frmGantiPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.Enter
                Simpan()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub



    Private Sub FormGantiPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtNewPassword.Properties.UseSystemPasswordChar = True
        txtPasswordLama.Properties.UseSystemPasswordChar = True
        txtUlangiPassword.Properties.UseSystemPasswordChar = True
        txtPasswordLama.Text = ""
        txtNewPassword.Text = ""
        txtUlangiPassword.Text = ""
        txtNewPassword.Focus()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Simpan()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Tutup()
    End Sub
End Class