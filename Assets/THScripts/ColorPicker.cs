using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class ColorPicker : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.InputField colorText;
    [SerializeField]
    private UnityEngine.UI.InputField text;
    [SerializeField]
    private UnityEngine.UI.Image colorPickerImage;
    [SerializeField]
    private UnityEngine.UI.Slider slider;

    public Texture2D colorPicker;
    [SerializeField]
    private int imageWidth = 100;
    [SerializeField]
    private int imageHeight = 100;
    [SerializeField]
    private float s = 1.0f;
    [SerializeField]
    private float l = 0.5f;

    [SerializeField]
    private int colorStep;
    [SerializeField]
    private UnityEngine.Color selectedColor;
    public UnityEngine.Color SelectedColor
    {
        get { return selectedColor; }
    }

    [SerializeField]
    private UnityEngine.UI.Image targetImage;

    private Point lastPoint;
    private string lastHex;
    private HslColor lastHSL;

    private void Awake()
    {
        lastPoint = new Point(imageWidth / 2, imageHeight / 2);
        SetL(0.5f);
    }

    public void SetL(float value)
    {
        l = value;
        text.text = l.ToString();
        SetColor();
        slider.value = l;
    }

    public void SetL(string value)
    {
        double newL;

        if (System.Double.TryParse(value, out newL))
        {
            if(newL < 0 || newL > 1)
            {
                return;
            }

            l = (float)newL;
            slider.value = l;
            text.text = l.ToString();
            SetColor();
        }
        
    }

    public void SetHexToColor(string hex)
    {
        UnityEngine.Color color;
        if(UnityEngine.ColorUtility.TryParseHtmlString(hex, out color))
        {
            targetImage.color = color;
        }
        else
        {
            colorText.text = lastHex;
        }
    }

    private void CalculateWheel()
    {
        List<PointF> points;
        List<System.Drawing.Color> colors;

        points = new List<PointF>();
        colors = new List<System.Drawing.Color>();

        PointF centerPoint;
        float radius;
        if (imageWidth > 16 && imageHeight > 16)
        {
            int w = imageWidth;
            int h = imageHeight;

            centerPoint = new PointF(w / 2.0f, h / 2.0f);
            radius = GetRadius(centerPoint);

            for (double angle = 0; angle < 360; angle += colorStep)
            {
                double angleR;
                PointF location;
                angleR = angle * (System.Math.PI / 180f);
                location = GetColorLocation(centerPoint, angleR, radius);

                points.Add(location);

                colors.Add(new HslColor(angle + 240, s, l).ToRgbColor());
                //colors.Add(ColorFromHSL(angle, s, l));
            }
            
        }
        else
            return;

        Brush result;

        if (points.Count != 0 && points.Count == colors.Count)
        {
            result = new System.Drawing.Drawing2D.PathGradientBrush(points.ToArray(), System.Drawing.Drawing2D.WrapMode.Clamp)
            {
                CenterPoint = centerPoint,
                CenterColor = System.Drawing.Color.White,
                SurroundColors = colors.ToArray(),
            };
        }
        else
        {
            result = null;
        }

        if (result == null)
            return;

        Image image;
        int halfSize;

        halfSize = imageWidth / 2;
        image = new Bitmap(imageWidth + 2, imageWidth + 2);

        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image);
        graphics.FillPie(result, new Rectangle(0, 0, imageWidth, imageHeight), 0, 360);

        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //using (Pen pen = new Pen(System.Drawing.Color.Black, 2))
        //{
        //    graphics.DrawEllipse(pen, new RectangleF(centerPoint.X - radius, centerPoint.Y - radius, radius * 2, radius * 2));
        //}

        Texture2D texture = new Texture2D(imageWidth + 2, imageHeight + 2, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(imageToByteArray(image));
        texture.Apply();
        colorPickerImage.sprite = Sprite.Create(texture, new Rect(0, 0, imageWidth + 2, imageHeight + 2), Vector2.one);
    }

    public void SetColor()
    {
        HslColor hslColor = new HslColor(lastHSL.H, lastHSL.S, l);
        System.Drawing.Color color = hslColor.ToRgbColor();
        selectedColor = new UnityEngine.Color(color.R / 256f, color.G / 256f, color.B / 256f, 1f);
        targetImage.color = selectedColor;
        colorText.text = "#" + UnityEngine.ColorUtility.ToHtmlStringRGB(selectedColor);
        lastHex = colorText.text;
        lastHSL = hslColor;
    }

    public void SetColor(Point point)
    {
        double dx;
        double dy;
        double angle;
        double distance;
        double saturation;

        System.Drawing.PointF centerPoint = new PointF(imageWidth / 2.0f, imageHeight / 2.0f);

        if(point == null)
        {
            point = lastPoint;
        }
        else
        {
            lastPoint = point;
        }
        
        dx = Mathf.Abs(point.X - centerPoint.X);
        dy = Mathf.Abs(point.Y - centerPoint.Y);
        angle = System.Math.Atan(dy / dx) / Mathf.PI * 180;
        distance = System.Math.Pow((System.Math.Pow(dx, 2) + (System.Math.Pow(dy, 2))), 0.5);

        double radius = GetRadius(centerPoint);

        if (distance >= radius)
            return;

        saturation = distance / radius;

        if (distance < 10)
        {
            saturation = 0; // snap to center
        }
        
        if (point.X < centerPoint.X)
        {
            angle = 180 - angle;
        }
        if (point.Y < centerPoint.Y)
        {
            angle = 360 - angle;
        }
        
        HslColor hslColor = new HslColor(angle, saturation, l);
        System.Drawing.Color color = hslColor.ToRgbColor();
        selectedColor = new UnityEngine.Color(color.R / 256f, color.G / 256f,  color.B / 256f, 1f);
        targetImage.color = selectedColor;
        colorText.text = "#" + UnityEngine.ColorUtility.ToHtmlStringRGB(selectedColor);
        lastHex = colorText.text;
        lastHSL = hslColor;
    }

    protected PointF GetColorLocation(PointF centerPoint, double angleR, double radius)
    {
        double x;
        double y;

        x = centerPoint.X + System.Math.Cos(angleR) * radius;
        y = centerPoint.Y - System.Math.Sin(angleR) * radius;

        return new PointF((float)x, (float)y);
    }

    public Point GetPointFromColor(HslColor color)
    {

        return new Point();
    }

    private void Update()
    {
        CalculateWheel();
    }

    protected float GetRadius(PointF centerPoint)
    {
        return System.Math.Min(centerPoint.X, centerPoint.Y);
    }

    public byte[] imageToByteArray(System.Drawing.Image imageIn)
    {
        int t1 = System.Environment.TickCount;
        var o = System.Drawing.GraphicsUnit.Pixel;
        RectangleF r1 = imageIn.GetBounds(ref o);
        Rectangle r2 = new Rectangle((int)r1.X, (int)r1.Y, (int)r1.Width, (int)r1.Height);
        System.Drawing.Imaging.BitmapData omg = ((Bitmap)imageIn).LockBits(r2, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        byte[] rgbValues = new byte[r2.Width * r2.Height * 4];
        System.Runtime.InteropServices.Marshal.Copy((System.IntPtr)omg.Scan0, rgbValues, 0, rgbValues.Length);
        ((Bitmap)imageIn).UnlockBits(omg);
        return rgbValues;
    }
}

