using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bePaint;
using System.Drawing.Drawing2D;
using System.Numerics;
namespace PaintStudy
{
    public partial class TfmPaintStudy : Form
    {
        Graphics g;
        int PointSize = 10;

        public TfmPaintStudy()
        {
            InitializeComponent();
        }

        private void btnPaint_Click(object sender, EventArgs e)
        {
            // Створюємо малюнок та об'єкт графіки для малювання
            Bitmap bmp = new Bitmap(pbPaint.Width, pbPaint.Height);
            g = Graphics.FromImage(bmp);
            try
            {
                // Заповнюємо фоном
                g.Clear(Color.LightYellow);
                // Змінюємо напрямок по Y
                g.ScaleTransform(1, -1);
                // Зміщуємо систему координат на вектор(100;100)
                g.TranslateTransform(100, 100 - 1 * pbPaint.Height);

                // Блок налаштувань
                PointSize = Convert.ToInt32(txtPointSize.Text);

                // Отримуємо дані
                PointF p1 = PointFromString(txtP1.Text);
                PointF p2 = PointFromString(txtP2.Text);
                PointF p3 = PointFromString(txtP3.Text);
                float dy = Convert.ToSingle(txtVal1.Text);

                // Малюємо координатні осі
                g.DrawLine(new Pen(Color.Green), new PointF(-100, 0), new PointF(pbPaint.Width, 0));
                g.DrawLine(new Pen(Color.Blue), new PointF(0, -100), new PointF(0, pbPaint.Height));
                DrawText("X", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Green, new PointF(pbPaint.Width -100 - 3f * PointSize, 0.5f * PointSize));
                DrawText("Y", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Blue, new PointF(PointSize / 2, pbPaint.Height - 100 - PointSize));

                // Перша пряма
                PaintLine(p1, p2, Color.Fuchsia);
                PaintPoint(p1, PointSize);
                PaintPoint(p2, PointSize);
                DrawText("P1(x1;y1)", new Font("Tahoma", 1.5f * PointSize), p1);
                DrawText("P2(x2;y2)", new Font("Tahoma", 1.5f * PointSize), p2);

                // Обраховуємо досліджуєму функцію
                PointF pres = bePaint.geometry.GetOrtoPointOnDistance(p1, p2, p3, dy);



                // Вивод результату
                PaintLine(p3, pres, DashStyle.DashDotDot, Color.Tomato);
                PaintPoint(p3, PointSize, Brushes.Orange);
                PaintPoint(pres, PointSize, Brushes.Red);

                DrawText("P_inst(xi;yi)", new Font("Tahoma", 1.5f * PointSize), p3);
                DrawText("Pres(x;y)", new Font("Tahoma", 1.5f * PointSize), pres);
            }
            finally
            {
                // Знищуємо об'єкт графіки та передаємо малюнок Пайнт боксу
                g.Dispose();
            }
            pbPaint.Image = bmp;
        }

        /// <summary>Виводимо текст</summary>
        /// <param name="s">Сам текст</param>
        /// <param name="f">Фонт</param>
        /// <param name="c">Точка виводу</param>
        private void DrawText(string s, Font f, PointF c)
        {
            SizeF size = g.MeasureString(s, f);
            PointF pt = new PointF(c.X + size.Height / 10, c.Y - size.Height / 10);

            g.TranslateTransform(pt.X, pt.Y);
            g.ScaleTransform(1, -1);
            g.DrawString(s, f, Brushes.Black, new PointF(0, 0));
            g.ScaleTransform(1, -1);
            g.TranslateTransform(-pt.X, -pt.Y);
        }
        /// <summary>Виводимо текст</summary>
        /// <param name="s">Сам текст</param>
        /// <param name="f">Фонт</param>
        /// <param name="color">Колір тексту</param>
        /// <param name="c">Точка виводу</param>
        private void DrawText(string s, Font f, Brush brush, PointF c)
        {
            SizeF size = g.MeasureString(s, f);
            PointF pt = new PointF(c.X + size.Height / 10, c.Y - size.Height / 10);

            g.TranslateTransform(pt.X, pt.Y);
            g.ScaleTransform(1, -1);
            g.DrawString(s, f, brush, new PointF(0, 0));
            g.ScaleTransform(1, -1);
            g.TranslateTransform(-pt.X, -pt.Y);
        }



