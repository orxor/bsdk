sampler2D implicitInput : register(s0);
float factor : register(c0);

struct hsl
    {
    float h;
    float s;
    float l;
    };

hsl rgb2hsl(float4 value)
    {
    float M = max(value.r,max(value.g,value.b));
    float m = min(value.r,min(value.g,value.b));
    float D = M - m;
    hsl r;
    if (M != m) {
        r.h = ((M == value.r)
            ? (value.g >= value.b)
                ? (((value.g - value.b)/D) + 0)
                : (((value.g - value.b)/D) + 6)
            : (M == value.g)
                ? (((value.b - value.r)/D) + 2)
                : (((value.r - value.g)/D) + 4))/6;
        r.s = D/(1 - abs(1 - (M + m)));
        r.l = (M + m)*0.5;
        }
    return r;
    }

float hue2rgb(float p, float q, float t) {
    if (t < 0) t += 1;
    if (t > 1) t -= 1;
    if (t < 1./6) return p + (q - p) * 6 * t;
    if (t < 1./2) return q;
    if (t < 2./3) return p + (q - p) * (2./3 - t) * 6;
    return p;
    }

float4 hsl2rgb(hsl value) {
    float4 r = {0.0,0.0,0.0,0.0};
    if (value.s > 0) {
        float q = value.l < 0.5 ? value.l * (1 + value.s) : value.l + value.s - value.l * value.s;
        float p = 2 * value.l - q;
        r.r = hue2rgb(p, q, value.h + 1./3);
        r.g = hue2rgb(p, q, value.h);
        r.b = hue2rgb(p, q, value.h - 1./3);
        }
    return r;
    }

float4 main(float2 uv : TEXCOORD) : COLOR
    {
    float4 color = tex2D(implicitInput, uv);
    float4 r;
    r.r = min(max(color.r + factor,0.0),1.0);
    r.g = min(max(color.g + factor,0.0),1.0);
    r.b = min(max(color.b + factor,0.0),1.0);
    r.a = color.a;
    return r;
    }