public struct HslColor
{
    public static readonly HslColor Empty;
    private double hue;
    private double saturation;
    private double luminance;
    private int alpha;

    public HslColor(int a, double h, double s, double l)
    {
        this.alpha = a;
        this.hue = h;
        this.saturation = s;
        this.luminance = l;
        this.A = a;
        this.H = this.hue;
        this.S = this.saturation;
        this.L = this.luminance;
    }

    public HslColor(double h, double s, double l)
    {
        this.alpha = 0xff;
        this.hue = h;
        this.saturation = s;
        this.luminance = l;
        //this.A = this.alpha;
        //this.H = this.hue;
        //this.S = this.saturation;
        //this.L = this.luminance;
    }

    public HslColor(System.Drawing.Color color)
    {
        this.alpha = color.A;
        this.hue = 0.0;
        this.saturation = 0.0;
        this.luminance = 0.0;
        this.RGBtoHSL(color);
    }

    public static HslColor FromArgb(int a, int r, int g, int b)
    {
        return new HslColor(System.Drawing.Color.FromArgb(a, r, g, b));
    }

    public static HslColor FromColor(System.Drawing.Color color)
    {
        return new HslColor(color);
    }

    public static HslColor FromAhsl(int a)
    {
        return new HslColor(a, 0.0, 0.0, 0.0);
    }