        private void PaintPoint(PointF p, int size)
        {
            RectangleF r = new RectangleF(p.X - size / 2, p.Y - size / 2, size, size);
            g.FillEllipse(Brushes.Blue, r);
            g.DrawArc(new Pen(Color.Black), r, 0, 360);
        }

        private void PaintPoint(PointF p, int size, Brush b)
        {
            RectangleF r = new RectangleF(p.X - size / 2, p.Y - size / 2, size, size);
            g.FillEllipse(b, r);
            g.DrawArc(new Pen(Color.Black), r, 0, 360);
        }

        private void PaintLine(PointF p1, PointF p2, Color c)
        {
            bePaint.LineDef l1 = new bePaint.LineDef(p1, p2);
            float x1 = -100;
            float y1 = -(l1.A * x1 + l1.C) / l1.B;

            float x2 = pbPaint.Width - 100;
            float y2 = -(l1.A * x2 + l1.C) / l1.B;
            g.DrawLine(new Pen(c), new PointF(x1, y1), new PointF(x2, y2));
        }
        private void PaintLine(LineDef line, Color c)
        {
            bePaint.LineDef l1 = new bePaint.LineDef(line.A,line.B,line.C);
            float x1 = -100;
            float y1 = -(l1.A * x1 + l1.C) / l1.B;

            float x2 = pbPaint.Width - 100;
            float y2 = -(l1.A * x2 + l1.C) / l1.B;
            g.DrawLine(new Pen(c), new PointF(x1, y1), new PointF(x2, y2));
        }
        private void PaintLine(PointF p1, PointF p2, DashStyle style, Color c)
        {
            Pen pen = new Pen(c);
            try
            {
                pen.DashStyle = style;
                pen.DashPattern = new float[] { 2 * PointSize, PointSize};
                bePaint.LineDef l1 = new bePaint.LineDef(p1, p2);
                float x1 = -100;
                float y1 = -(l1.A * x1 + l1.C) / l1.B;

                float x2 = pbPaint.Width - 100;
                float y2 = -(l1.A * x2 + l1.C) / l1.B;
                g.DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
            }
            finally { pen.Dispose(); }
        }


