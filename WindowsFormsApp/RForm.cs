using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Data.SqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace WindowsFormsApp
{
    public partial class RForm : Form
    {
        //variables vectores y Harcascades
        int con = 0;
        Image<Bgr, Byte> currentFrame;
        VideoCapture Grabar;
        HaarCascade face; //Rostro
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.4d, 0.4d);
        Image<Gray, byte> result = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NombrePersonas = new List<string>();
        int ContTrain, numLabels, t;
        string Nombre;
        DataGridView d = new DataGridView();


        private void FrameGrabar(object sender, EventArgs e)
        {
            lblCantidad.Text = "0";
            NombrePersonas.Add("");

            try
            {
                currentFrame = Grabar.QueryFrame().Size(320, 240, INTER.CV_INTER_CUBIC);

                gray = currentFrame.Convert<Gray, Byte>();

                MCvAvgComp[][] RostrosDetectados = gray.DetectHaarCascade(face, 1.2, 10, HaarDetectionType.DoCannyPruning, new Size(20, 20));

                foreach (MCvAvgComp R in RostrosDetectados[0])
                {
                    t = t + 1;
                    result = currentFrame.Copy(R.Rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    currentFrame.Draw(R.Rect, new Bgr(Color.Green), 1);

                    

                    NombrePersonas[t - 1] = Nombre;
                    NombrePersonas.Add("");

                    lblCantidad.Text = RostrosDetectados[0].Length.ToString();
                }
                t = 0;
                pictureBox1.Image = currentFrame;
                Nombre = "";
                NombrePersonas.Clear();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
        }

        private void reconocer()
        {
            try
            {
                Grabar = new VideoCapture();
                Grabar.QueryFrame();
                Application.Idle += new EventHandler(FrameGrabar);
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
        }

        private void DetenerReconocer()
        {
            try
            {
                Application.Idle -= new EventHandler(FrameGrabar);
                Grabar.Dispose();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
        }

        //************************
        public RForm()
        {
            InitializeComponent();

            face = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                Conexion.Consultar(d);

                string[] Labels = Conexion.Nombre;
                numLabels = Conexion.TotalRostros;
                ContTrain = numLabels;

                for (int i = 0; i < numLabels; i++)
                {
                    con = i;
                    Bitmap bmp = new Bitmap(Conexion.ConvertBinaryToImg(con));

                    trainingImages.Add(new Image<Gray, byte>(bmp));
                    labels.Add(Labels[i]);
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Error" + e);
            }
        }

        private void RForm_Load(object sender, EventArgs e)
        {
            reconocer();
            Conexion.Consultar(dataGridView1);
        }

        int posY = 0;
        int posX = 0;

        public Image TraineFace { get; private set; }

        private void panel1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) {
                posX = e.X;
                posY = e.Y;
            } else {
                Left = Left + (e.X - posX);
                Top = Top + (e.Y - posY);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Estas en: Registrar nuevo rostro");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            dbForm f3 = new dbForm();
            f3.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        //***********
        private void btnCapturar_Click(object sender, EventArgs e)
        {
            try
            {
                ContTrain += ContTrain;
                gray = currentFrame.Convert<Gray, Byte>();

                MCvAvgComp[][] RostrosDetectados = gray.DetectHaarCascade(face, 1.2, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                foreach (MCvAvgComp R in RostrosDetectados[0])
                {

                    TraineFace = currentFrame.Copy(R.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    break;

                }
                TraineFace = result.Resize(100, 100, INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);

                pictureBox2.Image = TraineFace;
            }
            catch
            {
            }
        }

        //***************
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
            {
                labels.Add(txtNombre.Text);
                Conexion.GuardarImagen(txtNombre.Text, Conexion.ConvertImgToBinary(pictureBox2.Image.Bitmap));
            }

            Conexion.Consultar(dataGridView1);
        }

        private void RForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetenerReconocer();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
