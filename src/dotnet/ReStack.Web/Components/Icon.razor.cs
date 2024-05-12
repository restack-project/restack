using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace ReStack.Web.Components;

public partial class Icon
{
    [Parameter] public FontAwesomeIcons Type { get; set; }
    [Parameter] public IconSize Size { get; set; } = IconSize.Md;
    [Parameter] public string CssClass { get; set; }
}

public enum FontAwesomeIcons
{
    [Description("fa-solid fa-layer-group")]
    LayerGroup,
    [Description("fa-solid fa-book-open")]
    BookOpen,
    [Description("fa-solid fa-gear")]
    Gear,
    [Description("fa-solid fa-angles-right")]
    AnglesRight,
    [Description("fa-solid fa-angles-left")]
    AnglesLeft,
    [Description("fa-solid fa-moon")]
    Moon,
    [Description("fa-solid fa-sun")]
    Sun,
    [Description("fa-solid fa-magnifying-glass")]
    MagnifyingGlass,
    [Description("fa-solid fa-circle-check")]
    CircleCheck,
    [Description("fa-solid fa-circle-xmark")]
    CircleXmark,
    [Description("fa-solid fa-circle")]
    Circle,
    [Description("fa-solid fa-circle-dot")]
    CircleDot,
    [Description("fa-solid fa-arrow-left")]
    ArrowLeft,
    [Description("fa-solid fa-copy")]
    Copy,
    [Description("fa-solid fa-xmark")]
    Xmark,
    [Description("fa-solid fa-bars")]
    Bars,
    [Description("fa-solid fa-file")]
    File,
    [Description("fa-solid fa-code")]
    Code
}

public enum IconSize
{
    [Description("fa-xs")]
    Xs,
    [Description("fa-sm")]
    Sm,
    [Description("fa-md")]
    Md,
    [Description("fa-lg")]
    Lg,
    [Description("fa-2x")]
    X2,
    [Description("fa-3x")]
    X3,
    [Description("fa-5x")]
    X5,
    [Description("fa-7x")]
    X7,
    [Description("fa-10x")]
    X10
}