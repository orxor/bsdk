using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BinaryStudio.PlatformUI.Media.Effects
    {
    /// <summary>
    /// $(UCRTContentRoot)\fxc /O0 /Fc /Zi /T ps_2_0 /Fo BrightnessEffect.ps BrightnessEffect.fx
    /// </summary>
    public sealed class BrightnessEffect : ShaderEffect
        {
        private static PixelShader shader;

        public BrightnessEffect() {
            if (shader == null) {
                shader = new PixelShader { UriSource = new Uri(@"pack://application:,,,/BinaryStudio.PlatformUI;component/BrightnessEffect.ps") };
                }
            PixelShader = shader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BrightnessFactorProperty);
            }

        #region P:Input:Brush
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(BrightnessEffect), 0);
        public Brush Input {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
            }
        #endregion
        #region P:DesaturationFactor:Double
        public static readonly DependencyProperty BrightnessFactorProperty = DependencyProperty.Register(nameof(BrightnessFactor), typeof(Double), typeof(BrightnessEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0), CoerceBrightnessFactor));
        private static Object CoerceBrightnessFactor(DependencyObject d, Object value) {
            var effect = (BrightnessEffect)d;
            var factor = (Double)value;
            if (factor < -1.0 || factor > 1.0) {
                return effect.BrightnessFactor;
                }
            return factor;
            }
        public Double BrightnessFactor {
            get { return (Double)GetValue(BrightnessFactorProperty); }
            set { SetValue(BrightnessFactorProperty, value); }
            }
        #endregion
        }
    }