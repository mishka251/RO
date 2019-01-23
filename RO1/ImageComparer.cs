using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;

namespace RO1
{

    enum distTypy { Pixel, Haar, Hist, Hamm }


    /// <summary>
    /// Отдельный элемент примитива
    /// </summary>
    class Block
    {
        public Point P;
        public Size S;
        public Block(Point p, Size s)
        {
            P = p;
            S = s;
        }
        public Block(int x, int y, int w, int h)
        {
            P = new Point(x, y);
            S = new Size(w, h);
        }
    }
    /// <summary>
    /// Отдельный примитив каскада
    /// </summary>
    class HaarFeatures
    {
        public Size S;
        public List<Block> WhiteHaar = new List<Block>(); //Набор белых примитивов
        public List<Block> BlackHaar = new List<Block>(); //Набор тёмных примитивов


        /// <summary>
        /// Добавить белый примитив
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void AddWhite(int x, int y, int w, int h)
        {
            WhiteHaar.Add(new Block(x, y, w, h));
        }
        /// <summary>
        /// Добавить белый элемент
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public void AddWhite(Point p, Size s)
        {
            WhiteHaar.Add(new Block(p, s));
        }
        /// <summary>
        /// Добавить тёмный элемент
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void AddBlack(int x, int y, int w, int h)
        {
            BlackHaar.Add(new Block(x, y, w, h));
        }
        /// <summary>
        /// Добавить тёмный элемент
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public void AddBlack(Point p, Size s)
        {
            BlackHaar.Add(new Block(p, s));
        }
        /// <summary>
        /// Установить размер примитива
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetSize(int w, int h)
        {
            S = new Size(w, h);
        }
        /// <summary>
        /// Установить размер примитива
        /// </summary>
        /// <param name="s"></param>
        public void SetSize(Size s)
        {
            S = s;
        }
    }
    static class ImageComparer
    {

        public static double dist(Image<Rgb, byte> img1, Image<Rgb, byte> img2, distTypy dt)
        {
            var im1 = predObr(img1);
            var im2 = predObr(img2);
            return dist(im1, im2, dt);
        }

        public static double dist(Image<Gray, byte> im1, Image<Gray, byte> im2, distTypy dt)
        {

            switch (dt)
            {
                case distTypy.Haar: return HaarDistance(im1, im2);
                case distTypy.Hamm: return distHam(im1, im2);
                case distTypy.Hist: return dist2(im1, im2);
                case distTypy.Pixel: return distPix(im1, im2);

                default: throw new Exception();
            }
        }


        /// <summary>
        /// Размер картинки для сравнения
        /// </summary>
        const int size = 20;
        /// <summary>
        /// Предобработка картинки
        /// 1)преобразуем в серый
        /// 2)обрезаем белое вокруг
        /// 3)приводим к размеру size x size
        /// </summary>
        /// <param name="img">исходная картинка</param>
        /// <returns>Обрезанная серая картинка</returns>
        public static Image<Gray, byte> predObr(Image<Rgb, byte> img)
        {
            var gray1 = img.Mat.ToImage<Gray, byte>();

            int minx = 0, miny = 0, maxx = gray1.Width, maxy = gray1.Height;

            for (int i = 0; i < gray1.Width; i++)
            {
                bool stop = false;
                for (int j = 0; j < gray1.Height; j++)
                    if (gray1[j, i].Intensity < 200)
                    {

                        stop = true;
                        break;
                    }
                if (stop)
                {
                    minx = i;
                    break;
                }
            }

            for (int i = 0; i < gray1.Height; i++)
            {
                bool stop = false;
                for (int j = 0; j < gray1.Width; j++)
                    if (gray1[i, j].Intensity < 200)
                    {

                        stop = true;
                        break;
                    }
                if (stop)
                {
                    miny = i;
                    break;
                }
            }

            for (int i = gray1.Height - 1; i >= 0; i--)
            {
                bool stop = false;
                for (int j = 0; j < gray1.Width; j++)
                    if (gray1[i, j].Intensity < 200)
                    {

                        stop = true;
                        break;
                    }
                if (stop)
                {
                    maxy = i;
                    break;
                }
            }

            for (int i = gray1.Width - 1; i >= 0; i--)
            {
                bool stop = false;
                for (int j = 0; j < gray1.Height; j++)
                    if (gray1[j, i].Intensity < 200)
                    {

                        stop = true;
                        break;
                    }
                if (stop)
                {
                    maxx = i;
                    break;
                }
            }

            gray1.ROI = new System.Drawing.Rectangle(minx, miny, maxx - minx, maxy - miny);

            return gray1.Resize(size, size, Inter.Linear);

        }

