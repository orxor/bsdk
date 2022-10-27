﻿using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PortableExecutable.Win32
    {
    public enum CV_AMD64
        {
        [Display(Name="al")] CV_AMD64_AL       =   1,
        CV_AMD64_CL       =   2,
        CV_AMD64_DL       =   3,
        CV_AMD64_BL       =   4,
        CV_AMD64_AH       =   5,
        CV_AMD64_CH       =   6,
        CV_AMD64_DH       =   7,
        CV_AMD64_BH       =   8,
        CV_AMD64_AX       =   9,
        CV_AMD64_CX       =  10,
        CV_AMD64_DX       =  11,
        CV_AMD64_BX       =  12,
        CV_AMD64_SP       =  13,
        CV_AMD64_BP       =  14,
        CV_AMD64_SI       =  15,
        CV_AMD64_DI       =  16,
        CV_AMD64_EAX      =  17,
        CV_AMD64_ECX      =  18,
        CV_AMD64_EDX      =  19,
        CV_AMD64_EBX      =  20,
        CV_AMD64_ESP      =  21,
        CV_AMD64_EBP      =  22,
        CV_AMD64_ESI      =  23,
        CV_AMD64_EDI      =  24,
        CV_AMD64_ES       =  25,
        CV_AMD64_CS       =  26,
        CV_AMD64_SS       =  27,
        CV_AMD64_DS       =  28,
        CV_AMD64_FS       =  29,
        CV_AMD64_GS       =  30,
        CV_AMD64_FLAGS    =  32,
        CV_AMD64_RIP      =  33,
        CV_AMD64_EFLAGS   =  34,

        // Control registers
        CV_AMD64_CR0      =  80,
        CV_AMD64_CR1      =  81,
        CV_AMD64_CR2      =  82,
        CV_AMD64_CR3      =  83,
        CV_AMD64_CR4      =  84,
        CV_AMD64_CR8      =  88,

        // Debug registers
        CV_AMD64_DR0      =  90,
        CV_AMD64_DR1      =  91,
        CV_AMD64_DR2      =  92,
        CV_AMD64_DR3      =  93,
        CV_AMD64_DR4      =  94,
        CV_AMD64_DR5      =  95,
        CV_AMD64_DR6      =  96,
        CV_AMD64_DR7      =  97,
        CV_AMD64_DR8      =  98,
        CV_AMD64_DR9      =  99,
        CV_AMD64_DR10     =  100,
        CV_AMD64_DR11     =  101,
        CV_AMD64_DR12     =  102,
        CV_AMD64_DR13     =  103,
        CV_AMD64_DR14     =  104,
        CV_AMD64_DR15     =  105,

        CV_AMD64_GDTR     =  110,
        CV_AMD64_GDTL     =  111,
        CV_AMD64_IDTR     =  112,
        CV_AMD64_IDTL     =  113,
        CV_AMD64_LDTR     =  114,
        CV_AMD64_TR       =  115,

        CV_AMD64_ST0      =  128,
        CV_AMD64_ST1      =  129,
        CV_AMD64_ST2      =  130,
        CV_AMD64_ST3      =  131,
        CV_AMD64_ST4      =  132,
        CV_AMD64_ST5      =  133,
        CV_AMD64_ST6      =  134,
        CV_AMD64_ST7      =  135,
        CV_AMD64_CTRL     =  136,
        CV_AMD64_STAT     =  137,
        CV_AMD64_TAG      =  138,
        CV_AMD64_FPIP     =  139,
        CV_AMD64_FPCS     =  140,
        CV_AMD64_FPDO     =  141,
        CV_AMD64_FPDS     =  142,
        CV_AMD64_ISEM     =  143,
        CV_AMD64_FPEIP    =  144,
        CV_AMD64_FPEDO    =  145,

        CV_AMD64_MM0      =  146,
        CV_AMD64_MM1      =  147,
        CV_AMD64_MM2      =  148,
        CV_AMD64_MM3      =  149,
        CV_AMD64_MM4      =  150,
        CV_AMD64_MM5      =  151,
        CV_AMD64_MM6      =  152,
        CV_AMD64_MM7      =  153,

        CV_AMD64_XMM0     =  154,   // KATMAI registers
        CV_AMD64_XMM1     =  155,
        CV_AMD64_XMM2     =  156,
        CV_AMD64_XMM3     =  157,
        CV_AMD64_XMM4     =  158,
        CV_AMD64_XMM5     =  159,
        CV_AMD64_XMM6     =  160,
        CV_AMD64_XMM7     =  161,

        CV_AMD64_XMM0_0   =  162,   // KATMAI sub-registers
        CV_AMD64_XMM0_1   =  163,
        CV_AMD64_XMM0_2   =  164,
        CV_AMD64_XMM0_3   =  165,
        CV_AMD64_XMM1_0   =  166,
        CV_AMD64_XMM1_1   =  167,
        CV_AMD64_XMM1_2   =  168,
        CV_AMD64_XMM1_3   =  169,
        CV_AMD64_XMM2_0   =  170,
        CV_AMD64_XMM2_1   =  171,
        CV_AMD64_XMM2_2   =  172,
        CV_AMD64_XMM2_3   =  173,
        CV_AMD64_XMM3_0   =  174,
        CV_AMD64_XMM3_1   =  175,
        CV_AMD64_XMM3_2   =  176,
        CV_AMD64_XMM3_3   =  177,
        CV_AMD64_XMM4_0   =  178,
        CV_AMD64_XMM4_1   =  179,
        CV_AMD64_XMM4_2   =  180,
        CV_AMD64_XMM4_3   =  181,
        CV_AMD64_XMM5_0   =  182,
        CV_AMD64_XMM5_1   =  183,
        CV_AMD64_XMM5_2   =  184,
        CV_AMD64_XMM5_3   =  185,
        CV_AMD64_XMM6_0   =  186,
        CV_AMD64_XMM6_1   =  187,
        CV_AMD64_XMM6_2   =  188,
        CV_AMD64_XMM6_3   =  189,
        CV_AMD64_XMM7_0   =  190,
        CV_AMD64_XMM7_1   =  191,
        CV_AMD64_XMM7_2   =  192,
        CV_AMD64_XMM7_3   =  193,

        CV_AMD64_XMM0L    =  194,
        CV_AMD64_XMM1L    =  195,
        CV_AMD64_XMM2L    =  196,
        CV_AMD64_XMM3L    =  197,
        CV_AMD64_XMM4L    =  198,
        CV_AMD64_XMM5L    =  199,
        CV_AMD64_XMM6L    =  200,
        CV_AMD64_XMM7L    =  201,

        CV_AMD64_XMM0H    =  202,
        CV_AMD64_XMM1H    =  203,
        CV_AMD64_XMM2H    =  204,
        CV_AMD64_XMM3H    =  205,
        CV_AMD64_XMM4H    =  206,
        CV_AMD64_XMM5H    =  207,
        CV_AMD64_XMM6H    =  208,
        CV_AMD64_XMM7H    =  209,

        CV_AMD64_MXCSR    =  211,   // XMM status register

        CV_AMD64_EMM0L    =  220,   // XMM sub-registers (WNI integer)
        CV_AMD64_EMM1L    =  221,
        CV_AMD64_EMM2L    =  222,
        CV_AMD64_EMM3L    =  223,
        CV_AMD64_EMM4L    =  224,
        CV_AMD64_EMM5L    =  225,
        CV_AMD64_EMM6L    =  226,
        CV_AMD64_EMM7L    =  227,

        CV_AMD64_EMM0H    =  228,
        CV_AMD64_EMM1H    =  229,
        CV_AMD64_EMM2H    =  230,
        CV_AMD64_EMM3H    =  231,
        CV_AMD64_EMM4H    =  232,
        CV_AMD64_EMM5H    =  233,
        CV_AMD64_EMM6H    =  234,
        CV_AMD64_EMM7H    =  235,

        // do not change the order of these regs, first one must be even too
        CV_AMD64_MM00     =  236,
        CV_AMD64_MM01     =  237,
        CV_AMD64_MM10     =  238,
        CV_AMD64_MM11     =  239,
        CV_AMD64_MM20     =  240,
        CV_AMD64_MM21     =  241,
        CV_AMD64_MM30     =  242,
        CV_AMD64_MM31     =  243,
        CV_AMD64_MM40     =  244,
        CV_AMD64_MM41     =  245,
        CV_AMD64_MM50     =  246,
        CV_AMD64_MM51     =  247,
        CV_AMD64_MM60     =  248,
        CV_AMD64_MM61     =  249,
        CV_AMD64_MM70     =  250,
        CV_AMD64_MM71     =  251,

        // Extended KATMAI registers
        CV_AMD64_XMM8     =  252,   // KATMAI registers
        CV_AMD64_XMM9     =  253,
        CV_AMD64_XMM10    =  254,
        CV_AMD64_XMM11    =  255,
        CV_AMD64_XMM12    =  256,
        CV_AMD64_XMM13    =  257,
        CV_AMD64_XMM14    =  258,
        CV_AMD64_XMM15    =  259,

        CV_AMD64_XMM8_0   =  260,   // KATMAI sub-registers
        CV_AMD64_XMM8_1   =  261,
        CV_AMD64_XMM8_2   =  262,
        CV_AMD64_XMM8_3   =  263,
        CV_AMD64_XMM9_0   =  264,
        CV_AMD64_XMM9_1   =  265,
        CV_AMD64_XMM9_2   =  266,
        CV_AMD64_XMM9_3   =  267,
        CV_AMD64_XMM10_0  =  268,
        CV_AMD64_XMM10_1  =  269,
        CV_AMD64_XMM10_2  =  270,
        CV_AMD64_XMM10_3  =  271,
        CV_AMD64_XMM11_0  =  272,
        CV_AMD64_XMM11_1  =  273,
        CV_AMD64_XMM11_2  =  274,
        CV_AMD64_XMM11_3  =  275,
        CV_AMD64_XMM12_0  =  276,
        CV_AMD64_XMM12_1  =  277,
        CV_AMD64_XMM12_2  =  278,
        CV_AMD64_XMM12_3  =  279,
        CV_AMD64_XMM13_0  =  280,
        CV_AMD64_XMM13_1  =  281,
        CV_AMD64_XMM13_2  =  282,
        CV_AMD64_XMM13_3  =  283,
        CV_AMD64_XMM14_0  =  284,
        CV_AMD64_XMM14_1  =  285,
        CV_AMD64_XMM14_2  =  286,
        CV_AMD64_XMM14_3  =  287,
        CV_AMD64_XMM15_0  =  288,
        CV_AMD64_XMM15_1  =  289,
        CV_AMD64_XMM15_2  =  290,
        CV_AMD64_XMM15_3  =  291,

        CV_AMD64_XMM8L    =  292,
        CV_AMD64_XMM9L    =  293,
        CV_AMD64_XMM10L   =  294,
        CV_AMD64_XMM11L   =  295,
        CV_AMD64_XMM12L   =  296,
        CV_AMD64_XMM13L   =  297,
        CV_AMD64_XMM14L   =  298,
        CV_AMD64_XMM15L   =  299,

        CV_AMD64_XMM8H    =  300,
        CV_AMD64_XMM9H    =  301,
        CV_AMD64_XMM10H   =  302,
        CV_AMD64_XMM11H   =  303,
        CV_AMD64_XMM12H   =  304,
        CV_AMD64_XMM13H   =  305,
        CV_AMD64_XMM14H   =  306,
        CV_AMD64_XMM15H   =  307,

        CV_AMD64_EMM8L    =  308,   // XMM sub-registers (WNI integer)
        CV_AMD64_EMM9L    =  309,
        CV_AMD64_EMM10L   =  310,
        CV_AMD64_EMM11L   =  311,
        CV_AMD64_EMM12L   =  312,
        CV_AMD64_EMM13L   =  313,
        CV_AMD64_EMM14L   =  314,
        CV_AMD64_EMM15L   =  315,

        CV_AMD64_EMM8H    =  316,
        CV_AMD64_EMM9H    =  317,
        CV_AMD64_EMM10H   =  318,
        CV_AMD64_EMM11H   =  319,
        CV_AMD64_EMM12H   =  320,
        CV_AMD64_EMM13H   =  321,
        CV_AMD64_EMM14H   =  322,
        CV_AMD64_EMM15H   =  323,

        // Low byte forms of some standard registers
        CV_AMD64_SIL      =  324,
        CV_AMD64_DIL      =  325,
        CV_AMD64_BPL      =  326,
        CV_AMD64_SPL      =  327,

        // 64-bit regular registers
        CV_AMD64_RAX      =  328,
        CV_AMD64_RBX      =  329,
        CV_AMD64_RCX      =  330,
        CV_AMD64_RDX      =  331,
        CV_AMD64_RSI      =  332,
        CV_AMD64_RDI      =  333,
        CV_AMD64_RBP      =  334,
        CV_AMD64_RSP      =  335,

        // 64-bit integer registers with 8-, 16-, and 32-bit forms (B, W, and D)
        CV_AMD64_R8       =  336,
        CV_AMD64_R9       =  337,
        CV_AMD64_R10      =  338,
        CV_AMD64_R11      =  339,
        CV_AMD64_R12      =  340,
        CV_AMD64_R13      =  341,
        CV_AMD64_R14      =  342,
        CV_AMD64_R15      =  343,

        CV_AMD64_R8B      =  344,
        CV_AMD64_R9B      =  345,
        CV_AMD64_R10B     =  346,
        CV_AMD64_R11B     =  347,
        CV_AMD64_R12B     =  348,
        CV_AMD64_R13B     =  349,
        CV_AMD64_R14B     =  350,
        CV_AMD64_R15B     =  351,

        CV_AMD64_R8W      =  352,
        CV_AMD64_R9W      =  353,
        CV_AMD64_R10W     =  354,
        CV_AMD64_R11W     =  355,
        CV_AMD64_R12W     =  356,
        CV_AMD64_R13W     =  357,
        CV_AMD64_R14W     =  358,
        CV_AMD64_R15W     =  359,

        CV_AMD64_R8D      =  360,
        CV_AMD64_R9D      =  361,
        CV_AMD64_R10D     =  362,
        CV_AMD64_R11D     =  363,
        CV_AMD64_R12D     =  364,
        CV_AMD64_R13D     =  365,
        CV_AMD64_R14D     =  366,
        CV_AMD64_R15D     =  367,

        // AVX registers 256 bits
        CV_AMD64_YMM0     =  368,
        CV_AMD64_YMM1     =  369,
        CV_AMD64_YMM2     =  370,
        CV_AMD64_YMM3     =  371,
        CV_AMD64_YMM4     =  372,
        CV_AMD64_YMM5     =  373,
        CV_AMD64_YMM6     =  374,
        CV_AMD64_YMM7     =  375,
        CV_AMD64_YMM8     =  376, 
        CV_AMD64_YMM9     =  377,
        CV_AMD64_YMM10    =  378,
        CV_AMD64_YMM11    =  379,
        CV_AMD64_YMM12    =  380,
        CV_AMD64_YMM13    =  381,
        CV_AMD64_YMM14    =  382,
        CV_AMD64_YMM15    =  383,

        // AVX registers upper 128 bits
        CV_AMD64_YMM0H    =  384,
        CV_AMD64_YMM1H    =  385,
        CV_AMD64_YMM2H    =  386,
        CV_AMD64_YMM3H    =  387,
        CV_AMD64_YMM4H    =  388,
        CV_AMD64_YMM5H    =  389,
        CV_AMD64_YMM6H    =  390,
        CV_AMD64_YMM7H    =  391,
        CV_AMD64_YMM8H    =  392, 
        CV_AMD64_YMM9H    =  393,
        CV_AMD64_YMM10H   =  394,
        CV_AMD64_YMM11H   =  395,
        CV_AMD64_YMM12H   =  396,
        CV_AMD64_YMM13H   =  397,
        CV_AMD64_YMM14H   =  398,
        CV_AMD64_YMM15H   =  399,

        //Lower/upper 8 bytes of XMM registers.  Unlike CV_AMD64_XMM<regnum><H/L>, these
        //values reprsesent the bit patterns of the registers as 64-bit integers, not
        //the representation of these registers as a double.
        CV_AMD64_XMM0IL    = 400,
        CV_AMD64_XMM1IL    = 401,
        CV_AMD64_XMM2IL    = 402,
        CV_AMD64_XMM3IL    = 403,
        CV_AMD64_XMM4IL    = 404,
        CV_AMD64_XMM5IL    = 405,
        CV_AMD64_XMM6IL    = 406,
        CV_AMD64_XMM7IL    = 407,
        CV_AMD64_XMM8IL    = 408,
        CV_AMD64_XMM9IL    = 409,
        CV_AMD64_XMM10IL    = 410,
        CV_AMD64_XMM11IL    = 411,
        CV_AMD64_XMM12IL    = 412,
        CV_AMD64_XMM13IL    = 413,
        CV_AMD64_XMM14IL    = 414,
        CV_AMD64_XMM15IL    = 415,

        CV_AMD64_XMM0IH    = 416,
        CV_AMD64_XMM1IH    = 417,
        CV_AMD64_XMM2IH    = 418,
        CV_AMD64_XMM3IH    = 419,
        CV_AMD64_XMM4IH    = 420,
        CV_AMD64_XMM5IH    = 421,
        CV_AMD64_XMM6IH    = 422,
        CV_AMD64_XMM7IH    = 423,
        CV_AMD64_XMM8IH    = 424,
        CV_AMD64_XMM9IH    = 425,
        CV_AMD64_XMM10IH    = 426,
        CV_AMD64_XMM11IH    = 427,
        CV_AMD64_XMM12IH    = 428,
        CV_AMD64_XMM13IH    = 429,
        CV_AMD64_XMM14IH    = 430,
        CV_AMD64_XMM15IH    = 431,

        CV_AMD64_YMM0I0    =  432,        // AVX integer registers
        CV_AMD64_YMM0I1    =  433,
        CV_AMD64_YMM0I2    =  434,
        CV_AMD64_YMM0I3    =  435,
        CV_AMD64_YMM1I0    =  436,
        CV_AMD64_YMM1I1    =  437,
        CV_AMD64_YMM1I2    =  438,
        CV_AMD64_YMM1I3    =  439,
        CV_AMD64_YMM2I0    =  440,
        CV_AMD64_YMM2I1    =  441,
        CV_AMD64_YMM2I2    =  442,
        CV_AMD64_YMM2I3    =  443,
        CV_AMD64_YMM3I0    =  444,
        CV_AMD64_YMM3I1    =  445,
        CV_AMD64_YMM3I2    =  446,
        CV_AMD64_YMM3I3    =  447,
        CV_AMD64_YMM4I0    =  448,
        CV_AMD64_YMM4I1    =  449,
        CV_AMD64_YMM4I2    =  450,
        CV_AMD64_YMM4I3    =  451,
        CV_AMD64_YMM5I0    =  452,
        CV_AMD64_YMM5I1    =  453,
        CV_AMD64_YMM5I2    =  454,
        CV_AMD64_YMM5I3    =  455,
        CV_AMD64_YMM6I0    =  456,
        CV_AMD64_YMM6I1    =  457,
        CV_AMD64_YMM6I2    =  458,
        CV_AMD64_YMM6I3    =  459,
        CV_AMD64_YMM7I0    =  460,
        CV_AMD64_YMM7I1    =  461,
        CV_AMD64_YMM7I2    =  462,
        CV_AMD64_YMM7I3    =  463,
        CV_AMD64_YMM8I0    =  464,
        CV_AMD64_YMM8I1    =  465,
        CV_AMD64_YMM8I2    =  466,
        CV_AMD64_YMM8I3    =  467,
        CV_AMD64_YMM9I0    =  468,
        CV_AMD64_YMM9I1    =  469,
        CV_AMD64_YMM9I2    =  470,
        CV_AMD64_YMM9I3    =  471,
        CV_AMD64_YMM10I0    =  472,
        CV_AMD64_YMM10I1    =  473,
        CV_AMD64_YMM10I2    =  474,
        CV_AMD64_YMM10I3    =  475,
        CV_AMD64_YMM11I0    =  476,
        CV_AMD64_YMM11I1    =  477,
        CV_AMD64_YMM11I2    =  478,
        CV_AMD64_YMM11I3    =  479,
        CV_AMD64_YMM12I0    =  480,
        CV_AMD64_YMM12I1    =  481,
        CV_AMD64_YMM12I2    =  482,
        CV_AMD64_YMM12I3    =  483,
        CV_AMD64_YMM13I0    =  484,
        CV_AMD64_YMM13I1    =  485,
        CV_AMD64_YMM13I2    =  486,
        CV_AMD64_YMM13I3    =  487,
        CV_AMD64_YMM14I0    =  488,
        CV_AMD64_YMM14I1    =  489,
        CV_AMD64_YMM14I2    =  490,
        CV_AMD64_YMM14I3    =  491,
        CV_AMD64_YMM15I0    =  492,
        CV_AMD64_YMM15I1    =  493,
        CV_AMD64_YMM15I2    =  494,
        CV_AMD64_YMM15I3    =  495,

        CV_AMD64_YMM0F0    =  496,        // AVX floating-point single precise registers
        CV_AMD64_YMM0F1    =  497,
        CV_AMD64_YMM0F2    =  498,
        CV_AMD64_YMM0F3    =  499,
        CV_AMD64_YMM0F4    =  500,
        CV_AMD64_YMM0F5    =  501,
        CV_AMD64_YMM0F6    =  502,
        CV_AMD64_YMM0F7    =  503,
        CV_AMD64_YMM1F0    =  504,
        CV_AMD64_YMM1F1    =  505,
        CV_AMD64_YMM1F2    =  506,
        CV_AMD64_YMM1F3    =  507,
        CV_AMD64_YMM1F4    =  508,
        CV_AMD64_YMM1F5    =  509,
        CV_AMD64_YMM1F6    =  510,
        CV_AMD64_YMM1F7    =  511,
        CV_AMD64_YMM2F0    =  512,
        CV_AMD64_YMM2F1    =  513,
        CV_AMD64_YMM2F2    =  514,
        CV_AMD64_YMM2F3    =  515,
        CV_AMD64_YMM2F4    =  516,
        CV_AMD64_YMM2F5    =  517,
        CV_AMD64_YMM2F6    =  518,
        CV_AMD64_YMM2F7    =  519,
        CV_AMD64_YMM3F0    =  520,
        CV_AMD64_YMM3F1    =  521,
        CV_AMD64_YMM3F2    =  522,
        CV_AMD64_YMM3F3    =  523,
        CV_AMD64_YMM3F4    =  524,
        CV_AMD64_YMM3F5    =  525,
        CV_AMD64_YMM3F6    =  526,
        CV_AMD64_YMM3F7    =  527,
        CV_AMD64_YMM4F0    =  528,
        CV_AMD64_YMM4F1    =  529,
        CV_AMD64_YMM4F2    =  530,
        CV_AMD64_YMM4F3    =  531,
        CV_AMD64_YMM4F4    =  532,
        CV_AMD64_YMM4F5    =  533,
        CV_AMD64_YMM4F6    =  534,
        CV_AMD64_YMM4F7    =  535,
        CV_AMD64_YMM5F0    =  536,
        CV_AMD64_YMM5F1    =  537,
        CV_AMD64_YMM5F2    =  538,
        CV_AMD64_YMM5F3    =  539,
        CV_AMD64_YMM5F4    =  540,
        CV_AMD64_YMM5F5    =  541,
        CV_AMD64_YMM5F6    =  542,
        CV_AMD64_YMM5F7    =  543,
        CV_AMD64_YMM6F0    =  544,
        CV_AMD64_YMM6F1    =  545,
        CV_AMD64_YMM6F2    =  546,
        CV_AMD64_YMM6F3    =  547,
        CV_AMD64_YMM6F4    =  548,
        CV_AMD64_YMM6F5    =  549,
        CV_AMD64_YMM6F6    =  550,
        CV_AMD64_YMM6F7    =  551,
        CV_AMD64_YMM7F0    =  552,
        CV_AMD64_YMM7F1    =  553,
        CV_AMD64_YMM7F2    =  554,
        CV_AMD64_YMM7F3    =  555,
        CV_AMD64_YMM7F4    =  556,
        CV_AMD64_YMM7F5    =  557,
        CV_AMD64_YMM7F6    =  558,
        CV_AMD64_YMM7F7    =  559,
        CV_AMD64_YMM8F0    =  560,
        CV_AMD64_YMM8F1    =  561,
        CV_AMD64_YMM8F2    =  562,
        CV_AMD64_YMM8F3    =  563,
        CV_AMD64_YMM8F4    =  564,
        CV_AMD64_YMM8F5    =  565,
        CV_AMD64_YMM8F6    =  566,
        CV_AMD64_YMM8F7    =  567,
        CV_AMD64_YMM9F0    =  568,
        CV_AMD64_YMM9F1    =  569,
        CV_AMD64_YMM9F2    =  570,
        CV_AMD64_YMM9F3    =  571,
        CV_AMD64_YMM9F4    =  572,
        CV_AMD64_YMM9F5    =  573,
        CV_AMD64_YMM9F6    =  574,
        CV_AMD64_YMM9F7    =  575,
        CV_AMD64_YMM10F0    =  576,
        CV_AMD64_YMM10F1    =  577,
        CV_AMD64_YMM10F2    =  578,
        CV_AMD64_YMM10F3    =  579,
        CV_AMD64_YMM10F4    =  580,
        CV_AMD64_YMM10F5    =  581,
        CV_AMD64_YMM10F6    =  582,
        CV_AMD64_YMM10F7    =  583,
        CV_AMD64_YMM11F0    =  584,
        CV_AMD64_YMM11F1    =  585,
        CV_AMD64_YMM11F2    =  586,
        CV_AMD64_YMM11F3    =  587,
        CV_AMD64_YMM11F4    =  588,
        CV_AMD64_YMM11F5    =  589,
        CV_AMD64_YMM11F6    =  590,
        CV_AMD64_YMM11F7    =  591,
        CV_AMD64_YMM12F0    =  592,
        CV_AMD64_YMM12F1    =  593,
        CV_AMD64_YMM12F2    =  594,
        CV_AMD64_YMM12F3    =  595,
        CV_AMD64_YMM12F4    =  596,
        CV_AMD64_YMM12F5    =  597,
        CV_AMD64_YMM12F6    =  598,
        CV_AMD64_YMM12F7    =  599,
        CV_AMD64_YMM13F0    =  600,
        CV_AMD64_YMM13F1    =  601,
        CV_AMD64_YMM13F2    =  602,
        CV_AMD64_YMM13F3    =  603,
        CV_AMD64_YMM13F4    =  604,
        CV_AMD64_YMM13F5    =  605,
        CV_AMD64_YMM13F6    =  606,
        CV_AMD64_YMM13F7    =  607,
        CV_AMD64_YMM14F0    =  608,
        CV_AMD64_YMM14F1    =  609,
        CV_AMD64_YMM14F2    =  610,
        CV_AMD64_YMM14F3    =  611,
        CV_AMD64_YMM14F4    =  612,
        CV_AMD64_YMM14F5    =  613,
        CV_AMD64_YMM14F6    =  614,
        CV_AMD64_YMM14F7    =  615,
        CV_AMD64_YMM15F0    =  616,
        CV_AMD64_YMM15F1    =  617,
        CV_AMD64_YMM15F2    =  618,
        CV_AMD64_YMM15F3    =  619,
        CV_AMD64_YMM15F4    =  620,
        CV_AMD64_YMM15F5    =  621,
        CV_AMD64_YMM15F6    =  622,
        CV_AMD64_YMM15F7    =  623,
        
        CV_AMD64_YMM0D0    =  624,        // AVX floating-point double precise registers
        CV_AMD64_YMM0D1    =  625,
        CV_AMD64_YMM0D2    =  626,
        CV_AMD64_YMM0D3    =  627,
        CV_AMD64_YMM1D0    =  628,
        CV_AMD64_YMM1D1    =  629,
        CV_AMD64_YMM1D2    =  630,
        CV_AMD64_YMM1D3    =  631,
        CV_AMD64_YMM2D0    =  632,
        CV_AMD64_YMM2D1    =  633,
        CV_AMD64_YMM2D2    =  634,
        CV_AMD64_YMM2D3    =  635,
        CV_AMD64_YMM3D0    =  636,
        CV_AMD64_YMM3D1    =  637,
        CV_AMD64_YMM3D2    =  638,
        CV_AMD64_YMM3D3    =  639,
        CV_AMD64_YMM4D0    =  640,
        CV_AMD64_YMM4D1    =  641,
        CV_AMD64_YMM4D2    =  642,
        CV_AMD64_YMM4D3    =  643,
        CV_AMD64_YMM5D0    =  644,
        CV_AMD64_YMM5D1    =  645,
        CV_AMD64_YMM5D2    =  646,
        CV_AMD64_YMM5D3    =  647,
        CV_AMD64_YMM6D0    =  648,
        CV_AMD64_YMM6D1    =  649,
        CV_AMD64_YMM6D2    =  650,
        CV_AMD64_YMM6D3    =  651,
        CV_AMD64_YMM7D0    =  652,
        CV_AMD64_YMM7D1    =  653,
        CV_AMD64_YMM7D2    =  654,
        CV_AMD64_YMM7D3    =  655,
        CV_AMD64_YMM8D0    =  656,
        CV_AMD64_YMM8D1    =  657,
        CV_AMD64_YMM8D2    =  658,
        CV_AMD64_YMM8D3    =  659,
        CV_AMD64_YMM9D0    =  660,
        CV_AMD64_YMM9D1    =  661,
        CV_AMD64_YMM9D2    =  662,
        CV_AMD64_YMM9D3    =  663,
        CV_AMD64_YMM10D0    =  664,
        CV_AMD64_YMM10D1    =  665,
        CV_AMD64_YMM10D2    =  666,
        CV_AMD64_YMM10D3    =  667,
        CV_AMD64_YMM11D0    =  668,
        CV_AMD64_YMM11D1    =  669,
        CV_AMD64_YMM11D2    =  670,
        CV_AMD64_YMM11D3    =  671,
        CV_AMD64_YMM12D0    =  672,
        CV_AMD64_YMM12D1    =  673,
        CV_AMD64_YMM12D2    =  674,
        CV_AMD64_YMM12D3    =  675,
        CV_AMD64_YMM13D0    =  676,
        CV_AMD64_YMM13D1    =  677,
        CV_AMD64_YMM13D2    =  678,
        CV_AMD64_YMM13D3    =  679,
        CV_AMD64_YMM14D0    =  680,
        CV_AMD64_YMM14D1    =  681,
        CV_AMD64_YMM14D2    =  682,
        CV_AMD64_YMM14D3    =  683,
        CV_AMD64_YMM15D0    =  684,
        CV_AMD64_YMM15D1    =  685,
        CV_AMD64_YMM15D2    =  686,
        CV_AMD64_YMM15D3    =  687
        }
    }