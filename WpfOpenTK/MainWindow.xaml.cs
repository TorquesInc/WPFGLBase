using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace WpfOpenTK
{
    /// <summary>
    /// Web上のいくつかのサンプルを参考に作成.
    /// </summary>
    public partial class MainWindow : Window
    {
        static GraphicsMode mode = new GraphicsMode(
            GraphicsMode.Default.ColorFormat,
            GraphicsMode.Default.Depth,
            8,//GraphicsMode.Default.Stencil,
            8,//GraphicsMode.Default.Samples,
            GraphicsMode.Default.AccumulatorFormat,
            GraphicsMode.Default.Buffers,
            GraphicsMode.Default.Stereo
            );
        GLControl glControl = new GLControl(mode);

        public MainWindow()
        {
            InitializeComponent();

            glControl.Load += glControl_Load;
            glControl.Paint += glControl_Paint;
            glControl.Resize += glControl_Resize;


            glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseUp);

            glHost.Child = glControl;

        }

        private void PrintInfo()
        {
            // OpenTKのバージョンを取得.
            string infoString = "OpenTK "+ typeof(Toolkit).Assembly.GetName().Version;

            // OpenGLのバージョンを取得.
            var  version = GL.GetString(StringName.Version);
            infoString += ", OpenGL " + version;

            //var major = GL.GetInteger(GetPName.MajorVersion);
            //var minor = GL.GetInteger(GetPName.MinorVersion);
            //infoString += "(" + major+")("+minor+")";

            appLabel.Content = infoString;
            Console.WriteLine($"{infoString}");
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            PrintInfo();
 
            // 背景色.
            GL.ClearColor(Color4.White);

            // ビューポートの設定.
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            // 
            GL.MatrixMode(MatrixMode.Projection);
            // fovy, aspect, near, far
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, glControl.AspectRatio, 0.2f, 5);
            GL.LoadMatrix(ref proj);

            // 視界の設定
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 look = Matrix4.LookAt(Vector3.One, new Vector3(0, 0, 0.75f), Vector3.UnitZ);
            GL.LoadMatrix(ref look);

            // デプスバッファの使用
            GL.Enable(EnableCap.DepthTest);

            // 光源の使用
            GL.Enable(EnableCap.Lighting);
        }

        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Material(MaterialFace.Front, MaterialParameter.Emission, Color4.Blue);
            tube(2, 0.1f, 0.1f);

            glControl.SwapBuffers();
        }

        float rx, ry;
        void tube(float length, float radius1, float radius2)
        {

            GL.PushMatrix();
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Normal3(Vector3.One);
            for (int deg = 0; deg <= 360; deg = deg + 3)
            {
                rx = (float)Math.Cos((float)Math.PI * deg / 180);
                ry = (float)Math.Sin((float)Math.PI * deg / 180);
                GL.Vertex3(rx * radius2, ry * radius2, length / 2);
                GL.Vertex3(rx * radius1, ry * radius1, -length / 2);

            }
            GL.End();
            GL.PopMatrix();

        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

        }

        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point mpos = new Point(e.X, e.Y);
            Console.WriteLine($"glControl_MouseDown ({mpos.X},{mpos.Y})");
        }

        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point mpos = new Point(e.X, e.Y);
            //Console.WriteLine($"glControl_MouseMove ({mpos.X},{mpos.Y})");
        }
        private void glControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point mpos = new Point(e.X, e.Y);
            Console.WriteLine($"glControl_MouseUp ({mpos.X},{mpos.Y})");
        }
    }
}
