using BetaGUI;
using Mogre;
using Wof.Model.Level.Effects;
using System.Drawing;
using System;

namespace Wof.Controller.EffectBars
{
    internal class BulletTimeBar
    {
        private const string ImageBar = @"bulletTimeBar.PNG";
        private float _heigth = 100.0f;
        private float _width = 40f;
        private Window _bar;
        private OverlayContainer _barOverConta;
        private PointF _startPoint = Point.Empty;

        public BulletTimeBar(GUI gui, Viewport viewport)
        {
            _startPoint = new PointF(viewport.ActualWidth - _width, viewport.ActualHeight - 140);
            _bar = gui.createWindow(new Vector4(_startPoint.X, _startPoint.Y, 40, _heigth), String.Empty, (int)wt.NONE, String.Empty);
            _barOverConta = _bar.createStaticImage(new Vector4(0, 0, _width, _heigth), ImageBar);
        }

        public void Update(int time)
        {
            EffectsManager.Instance.UpdateEffect(time, EffectType.BulletTimeEffect);
            float h = EffectsManager.Instance.GetEffectLevel(EffectType.BulletTimeEffect) * _heigth;
            //_barOverConta.SetPosition(_startPoint.X, _startPoint.Y - (_heigth - h));
            _barOverConta.SetDimensions(_width, h);
            _barOverConta.Show();
        }
    }
}