    public static HslColor FromAhsl(int a, HslColor hsl)
    {
        return new HslColor(a, hsl.hue, hsl.saturation, hsl.luminance);
    }

    public static HslColor FromAhsl(double h, double s, double l)
    {
        return new HslColor(0xff, h, s, l);
    }

    public static HslColor FromAhsl(int a, double h, double s, double l)
    {
        return new HslColor(a, h, s, l);
    }

    public static bool operator ==(HslColor left, HslColor right)
    {
        return (((left.A == right.A) && (left.H == right.H)) && ((left.S == right.S) && (left.L == right.L)));
    }

    public static bool operator !=(HslColor left, HslColor right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is HslColor)
        {
            HslColor color = (HslColor)obj;
            if (((this.A == color.A) && (this.H == color.H)) && ((this.S == color.S) && (this.L == color.L)))
            {
                return true;
            }
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (((this.alpha.GetHashCode() ^ this.hue.GetHashCode()) ^ this.saturation.GetHashCode()) ^ this.luminance.GetHashCode());
    }

    public double H
    {
        get
        {
            return this.hue;
        }
        set
        {
            this.hue = value;
            this.hue = (this.hue > 1.0) ? 1.0 : ((this.hue < 0.0) ? 0.0 : this.hue);
        }
    }

    public double S
    {
        get
        {
            return this.saturation;
        }
        set
        {
            this.saturation = value;
            this.saturation = (this.saturation > 1.0) ? 1.0 : ((this.saturation < 0.0) ? 0.0 : this.saturation);
        }
    }
    
    public double L
    {
        get
        {
            return this.luminance;
        }
        set
        {
            this.luminance = value;
            this.luminance = (this.luminance > 1.0) ? 1.0 : ((this.luminance < 0.0) ? 0.0 : this.luminance);
        }
    }
    
    public System.Drawing.Color RgbValue
    {
        get
        {
            return this.HSLtoRGB();
        }
        set
        {
            this.RGBtoHSL(value);
        }
    }
    public int A
    {
        get
        {
            return this.alpha;
        }
        set
        {
            this.alpha = (value > 0xff) ? 0xff : ((value < 0) ? 0 : value);
        }
    }
    public bool IsEmpty
    {
        get
        {
            return ((((this.alpha == 0) && (this.H == 0.0)) && (this.S == 0.0)) && (this.L == 0.0));
        }
    }

    public System.Drawing.Color ToRgbColor()
    {
        return this.ToRgbColor(this.A);
    }

    public System.Drawing.Color ToRgbColor(int alpha)
    {
        double q;
        if (this.L < 0.5)
        {
            q = this.L * (1 + this.S);
        }
        else
        {
            q = this.L + this.S - (this.L * this.S);
        }
        double p = 2 * this.L - q;
        double hk = this.H / 360;

        // r,g,b colors
        double[] tc = new[]
                {
                      hk + (1d / 3d), hk, hk - (1d / 3d)
                    };
        double[] colors = new[]
                    {
                          0.0, 0.0, 0.0
                        };

        for (int color = 0; color < colors.Length; color++)
        {
            if (tc[color] < 0)
            {
                tc[color] += 1;
            }
            if (tc[color] > 1)
            {
                tc[color] -= 1;
            }

            if (tc[color] < (1d / 6d))
            {
                colors[color] = p + ((q - p) * 6 * tc[color]);
            }
            else if (tc[color] >= (1d / 6d) && tc[color] < (1d / 2d))
            {
                colors[color] = q;
            }
            else if (tc[color] >= (1d / 2d) && tc[color] < (2d / 3d))
            {
                colors[color] = p + ((q - p) * 6 * (2d / 3d - tc[color]));
            }
            else
            {
                colors[color] = p;
            }

            colors[color] *= 255;
        }

        return System.Drawing.Color.FromArgb(alpha, (int)colors[0], (int)colors[1], (int)colors[2]);
    }

