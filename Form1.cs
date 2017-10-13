using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Imaging;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace TaiwanSmaile_Taipei101
{
    public partial class Form1 : Form
    {
        //宣告分割格數
        int rx = 1920, ry = 1080; 
        int cnt_x, cnt_y; 
        int b_size = 30; 
        int scale = 8; 
        
        private Capture camera; //camera file
        private HaarCascade haar; //抓取xml
        
        Image<Bgr, byte> face_image; //EmguCV抓取image
        Image<Gray, byte> gray;
        
        //人臉抓取的圖像處理
        MCvAvgComp[] faces;
        MCvAvgComp[] faces_empty;
        Bitmap ExtFaces;
        Bitmap ExtractedFace;
        Graphics FaceCanvas;
        int faceNo; //人臉索引值宣告
        int last_faceNo = 0; //前一個人臉索引值

        PictureBox[] pic = new PictureBox[2310]; //PictureBox陣列存放faceNO

        Bitmap taiwan = new Bitmap(@"images101-2.bmp");//台灣底圖置入宣告

        //台灣logo宣告
        string logo = "Taiwanlog.jpg";
        Image Logo;

        //半透明參數宣告
        //圖片透明度陣列參數
        float[][] nArray ={ new float[] {1, 0, 0, 0, 0}, 
                    new float[] {0, 1, 0, 0, 0}, 
                    new float[] {0, 0, 1, 0, 0}, 
                    new float[] {0, 0, 0, 0.4f, 0}, 
                    new float[] {0, 0, 0, 0, 1}};
        ColorMatrix matrix;
        ImageAttributes attributes = new ImageAttributes();

        //初始圖片置入處理
        //字串陣列，存放原始圖片檔名
        string[] baseImage = { "01.jpg", "02.jpg", "03.jpg", "04.jpg", "05.jpg", "06.jpg", "07.jpg", "09.jpg", "10.jpg", "11.jpg", "12.jpg", "13.jpg", "14.jpg", "15.jpg", "16.jpg", "17.jpg", "18.jpg", "19.jpg", "20.jpg" };
        Random rnd = new Random();//亂數宣告
        Image photo;//宣告原始圖片
        Image[] photos = new Image[20];

        //動畫處理宣告
        int vx, vy; //動畫x,y變化量
        int v_size; //動畫大小變化量(正方形)
        int fps = 15; //動畫影格
        int ani_count = 0; //動畫計數

        int c_flag = 0; //圖片顯示計數器
        //字串陣列，存放倒數圖片檔名
        string[] count_time = { "1.jpg", "2.jpg", "3.jpg", "4.jpg" };
        Image timePhoto;//宣告倒數圖片
        Image[] timePhotos = new Image[6];//存放陣列

       /* Image startPicture;//宣告開始圖片
        string startpic = "start.jpg";//開始圖片檔名*/

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Logo位置處理
            Logo = Image.FromFile(logo);
            logo_pic.Image = Logo;
            logo_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            logo_pic.Location = new Point(b_size, (ry - logo_pic.Size.Height) - b_size);

           /* //開始圖片位置處理
            startPicture = Image.FromFile(startpic);
            start_pic.Image = startPicture;
            start_pic.SizeMode = PictureBoxSizeMode.Zoom;
            start_pic.Location = new Point(b_size + (logo_pic.Size.Width + b_size ), (ry - start_pic.Size.Height) - b_size);
            startPicture.RotateFlip(RotateFlipType.Rotate270FlipNone); //旋轉圖片*/


            //初始圖片置入處理
            for (int p = 0; p < baseImage.Length; p++)
            {
                photo = Image.FromFile(@"baseImage\" + baseImage[p]); //依照路徑取出原始圖檔
                photo.RotateFlip(RotateFlipType.Rotate270FlipNone); //旋轉圖片
                photos[p] = photo; //圖片存放陣列
            }

            //倒數圖片置入處理
            for (int q = 0; q < count_time.Length; q++)
            {
                timePhoto = Image.FromFile(@"countImage\" + count_time[q]); //依照路徑取出原始圖檔
                //timePhoto.RotateFlip(RotateFlipType.Rotate270FlipNone); //旋轉圖片
                timePhotos[q] = timePhoto; //圖片存放陣列 
            }
            Countbox.Location = new Point(b_size + (logo_pic.Size.Width + b_size * 3), (ry - Countbox.Size.Height) - b_size);
            Countbox.SizeMode = PictureBoxSizeMode.Zoom;

            //計算分割格數
            cnt_x = rx / b_size; //計算X 軸格數
            cnt_y = ry / b_size; //計算Y 軸格數

            //左下圖片設定
            pictureBoxCenter.Visible = false; //隱藏pictureBoxCenter
            pictureBoxCenter.Size = new Size(b_size * scale, b_size * scale); //計算pictureBoxCenter大小
            pictureBoxCenter.Location = new Point(rx - (b_size * scale), ry - (b_size * scale)); //計算pictureBoxCenter位置

            v_size = (b_size * scale) / (fps + fps/12);//動畫的size變化量

            camera = new Capture(0); //取得camera值
            faceNo = 0; //定義人臉索引值

            Picture_timer.Stop(); //Picture_timer停止
            Animation_timer.Stop();


            //迴圈新增格數排列
            int sy = ry, sx = 0; //(x,y) = (sx,sy)
            int drt = -1; //初始判斷換行參數 
            for (int i = 0; i < cnt_x; i++)
            {
                for (int j = 0; j < cnt_y; j++)
                {
                    //定義新增格子
                    PictureBox px = new PictureBox(); //重複新增PictureBox
                    px.Width = b_size;//設定PictureBox長寬(正方形)
                    px.Height = b_size;
                    
                    //Y 軸由左至右依序遞減新增PictureBox
                    if (drt < 0)
                    {
                        sy = sy + drt * b_size; 
                    }
                    
                    px.Location = new Point(sx, sy);//設定新增格數的Location
                    px.Visible = true;
                    Controls.Add(px);
                    
                    int mIndex = rnd.Next(baseImage.Length); //設定亂數索引值
                    photo = photos[mIndex];
                    px.Image = photo; //初始圖片放入px
                    
                    //尋找哪一個為台灣內的格子
                    //運算台灣底圖的解析度
                    int pb_x  = px.Location.X;
                    int pb_y = px.Location.Y;
                    pb_x = pb_x / b_size;
                    pb_y = (pb_y + b_size) / b_size;
                    //taiwan.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    Color tw_color = taiwan.GetPixel(pb_x, pb_y-1);
                   
                    Color sc = tw_color;

                    //半透明處理
                    matrix = new ColorMatrix(nArray);
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    Image srcImage = photo;
                    Bitmap resultImage = new Bitmap(srcImage.Width, srcImage.Height);
                    Graphics g = Graphics.FromImage(resultImage);
                    g.DrawImage(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, attributes);

                    int rgb_r = sc.R;
                    int rgb_g = sc.G;
                    int rgb_b = sc.B;
                    
                    //判斷底圖為白色就是台灣內的格子，是否顯示為半透明
                    if (rgb_r == 255 && rgb_g == 255 && rgb_b == 255)
                    {
                        px.Image = photo;
                    }
                    else
                    {
                        px.Image = resultImage;
                    }
                    
                    //Y 軸由右至左依序遞減新增PictureBox
                    if (drt > 0)
                    {
                        sy = sy + drt * b_size;
                    }
                    
                    px.SizeMode = PictureBoxSizeMode.Zoom;
                    pic[faceNo] = px;//將faceNO放入picturebox陣列
                    faceNo++;
                }

                //跳行
                sx = sx + b_size;
                drt = -1 * drt;
            }
            
            faceNo = 0;//初始定義為0
            string date_path = @"C:\Emgu\emgucv-windows-universal-cuda 2.9.0.1922\opencv\data\haarcascades\haarcascade_frontalface_default.xml";
            haar = new HaarCascade(date_path);//抓取人臉xml路徑
        }

        //啟動camera，抓取人臉速度與timer一樣
        private void Camera_timer_Tick(object sender, EventArgs e)
        {
            if (camera != null)
            {
                face_image = camera.QueryFrame();//取得camera輸出影像
                try
                {
                    face_detect();//人臉抓取
                }
                catch
                {

                }
            }
        }

        private void face_detect()
        {
            gray = face_image.Convert<Gray, byte>();//將camera輸出影像轉為灰階(EmguCV 語法)

            //人臉抓取語法
            faces =
                gray.DetectHaarCascade(haar, 1.2, 10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(25, 25))[0];
            

            Image Input = face_image.ToBitmap();//轉檔camera輸出影像為Bitmap

            //判斷人臉抓取是否為空
            if (faces.Length > 0)
            {
                if (c_flag == 0)
                {
                    Camera_timer.Stop();//抓取到人臉後camera_timer停止
                    
                    Count_timer.Start();//計數圖片顯示
                }
                else
                {
                    //Array.Clear(pic, 0, pic.Length);
                    Countbox.Image = timePhotos[c_flag];
                    c_flag = 0;
                    //Array.Clear(pic, 0, pic.Length);
                    Camera_timer.Stop();//抓取到人臉後camera_timer停止

                    MCvAvgComp face = faces[0];//宣告取得的人臉為face
                    //face_image.Draw(face.rect, new Bgr(Color.Red), 3);//將camera輸出影像對應抓取到的人臉，標記紅框

                    //將取得的人臉分割出來並另存為Bitmap
                    ExtractedFace = new Bitmap(face.rect.Width, face.rect.Height);
                    FaceCanvas = Graphics.FromImage(ExtractedFace);
                    FaceCanvas.DrawImage(Input, 0, 0, face.rect, GraphicsUnit.Pixel);
                    ExtractedFace.RotateFlip(RotateFlipType.Rotate270FlipNone);//旋轉影像

                    pictureBoxCenter.Visible = true;//顯示大圖影像

                    pictureBoxCenter.Image = ExtractedFace;//將分割的人臉載入當前的picturebox中
                    pictureBoxCenter.SizeMode = PictureBoxSizeMode.Zoom;

                    Invalidate();//清空box，重新繪製圖片

                    //半透明處理
                    matrix = new ColorMatrix(nArray);
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    Image srcImage = (Image)ExtractedFace;
                    Bitmap resultImage = new Bitmap(srcImage.Width, srcImage.Height);
                    Graphics g = Graphics.FromImage(resultImage);
                    g.DrawImage(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, attributes);
                    ExtFaces = resultImage;

                    in_taiwan();

                    //分割動畫距離格數
                    vx = (pictureBoxCenter.Location.X - pic[faceNo].Location.X) / fps;
                    vy = (pictureBoxCenter.Location.Y - pic[faceNo].Location.Y) / fps;

                    picout();

                    faceNo = (faceNo + 1) % (cnt_x * cnt_y);//faceNO++，限制圖片載量(台灣外部)
                    faces = null;//抓取一次人臉，清空一次，釋放記憶體
                    
                    Countbox.Visible = false;
                   
                }
                
            }

            //攝影機影像顯示
            pictureBoxCamera.Image = face_image.ToBitmap();
            pictureBoxCamera.SizeMode = PictureBoxSizeMode.Zoom;
        }

        //判斷在台灣內顯示
        private void in_taiwan()
        {
            bool b_white = false;
            while(!b_white)
            {
                int pb_x = pic[faceNo].Location.X;
                int pb_y = pic[faceNo].Location.Y;
                
                pb_x = pb_x / b_size;
                pb_y = (pb_y + b_size) / b_size;
                
                Color tw_color = taiwan.GetPixel(pb_x, pb_y - 1);
                Color sc = tw_color;

                int rgb_r = sc.R;
                int rgb_g = sc.G;
                int rgb_b = sc.B;
                if (rgb_r == 255 && rgb_g == 255 && rgb_b == 255)
                {
                    b_white = true;
                }
                else
                {
                    faceNo = (faceNo + 1) % (cnt_x * cnt_y);//faceNO++，限制圖片載量(台灣內部)
                }
            }
        }
        
        private void picout()
        {
            Picture_timer.Start();//box載入影像後停留timer
            
            pic[last_faceNo].Size = new Size(b_size, b_size);//抓取新人臉後，前一張影像回復為原來大小
            pic[faceNo].Image = ExtractedFace;
            
            //影像紅框顯示
            Redbox.Location = new Point(pic[faceNo].Location.X - 3, pic[faceNo].Location.Y - 3);
            Redbox.Visible = true;
            Redbox.BringToFront();

            //最新人臉影像放大
            pic[faceNo].Size = new Size(b_size+54, b_size+54);
            pic[faceNo].BringToFront();

            last_faceNo = faceNo;//抓取新人臉後，定義前一張影像
            pic[faceNo].SizeMode = PictureBoxSizeMode.Zoom;
            Invalidate();
        }

        //pictureBoxCenter載入影像停留時間
        private void Picture_timer_Tick(object sender, EventArgs e)
        {
            pictureBoxCenter.Visible = false;
            Invalidate();

            Picture_timer.Stop();
            
            ani_count = 0;//動畫計數器
            pictureBoxCenter.Visible = true;
            Animation_timer.Start();//啟動動畫
        }

        //啟動動畫timer
        private void Animation_timer_Tick(object sender, EventArgs e)
        {
            //定義距離每格數的(X，Y)
            int new_x = pictureBoxCenter.Location.X - vx;
            int new_y = pictureBoxCenter.Location.Y - vy;
            pictureBoxCenter.Location = new Point(new_x, new_y);

            //定義每次移動一格，box大小的變化量
            int new_w = pictureBoxCenter.Width - v_size;
            int new_h = pictureBoxCenter.Height - v_size;
            pictureBoxCenter.Size = new Size(new_w, new_h);

            ani_count++;//每移動一格，計數器+1(移動一格timer執行一次)

            //設定影像紅框閃爍，以2的餘數做判斷
            if((ani_count%2) == 1) 
            {
                Redbox.Visible = true;
            }
            else
            {
                Redbox.Visible = false;
            }

            //若做完動畫後，pictureBoxCenter回復原來大小與位置，等待下一次執行
            if (ani_count > (fps - 1))
            {
                ani_count = 0;

                pictureBoxCenter.Size = new Size(b_size * scale, b_size * scale); //計算pictureBoxCenter大小
                pictureBoxCenter.Location = new Point(rx - (b_size * scale), ry - (b_size * scale)); //計算pictureBoxCenter位置
                pictureBoxCenter.Visible = false;

                Animation_timer.Stop();//動畫結束

                Camera_timer.Start();//重新啟動偵測人臉
            
            }
        }

        private void Count_timer_Tick(object sender, EventArgs e)
        {
            
            Countbox.Visible = true;
            Countbox.Image = timePhotos[c_flag];
            c_flag++;
            
            if(c_flag > 3)
            {
                face_image = camera.QueryFrame();
                faces =
                    gray.DetectHaarCascade(haar, 1.2, 10,
                    HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(25, 25))[0];
                
                Count_timer.Stop();
                
                Camera_timer.Start();
            }
        }
    }
}