        private PointF PointFromString(string s)
        {
            PointF p = new PointF();
            int i = s.IndexOf(';');
            float x = Convert.ToSingle(s.Substring(0, i));
            float y = Convert.ToSingle(s.Substring(i+1, s.Length - i - 1));
            p.X = x;
            p.Y = y;
            return p;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Створюємо малюнок та об'єкт графіки для малювання
            Bitmap bmp = new Bitmap(pbPaint.Width, pbPaint.Height);
            g = Graphics.FromImage(bmp);
            try
            {
                // Заповнюємо фоном
                g.Clear(Color.LightYellow);
                // Змінюємо напрямок по Y
                g.ScaleTransform(1, -1);
                // Зміщуємо систему координат на вектор(100;100)
                g.TranslateTransform(100, 100 - 1 * pbPaint.Height);

                // Блок налаштувань
                PointSize = Convert.ToInt32(txtPointSize.Text);

                // Отримуємо дані
                PointF p1 = PointFromString(textBox1.Text);
                PointF p2 = PointFromString(textBox2.Text);
                float delta = Convert.ToSingle(textBox3.Text);

                // Малюємо координатні осі
                g.DrawLine(new Pen(Color.Green), new PointF(-100, 0), new PointF(pbPaint.Width, 0));
                g.DrawLine(new Pen(Color.Blue), new PointF(0, -100), new PointF(0, pbPaint.Height));
                DrawText("X", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Green, new PointF(pbPaint.Width - 100 - 3f * PointSize, 0.5f * PointSize));
                DrawText("Y", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Blue, new PointF(PointSize / 2, pbPaint.Height - 100 - PointSize));

                // Перша пряма
                PaintLine(p1, p2, Color.Fuchsia);
                PaintPoint(p1, PointSize);
                PaintPoint(p2, PointSize);
                DrawText("P1(x1;y1)", new Font("Tahoma", 1.5f * PointSize), p1);
                DrawText("P2(x2;y2)", new Font("Tahoma", 1.5f * PointSize), p2);

                // Обраховуємо досліджуєму функцію
               LineDef pres = bePaint.geometry.GetLineKollinear(p1, p2, delta);

                PaintLine(pres, Color.Red);
                // Вивод результату
               // PaintLine( pres, DashStyle.DashDotDot, Color.Tomato);
               // PaintPoint(p3, PointSize, Brushes.Orange);
               // PaintPoint(pres, PointSize, Brushes.Red);

              //  DrawText("P_inst(xi;yi)", new Font("Tahoma", 1.5f * PointSize), p3);
             //   DrawText("Pres(x;y)", new Font("Tahoma", 1.5f * PointSize), pres);
            }
            finally
            {
                // Знищуємо об'єкт графіки та передаємо малюнок Пайнт боксу
                g.Dispose();
            }
            pbPaint.Image = bmp;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            // Створюємо малюнок та об'єкт графіки для малювання
            Bitmap bmp = new Bitmap(pbPaint.Width, pbPaint.Height);
            g = Graphics.FromImage(bmp);
            try
            {
                // Заповнюємо фоном
                g.Clear(Color.LightYellow);
                // Змінюємо напрямок по Y
                g.ScaleTransform(1, -1);
                // Зміщуємо систему координат на вектор(100;100)
                g.TranslateTransform(100, 100 - 1 * pbPaint.Height);

                // Блок налаштувань
                PointSize = Convert.ToInt32(txtPointSize.Text);

                // Отримуємо дані
                string[] SplitLineFromString = textBox4.Text.Split(';');
                float A = Convert.ToSingle(SplitLineFromString[0]);
                float B = Convert.ToSingle(SplitLineFromString[1]);
                float C = Convert.ToSingle(SplitLineFromString[2]);
                string[] SplitVectorFromString = textBox5.Text.Split(';');
                LineDef line = new LineDef(Convert.ToSingle(SplitLineFromString[0]), Convert.ToSingle(SplitLineFromString[1]), Convert.ToSingle(SplitLineFromString[2]));
                Vector2 vector = new Vector2(Convert.ToSingle(SplitVectorFromString[0]), Convert.ToSingle(SplitVectorFromString[1]));
             


                // Малюємо координатні осі
                g.DrawLine(new Pen(Color.Green), new PointF(-100, 0), new PointF(pbPaint.Width, 0));
                g.DrawLine(new Pen(Color.Blue), new PointF(0, -100), new PointF(0, pbPaint.Height));
                DrawText("X", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Green, new PointF(pbPaint.Width - 100 - 3f * PointSize, 0.5f * PointSize));
                DrawText("Y", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Blue, new PointF(PointSize / 2, pbPaint.Height - 100 - PointSize));

                // Перша пряма
                LineDef l1 = new LineDef(A,B,C);
                PaintLine(l1, Color.Fuchsia);
                //DrawText("P1(x1;y1)", new Font("Tahoma", 1.5f * PointSize), p1);
                float Vx = Convert.ToSingle(SplitVectorFromString[0]);
                float Vy = Convert.ToSingle(SplitVectorFromString[1]);


                //Малюємо вектор
                PointF p1 = new PointF(400, CalcLineY(l1, 400));
                PointF p2 = new PointF(p1.X + Vx, p1.Y + Vy);
                PaintPoint(p1, PointSize, Brushes.Green);
                PaintPoint(p2, PointSize, Brushes.GreenYellow);
                DrawArrowLine(new Pen(Brushes.Blue, 2), p1, p2, PointSize, 2);

                // Обраховуємо досліджуєму функцію
                LineDef lRes = bePaint.geometry.ShiftLine(l1, new Vector2(Vx, Vy));

                // Вивод результату
                PaintLine(lRes, Color.Orange);
                // Обраховуємо досліджуєму функцію

              //  LineDef pres = bePaint.geometry.ShiftLine(line, vector);

               // PaintLine(pres, Color.Red);
                // Вивод результату
                // PaintLine( pres, DashStyle.DashDotDot, Color.Tomato);
                // PaintPoint(p3, PointSize, Brushes.Orange);
                // PaintPoint(pres, PointSize, Brushes.Red);

                //  DrawText("P_inst(xi;yi)", new Font("Tahoma", 1.5f * PointSize), p3);
                //   DrawText("Pres(x;y)", new Font("Tahoma", 1.5f * PointSize), pres);
            }
            finally
            {
                // Знищуємо об'єкт графіки та передаємо малюнок Пайнт боксу
                g.Dispose();
            }
            pbPaint.Image = bmp;
        }

        #region 
        private void PaintVector(Vector2 start, Vector2 end, Color color)
        {
            // Малюємо основну лінію вектора
            Pen pen = new Pen(color, 2); // Ширина лінії вектора - 2 пікселі
            g.DrawLine(pen, start.X, start.Y, end.X, end.Y);

            // Додаємо стрілку на кінці вектора
            DrawArrow(pen, start, end);
        }