    private System.Drawing.Color HSLtoRGB()
    {
        int num2;
        int red = this.Round(this.luminance * 255.0);
        int blue = this.Round(((1.0 - this.saturation) * (this.luminance / 1.0)) * 255.0);
        double num4 = ((double)(red - blue)) / 255.0;
        if ((this.hue >= 0.0) && (this.hue <= 0.16666666666666666))
        {
            num2 = this.Round((((this.hue - 0.0) * num4) * 1530.0) + blue);
            return System.Drawing.Color.FromArgb(this.alpha, red, num2, blue);
        }
        if (this.hue <= 0.33333333333333331)
        {
            num2 = this.Round((-((this.hue - 0.16666666666666666) * num4) * 1530.0) + red);
            return System.Drawing.Color.FromArgb(this.alpha, num2, red, blue);
        }
        if (this.hue <= 0.5)
        {
            num2 = this.Round((((this.hue - 0.33333333333333331) * num4) * 1530.0) + blue);
            return System.Drawing.Color.FromArgb(this.alpha, blue, red, num2);
        }
        if (this.hue <= 0.66666666666666663)
        {
            num2 = this.Round((-((this.hue - 0.5) * num4) * 1530.0) + red);
            return System.Drawing.Color.FromArgb(this.alpha, blue, num2, red);
        }
        if (this.hue <= 0.83333333333333337)
        {
            num2 = this.Round((((this.hue - 0.66666666666666663) * num4) * 1530.0) + blue);
            return System.Drawing.Color.FromArgb(this.alpha, num2, blue, red);
        }
        if (this.hue <= 1.0)
        {
            num2 = this.Round((-((this.hue - 0.83333333333333337) * num4) * 1530.0) + red);
            return System.Drawing.Color.FromArgb(this.alpha, red, blue, num2);
        }
        return System.Drawing.Color.FromArgb(this.alpha, 0, 0, 0);
    }

    private void RGBtoHSL(System.Drawing.Color color)
    {
        int r;
        int g;
        double num4;
        this.alpha = color.A;
        if (color.R > color.G)
        {
            r = color.R;
            g = color.G;
        }
        else
        {
            r = color.G;
            g = color.R;
        }
        if (color.B > r)
        {
            r = color.B;
        }
        else if (color.B < g)
        {
            g = color.B;
        }
        int num3 = r - g;
        this.luminance = ((double)r) / 255.0;
        if (r == 0)
        {
            this.saturation = 0.0;
        }
        else
        {
            this.saturation = ((double)num3) / ((double)r);
        }
        if (num3 == 0)
        {
            num4 = 0.0;
        }
        else
        {
            num4 = 60.0 / ((double)num3);
        }
        if (r == color.R)
        {
            if (color.G < color.B)
            {
                this.hue = (360.0 + (num4 * (color.G - color.B))) / 360.0;
            }
            else
            {
                this.hue = (num4 * (color.G - color.B)) / 360.0;
            }
        }
        else if (r == color.G)
        {
            this.hue = (120.0 + (num4 * (color.B - color.R))) / 360.0;
        }
        else if (r == color.B)
        {
            this.hue = (240.0 + (num4 * (color.R - color.G))) / 360.0;
        }
        else
        {
            this.hue = 0.0;
        }
    }

    private int Round(double val)
    {
        return (int)(val + 0.5);
    }

    static HslColor()
    {
        Empty = new HslColor();
    }
}