        /// <summary>
        /// Простая предобработка(не используется)
        /// Перегоняем изображение в серое и меняем размер до 50х50
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image<Gray, byte> predObr2(Image<Rgb, byte> img)
        {
            var gray1 = img.Mat.ToImage<Gray, byte>();
            return gray1.Resize(size, size, Inter.Linear);
        }
        /// <summary>
        /// расстояние(вариант1) не используется
        /// </summary>
        /// <param name="A">картинка 1</param>
        /// <param name="B">картинка 2</param>
        /// <returns></returns>
        public static double dist1(Image<Rgb, byte> A, Image<Rgb, byte> B)
        {
            var resize1 = predObr2(A);
            var resize2 = predObr2(B);

            return distPix(resize1, resize2);
        }
        /// <summary>
        /// Расстояние 2
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double dist2(Image<Rgb, byte> A, Image<Rgb, byte> B)
        {

            var resize1 = predObr(A);
            var resize2 = predObr(B);

            return distPix(resize1, resize2);
        }

        /// <summary>
        /// Расстояние 2
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double distPix(Image<Gray, byte> A, Image<Gray, byte> B)
        {
            double d = 0;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    d += Math.Abs(A[i, j].Intensity - B[i, j].Intensity);

            return d / (size * size * 255);
        }

        /// <summary>
        /// Втораяпопытка найти разницу изображений - гистограммы. фейл
        /// </summary>
        /// <param name="im1"></param>
        /// <param name="im2"></param>
        /// <returns></returns>
        public static double dist2(Image<Gray, byte> im1, Image<Gray, byte> im2)
        {
            DenseHistogram hist1 = new DenseHistogram(256, new RangeF(0.0f, 255.0f));
            DenseHistogram hist2 = new DenseHistogram(256, new RangeF(0.0f, 255.0f));

            hist1.Calculate(new Image<Gray, byte>[] { im1 }, true, null);
            hist2.Calculate(new Image<Gray, byte>[] { im2 }, true, null);

            return CvInvoke.CompareHist(hist1, hist2, HistogramCompMethod.Correl);
        }

        /// <summary>
        /// Первая попытка найти схожесть изображений - хеши
        /// </summary>
        /// <param name="img1">Первая картинка</param>
        /// <param name="img2">Вторая картинка</param>
        /// <returns></returns>
        public static ulong distHam(Image<Gray, byte> img1, Image<Gray, byte> img2)
        {
            ulong hash1 = calcImageHash(img1, false);
            ulong hash2 = calcImageHash(img2, false);

           
            // рассчитаем расстояние Хэмминга
           return  calcHammingDistance(hash1, hash2);
        }

