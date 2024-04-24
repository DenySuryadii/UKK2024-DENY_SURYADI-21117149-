Imports DevExpress.LookAndFeel

Public Class frmSetupLogin

    Sub Login()

        'pengecekan inputan
        If txtUsername.Text = "" Or txtPassword.Text = "" Then
            MessageBox.Show("Username, dan Password wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Try
            'pengambilan data ke data base 
            Dim Pengguna = EksekusiSQL("select * from pengguna where USERNAME='" & txtUsername.Text & "' and PASSWORD='" & txtPassword.Text & "'").Select()
            If Pengguna.Length > 0 Then ' cek jumlah datanya
                'jika ada
                My.Settings.lgnID = Pengguna(0).Item("ID_PENGGUNA")
                My.Settings.lgnNama = Pengguna(0).Item("NAMA_PENGGUNA")
                My.Settings.lgnAkses = Pengguna(0).Item("HAK_AKSES")
                Select Case My.Settings.lgnAkses
                    Case "Admin"
                        ' Jika pengguna adalah admin, mungkin Anda ingin memberikan akses penuh ke seluruh fitur.
                        ' Di sini Anda bisa memanggil fungsi untuk mengatur menu sesuai dengan peran pengguna.
                        PortalMenu(True, True, True, True, True, False, False, True, False, False, False)
                    Case "OPERATOR"
                        ' Jika pengguna adalah pengguna biasa, mungkin Anda ingin memberikan akses terbatas.
                        ' Di sini Anda bisa memanggil fungsi untuk mengatur menu sesuai dengan peran pengguna.
                        PortalMenu(False, False, False, False, True, True, False, False, False, False, False)
                    Case "KASIR"
                        ' Jika pengguna adalah pengguna biasa, mungkin Anda ingin memberikan akses terbatas.
                        ' Di sini Anda bisa memanggil fungsi untuk mengatur menu sesuai dengan peran pengguna.
                        '     Sub PortalMenu(ByVal sBarang As Boolean, ByVal sMenu As Boolean, ByVal sPemasok As Boolean, ByVal sPelanggan
                        ' As Boolean, ByVal sPembelian As Boolean, ByVal sPenjualan As Boolean, ByVal sRekapPenjualan As Boolean,
                        ' ByVal sRekapPembelian As Boolean, ByVal sKonfigurasiKoneksi As Boolean, ByVal sKonfigurasiSistem As Boolean, ByVal sPengguna As Boolean)
                        PortalMenu(False, False, False, False, True, True, True, True, False, False, False)
                    Case Else
                        ' Akses default untuk peran pengguna yang tidak terdefinisi.
                        ' Di sini Anda bisa menentukan apa yang harus dilakukan ketika peran pengguna tidak dikenali.
                End Select

                'lanjutkan ke menu utama
                frmSetupMenuUtama.WindowState = FormWindowState.Maximized
                frmSetupMenuUtama.Show()
                Me.Close()


            Else
                'jika tidak ada
                MessageBox.Show("Username/Password tidak ditemukan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUsername.Focus()
            End If
        Catch ex As Exception
            If MessageBox.Show("Koneksi gagal. Lanjutkan ke konfigurasi koneksi?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                frmSetupKonfigurasiKoneksi.ShowDialog()

            End If
        End Try
    End Sub

    Sub Tutup()
        Application.Exit() 'menutup aplikasi bukan menutup from
    End Sub

    Sub Daftar()
        frmMasterPengguna.Show()
    End Sub

    Private Sub frmSetupLogin_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                Login()
            Case Keys.F4
                Daftar()
            Case Keys.Escape
                Tutup()

        End Select
    End Sub

    Private Sub frmSetupLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtPassword.Properties.UseSystemPasswordChar = True
        GroupControl1.Text = ""
        txtPassword.Text = ""
        GroupControl1.Focus()

    End Sub

    Private Sub SimpleButton2_Click_1(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Tutup()
    End Sub

    Private Sub SimpleButton1_Click_1(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Login()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'daftar akun
        If MessageBox.Show("Belum punya akun?.Lanjutkan ke Daftar Akun?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            frmMasterPengguna.Show()
            frmMasterPengguna.WindowState = FormWindowState.Maximized
        End If
    End Sub
End Class