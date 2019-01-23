using System;
using System.Collections.Generic;
using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace RO1
{
    class MyDetector
    {
        string proj;
        //VideoCapture capture;
        CascadeClassifier faceCascade;
        CascadeClassifier eyeCascade;

        List<System.Drawing.Rectangle> eyesLocs;
        List<Image<Gray, byte>> OpenEyes, CloseEyes;

        public MyDetector()
        {
            proj = Directory.GetCurrentDirectory();
            proj = Directory.GetParent(proj).FullName;
            proj = Directory.GetParent(proj).FullName;
            //for face
            string file1 = proj + "\\xml\\" + "haarcascade_frontalface_alt.xml";
            string file2 = proj + "\\xml\\" + "haarcascade_eye.xml";


            faceCascade = new CascadeClassifier(file1);
            eyeCascade = new CascadeClassifier(file2);

            LoadReadySamples();

        }

        public void ProcessImage(Image<Rgb, byte> image, distTypy dt)
        {
            image.Save("image.jpg");
            var eyes = SearchEye(image);
            if (eyes.Count == 0)
                throw new Exception("Нет глаз");
            int ind = 0;
            foreach (var eye in eyes)
            {
                System.Drawing.Color color = System.Drawing.Color.Green;

                if (checkEye(eye, dt) == State.Closed)
                    color = System.Drawing.Color.Red;
                else
                    color = System.Drawing.Color.Green;
                eye.Save("eye" + ind + ".jpg");

                image.Draw(eyesLocs[ind], new Rgb(color), 10);
                ind++;
            }
        }

        public void Save()
        {
            for (int i = 0; i < CloseEyes.Count; i++)
                CloseEyes[i].Bitmap.Save("close" + i + ".jpg");

            for (int i = 0; i < OpenEyes.Count; i++)
                OpenEyes[i].Bitmap.Save("open" + i + ".jpg");
        }

        /// <summary>
        /// Загрузка примеров(с обучением)
        /// Берутся картинки из папки Trainig
        /// В них выделяются глаза и по названию(open/close) заносятся в примеры
        /// </summary>
        public void LoadSamples()
        {
            OpenEyes = new List<Image<Gray, byte>>();
            CloseEyes = new List<Image<Gray, byte>>();

            DirectoryInfo trainig = new DirectoryInfo(proj + "\\Training\\");
            var files = trainig.GetFiles();
            foreach (var file in files)
            {
                if (file.Extension == ".png" || file.Extension == ".jpg")
                {
                    Image<Rgb, byte> img = new Image<Rgb, byte>(file.FullName);
                    var eyes = SearchEye(img);
                    for (int i = 0; i < eyes.Count; i++)
                        eyes[i].ROI = new System.Drawing.Rectangle(5, 5, 60, 60);
                    if (file.Name.Contains("open"))
                        OpenEyes.AddRange(eyes);
                    else
                        CloseEyes.AddRange(eyes);
                }
            }
        }

        /// <summary>
        /// Загрузка готовых примеров из папки с ехешником
        /// </summary>
        public void LoadReadySamples()
        {
            OpenEyes = new List<Image<Gray, byte>>();
            CloseEyes = new List<Image<Gray, byte>>();
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var fils = dir.GetFiles();
            foreach (var file in fils)
            {
                if (!(file.Extension == ".png" || file.Extension == ".jpg"))
                    continue;

                if (file.Name.Contains("open"))
                    OpenEyes.Add(new Image<Gray, byte>(file.FullName));

                if (file.Name.Contains("close"))
                    CloseEyes.Add(new Image<Gray, byte>(file.FullName));

            }

        }

        /// <summary>
        /// Поиск глаз на картинке
        /// </summary>
        /// <param name="image">Картинка</param>
        /// <returns></returns>
        List<Image<Gray, byte>> SearchEye(Image<Rgb, Byte> image)
        {
            Image<Gray, Byte> grayImage = image.Mat.ToImage<Gray, Byte>().Clone();
            var Face = faceCascade.DetectMultiScale(grayImage);
            List<Image<Gray, byte>> eyes = new List<Image<Gray, byte>>();
            eyesLocs = new List<System.Drawing.Rectangle>();
            const int dy = 60;
            // var Face = grayImage.DetectHaarCascade(faceCascade)[0];
            foreach (var face in Face)
            {
                var ROI = face;
                ROI.Height /= 2;
                grayImage.ROI = ROI;
                var Eye =
                    eyeCascade.DetectMultiScale(image: grayImage);

                foreach (var eye in Eye)
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle
                    {
                        X = ROI.X + eye.X,
                        Y = ROI.Y + eye.Y,
                        Size = eye.Size
                    };
                    Image<Gray, byte> eye_img = grayImage.Clone();
                    eye_img.ROI = rect;
                    eye_img = eye_img.ThresholdAdaptive(new Gray(200), AdaptiveThresholdType.MeanC,
                      ThresholdType.Binary, 7, new Gray(12));

                    var sc1 = new Gray(190);
                    var sc2 = new Gray(255);
                    var eye2 = eye_img.InRange(sc1, sc2);
                    eyes.Add(eye2.Clone().Resize(60, 60, Inter.Linear));
                    eyesLocs.Add(rect);
                }
            }

            if (Face.Length == 0)
            {
                var Eye = eyeCascade.DetectMultiScale(image: grayImage);
                foreach (var eye in Eye)
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle
                    {
                        X = eye.X,
                        Y = eye.Y,
                        Size = eye.Size
                    };
                    Image<Gray, byte> eye_img = grayImage.Clone();
                    eye_img.ROI = rect;


                    eye_img = eye_img.ThresholdAdaptive(new Gray(200), AdaptiveThresholdType.MeanC,
                      ThresholdType.Binary, 7, new Gray(12));

                    var sc1 = new Gray(190);
                    var sc2 = new Gray(255);
                    var eye2 = eye_img.InRange(sc1, sc2);
                    eyes.Add(eye2.Clone().Resize(60, 60, Inter.Linear));
                    eyesLocs.Add(rect);
                }
            }

            return eyes;
        }

        public enum State { Opened, Closed }

        /// <summary>
        /// Проверка глаза - собственно наша задача
        /// </summary>
        /// <param name="eye">Картинка с глазом</param>
        /// <returns></returns>
        public State checkEye(Image<Gray, byte> eye, distTypy dt)
        {
            eye.ROI = new System.Drawing.Rectangle(0, 0, 60, 60);
            double open_range = double.MaxValue,// ImageComparer.dist2(eye, OpenEyes[0]), //eye.AbsDiff(OpenEyes[0]).CountNonzero().Sum(),
                close_range = double.MaxValue; //ImageComparer.dist2(eye, CloseEyes[0]);// eye.AbsDiff(CloseEyes[0]).CountNonzero().Sum(); ;

            for (int i = 0; i < OpenEyes.Count; i++)
                open_range = Math.Min(ImageComparer.dist(eye, OpenEyes[i], dt), open_range);

            for (int i = 0; i < CloseEyes.Count; i++)
                close_range = Math.Min(ImageComparer.dist(eye, CloseEyes[i], dt), close_range);

            return close_range < open_range ? State.Closed : State.Opened;
        }

    }
}