        /// <summary>
        /// Для1 попытки
        /// </summary>
        /// <param name="src"></param>
        /// <param name="show_results"></param>
        /// <returns></returns>
        static ulong calcImageHash(Image<Gray, byte> src, bool show_results)
        {
            // построим хэш
            ulong hash = 0;

            int i = 0;
            // пробегаемся по всем пикселям изображения
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    // 1 канал
                    if (src[x, y].Intensity > 10)
                    {
                        hash |= (1u << i);
                    }
                    i++;
                }
            }
            return hash;
        }
        /// <summary>
        /// Для1 попытки
        ///         // рассчёт расстояния Хэмминга между двумя хэшами
        // http://en.wikipedia.org/wiki/Hamming_distance
        // http://ru.wikipedia.org/wiki/Расстояние_Хэмминга
        //

        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static ulong calcHammingDistance(ulong x, ulong y)
        {
            ulong dist = 0, val = x ^ y;

            // Count the number of set bits
            while (val > 0)
            {
                ++dist;
                val &= val - 1;
            }

            return dist;
        }




















        /// <summary>
        /// Расстояние через Хаара(рабочее на момент написания коммента 18.11 2 часа ночи
        /// </summary>
        /// <param name="im1">Первая картинка</param>
        /// <param name="im2">Вторая картинка</param>
        /// <returns></returns>
        public static double HaarDistance(Image<Gray, byte> im1, Image<Gray, byte> im2)

        {
            double d = double.MaxValue;

            var img1 = im1.Integral();//перегоняем в даблы
            var img2 = im2.Integral();

            var haar1 = ExtractHaar(img1);//находим каскады
            var haar2 = ExtractHaar(img2);
            return CorrelateTwoDecompression(haar1, haar2);//сравниваем
        }



        static List<HaarFeatures> Cascade = new List<HaarFeatures>();

        /// <summary>
        /// Конструктор для создания каскада
        /// </summary>
        static ImageComparer()
        {
            #region CreateCascade
            int l = 6; // Размер минимального квадрата паттерна
            // Используемые признаки хаара
            // 0 - белый, * - чёрный
            for (int s = l; s < l * 3 + 1; s += l)
            {
                //000
                //***
                HaarFeatures HF = new HaarFeatures();
                HF.AddWhite(0, 0, s * 3, s);
                HF.AddBlack(0, s, s * 3, s);
                HF.SetSize(s * 3, s * 2);
                Cascade.Add(HF);
                //0*
                //0*
                //0*
                HF = new HaarFeatures();
                HF.AddWhite(0, 0, s, s * 3);
                HF.AddBlack(s, 0, s, s * 3);
                HF.SetSize(s * 2, s * 3);
                Cascade.Add(HF);
                //0
                //*
                //0
                HF = new HaarFeatures();
                HF.AddWhite(0, 0, s, s);
                HF.AddWhite(0, s * 2, s, s);
                HF.AddBlack(0, s, s, s);
                HF.SetSize(s, s * 3);
                Cascade.Add(HF);
                //0*0
                HF = new HaarFeatures();
                HF.AddWhite(0, 0, s, s);
                HF.AddWhite(s * 2, 0, s, s);
                HF.AddBlack(s, 0, s, s);
                HF.SetSize(s * 3, s);
                Cascade.Add(HF);
                //0*
                //*0
                HF = new HaarFeatures();
                HF.AddWhite(0, 0, s, s);
                HF.AddWhite(s, s, s, s);
                HF.AddBlack(s, 0, s, s);
                HF.AddBlack(0, s, s, s);
                HF.SetSize(s * 2, s * 2);
                //000
                //0*0
                //000
                HF = new HaarFeatures();
                HF.AddWhite(0, 0, s * 3, s);
                HF.AddWhite(0, s * 2, s * 3, s);
                HF.AddWhite(0, s, s, s);
                HF.AddWhite(s * 2, s, s, s);
                HF.AddBlack(s, s, s, s);
                HF.SetSize(s * 3, s * 3);
                Cascade.Add(HF);
            }
            #endregion
        }

        /// <summary>
        /// Получаем хаар репрезентацию области 
        /// </summary>
        /// <param name="Im">Изображение</param>
        /// <returns></returns>
        static List<double> ExtractHaar(Image<Gray, double> Im)
        {

            /////////////////////////////////
            //  Перебираем все элементы из каскада
            //    Перебираем все возможные положения элемента по X, Y
            //      Учитываем тёмные элементы
            //      Учитываем светлые элементы
            List<double> Decompression = new List<double>(); //Получаемый Хаар

                                                             //Для всех созданных в каскаде объектов
                                                             //Делаем их экстракцию
            for (int i = 0; i < Cascade.Count; i++)
            {
                for (int x = 0; x < 0 + Im.Width - Cascade[i].S.Width; x += Cascade[i].S.Width)
                    for (int y = 0; y < 0 + Im.Height - Cascade[i].S.Height; y += Cascade[i].S.Height)
                    {
                        double Sum1 = 0;
                        double Sum2 = 0;
                        //Тёмные части паттерна
                        for (int j = 0; j < Cascade[i].BlackHaar.Count; j++)
                        {
                            double subsum = 0;
                            Point xy = Cascade[i].BlackHaar[j].P;
                            Size ss = Cascade[i].BlackHaar[j].S;
                            //////////////////////////////////////
                            //Подсчёт суммы в интересующей области
                            subsum += Im.Data[y + xy.Y + ss.Height, x + xy.X + ss.Width, 0];
                            subsum -= Im.Data[y + xy.Y + ss.Height, x + xy.X, 0];
                            subsum -= Im.Data[y + xy.Y, x + xy.X + ss.Width, 0];
                            subsum += Im.Data[y + xy.Y, x + xy.X, 0];
                            /////////////////////////////////////
                            Sum1 += subsum; //Интеграл по тёмной области
                        }
                        //Светлые части паттерна
                        for (int j = 0; j < Cascade[i].WhiteHaar.Count; j++)
                        {
                            double subsum = 0;
                            Point xy = Cascade[i].WhiteHaar[j].P;
                            Size ss = Cascade[i].WhiteHaar[j].S;
                            //////////////////////////////////////
                            //Подсчёт суммы в интересующей области
                            subsum += Im.Data[y + xy.Y + ss.Height, x + xy.X + ss.Width, 0];
                            subsum -= Im.Data[y + xy.Y + ss.Height, x + xy.X, 0];
                            subsum -= Im.Data[y + xy.Y, x + xy.X + ss.Width, 0];
                            subsum += Im.Data[y + xy.Y, x + xy.X, 0];
                            /////////////////////////////////////
                            Sum2 += subsum; //Интеграл по светлой области
                        }
                        if (Sum2 != 0)
                            Decompression.Add(Sum1 / Sum2); //Сохраняем число, характеризующее элемент
                        else
                            Decompression.Add(0);
                    }
            }
            return Decompression;
        }


        /// <summary>
        /// Сравниваем два вектора полученных из каскадов
        /// </summary>
        /// <param name="D1">Первый вектор</param>
        /// <param name="D2">Второй вектор</param>
        /// <returns></returns>
        private static double CorrelateTwoDecompression(List<double> D1, List<double> D2)
        {
            //Проверка на случай вдруг что пошло не так
            if (D1.Count != D2.Count)
            {
                throw new Exception("Каскады разных размеров");
            }
            double d = 0;
            for (int i = 0; i < D1.Count; i++)
            {
                d += Math.Abs(D1[i] - D2[i]); //Характеристическим числом близасти векторов будет являтся абсолютная разность между ними
            }
            return d;

        }
    }
}