        private void DrawArrow(Pen pen, Vector2 start, Vector2 end)
        {
            // Обчислюємо напрямок вектора (кут)
            float arrowSize = 10; // Розмір стрілки
            double angle = Math.Atan2(end.Y - start.Y, end.X - start.X); // Кут нахилу

            // Малюємо стрілки, які виходять із кінцевої точки вектора
            PointF arrowPoint1 = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));

            PointF arrowPoint2 = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

            g.DrawLine(pen, end.X, end.Y, arrowPoint1.X, arrowPoint1.Y);
            g.DrawLine(pen, end.X, end.Y, arrowPoint2.X, arrowPoint2.Y);
        }

        private float CalcLineY(LineDef l, float x)
        {
            float y = -(l.A * x + l.C) / l.B;
            return y;
        }
        /// <summary>Малюємо лінію зі стрілками</summary>
        /// <param name="pen">Олівець для малювання</param>
        /// <param name="p1">Початкова точка</param>
        /// <param name="p2">Кінцева точка</param>
        /// <param name="ars">Розмір стрілки</param>
        /// <param name="arrows">0 - без стрілок, 1 - початкова, 2 - кінцева, 3 - обидві</param>
        private void DrawArrowLine(Pen pen, PointF p1, PointF p2, int ars, int arrows)
        {
            double a1 = Math.PI / 9;
            double a2 = -1 * a1;
            Vector2 v = new Vector2(p2.X - p1.X, p2.Y - p1.Y);
            double d = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            int ar2 = ars / 2;
            if (ar2 == 0) ar2 = 1;
            g.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);

            if (arrows == 1 || arrows == 3)
            {
                Vector2 v12 = geometry.GetNormOrt(new Vector2(p2.X - p1.X, p2.Y - p1.Y), ars);
                Vector2 v12_1 = geometry.RotateVector(v12, a1);
                Vector2 v12_2 = geometry.RotateVector(v12, a2);
                if (d > 2 * ars)
                {
                    g.DrawLine(pen, p1.X, p1.Y, p1.X + v12_1.X, p1.Y + v12_1.Y);
                    g.DrawLine(pen, p1.X, p1.Y, p1.X + v12_2.X, p1.Y + v12_2.Y);
                }
                else g.DrawLine(pen, p1.X - ar2, p1.Y - ar2, p1.X + ar2, p1.Y + ar2);
            }

            if (arrows == 2 || arrows == 3)
            {
                Vector2 v21 = geometry.GetNormOrt(new Vector2(p1.X - p2.X, p1.Y - p2.Y), ars);
                Vector2 v21_1 = geometry.RotateVector(v21, a1);
                Vector2 v21_2 = geometry.RotateVector(v21, a2);
                if (d > 2 * ars)
                {
                    g.DrawLine(pen, p2.X, p2.Y, p2.X + v21_1.X, p2.Y + v21_1.Y);
                    g.DrawLine(pen, p2.X, p2.Y, p2.X + v21_2.X, p2.Y + v21_2.Y);
                }
                else g.DrawLine(pen, p2.X - ar2, p2.Y - ar2, p2.X + ar2, p2.Y + ar2);
            }
        }
        #endregion

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Створюємо малюнок та об'єкт графіки для малювання
            Bitmap bmp = new Bitmap(pbPaint.Width, pbPaint.Height);
            g = Graphics.FromImage(bmp);
            try
            {
                // Заповнюємо фоном
                g.Clear(Color.LightYellow);
                // Змінюємо напрямок по Y
                g.ScaleTransform(1, -1);
                // Зміщуємо систему координат на вектор(100;100)
                g.TranslateTransform(100, 100 - 1 * pbPaint.Height);

                // Блок налаштувань
                PointSize = Convert.ToInt32(txtPointSize.Text);

                // Отримуємо дані
                PointF p0 = PointFromString(textBox6.Text);
                PointF p1 = PointFromString(textBox7.Text);
                float R0 = Convert.ToSingle(textBox8.Text);
                float R1 = Convert.ToSingle(textBox9.Text);
               

            
                // Малюємо координатні осі
                g.DrawLine(new Pen(Color.Green), new PointF(-100, 0), new PointF(pbPaint.Width, 0));
                g.DrawLine(new Pen(Color.Blue), new PointF(0, -100), new PointF(0, pbPaint.Height));
                DrawText("X", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Green, new PointF(pbPaint.Width - 100 - 3f * PointSize, 0.5f * PointSize));
                DrawText("Y", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Blue, new PointF(PointSize / 2, pbPaint.Height - 100 - PointSize));

                // Перша пряма
                // PaintLine(p1, p2, Color.Fuchsia);
                // PaintPoint(p1, PointSize);
                // PaintPoint(p2, PointSize);
                // DrawText("P1(x1;y1)", new Font("Tahoma", 1.5f * PointSize), p1);
                //  DrawText("P2(x2;y2)", new Font("Tahoma", 1.5f * PointSize), p2);


                // Малюємо перше коло
                g.DrawEllipse(new Pen(Color.Red, 2), p0.X - R0, p0.Y - R0, 2 * R0, 2 * R0);

                // Малюємо друге коло
                g.DrawEllipse(new Pen(Color.Blue, 2), p1.X - R1, p1.Y - R1, 2 * R1, 2 * R1);

                // Якщо потрібно, можна додати обчислення точок перетину
                PointF[] pts;
                int pres = bePaint.geometry.CrossPoint(p0, p1, R0, R1, out pts);

                // Малюємо точки перетину (якщо вони існують)
                if (pts != null)
                {
                    foreach (var pt in pts)
                    {
                        PaintPoint(pt, PointSize, Brushes.Black);
                    }
                }

                //PaintLine(pres, Color.Red);

                // Вивод результату
                // PaintLine( pres, DashStyle.DashDotDot, Color.Tomato);
                // PaintPoint(p3, PointSize, Brushes.Orange);
                // PaintPoint(pres, PointSize, Brushes.Red);

                //  DrawText("P_inst(xi;yi)", new Font("Tahoma", 1.5f * PointSize), p3);
                //   DrawText("Pres(x;y)", new Font("Tahoma", 1.5f * PointSize), pres);
            }
            finally
            {
                // Знищуємо об'єкт графіки та передаємо малюнок Пайнт боксу
                g.Dispose();
            }
            pbPaint.Image = bmp;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Створюємо малюнок та об'єкт графіки для малювання
            Bitmap bmp = new Bitmap(pbPaint.Width, pbPaint.Height);
            g = Graphics.FromImage(bmp);
            try
            {
                // Заповнюємо фоном
                g.Clear(Color.LightYellow);
                // Змінюємо напрямок по Y
                g.ScaleTransform(1, -1);
                // Зміщуємо систему координат на вектор(100;100)
                g.TranslateTransform(100, 100 - 1 * pbPaint.Height);

                // Блок налаштувань
                PointSize = Convert.ToInt32(txtPointSize.Text);

                // Отримуємо дані
                string[] SplitLineFromString = textBox12.Text.Split(';');
                string[] SplitLineFromString2 = textBox13.Text.Split(';');
                float A = Convert.ToSingle(SplitLineFromString[0]);
                float B = Convert.ToSingle(SplitLineFromString[1]);
                float C = Convert.ToSingle(SplitLineFromString[2]);
                float A1 = Convert.ToSingle(SplitLineFromString2[0]);
                float B1 = Convert.ToSingle(SplitLineFromString2[1]);
                float C1 = Convert.ToSingle(SplitLineFromString2[2]);
              
                LineDef line = new LineDef(Convert.ToSingle(SplitLineFromString[0]), Convert.ToSingle(SplitLineFromString[1]), Convert.ToSingle(SplitLineFromString[2]));
                LineDef line2 = new LineDef(Convert.ToSingle(SplitLineFromString2[0]), Convert.ToSingle(SplitLineFromString2[1]), Convert.ToSingle(SplitLineFromString2[2]));



                // Малюємо координатні осі
                g.DrawLine(new Pen(Color.Green), new PointF(-100, 0), new PointF(pbPaint.Width, 0));
                g.DrawLine(new Pen(Color.Blue), new PointF(0, -100), new PointF(0, pbPaint.Height));
                DrawText("X", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Green, new PointF(pbPaint.Width - 100 - 3f * PointSize, 0.5f * PointSize));
                DrawText("Y", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Blue, new PointF(PointSize / 2, pbPaint.Height - 100 - PointSize));

                // Перша пряма
                LineDef l1 = new LineDef(A, B, C);
                LineDef l2 = new LineDef(A1, B1, C1);
                PaintLine(l1, Color.Fuchsia);
                PaintLine(l2, Color.Blue);
                //DrawText("P1(x1;y1)", new Font("Tahoma", 1.5f * PointSize), p1);

                //Малюємо вектор

                // Обраховуємо досліджуєму функцію
                 double s = bePaint.geometry.AngleBetweenLine(l1, l2);

                // Вивод результату
             
                // Обраховуємо досліджуєму функцію

                //  LineDef pres = bePaint.geometry.ShiftLine(line, vector);

                // PaintLine(pres, Color.Red);
                // Вивод результату
                // PaintLine( pres, DashStyle.DashDotDot, Color.Tomato);
                // PaintPoint(p3, PointSize, Brushes.Orange);
                // PaintPoint(pres, PointSize, Brushes.Red);

                //  DrawText("P_inst(xi;yi)", new Font("Tahoma", 1.5f * PointSize), p3);
                //   DrawText("Pres(x;y)", new Font("Tahoma", 1.5f * PointSize), pres);
            }
            finally
            {
                // Знищуємо об'єкт графіки та передаємо малюнок Пайнт боксу
                g.Dispose();
            }
            pbPaint.Image = bmp;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Створюємо малюнок та об'єкт графіки для малювання
            Bitmap bmp = new Bitmap(pbPaint.Width, pbPaint.Height);
            g = Graphics.FromImage(bmp);
            try
            {
                // Заповнюємо фоном
                g.Clear(Color.LightYellow);
                // Змінюємо напрямок по Y
                g.ScaleTransform(1, -1);
                // Зміщуємо систему координат на вектор(100;100)
                g.TranslateTransform(100, 100 - 1 * pbPaint.Height);

                // Блок налаштувань
                PointSize = Convert.ToInt32(txtPointSize.Text);

                // Отримуємо дані

                string[]arrayvector1 = textBox10.Text.Split(';');
                string[] arrayvector2 = textBox11.Text.Split(';');
                Vector2 vector= new Vector2(Convert.ToSingle(arrayvector1[0]), Convert.ToSingle(arrayvector1[1]));
                Vector2 vector1 = new Vector2(Convert.ToSingle(arrayvector2[0]), Convert.ToSingle(arrayvector2[1]));




                // Малюємо координатні осі
                g.DrawLine(new Pen(Color.Green), new PointF(-100, 0), new PointF(pbPaint.Width, 0));
                g.DrawLine(new Pen(Color.Blue), new PointF(0, -100), new PointF(0, pbPaint.Height));
                DrawText("X", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Green, new PointF(pbPaint.Width - 100 - 3f * PointSize, 0.5f * PointSize));
                DrawText("Y", new Font("Tahoma", 2f * PointSize, FontStyle.Bold), Brushes.Blue, new PointF(PointSize / 2, pbPaint.Height - 100 - PointSize));


                // Перша пряма
                Vector2 start = new Vector2(0, 0); // Початок вектора в центрі координат
                Vector2 end = new Vector2(Convert.ToSingle(arrayvector1[0]), Convert.ToSingle(arrayvector1[1]));
                // Малюємо перший вектор
                PaintVector(start, end, Color.Red);
                // Тепер малюємо другий вектор

                Vector2 secondEnd = new Vector2(Convert.ToSingle(arrayvector2[0]), Convert.ToSingle(arrayvector2[1]));
                PaintVector(start, secondEnd, Color.Blue);

             
                //DrawText("P1(x1;y1)", new Font("Tahoma", 1.5f * PointSize), p1);

                //Малюємо вектор

                // Обраховуємо досліджуєму функцію
                double s = bePaint.geometry.AngleBetweenVector(start, end, true);

                // Вивод результату

                // Обраховуємо досліджуєму функцію

                //  LineDef pres = bePaint.geometry.ShiftLine(line, vector);

                // PaintLine(pres, Color.Red);
                // Вивод результату
                // PaintLine( pres, DashStyle.DashDotDot, Color.Tomato);
                // PaintPoint(p3, PointSize, Brushes.Orange);
                // PaintPoint(pres, PointSize, Brushes.Red);

                //  DrawText("P_inst(xi;yi)", new Font("Tahoma", 1.5f * PointSize), p3);
                //   DrawText("Pres(x;y)", new Font("Tahoma", 1.5f * PointSize), pres);
            }
            finally
            {
                // Знищуємо об'єкт графіки та передаємо малюнок Пайнт боксу
                g.Dispose();
            }
            pbPaint.Image = bmp;
        }
    }
}
