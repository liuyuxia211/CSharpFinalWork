using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace SealForm
{

    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Bitmap previousMap = null;
        ImageEffectManager imageManager;
        ImageEventHandlers imageHandlers;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage ; //这里用来设置如何处理图片

            imageHandlers = new ImageEventHandlers();
            
            imageManager = imageHandlers.ImageManager;//将handler中的manager赋值给form中的manager
            imageHandlers.PictureBox1 = pictureBox1;//将form中的picturebox赋值给handler中picturebox，不然为空
            imageHandlers.TimeLabel = label1;
           imageHandlers.TimeLabel.DataBindings.Add("Text", imageHandlers,"TimeElapsed");
            //imageHandlers.TimeLabel.Text = "";

            //各种事件
            pictureBox1.AllowDrop = true;
            //pictureBox2.AllowDrop = true;
            //图片事件
            pictureBox1.MouseWheel += imageHandlers.pic1_MouseWheel;
            pictureBox1.MouseMove += imageHandlers.pictureBox1_MouseMove;
            pictureBox1.MouseDown += imageHandlers.pictureBox1_MouseDown;
            pictureBox1.MouseUp += imageHandlers.pictureBox1_MouseUp;
            pictureBox1.DragEnter += imageHandlers.pictureBox1_DragEnterLoad;
            pictureBox1.DragDrop += imageHandlers.pictureBox1_DragDropLoad;
            //MouseWheel += new MouseEventHandler();
            //按钮事件
            button1.Click += imageHandlers.loadImage_Click;
            button2.Click += imageHandlers.saveImage_Click;
            button5.Click += imageHandlers.waterPic_Click;
            button6.Click += imageHandlers.cropPic_Click;
            button7.Click += imageHandlers.insertPic_Click;
            button8.Click += imageHandlers.drawOut_Click;



        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            /*
                TimeCounter.Start();
                //string path = openFileDialog1.FileName;
                Image image = (Bitmap)Image.FromFile(@"C:\Users\asus\Desktop\Gavin.jpg");
                bitmap = new Bitmap(image, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bitmap.Clone() as Image;
                imageManager.oldBitmap = bitmap.Clone() as Bitmap;
                TimeCounter.Stop();
                label1.Text = TimeCounter.Seconds.ToString();
                */
           // }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int width = pictureBox1.Width / 2;
            int height = pictureBox1.Height / 2;
            Image image;
            image = imageManager.resizeImage(pictureBox1.Image,width,height);
            pictureBox1.Image = image;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            /*
            TimeCounter.Start();
            imageManager.ChangeEffect(ImageEffect.GrayScale);
            pictureBox2.Image = imageManager.newBitmap.Clone() as Image;
            TimeCounter.Stop();
            label1.Text = TimeCounter.Seconds.ToString();
            */
        }
        private void button4_Click(object sender, EventArgs e)
        {
            /*
            TimeCounter.Start();
            imageManager.ChangeEffect(ImageEffect.Sharpen);
            pictureBox2.Image = imageManager.newBitmap.Clone() as Image;
            TimeCounter.Stop();
            label1.Text = TimeCounter.Seconds.ToString();
            */
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            

            if (pictureBox != null && e.Button == MouseButtons.Left)
            {
                Bitmap mapData = pictureBox.Image as Bitmap;
                DoDragDrop(mapData, DragDropEffects.Copy);

            }
        }

        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine(e.AllowedEffect);
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                // Save the current Fill brush so that you can revert back to this value in DragLeave.
                previousMap = pictureBox.Image.Clone() as Bitmap;

                // If the DataObject contains string data, extract it.
                if (e.Data.GetDataPresent(DataFormats.Bitmap))
                {
                    Bitmap dataMap = (Bitmap)e.Data.GetData(DataFormats.Bitmap);

                    // If the string can be converted into a Brush, convert it.


                    if (dataMap != null)
                    {
                        e.Effect = e.AllowedEffect;
                        pictureBox.Image = dataMap;

                    }
                }
                else if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    string effect = e.Data.GetData(DataFormats.Text).ToString();
                    Console.WriteLine(effect);
                    switch (effect)
                    {
                        case "fun1":
                            //imageManager.ChangeEffect(ImageEffect.GrayScale);
                            //pictureBox.Image = imageManager.newBitmap.Clone() as Image;
                            break;
                        default:
                            break;
                    }
                }

            }


        }
        private void pictureBox2_DragLeave(object sender, EventArgs e)
        {
            //Bitmap map = sender as Bitmap;
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Image = previousMap;
            }
            
        }

        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {

            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {


                // If the DataObject contains string data, extract it.
                if (e.Data.GetDataPresent(DataFormats.Bitmap))
                {
                    Bitmap dataMap = (Bitmap)e.Data.GetData(DataFormats.Bitmap);

                    // If the string can be converted into a Brush, convert it.


                    if (dataMap != null)
                    {
                        // e.Effect = e.AllowedEffect;
                        pictureBox.Image = dataMap;

                    }
                }
            }
        }



        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("hello");
            //string path = e.Data.GetData(DataFormats.FileDrop).ToString();
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            try
            {
                Image image = Image.FromFile(path);
                pictureBox1.Image = new Bitmap(image, pictureBox1.Width, pictureBox1.Height);
                imageManager.oldBitmap = image.Clone() as Bitmap;
            }
            catch (Exception E)
            {

                Console.WriteLine(E.Message);
            }


            Console.WriteLine(path);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button3_MouseMove_1(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;

            if (button != null && e.Button == MouseButtons.Left)
            {
                DoDragDrop(button.Text, DragDropEffects.Copy);
            }
        }

        private void button4_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Image image;
            image = imageManager.MakeWaterMark(pictureBox1.Image);
            pictureBox2.Image = image;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Image image;
           //image =  imageManager.DrawOutCropArea(0, 0, 200, 200);
           // pictureBox2.Image = image;
            //pictureBox1.Image = imageManager.Crop(0, 0, 200, 200);
        }

        private void button7_Click(object sender, EventArgs e)
        {
           // imageManager.oldBitmap = pictureBox2.Image as Bitmap;
            pictureBox2.Image = imageManager.InsertImage(pictureBox1.Image,0,0);
        }


    }


}



